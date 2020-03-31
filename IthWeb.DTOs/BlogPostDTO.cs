using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IthWeb.DTOs
{
    public class BlogPostDTO
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<CommentDTO> Comments {get;set;}
    }
}
