using SocialCommon.Models;

namespace SocialBL.Managers
{
    public interface ISocialManager
    {
        void AddUser(User user);
        void Follow(string SourceUserId, string targetUserId);
        string ValidateToken(string token);
    }
}