
using Newtonsoft.Json.Linq;
using SocialBL.Interfaces;
using SocialCommon.Exceptions;
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
    public class SocialUserManager : ISocialUserManager
    {
        IGraphDB _graphDB;

        //Ctor
        public SocialUserManager()
        {
            _graphDB = new neo4jDB();
        }


        //Public Methods
        public void AddUser(User user)
        {
            try
            {
                _graphDB.addUser(user);
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }
        }

        public void Follow(string SourceUserId, string targetUserId)
        {
            try
            {
                _graphDB.creatConection(SourceUserId, targetUserId, "Follow");
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }
        }

        public void UnFollow(string SourceUserId, string targetUserId)
        {
            try
            {
                _graphDB.DeleteConection(SourceUserId, targetUserId, "Follow");
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }
        }

        public void BlockUser(string SourceUserId, string targetUserId)
        {
            try
            {
                _graphDB.creatConection(SourceUserId, targetUserId, "Block");
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }

            UnFollow(SourceUserId, targetUserId);
            UnFollow(targetUserId, SourceUserId);
        }

        public void UnBlockUser(string SourceUserId, string targetUserId)
        {
            try
            {
                _graphDB.DeleteConection(SourceUserId, targetUserId, "Block");
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
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
