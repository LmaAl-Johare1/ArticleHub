using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class User
    {
        public User()
        {
            user_followers = new List<UserFollower>();
            user_followings = new List<UserFollower>();
            user_articles = new List<Article>();
            user_comments = new List<ArticleComment>();
            user_likes = new List<ArticleLike>();
        }

        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string bio { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        public List<UserFollower> user_followers { get; set; }
        public List<UserFollower> user_followings { get; set; }
        public List<Article> user_articles { get; set; }
        public List<ArticleComment> user_comments { get; set; }
        public List<ArticleLike> user_likes { get; set; }

    }
}