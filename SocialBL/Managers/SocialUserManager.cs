
using Newtonsoft.Json.Linq;
using SocialBL.Interfaces;
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
    public class SocialUserManager: ISocialUserManager
    {
        IGraphDB _graphDB;
        public SocialUserManager()
        {
            _graphDB = new neo4jDB();
        }



        public void AddUser(User user)
        {
            _graphDB.addUser(user);
        }

        public void Follow(string SourceUserId,string targetUserId)
        {
            //_graphDB.Follow(SourceUserId, targetUserId);
            _graphDB.creatConection(SourceUserId, targetUserId, "Follow");
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
