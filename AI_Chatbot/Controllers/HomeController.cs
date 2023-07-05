
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace ChatBotMvc.Controllers
{
    public class HomeController : Controller
    {
        const string API_KEY = "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";
        private readonly ILogger<HomeController> _logger;
        static readonly HttpClient client = new HttpClient();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Create()
        {

            return View();
        }

        public async Task<IActionResult> Get(string input, string context)
        {
            var options = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new
            {
                role = "system",
                content = context
            },
            new
            {
                role = "user",
                content = input
            }
        },
                max_tokens = 3500,
                temperature = 0.2
            };

        
        


        var json = System.Text.Json.JsonSerializer.Serialize(options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", API_KEY);

           
                try
                {
                    var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();

                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);
                    string result = jsonResponse.choices[0].message.content;

                    ViewBag.Response = result; // Store the response in ViewBag

                    return PartialView("_ChatLog"); // Return the partial view
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            return View();
        }

        }
    }