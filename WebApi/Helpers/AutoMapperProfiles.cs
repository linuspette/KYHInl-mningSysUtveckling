using AutoMapper;
using Shared.Models.Input.Devices;
using Shared.Models.Input.Roles;
using Shared.Models.Input.Users;
using Shared.Models.View.Device;
using Shared.Models.View.Roles;
using Shared.Models.View.Tokens;
using Shared.Models.View.User;
using WebApi.Models.DataEntities.Authentication;
using WebApi.Models.DataEntities.Devices;

namespace WebApi.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        //User profiles
        CreateMap<UserEntity, User>().ReverseMap();
        CreateMap<UserEntity, AccountTokenHandler>();

        CreateMap<SignIn, AccountTokenHandler>();
        CreateMap<SignUp, UserEntity>();

        //Iotdevice profiles
        CreateMap<IotDeviceEntity, IotDevice>().ReverseMap();
        CreateMap<AddDeviceRequest, IotDeviceEntity>();

        //Token Profiles
        CreateMap<JwtTokenEntity, JwtToken>().ReverseMap();
    }
}