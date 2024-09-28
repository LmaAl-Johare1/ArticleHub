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
        public IFormFile image { get; set; }
        public string user_first_name { get; set; }
        public string user_last_name { get; set; }
        public DateTime created { get; set; }
        public List<string> tags { get; set; }
        public int likes_count { get; set; }
        public List<ArticleCommentDto> comments { get; set; }
    }
}

//the favarate article 
//tags no need we can store them in the front end 
//articles
//the home api is the same as the search api 
//the parameter for the article as a variables