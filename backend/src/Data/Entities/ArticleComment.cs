using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class ArticleComment
    {
        public int id { get; set; }
        public string body { get; set; }
        public int user_id { get; set; }
        public int article_id { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        
        public Article article { get; set; }
        public User user { get; set; }
    }
}