using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WestUs3_OCR_OPC.Models;
using WestUs3_OCR_OPC.Processors;

namespace WestUs3OcrPoc.Handlers
{
    public class ComputerVision
    {
        private readonly string _endPoint;
        private readonly string _subscriptionKey;
        private readonly ILogger Log;
        private readonly ComputerVisionClient Client;

        public ComputerVision(string endPoint, string subscriptionKey, ILogger log)
        {
            _endPoint = endPoint;
            _subscriptionKey = subscriptionKey;
            Log = log;
            Client = Authenticate(subscriptionKey, endPoint);
        }

        public ComputerVisionClient Authenticate(string subKey, string endPoint)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(subKey))
              { Endpoint = endPoint };
            return client;
        }

        public async Task<IList<ReadResult>> ReadFileUrl(OcrRequest request)
        {
            Log.LogInformation("----------------------------------------------------------");
            Log.LogInformation($"Read file from URL: {request.Url}, Using MODELVERSION: {request.ModelVersion}");


            // Read text from URL
            var textHeaders = await Client.ReadAsync(request.Url, language: request.Language, modelVersion: request.ModelVersion, readingOrder: request.readingOrder);
            //var ocrResult = await Client.RecognizePrintedTextAsync(true, request.Url, modelVersion: request.ModelVersion);

            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);

            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;

            Log.LogInformation($"Extracting text from URL file {Path.GetFileName(request.Url)}...");

            Console.WriteLine($"Extracting text from URL file {Path.GetFileName(request.Url)}...");
            do
            {
                results = await Client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            var indexes = new ScoreCard(results, request).GetNameIndexes();


            // Display the found text.
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    Log.LogInformation(line.Text);
                }
            }

            Log.LogInformation("Done");
            return textUrlFileResults;
        }
    }
}
