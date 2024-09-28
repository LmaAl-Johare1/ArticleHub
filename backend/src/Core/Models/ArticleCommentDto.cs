using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class ArticleCommentDto
    {
        public string body { get; set; }
        public string user_first_name { get; set; }
        public string user_last_name { get; set; }
        public DateTime created { get; set; }
    }
}
