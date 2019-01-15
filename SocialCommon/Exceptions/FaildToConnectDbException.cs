using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialCommon.Exceptions
{
    public class FaildToConnectDbException : Exception
    {
        public FaildToConnectDbException()
        {

        }

        public FaildToConnectDbException(string message)
            : base(message)
        {

        }
    }
}
