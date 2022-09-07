using AutoMapper;
using ClassLibrary.Models.Input.Roles;
using ClassLibrary.Models.View.Roles;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
using WebApi.Models.DataEntities;

namespace WebApi.Repositories;

public interface IRoleManager
{
    public Task<List<Role>> GetAllRolesAsync();
    public Task<bool> CheckIfRoleExistsAsync(string role);
    public Task<IActionResult> CreateRoleAsync(CreateRole role);
}

public class RoleRepository : EntityFrameworkRepository<RoleEntity>, IRoleManager
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RoleRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Role>> GetAllRolesAsync()
    {
        var roles = _mapper.Map<List<Role>>(await ReadRecordsAsync());

        return roles ?? null!;
    }

    public async Task<bool> CheckIfRoleExistsAsync(string role)
    {
        if (await ReadRecordAsync(x => x.Name == role) == null)
            return false;
        return true;
    }

    public async Task<IActionResult> CreateRoleAsync(CreateRole role)
    {
        try
        {
            if (await CheckIfRoleExistsAsync(role.Name))
                return new ConflictObjectResult("This role already exists");

            var roleEntity = _mapper.Map<RoleEntity>(role);

            await CreateRecordAsync(roleEntity);
            return new OkObjectResult("Role was created successfully");
        }
        catch { }

        return new BadRequestObjectResult("Unable to process the create role procces");
    }
}