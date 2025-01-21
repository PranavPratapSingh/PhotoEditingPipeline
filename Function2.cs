using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Core;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System;

namespace ppsEditor
{
    public class Function2
    {
        private readonly ILogger _logger;

        public Function2(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function2>();
        }


        [Function("Function2")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string body = new StreamReader(req.Body).ReadToEnd();
            dynamic jsonParse = JsonConvert.DeserializeObject(body);

            String base64Image = jsonParse.image;

            String resp = "Unable to Parse";

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            byte[] imageBytes = Convert.FromBase64String(base64Image);

            // Load the image from the byte array
            using (var pngStream = new MemoryStream(imageBytes))
            using (var image = Image.FromStream(pngStream))
            using (var jpgStream = new MemoryStream())
            {
                // Save the image as a JPEG into the MemoryStream
                image.Save(jpgStream, ImageFormat.Jpeg);

                // Get the byte array of the JPEG
                byte[] jpgBytes = jpgStream.ToArray();

                // Encode the JPEG byte array into a Base64 string
                resp = Convert.ToBase64String(jpgBytes);
            }

            response.WriteString(resp);

            return response;
        }
    }
}
