using SocialCommon.Models;
using SocialRepository.GraphDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialBL.Managers
{
    public class SocialPostManager
    {

        IGraphDB _graphDB;
        public SocialPostManager()
        {
            _graphDB = new neo4jDB();
        }

        public void AddPost(Post post, string userId)
        {

            _graphDB.addPost(post);
            Thread.Sleep(2000);
            _graphDB.creatConection(userId, post.postID, "publish");
        }

        public List<Post> getAllPosts(string userId)
        {
            return _graphDB.getAllPosts(userId);   

        }

        public List<Post> getMyPosts(string userId)
        {
           return _graphDB.getMyPosts(userId);
        }

        public void addComment(Comment comment,string userId,string postId)
        {
            var x=_graphDB.AddComment(comment);
            if (x.Result)
            {
            _graphDB.creatConection(userId,comment.CommentID, "UserComment");  ///for connect post and comment
            _graphDB.creatConection(postId,comment.CommentID,"PostComment");  ///for connect user to comment that he wrote
            }
        }

        public string ValidateToken(string token)
        {
            string url = "http://localhost:49884/api/token/validateManualToken";
            string userId = null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-token", token);
                var task = client.GetAsync(url);
                task.Wait();
                if (task.Result.IsSuccessStatusCode)
                {
                    userId = task.Result.ToString();
                }
            }
            return userId;
        }
    }
}
