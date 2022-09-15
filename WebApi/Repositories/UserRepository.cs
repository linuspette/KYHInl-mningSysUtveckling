using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Input.Users;
using Shared.Models.View.User;
using WebApi.Helpers;
using WebApi.Models.DataEntities.Authentication;

namespace WebApi.Repositories;

public interface IUserManager
{
    public Task<IActionResult> AddUserAsync(AddUser user);
    public Task<IActionResult> SignInAsync(SignIn user);
    public Task<User?> GetUserByUsernameAsync(string username);
    public Task<IActionResult> GetAllUsersAsync(int take = 0);
}

public class UserRepository : EntityFrameworkRepository<UserEntity>, IUserManager
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly ITokenHandler _tokenHandler;

    public UserRepository(ApplicationDbContext context, IMapper mapper, IConfiguration configuration, ITokenHandler tokenHandler) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
        _tokenHandler = tokenHandler;
    }
    //Overrided methods
    protected override async Task<IEnumerable<UserEntity>> ReadRecordsAsync(int take = 0)
    {
        try
        {
            if (take != 0)
                return await _context.Users
                    .Take(take).ToListAsync() ?? null!;

            return await _context.Users
                .ToListAsync() ?? null!;
        }
        catch { }
        return null!;
    }


    //Public methods
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var user = _mapper.Map<User>(await ReadRecordAsync(x => x.Username == username));
        return user ?? null;
    }
    public async Task<IActionResult> AddUserAsync(AddUser user)
    {
        try
        {
            if (await ReadRecordAsync(x => x.Username == user.Username) != null)
                return new ConflictObjectResult("This user already exists");

            var userEntity = _mapper.Map<UserEntity>(user);
            userEntity.CreatePassword(user.Password);

            var result = await CreateRecordAsync(userEntity);
            if(result != null)
                return new OkObjectResult("User was created successfully");
        }
        catch { }

        return new BadRequestObjectResult("Unable to process user sign up request");
    }
    public async Task<IActionResult> SignInAsync(SignIn user)
    {
        try
        {
            var userEntity = await ReadRecordAsync(x => x.Username == user.Username);
            if (userEntity != null && userEntity.ValidatePassword(user.Password))
            {
                if (userEntity.IsActive)
                {
                    //TO DO
                    //Lägga till check för när lösenord senast ändrades
                    var token = await _tokenHandler.GenerateTokenAsync(userEntity);
                    if (token != null)
                        return new OkObjectResult(token);
                    return new BadRequestObjectResult("Access token could not be created");
                }

                return new UnauthorizedObjectResult("Account is disabled, please contact an administrator");
            }

            return new UnauthorizedObjectResult("Incorrect username or password");

        }
        catch { }
        return new BadRequestObjectResult("Unable to process user sign in request");
    }
    public async Task<IActionResult> GetAllUsersAsync(int take = 0)
    {
        var users = _mapper.Map<List<User>>(await ReadRecordsAsync(take));

        if (users != null)
            return new OkObjectResult(users);

        return new NoContentResult();
    }
}