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
using Microsoft.Extensions.Configuration;
using IthWeb.DTOs;

namespace IthWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogService _blogService;

        // Here we use Dependency Injection to get a blogservice
        public HomeController(
            IBlogService blogService)
        {
            _blogService = blogService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var posts = await _blogService.GetAllPosts();

            var vm = new HomeViewModel()
            {
                BlogPosts = posts.ToList()
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index([Bind("Title", "Text", "ImageFile")] BlogPostInputModel inputModel)
        {
            inputModel.Author = User.Identity.Name;

            var isCreateSuccessful = await _blogService.CreatePost(inputModel);

            if (!isCreateSuccessful)
            {
                TempData["PostError"] = "Something went wrong, try again or contact support!";
            }

            return RedirectToAction();
        }

        [HttpGet("{id}")]
        [Route("/Home/ViewPost", Name = "ViewPost")]
        public async Task<IActionResult> ViewPost(int id)
        {
            var post = await _blogService.GetPostById(id);

            if (post != null)
            {
                return View(post);
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("{id}")]
        [Route("/Home/EditPost", Name = "EditPost")]
        public async Task<IActionResult> EditPost(int id)
        {
            var post = await _blogService.GetPostById(id);

            if (post == null)
            {
                return NotFound();
            }

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

        [Authorize]
        [HttpPost]
        [Route("/Home/EditPost", Name = "EditPost")]
        public async Task<IActionResult> EditPost([Bind("Id", "Title", "ImageFile", "ImageUrl", "Text", "PublishedDate", "Author")] BlogPostInputModel inputModel)
        {
            var isUpdateSuccessful = await _blogService.UpdatePost(inputModel);

            if (isUpdateSuccessful)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Error");
        }

        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            var isDeleteSuccessful = await _blogService.DeletePost(id);

            if (isDeleteSuccessful)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Error");
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
