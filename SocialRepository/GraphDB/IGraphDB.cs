using SocialCommon.Models;

namespace SocialRepository.GraphDB
{
    public interface IGraphDB
    {

        void addUser(User user);
        void addPost(Post post,User user);
        void creatConection(string source, string target, string type);
    }
}