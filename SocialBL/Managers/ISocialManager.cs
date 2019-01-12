using SocialCommon.Models;

namespace SocialBL.Managers
{
    public interface ISocialManager
    {
        void addUser(User user);
        void addPost(Post post, User user);
    }
}