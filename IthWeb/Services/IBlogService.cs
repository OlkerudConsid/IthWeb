using IthWeb.DTOs;
using IthWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IthWeb.Services
{
    public interface IBlogService
    {
        public Task<IEnumerable<BlogPostDTO>> GetAllPosts();
        public Task<BlogPostDTO> GetPostById(int id);
        public Task<bool> CreatePost(BlogPostInputModel newPost);
        public Task<bool> UpdatePost(BlogPostInputModel updatedPost);
        public Task<bool> DeletePost(int id);
    }
}
