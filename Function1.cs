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
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }


        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string body = new StreamReader(req.Body).ReadToEnd();
            dynamic jsonParse = JsonConvert.DeserializeObject(body);

            String base64Image = jsonParse.image;
            int width = Convert.ToInt32(jsonParse.width);
            int height = Convert.ToInt32(jsonParse.height);

            String resp = "Unable to Parse";

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            byte[] imageBytes = Convert.FromBase64String(base64Image);

            // Load the image from the byte array
            using (MemoryStream ms = new MemoryStream(imageBytes))
            using (Image originalImage = Image.FromStream(ms))
            {
                // Create a new bitmap with the desired dimensions
                Bitmap resizedImage = new Bitmap(width, height);

                // Draw the original image onto the new bitmap with the new dimensions
                using (Graphics graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    graphics.DrawImage(originalImage, 0, 0, width, height);
                }

                // Convert the resized image back to a byte array
                using (MemoryStream outputMs = new MemoryStream())
                {
                    resizedImage.Save(outputMs, ImageFormat.Png); // Save in PNG format (you can change this)
                    byte[] resizedImageBytes = outputMs.ToArray();

                    // Encode the byte array to a base64 string
                    resp = Convert.ToBase64String(resizedImageBytes);
                }
            }

            response.WriteString(resp);

            return response;
        }
    }
}
