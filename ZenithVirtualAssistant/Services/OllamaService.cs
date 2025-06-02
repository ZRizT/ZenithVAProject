using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
namespace ZenithVirtualAssistant.Services
{
    public class OllamaService
    {
        private readonly HttpClient _client;
        private readonly string _endpoint;
        private readonly string _model;

        public OllamaService(string configPath)
        {
            _client = new HttpClient();
            var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath));
            _endpoint = config.OllamaEndpoint;
            _model = config.ModelName;
        }

        public async Task<string> ProcessCommandAsync(string command)
        {
            var requestBody = new
            {
                model = _model,
                prompt = command,
                stream = false
            };
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(_endpoint, content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OllamaResponse>(responseBody);
                return result.Response;
            }
            catch (Exception ex)
            {
                return $"Error communicating with Ollama: {ex.Message}";
            }
        }

        private class Config
        {
            public string OllamaEndpoint { get; set; }
            public string ModelName { get; set; }
        }

        private class OllamaResponse
        {
            public string Response { get; set; }
        }
    }
}