using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserProfileDto
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string bio { get; set; }
        public string followers_count { get; set; }
        public string followings_count { get; set; }
        public string articles_count { get; set; }
        public bool is_following { get; set; }
        public List<ArticleCardDto> articles { get; set; }
    }
}