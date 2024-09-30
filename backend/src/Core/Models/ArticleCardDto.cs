using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class ArticleCardDto
    {
        public string title { get; set; }
        public string body { get; set; }
        public IFormFile image { get; set; }
        public int likes_count { get; set; }
    }
}