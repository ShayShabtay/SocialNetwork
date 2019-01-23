using Amazon.Runtime;
using SocialBL.Interfaces;
using SocialCommon.Models;
using SocialRepository.GraphDB;
using SocialRepository.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialBL.Managers
{
    public class SocialPostManager : ISocialPostManager
    {

        IGraphDB _graphDB;
        const string bucketName = "omer-buckets";
        
        //Ctor
        public SocialPostManager()
        {
            _graphDB = new Neo4jDB();
        }

        //Public Methods
        public void AddPost(Post post, string userId)
        {
            _graphDB.AddPost(post);
            Thread.Sleep(2000);
            _graphDB.CreateRelationship(userId, post.PostID, "Publish");
        }

        public void AddComment(Comment comment,string userId,string postId)
        {
            _graphDB.AddComment(comment);
            _graphDB.CreateRelationship(userId,comment.CommentID, "UserComment");  ///for connect post and comment
            _graphDB.CreateRelationship(postId,comment.CommentID,"PostComment");  ///for connect user to comment that he wrote
        }

        public void AddLikeToPost(string userId, string postId)
        {
                _graphDB.CreateRelationship(userId, postId, "LikePost");  
        }

        public void UnLikePost(string userId, string postId)
        {
            _graphDB.DeleteRelationship(userId, postId, "LikePost");
        }

        public void AddLikeToComment(string userId, string commentId)
        {
            _graphDB.CreateRelationship(userId, commentId, "LikeComment");

        }

        public void UnLikeComment(string userId, string commentId)
        {
            _graphDB.DeleteRelationship(userId, commentId, "LikeComment");
        }

        public List<ClientPost> GetAllPosts(string userId)
        {
            List<Post> posts = _graphDB.GetAllPosts(userId);
            List<ClientPost> postsToClient = new List<ClientPost>();

            foreach (var post in posts)
            {
                ClientPost clientPost = new ClientPost(post);
                clientPost.Comments = _graphDB.GetCommentsForPost(post.PostID);
                clientPost.PostOwner = _graphDB.GetPostOwner(post.PostID);
                clientPost.IsLike = _graphDB.IsUserLikePost(userId, post.PostID);
                //foreach (var comment in clientPost.Comments)
                //{
                //    comment.UsersLike = _graphDB.GetLikesForComment(comment.CommentID);
                //}
                List<User> usersLike = _graphDB.GetLikesForPost(post.PostID);
                clientPost.UsersLikes = usersLike;
                clientPost.LikeCount = usersLike.Count;
                postsToClient.Add(clientPost);
            }
            return postsToClient;

        }

        public List<ClientPost> GetMyPosts(string userId)
        {
            List<Post> posts=_graphDB.GetMyPosts(userId);
            List<ClientPost> postsToClient = new List<ClientPost>();

            foreach (var post in posts)
            {
                ClientPost clientPost = new ClientPost(post);
                clientPost.Comments = _graphDB.GetCommentsForPost(post.PostID);
                clientPost.PostOwner = _graphDB.GetPostOwner(post.PostID);
                //foreach (var comment in clientPost.Comments)
                //{
                //    comment.UsersLike = _graphDB.GetLikesForComment(comment.CommentID);
                //}
                List<User> usersLike=_graphDB.GetLikesForPost(post.PostID);
                clientPost.UsersLikes = usersLike;
                clientPost.LikeCount = usersLike.Count;
                postsToClient.Add(clientPost);
            }
            return postsToClient; 
        }

        public string SaveImage(Stream image,string userId)
        {
            StorgeHelper storgeHelper = new StorgeHelper();

            string imageKey= Guid.NewGuid().ToString();
            try
            {
            string imageUrl=storgeHelper.uploadImageToS3(image,userId,imageKey);
            return imageUrl;
            }
            catch (Exception)
            {

            return null;
            
            }
        }

        public string ValidateToken(string token)
        {
            string url = "http://localhost:49884/api/token/validateManualToken";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-token", token);
                var res = client.GetAsync(url).Result;

                if (res.IsSuccessStatusCode)
                {
                    string userId = res.Content.ReadAsStringAsync().Result;
                    userId = userId.Replace("\"", "");

                    return userId;
                }
            }
            return null;
        }

        public void GetTemporaryToken()
        {
            TemporaryS3Token.ListObjectsAsync().Wait();
        }
    }
}
