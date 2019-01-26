using Amazon.Runtime;
using SocialCommon.Models;
using SocialCommon.ModelsDTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Interfaces
{
    public interface ISocialPostManager
    {
        void AddPost(PostDTO post, string userId);
        void AddComment(CommentDTO comment, string userId, string postId);
        void AddLikeToPost(string userId, string postId);
        void UnLikePost(string userId, string postId);
        void AddLikeToComment(string userId, string commentId);
        void UnLikeComment(string userId, string commentId);
        List<ClientPost> GetAllPosts(string userId);
        List<ClientPost> GetMyPosts(string userId);

        string SaveImage(Stream image, string userId);
        string ValidateToken(string token);
        void GetTemporaryToken();
        string GetUserByPostID(string postId);
    }
}
