using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCommon.Execeptions
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException()
        {

        }

        public AlreadyExistException(string field)
            : base(String.Format($"{field} is already exists"))
        {

        }
    }
}
