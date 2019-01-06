using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationCommon.Execeptions
{
    public class NotMatchException : Exception
    {
        public NotMatchException()
        {

        }

        public NotMatchException(string field)
            : base(String.Format($"Invalid {field}"))
        {

        }
    }
}
