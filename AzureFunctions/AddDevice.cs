using System;
using System.IO;
using System.Threading.Tasks;
using ClassLibrary.Models.Input.Devices;
using ClassLibrary.Models.Response.Devices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Devices;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public static class AddDevice
    {
        private static readonly RegistryManager _registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));

        [FunctionName("AddDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "devices")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<AddDeviceRequest>(await new StreamReader(req.Body).ReadToEndAsync());
                if (data == null || data.DeviceId == null)
                    return new BadRequestObjectResult("DeviceId is required");

                var device = await _registryManager.GetDeviceAsync(data.DeviceId);
                if (device != null)
                    return new ConflictObjectResult(new AddDeviceResponse
                    {
                        Message = "Device already exists.",
                        IotHub = Environment.GetEnvironmentVariable("IotHubName"),
                        Device = device,
                        DeviceTwin = await _registryManager.GetTwinAsync(device.Id)
                    });

            }
            catch (Exception e)
            {
            }

            return new BadRequestResult();
        }
    }
}
