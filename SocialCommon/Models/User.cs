using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialCommon.Models
{
    public class User
    {
        public string userID{get;set;}
        public string Name { get; set; }
        public User(string userId,string userName)
        {
            this.userID = userId;
            this.Name = userName;
        }

        public User()
        {
                
        }
    }
}
