using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationCommon.Models
{
    [DynamoDBTable("Users")]
    public class User
    {
        public User()
        {
            UserId = Guid.NewGuid().ToString();
        }

        [DynamoDBHashKey]
        public string Email { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }
    }
}
