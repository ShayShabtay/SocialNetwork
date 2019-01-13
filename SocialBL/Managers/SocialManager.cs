
using Newtonsoft.Json.Linq;
using SocialCommon.Models;
using SocialRepository.GraphDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Managers
{
    public class SocialManager:ISocialManager
    {
        IGraphDB _graphDB;
        public SocialManager()
        {
            _graphDB = new neo4jDB();
        }

        public void AddPost(Post post,User user)
        {
            _graphDB.addPost(post,user);
          //  graphDB.creatConection(user,post,"publish");
        }

        public void AddUser(User user)
        {
            _graphDB.addUser(user);
        }

        public void Follow(string SourceUserId,string targetUserId)
        {
            _graphDB.Follow(SourceUserId, targetUserId);
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
                    userId =  task.Result.ToString();
                }
            }
            return userId;
        }

       
    }
}
