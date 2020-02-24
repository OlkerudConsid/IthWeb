using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IthWeb.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace IthWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44330/api/BlogPosts");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "IthWeb");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var posts = await JsonSerializer.DeserializeAsync<List<BlogPost>>(responseStream,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                return View(posts);
            }

            return View(new List<BlogPost>());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string Title, string Text)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44330/api/BlogPosts/");
            var newPost = new BlogPost()
            {
                Author = User.Identity.Name,
                Title = Title,
                Text = Text,
                PublishedDate = DateTime.Now
            };
            var postJson = JsonSerializer.Serialize(newPost);

            request.Headers.Add("User-Agent", "IthWeb");
            request.Content = new StringContent(postJson, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                TempData["PostError"] = "Something went wrong, try again or contact support!";
            }

            return RedirectToAction();
        }

        public async Task<IActionResult> DeletePost(int id)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:44330/api/BlogPosts/{id}");
            request.Headers.Add("User-Agent", "IthWeb");

            var response = await client.SendAsync(request);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
