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
        void UnFollow(string SourceUserId, string targetUserId);
        void BlockUser(string SourceUserId,string targetUserId);
        void UnBlockUser(string SourceUserId, string targetUserId);
        List<User> GetAllUsers(string userId);
        List<User> GetFollowers(string userId);
        List<User> GetFollowing(string userId);
        List<User> GetBlockedUsers(string userId);
        string ValidateToken(string token);
    }
}
