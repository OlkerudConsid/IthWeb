﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IthWebAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Text { get; set; }
        public int BlogPostId { get; set; }
    }
}
