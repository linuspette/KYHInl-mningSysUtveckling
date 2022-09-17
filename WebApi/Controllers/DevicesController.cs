using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Input.Devices;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IIotDeviceManager _iotDeviceManager;

        public DevicesController(IIotDeviceManager iotDeviceManager)
        {
            _iotDeviceManager = iotDeviceManager;
        }

        [HttpPost]
        [Route("getuserdevices")]
        public async Task<IActionResult> GetUserIotDevices(Guid id, int take = 0)
        {
            try
            {
                return new OkObjectResult(await _iotDeviceManager.GetUserDevicesAsync(id, take));
            }
            catch { }

            return new BadRequestObjectResult("Something went wrong while fetching your Iot devices. Try again later");
        }

        [HttpPost]
        [Route("createdevice")]
        public async Task<IActionResult> CreateIotDeviceAsync(AddDeviceRequest model)
        {
            var response = await _iotDeviceManager.AddIotDeviceAsync(model);
            return response;
        }
    }
}
