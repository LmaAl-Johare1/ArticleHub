using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserDto
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string bio { get; set; }
        public string followers_count { get; set; }
        public string followings_count { get; set; }
        public string articles_count { get; set; }
        public List<ArticleDto> articles { get; set; }
    }
}