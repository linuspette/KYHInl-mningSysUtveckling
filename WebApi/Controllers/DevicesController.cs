using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Input.Devices;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly IIotDeviceManager _iotDeviceManager;

        public DevicesController(IIotDeviceManager iotDeviceManager)
        {
            _iotDeviceManager = iotDeviceManager;
        }

        [HttpPost]
        [Route("getuserdevices")]
        public async Task<IActionResult> GetUserIotDevices(int take = 0)
        {
            try
            {
                var id = User.Claims?.FirstOrDefault(x => x.Type == "id")?.Value ?? null!;

                if (!string.IsNullOrEmpty(id))
                    return new OkObjectResult(await _iotDeviceManager.GetUserDevicesAsync(Guid.Parse(id), take));
            }
            catch { }

            return new BadRequestObjectResult("Something went wrong while fetching your Iot devices. Try again later");
        }

        [HttpPost]
        [Route("createdevice")]
        public async Task<IActionResult> CreateIotDeviceAsync(AddDeviceRequest model)
        {
            try
            {
                var id = User.Claims?.FirstOrDefault(x => x.Type == "id")?.Value ?? null!;
                var response = await _iotDeviceManager.AddIotDeviceAsync(model,Guid.Parse(id));
                return response;
            }
            catch { }

            return new BadRequestObjectResult("Something went wrong, please try again later");
        }
    }
}
