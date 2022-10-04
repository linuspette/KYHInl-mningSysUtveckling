using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Models.Input.Devices;
using Shared.Models.Response.Devices;

namespace AzureFunctions
{
    public static class AddDevice
    {
        private static readonly RegistryManager _registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));
        [FunctionName("AddDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "devices/add")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var body = JsonConvert.DeserializeObject<AddDeviceRequest>(await new StreamReader(req.Body).ReadToEndAsync());

                if (string.IsNullOrEmpty(body.DeviceId))
                    return new BadRequestObjectResult("Device ID must be supplied");

                if (await _registryManager.GetDeviceAsync(body.DeviceId) != null)
                    return new ConflictObjectResult("A device with this ID already exists");

                var device = await _registryManager.AddDeviceAsync(new Device(body.DeviceId));
                return new OkObjectResult(new HttpDeviceResponse("Device created successfully", device));
            }
            catch {}

            return new BadRequestObjectResult("Device creation failed");
        }
    }
}
