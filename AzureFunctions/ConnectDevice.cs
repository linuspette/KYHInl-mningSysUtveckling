using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Models.Input.Devices;
using Shared.Models.Response.Devices;

namespace AzureFunctions
{
    public static class ConnectDevice
    {
        private static readonly RegistryManager _registryManager =
                RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));

        [FunctionName("ConnectDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "devices/connect")] HttpRequest req,
            ILogger log)
        {

            try
            {
                var body = JsonConvert.DeserializeObject<HttpDeviceRequest>(await new StreamReader(req.Body).ReadToEndAsync());

                if (string.IsNullOrEmpty(body.DeviceId))
                    return new BadRequestObjectResult(new HttpDeviceResponse("DeviceId is required"));

                var device = await _registryManager.GetDeviceAsync(body.DeviceId);

                if (device == null)
                    device = await _registryManager.AddDeviceAsync(new Device(body.DeviceId));

                if (device != null)
                {
                    var twin = await _registryManager.GetTwinAsync(device.Id);
                    twin.Properties.Desired["interval"] = 10000;

                    await _registryManager.UpdateTwinAsync(twin.DeviceId, twin, twin.ETag);
                }

                return new OkObjectResult(new HttpDeviceResponse("Device connected", device));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new HttpDeviceResponse("Unable to connect to device", ex.Message));
            }

        }
    }
}
