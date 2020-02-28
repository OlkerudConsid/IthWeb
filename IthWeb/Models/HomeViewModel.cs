using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IthWeb.Models
{
    public class HomeViewModel
    {
        public List<BlogPost> BlogPosts { get; set; }
        public BlogPostInputModel InputModel { get; set; }
    }

    public class BlogPostInputModel
    {
        public int Id { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
