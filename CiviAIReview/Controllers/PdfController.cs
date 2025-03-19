using System.Text;
using CiviAIReview.Service;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CiviAIReview.Controllers
{
    public class PdfController : Controller
    {
        private readonly OpenAiSvc _openAiSvc;
        private readonly IConfiguration _configuration;
        public PdfController(OpenAiSvc openAiSvc, IConfiguration configuration)
        {
            _openAiSvc = openAiSvc;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Pdf/UploadPdfAndGenerateQA")]
        public async Task<IActionResult> UploadPdfAndGenerateQA(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                return Json(new { success = false, message = "No file uploaded!" });
            }

            try
            {
                // Extract text from PDF using iText7
                string pdfContent = string.Empty;

                using (var stream = pdfFile.OpenReadStream())
                using (var pdfReader = new PdfReader(stream))
                using (var pdfDoc = new PdfDocument(pdfReader))
                {
                    int pageCount = pdfDoc.GetNumberOfPages();
                    StringWriter writer = new StringWriter();

                    for (int i = 1; i <= pageCount; i++)
                    {
                        string pageContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));
                        writer.WriteLine(pageContent);
                    }

                    pdfContent = writer.ToString();
                }

                // Call ChatGPT API to generate questions and answers
                var chatGptResponse = await CallChatGPTForQA(pdfContent);

                return Json(new { success = true, content = chatGptResponse });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error processing file: " + ex.Message });
            }
        }

        private async Task<string> CallChatGPTForQA(string content)
        {
            try
            {
                var apiKey = _configuration["OpenAI:ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                    throw new Exception("API key is missing!");

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                var requestBody = new
                {
                    model = "gpt-4", 
                    messages = new[]
                    {
                new { role = "system", content = "You're an AI that generates questions and answers based on the provided content." },
                new { role = "user", content = $"Extract key points and generate relevant questions and answers from this content: {content}" }
            },
                    temperature = 0.7,
                    max_tokens = 1000
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                // Call the API
                var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Handle API errors
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"API Error: {response.StatusCode} - {responseContent}");
                }

                // Parse the response
                dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);
                return parsedResponse.choices[0].message.content.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ChatGPT API error: " + ex.Message);
                return $"AI failed to generate questions. Error: {ex.Message}";
            }
        }



    }
}
