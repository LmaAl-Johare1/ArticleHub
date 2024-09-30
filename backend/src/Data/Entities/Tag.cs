using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class Tag
    {
        public Tag()
        {
            articles = new List<ArticleTag>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        public List<ArticleTag> articles { get; set; }
    }
}
