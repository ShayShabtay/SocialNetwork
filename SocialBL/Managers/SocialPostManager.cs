using SocialCommon.Models;
using SocialRepository.GraphDB;
using SocialRepository.Storage;
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
        const string bucketName = "omer-buckets";
        
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

        public List<ClientPost> getAllPosts(string userId)
        {
           // return 

            List<Post> posts = _graphDB.getAllPosts(userId);
            List<ClientPost> postsToClient = new List<ClientPost>();

            foreach (var post in posts)
            {
                ClientPost clientPost = new ClientPost(post);
                clientPost.Comments = _graphDB.getCommentsForPost(post.postID);
                List<User> usersLike = _graphDB.getLikesForPost(post.postID);
                clientPost.UsersLikes = usersLike;
                clientPost.LikeCount = usersLike.Count;
                postsToClient.Add(clientPost);
            }
            return postsToClient;

        }

        public List<ClientPost> getMyPosts(string userId)
        {
            List<Post> posts=_graphDB.getMyPosts(userId);
            List<ClientPost> postsToClient = new List<ClientPost>();

            foreach (var post in posts)
            {
                ClientPost clientPost = new ClientPost(post);
                clientPost.Comments = _graphDB.getCommentsForPost(post.postID);
                List<User> usersLike=_graphDB.getLikesForPost(post.postID);
                clientPost.UsersLikes = usersLike;
                clientPost.LikeCount = usersLike.Count;
                postsToClient.Add(clientPost);
            }
            return postsToClient; 
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

        public string SaveImage(byte[] image,string userId)
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

        public void addLike(string userId, string postId)
        {
                _graphDB.creatConection(userId, postId, "Like");  
        }
    }
}
