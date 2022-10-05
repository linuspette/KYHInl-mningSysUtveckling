using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Shared.Models.Response.Devices;
using System;
using System.Threading.Tasks;

namespace AzureFunctions
{
    public static class ConnectDevice
    {
        [FunctionName("ConnectDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "devices/connect")] HttpRequest req,
            ILogger log)
        {

            try
            {
                using var registryManager =
                    RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));

                var device = await registryManager.GetDeviceAsync(req.Query["deviceId"]);
                //If device is null, create new device
                device ??= await registryManager.AddDeviceAsync(new Device(req.Query["deviceId"]));

                return new OkObjectResult($"{Environment.GetEnvironmentVariable("IotHub").Split(";")[0]};DeviceId{device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new HttpDeviceResponse("Unable to connect to device", ex.Message));
            }

        }
    }
}
