using AutoMapper;
using Shared.Models.View.Tokens;
using WebApi.Helpers;
using WebApi.Models.DataEntities.Authentication;

namespace WebApi.Repositories;

public interface ITokenHandler
{
    public enum StatusCode
    {
        Success,
        Failed,
        Valid,
        Expired,
        NotAuthorized
    }
    public Task<string> GenerateTokenAsync(UserEntity user);
    public Task<ITokenHandler.StatusCode> ValidateTokenAsync(string value);
    public Task<JwtToken> UpdateTokenAsync(Guid id, JwtToken token);
    public Task<List<JwtToken>> GetAllTokensAsync();
    public Task MakeAllTokensInvalid();
    public Task DeleteInactiveTokens();
}
public class JwtTokenRepository : EntityFrameworkRepository<JwtTokenEntity>, ITokenHandler
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public JwtTokenRepository(ApplicationDbContext context, IMapper mapper, IConfiguration configuration) : base(context, mapper)
    {
        _mapper = mapper;
        _configuration = configuration;
    }

    //Public methods
    public async Task<string> GenerateTokenAsync(UserEntity user)
    {
        var tokenHandler = _mapper.Map<AccountTokenHandler>(user);

        var token = tokenHandler.GenerateToken(_configuration.GetValue<string>("SysDevSecret"));

        if (await AddTokenToDatabaseAsync(token) == ITokenHandler.StatusCode.Failed)
            return null!;

        return token;
    }
    public async Task<ITokenHandler.StatusCode> ValidateTokenAsync(string value)
    {

        try
        {
            var token = await ReadRecordAsync(x => x.Token == value);
            if (token != null)
            {
                if (DateTime.Now < token.Expires && token.IsActive)
                    return ITokenHandler.StatusCode.Valid;
                else
                    return ITokenHandler.StatusCode.Expired;
                    
            }
        }
        catch { }

        return ITokenHandler.StatusCode.NotAuthorized;
    }
    public async Task<JwtToken> UpdateTokenAsync(Guid id, JwtToken token)
    {
        if (id == token.Id)
            return _mapper.Map<JwtToken>(await UpdateRecordAsync(x => x.Id == id, _mapper.Map<JwtTokenEntity>(token)));

        return null!;
    }
    public async Task MakeAllTokensInvalid()
    {
        var tokens = await GetAllTokensAsync();

        foreach (var jwtToken in tokens)
        {
            jwtToken.IsActive = false;
            await UpdateTokenAsync(jwtToken.Id, jwtToken);
        }
    }
    public async Task DeleteInactiveTokens()
    {
        var tokens = await GetAllTokensAsync();
        foreach (var jwtToken in tokens)
        {
            if (DateTime.Now > jwtToken.Expires.AddDays(2))
            {
                await DeleteRecordAsync(x => x.Id == jwtToken.Id);
            }
        }
    }
    public async Task<List<JwtToken>> GetAllTokensAsync()
    {
        return _mapper.Map<List<JwtToken>>(await ReadRecordsAsync()) ?? null!;
    }

    //Private methods
    private async Task<ITokenHandler.StatusCode> AddTokenToDatabaseAsync(string token)
    {
        var tokenEntity = new JwtTokenEntity
        {
            Token = token
        };
        tokenEntity.SetCreationAndExpiration(DateTime.Now);

        try
        {
            await CreateRecordAsync(tokenEntity, Guid.Empty);
            return ITokenHandler.StatusCode.Success;
        }
        catch { };
        return ITokenHandler.StatusCode.Failed;
    }
}