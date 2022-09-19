using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Input.Devices;
using Shared.Models.View.Device;
using WebApi.Helpers;
using WebApi.Models.DataEntities.Devices;

namespace WebApi.Repositories;

public interface IIotDeviceManager
{
    public Task<List<IotDevice>> GetAllDevicesAsync(int take = 0);
    public Task<IActionResult> AddIotDeviceAsync(AddDeviceRequest model);
    public Task<List<IotDevice>> GetUserDevicesAsync(Guid userId, int take = 0);
}
public class IotDeviceRepository : EntityFrameworkRepository<IotDeviceEntity>, IIotDeviceManager
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAzureFunctionsClient _azureFunctionsClient;

    public IotDeviceRepository(ApplicationDbContext context, IMapper mapper, IAzureFunctionsClient azureFunctionsClient) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
        _azureFunctionsClient = azureFunctionsClient;
    }

    public async Task<List<IotDevice>> GetAllDevicesAsync(int take = 0)
    {
        return _mapper.Map<List<IotDevice>>(await ReadRecordsAsync(take)) ?? null!;
    }
    public async Task<List<IotDevice>> GetUserDevicesAsync(Guid userId, int take = 0)
    {
        var user = await _context.Users
            .Include(x => x.IotDevices)
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            return null!;

        return _mapper.Map<List<IotDevice>>(user.IotDevices) ?? null!;
    }
    public async Task<IActionResult> AddIotDeviceAsync(AddDeviceRequest model)
    {
        try
        {
            if (await ReadRecordAsync(x => x.DeviceId == model.DeviceId) != null)
                return new ConflictObjectResult("Device already exists");

            if (await _azureFunctionsClient.AddIotDevice(model) == IAzureFunctionsClient.StatusCode.Failed)
                return new BadRequestObjectResult("Device creation failed");

            var result = await CreateRecordAsync(_mapper.Map<IotDeviceEntity>(model), model.userId);

            if (result != null)
                return new OkObjectResult("Device creation succeded");
        }
        catch { }

        return new BadRequestObjectResult("Device creation failed");
    }
    protected override async Task<IotDeviceEntity> CreateRecordAsync(IotDeviceEntity record, Guid id)
    {
        var user = await _context.Users.Include(x => x.IotDevices).FirstOrDefaultAsync(x => x.Id == id);
        if (user != null)
        {
            user.IotDevices.Add(record);

            try
            {
                _context.IotDevices.Add(record);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return null!;
            }

            try
            {
                _context.Attach(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch
            {
                return null!;
            }

            return record;
        }


        return null!;
    }
}