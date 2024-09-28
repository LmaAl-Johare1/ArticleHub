using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class ArticleTag
    {
        public int tag_id { get; set; }
        public int article_id { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        public Article article { get; set; }
        public Tag tag { get; set; }
    }
}
