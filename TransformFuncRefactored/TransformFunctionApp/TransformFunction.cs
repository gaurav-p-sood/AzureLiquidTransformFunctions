using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;

namespace TransformFunctionApp
{
    public static class TransformFunction
    {
        [FunctionName("TransformFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "template/{templateName}")] HttpRequest req,
            [Blob("mappers/{templateName}", FileAccess.Read, Connection = "AzureWebJobsStorage")] Stream blob,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            if (blob == null)
            {
                Exception ex = new System.Exception("Template could not be found or it could not be loaded. Please verifiy storage account for correct template file in blob container.");
                return new BadRequestObjectResult(ex.Message);
            }

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                var streamReader = new StreamReader(blob); //blob.ReadAsync(buffer, 0, (int)blob.Length);

                string blobstring = streamReader.ReadToEnd().ToString();

                string contentType = req.ContentType.ToString();

                string accept = req.Headers["Accept"].ToString();

                //json to json 
                if (contentType == "application/json" && accept == "application/json")
                {
                    log.LogInformation("Running Liquid Transfomer.");
                    var output = TransformHelper.ApplyJSONToJSONLiquidTranform(blobstring, requestBody);

                    return new OkObjectResult(output);
                }

                //xml to json
                if (contentType == "application/xml" && accept == "application/json")
                {
                    log.LogInformation("Running Liquid Transfomer.");
                    var output = TransformHelper.ApplyXMLToJSONLiquidTranform(blobstring, requestBody);
                    return new OkObjectResult(output);
                }

                //xml to xml 
                if (contentType == "application/xml" && accept == "application/xml")
                {
                    log.LogInformation("Running XSL Transfomer.");
                    MemoryStream transformedXMlStream = TransformHelper.ApplyXSLTransform(requestBody, blobstring);

                    var xmlString = Encoding.UTF8.GetString(transformedXMlStream.ToArray());


                   // string responseMessage = string.IsNullOrEmpty(xmlString)
                   //? "This HTTP triggered function executed successfully. Pass a template name in the query string."
                   //: $"{xmlString}";
                    transformedXMlStream.Dispose();
                    return new OkObjectResult(xmlString);
                }

                else
                {
                    string responseMessage = "Tranformation failed: Please add Content-Type header values supported are application/json or application/xml";
                    throw new System.Exception(responseMessage);

                }

            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }

        }

    }
}
