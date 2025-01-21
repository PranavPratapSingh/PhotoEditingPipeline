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
using System.Text;

namespace ppsEditor
{
    public class Function3
    {
        private readonly ILogger _logger;

        public Function3(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function3>();
        }

        static string GetValue(PropertyItem property)
        {
            // Decode specific metadata properties based on ID
            if (property.Type == 2) // ASCII
            {
                return System.Text.Encoding.ASCII.GetString(property.Value).Trim('\0');
            }
            else if (property.Type == 5) // Rational (e.g., GPS coordinates)
            {
                return BitConverter.ToUInt32(property.Value, 0).ToString();
            }
            return BitConverter.ToString(property.Value);
        }

        static int ReadBigEndianInt(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(4);
            if (bytes.Length < 4) throw new EndOfStreamException("Unexpected end of stream.");
            return (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
        }


        [Function("Function3")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string body = new StreamReader(req.Body).ReadToEnd();
            dynamic jsonParse = JsonConvert.DeserializeObject(body);

            String base64Image = jsonParse.image;

            String resp = "";

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            byte[] imageBytes = Convert.FromBase64String(base64Image);

            // Load the image from the byte array
            using (MemoryStream ms = new MemoryStream(imageBytes))
            using (Image image = Image.FromStream(ms))
            {
                PropertyItem[] properties = image.PropertyItems;

                foreach (var property in properties)
                {
                    resp += $"Property ID: {property.Id}, Type: {property.Type}, Value: {GetValue(property)}";
                }
            }

            if (resp == "")
            {
                resp = "No Metadata";
            }

            response.WriteString(resp);

            return response;
        }
    }
}
