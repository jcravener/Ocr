using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WestUs3OcrPoc.Handlers;
using Newtonsoft.Json;

namespace WestUs3_OCR_OPC
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var ocrReq = await new BoilerPlate(log, req).GetRequest();

            var cv = new ComputerVision(
                System.Environment.GetEnvironmentVariable("ENDPOINT"),
                System.Environment.GetEnvironmentVariable("SUBSCRIPTION_KEY"),
                log
                );

            var readResults = await cv.ReadFileUrl(ocrReq);

            //return new ObjectResult(JsonConvert.DeserializeObject<IList<ReadResult>>(readResults));
            return new ObjectResult(JsonConvert.SerializeObject(readResults));
        }
    }
}
