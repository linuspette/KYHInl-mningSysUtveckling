using AutoMapper;
using ClassLibrary.Models.Input.Roles;
using ClassLibrary.Models.Input.Users;
using ClassLibrary.Models.View.Roles;
using ClassLibrary.Models.View.Tokens;
using ClassLibrary.Models.View.User;
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