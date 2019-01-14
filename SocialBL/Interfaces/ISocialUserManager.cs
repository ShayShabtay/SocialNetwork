using SocialCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Interfaces
{
    public interface ISocialUserManager
    {
        void AddUser(User user);
        void Follow(string SourceUserId, string targetUserId);
        void BlockUser(string SourceUserId,string targetUserId);
        string ValidateToken(string token);
    }
}
