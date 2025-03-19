using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CiviAIReview.Service
{
    public class OpenAiSvc
    {
        private readonly string _apiKey;
        public OpenAiSvc(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"];
        }

        public async Task<string> AnalyzeWithChatGPT(string content)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var data = new
            {
                model = "gpt-4",
                messages = new[]
                {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = $"Analyze this document and create question and answer:\n{content}" }
            }
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                return $"Error: {response.StatusCode}";
            }

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

    }
}
