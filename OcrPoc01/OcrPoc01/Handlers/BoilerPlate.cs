using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using WestUs3_OCR_OPC.Models;

namespace WestUs3OcrPoc.Handlers
{
    public class BoilerPlate
    {
        private HttpRequest Request;
        private ILogger Log;

        public BoilerPlate(ILogger log, HttpRequest req)
        {
            Request = req;
            Log = log;
        }

        public async Task<IActionResult> ProcessRequest()
        {
            string name = Request.Query["name"];
            var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        public async Task<OcrRequest> GetRequest()
        {
            var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
            var ocrRequest = JsonConvert.DeserializeObject<OcrRequest>(requestBody);

            if (ocrRequest is null)
            {
                Log.LogWarning("Requst was empty.");
            }
            else
            {
                Log.LogInformation($"Got URL: {ocrRequest.Url}, ModelVersion: {ocrRequest.ModelVersion}");
            }

            return ocrRequest;
        }
    }
}
