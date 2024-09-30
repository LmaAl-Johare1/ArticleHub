using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class UserFollower
    {
        public int User_follower_id { get; set; }
        public int user_followeing_id { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        public User follower { get; set; }
        public User followeing { get; set; }
    }
}
