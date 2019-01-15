using SocialCommon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialRepository.GraphDB
{
    public interface IGraphDB
    {

        void addUser(User user);
        void addPost(Post post);
        void creatConection(string source, string target, string type);
        void DeleteConection(string source, string target, string type);
        void Follow(string SourceUserId, string targetUserId);
        List<Post> getAllPosts(string userId);
        List<Post> getMyPosts(string userId);
        Task<bool> addComment(Comment comment);
        List<Comment> getCommentsForPost(string postID);
        List<User> getLikesForPost(string postID);
    }
}