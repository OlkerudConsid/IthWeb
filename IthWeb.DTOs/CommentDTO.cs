using System;
using System.Collections.Generic;
using System.Text;

namespace IthWeb.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Text { get; set; }
        public int BlogPostId { get; set; }
    }
}
