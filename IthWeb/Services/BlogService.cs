using IthWeb.DTOs;
using IthWeb.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IthWeb.Services
{
    public class BlogService : IBlogService
    {
        private readonly ILogger<BlogService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IImageFileService _imageFileService;
        private readonly string apiRootUrl;

        public BlogService(ILogger<BlogService> logger, IHttpClientFactory clientFactory, IConfiguration config, IImageFileService imageFileService)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _config = config;
            _imageFileService = imageFileService;

            apiRootUrl = _config.GetValue(typeof(string), "BlogApiRoot").ToString();
        }

        /// <summary>
        /// Gets all blogposts asynchronously.
        /// </summary>
        /// <returns>A collection of BlogPosts.</returns>
        public async Task<IEnumerable<BlogPostDTO>> GetAllPosts()
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiRootUrl}BlogPosts");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "IthWeb");

            // Send the request and await the response from the API
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Read the content of the API response and parse it to our model
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var posts = await JsonSerializer.DeserializeAsync<List<BlogPostDTO>>(responseStream,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                var orderedPosts = posts.OrderBy(x => x.PublishedDate);

                return orderedPosts;
            }
            else
            {
                _logger.LogError("GetAllPosts failed. Response: {response}", response);
                return new List<BlogPostDTO>();
            }
        }

        /// <summary>
        /// Gets a single BlogPost asynchronously by id.
        /// </summary>
        /// <param name="id">The id of the post we want to get.</param>
        /// <returns>A BlogPost or null if unsuccessful.</returns>
        public async Task<BlogPostDTO> GetPostById(int id)
        {
            // Get the specific post (specified by id)
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiRootUrl}BlogPosts/{id}");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "IthWeb");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var post = await JsonSerializer.DeserializeAsync<BlogPostDTO>(responseStream,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                return post;
            }
            else
            {
                _logger.LogError("Could not get post with id: {id}. Response: {response}", id, response);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPost"></param>
        /// <returns></returns>
        public async Task<bool> CreatePost(BlogPostInputModel newPost)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiRootUrl}BlogPosts/");

            var blogPostDTO = new BlogPostDTO()
            {
                Author = newPost.Author,
                Title = newPost.Title,
                Text = newPost.Text,
                PublishedDate = DateTime.Now
            };

            if (newPost.ImageFile != null)
            {
                newPost.ImageUrl = await _imageFileService.SaveImage(newPost.ImageFile);
            }

            // Serialize the model and set it as the content of our request
            var postJson = JsonSerializer.Serialize(newPost);

            request.Headers.Add("User-Agent", "IthWeb");
            request.Content = new StringContent(postJson, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedPost"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePost(BlogPostInputModel updatedPost)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"{apiRootUrl}BlogPosts/{updatedPost.Id}");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "IthWeb");

            var editedPost = new BlogPostDTO()
            {
                Id = updatedPost.Id,
                Author = updatedPost.Author,
                PublishedDate = updatedPost.PublishedDate,
                Title = updatedPost.Title,
                Text = updatedPost.Text,
                ImageUrl = updatedPost.ImageUrl
            };

            if (updatedPost.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(updatedPost.ImageUrl))
                {
                    var imageFileName = updatedPost.ImageUrl.Split('/').LastOrDefault();
                    await _imageFileService.DeleteImage(imageFileName);
                }

                editedPost.ImageUrl = await _imageFileService.SaveImage(updatedPost.ImageFile);
            }

            var postJson = JsonSerializer.Serialize(editedPost);
            request.Content = new StringContent(postJson, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeletePost(int id)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiRootUrl}BlogPosts/{id}");
            request.Headers.Add("User-Agent", "IthWeb");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var post = await JsonSerializer.DeserializeAsync<BlogPostDTO>(responseStream,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                if (!string.IsNullOrEmpty(post.ImageUrl))
                {
                    var imageFileName = post.ImageUrl.Split('/').LastOrDefault();

                    await _imageFileService.DeleteImage(imageFileName);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
