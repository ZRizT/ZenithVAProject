using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZenithVirtualAssistant.Services
{
    public class OllamaService
    {
        private readonly HttpClient _client;

        public OllamaService()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:11434") };
        }

        public async Task<string> ProcessCommand(string command)
        {
            try
            {
                var requestBody = new { model = "llama3", prompt = command };
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("/api/generate", content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
                return result.response ?? "Sorry, I couldn't process that command.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}