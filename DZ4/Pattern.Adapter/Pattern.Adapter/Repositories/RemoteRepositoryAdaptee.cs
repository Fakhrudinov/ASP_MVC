using Pattern.Adapter.Model;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pattern.Adapter.Repositories
{
    class RemoteRepositoryAdaptee
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<RemoteResponceObject> GetHttpRemoteRequest(string url, CancellationTokenSource cts)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url, cts.Token);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                RemoteResponceObject result = JsonSerializer.Deserialize<RemoteResponceObject>(responseBody);

                Console.WriteLine($"Https Task with url {url} completed.");
                return result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }

        public async Task<RemoteResponceObject> ReadTextRemoteAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);

                RemoteResponceObject result = JsonSerializer.Deserialize<RemoteResponceObject>(json);
                Console.WriteLine($"Task ReadTextRemoteAsync from {filePath} completed.");

                // проверка успешности сериализации
                if (result.info == null || result.productName == null)
                {
                    return null;
                }

                return result;
            }
            else
            {
                return null;
            }                
        }

        public Task<RemoteResponceObject> ReadTextXMLRemoteAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RemoteResponceObject));

                FileStream fs = new FileStream(filePath, FileMode.Open);
                StreamReader reader = new StreamReader(fs);
                RemoteResponceObject result = (RemoteResponceObject)serializer.Deserialize(reader);                

                Console.WriteLine($"Task ReadTextXMLRemoteAsync from {filePath} completed.");

                // проверка успешности сериализации
                if (result.info == null || result.productName == null)
                {
                    return null;
                }

                return Task.FromResult(result);
            }
            else
            {
                return null;
            }
        }
    }
}
