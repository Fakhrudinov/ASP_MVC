using Pattern.Adapter.Model;
using Pattern.Adapter.Repositories.Interfaces;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Pattern.Adapter.Repositories
{
    public class DirectRepository : IDirectRepository
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<ResponceObject> ReadTextDirectlyAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);

                ResponceObject result = JsonSerializer.Deserialize<ResponceObject>(json);
                result.source = "File direct model";

                Console.WriteLine($"Task ReadTextDirectlyAsync from {filePath} completed.");
                return result;
            }
            else
                return null;
        }

        public async Task<ResponceObject> GetHttpDirectRequest(string url, CancellationTokenSource cts)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url, cts.Token);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                ResponceObject result = JsonSerializer.Deserialize<ResponceObject>(responseBody);

                Console.WriteLine($"Https Task with url {url} completed.");
                result.source = "Http direct model";
                return result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }
    }
}
