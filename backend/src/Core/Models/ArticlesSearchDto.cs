using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class ArticlesSearchDto
    {
        public int Offset { get; set; } = 0;
        public string keyword { get; set; }
        public string tag { get; set; }
    }
}