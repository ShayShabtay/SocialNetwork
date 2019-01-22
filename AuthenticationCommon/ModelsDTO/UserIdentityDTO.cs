using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationCommon.ModelsDTO
{
    public class UserIdentityDTO
    {
        public string UserId { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string WorkPlace { get; set; }
    }
}
