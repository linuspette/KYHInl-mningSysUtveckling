using AutoMapper;
using Shared.Models.Input.Roles;
using Shared.Models.Input.Users;
using Shared.Models.View.Roles;
using Shared.Models.View.Tokens;
using Shared.Models.View.User;
using WebApi.Models.DataEntities;

namespace WebApi.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        //User profiles
        CreateMap<UserEntity, User>().ReverseMap();
        CreateMap<UserEntity, AccountTokenHandler>();

        CreateMap<SignIn, AccountTokenHandler>();
        CreateMap<AddUser, UserEntity>();

        //Role profiles
        CreateMap<RoleEntity, Role>().ReverseMap();
        CreateMap<CreateRole, RoleEntity>();

        //Token Profiles
        CreateMap<JwtTokenEntity, JwtToken>().ReverseMap();
    }
}