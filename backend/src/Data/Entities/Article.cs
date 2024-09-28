using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class Article
    {
        public Article()
        {
            article_tags = new List<ArticleTag>();
            article_comments = new List<ArticleComment>();
            article_likes = new List<ArticleLike>();

        }

        public int id { get; set; }
        public string slug { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string image { get; set; }
        public int user_id { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        public User user { get; set; }
        public List<ArticleTag> article_tags { get; set; }
        public List<ArticleComment> article_comments { get; set; }
        public List<ArticleLike> article_likes { get; set; }

    }
}