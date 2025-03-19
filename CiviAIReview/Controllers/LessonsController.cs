using System.Text;
using System.Text.Json;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.AspNetCore.Mvc;
namespace CiviAIReview.Controllers
{
    public class LessonsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProcessPDF()
        {
            return View();
        }
        public IActionResult ProcessLessons()
        {
            return View();
        }

        [HttpPost]
        [Route("Lessons/ExtractPdfContent")]
        public async Task<IActionResult> ExtractPdfContent(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "No file uploaded." });
            }

            try
            {
                string extractedText = "";
                using (var stream = file.OpenReadStream())
                {
                    PdfReader pdfReader = new PdfReader(stream);
                    PdfDocument pdfDoc = new PdfDocument(pdfReader);

                    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                    {
                        extractedText += PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));
                    }

                    pdfDoc.Close();
                }

                return Json(new { success = true, content = extractedText });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("Lessons/AnalyzeWithChatGPT")]
        private async Task<string> AnalyzeWithChatGPT(string content)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer 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");

            var data = new
            {
                model = "gpt-4",
                messages = new[]
                {
            new { role = "system", content = "You are a helpful assistant." },
            new { role = "user", content = $"Analyze this document:\n{content}" }
        }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }


    }
}
