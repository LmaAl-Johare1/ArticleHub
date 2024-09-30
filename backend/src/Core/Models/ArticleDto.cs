using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class ArticleDto
    {
        public string title { get; set; }
        public string body { get; set; }
        public string image { get; set; }
        public string user_first_name { get; set; }
        public string user_last_name { get; set; }
        public DateTime created { get; set; }
        public List<string> tags { get; set; }
        public int likes_count { get; set; }
        public List<ArticleCommentDto> comments { get; set; }
    }
}
