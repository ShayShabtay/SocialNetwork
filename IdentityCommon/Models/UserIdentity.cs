using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCommon.Models
{
    [DynamoDBTable("UsersIdentity")]
    public class UserIdentity
    {
        [DynamoDBHashKey]
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string WorkPlace { get; set; }
    }
}
