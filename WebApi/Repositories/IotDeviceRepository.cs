using AutoMapper;
using Shared.Models.View.Device;
using WebApi.Helpers;
using WebApi.Models.DataEntities.Devices;

namespace WebApi.Repositories;

public interface IIotDeviceManager
{
    public Task<List<IotDevice>> GetAllDevicesAsync(int take = 0);
}
public class IotDeviceRepository : EntityFrameworkRepository<IotDeviceEntity>, IIotDeviceManager
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public IotDeviceRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<IotDevice>> GetAllDevicesAsync(int take = 0)
    {
        return _mapper.Map<List<IotDevice>>(await ReadRecordsAsync(take));
    }
}