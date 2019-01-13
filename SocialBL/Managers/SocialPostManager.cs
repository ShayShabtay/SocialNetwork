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
