using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class ArticleForCreationDto
    {
        public string title { get; set; }
        public string body { get; set; }
        public IFormFile image { get; set; }
        public List<string> tags { get; set; }
    }
}