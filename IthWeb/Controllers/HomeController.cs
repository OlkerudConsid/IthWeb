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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using IthWeb.Services;

namespace IthWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IImageFileService _imageFileService;

        // Here we use Dependency Injection to get a logger and a clientfactory
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory, IImageFileService imageFileService)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _imageFileService = imageFileService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44330/api/BlogPosts");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "IthWeb");

            // Send the request and await the response from the API
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Read the content of the API response and parse it to our model
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var posts = await JsonSerializer.DeserializeAsync<List<BlogPost>>(responseStream,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                // Create an instance of our ViewModel and populate it with the data parsed from the response
                var vm = new HomeViewModel();
                vm.BlogPosts = posts.OrderBy(x => x.PublishedDate).ToList();

                return View(vm);
            }

            // If "response.IsSuccessStatusCode == false" then we just return the view with an empty ViewModel
            return View(new HomeViewModel());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index([Bind("Title", "Text", "ImageFile")] BlogPostInputModel inputModel)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44330/api/BlogPosts/");

            var newPost = new BlogPost()
            {
                Author = User.Identity.Name,
                Title = inputModel.Title,
                Text = inputModel.Text,
                PublishedDate = DateTime.Now
            };

            if (inputModel.ImageFile != null)
            {
                newPost.ImageUrl = await _imageFileService.SaveImage(inputModel.ImageFile);
            }

            // Serialize the model and set it as the content of our request
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

        [Authorize]
        [HttpGet("{id}")]
        [Route("/Home/EditPost", Name = "EditPost")]
        public async Task<IActionResult> EditPost(int id)
        {
            // Get the specific post (specified by id) that we want to edit
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44330/api/BlogPosts/{id}");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "IthWeb");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var post = await JsonSerializer.DeserializeAsync<BlogPost>(responseStream,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                var vm = new BlogPostInputModel()
                {
                    Id = post.Id,
                    Author = post.Author,
                    PublishedDate = post.PublishedDate,
                    Title = post.Title,
                    Text = post.Text,
                    ImageUrl = post.ImageUrl
                };

                return View(vm);
            }

            return Error();
        }

        [Authorize]
        [HttpPost]
        [Route("/Home/EditPost", Name = "EditPost")]
        public async Task<IActionResult> EditPost([Bind("Id", "Title", "ImageFile", "ImageUrl", "Text", "PublishedDate", "Author")] BlogPostInputModel inputModel)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:44330/api/BlogPosts/{inputModel.Id}");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "IthWeb");

            var editedPost = new BlogPost()
            {
                Id = inputModel.Id,
                Author = inputModel.Author,
                PublishedDate = inputModel.PublishedDate,
                Title = inputModel.Title,
                Text = inputModel.Text,
                ImageUrl = inputModel.ImageUrl
            };

            if (inputModel.ImageFile != null)
            {
                editedPost.ImageUrl = await _imageFileService.SaveImage(inputModel.ImageFile);
            }

            var postJson = JsonSerializer.Serialize(editedPost);
            request.Content = new StringContent(postJson, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return Error();
        }

        [Authorize]
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
