using SocialCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Interfaces
{
    public interface ISocialPostManager
    {
        void AddPost(Post post, string userId);
        void AddComment(Comment comment, string userId, string postId);
        void AddLike(string userId, string postId);
        List<ClientPost> GetAllPosts(string userId);
        List<ClientPost> GetMyPosts(string userId);
        string SaveImage(byte[] image, string userId);
        string ValidateToken(string token);
        void UnLike(string userId, string postId);
        string GetUserByPostID(string postId);
    }
}
