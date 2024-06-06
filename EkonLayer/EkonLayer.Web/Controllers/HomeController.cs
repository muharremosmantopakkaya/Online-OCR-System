using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EkonLayer.Web.Models;

namespace EkonLayer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new OcrResponse());
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, string language = "tr")
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Dosya bulunamadı");
                return View("Index", new OcrResponse());
            }

            try
            {
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploadsFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                using (var content = new MultipartFormDataContent())
                {
                    var streamContent = new StreamContent(new FileStream(filePath, FileMode.Open));
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    content.Add(streamContent, "file", fileName);
                    content.Add(new StringContent(language), "language");

                    var client = _httpClientFactory.CreateClient();
                    var response = await client.PostAsync("http://localhost:5000/upload", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var ocrResponse = JsonConvert.DeserializeObject<OcrResponse>(responseContent);

                        return View("Index", ocrResponse);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError("File", $"OCR işleminde hata: {errorContent}");
                        return View("Index", new OcrResponse());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hata: {ex.Message}");
                ModelState.AddModelError("File", $"Bir hata oluştu: {ex.Message}");
                return View("Index", new OcrResponse());
            }
        }
    }
}
