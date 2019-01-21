using SocialCommon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialRepository.GraphDB
{
    public interface IGraphDB
    {
        void CreateRelationship(string source, string target, string type);
        void DeleteRelationship(string source, string target, string type);

        void AddUser(User user);
        List<User> GetAllUsers(string userId);
        List<User> GetFollowers(string userId);
        List<User> GetFollowing(string userId);
        List<User> GetBlockedUsers(string userId);

        void AddPost(Post post);
        List<Post> GetAllPosts(string userId);
        List<Post> GetMyPosts(string userId);
        void AddComment(Comment comment);
        List<Comment> GetCommentsForPost(string postID);
        List<User> GetLikesForPost(string postID);
        string getUserByPostId(string postId);
    }
}