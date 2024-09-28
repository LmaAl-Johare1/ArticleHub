using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserForUpdateDto
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string bio { get; set; }
    }
}
