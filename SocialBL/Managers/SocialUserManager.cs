
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
            _graphDB = new Neo4jDB();
        }


        //Public Methods
        public void AddUser(User user)
        {
            try
            {
                _graphDB.AddUser(user);
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
                _graphDB.CreateRelationship(SourceUserId, targetUserId, "Follow");
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
                _graphDB.DeleteRelationship(SourceUserId, targetUserId, "Follow");
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
                _graphDB.CreateRelationship(SourceUserId, targetUserId, "Block");
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
                _graphDB.DeleteRelationship(SourceUserId, targetUserId, "Block");
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }
        }

        public bool IsUserFollowUser(string SourceUserId, string targetUserId)
        {
            try
            {
                return _graphDB.IsFollow(SourceUserId, targetUserId);
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }
        }

        public List<User> GetAllUsers(string userId)
        {
            try
            {
                return _graphDB.GetAllUsers(userId);
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }
        }

        public List<User> GetFollowers(string userId)
        {
            try
            {
                return _graphDB.GetFollowers(userId);
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }
        }

        public List<User> GetFollowing(string userId)
        {
            try
            {
                return _graphDB.GetFollowing(userId);
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }
        }

        public List<User> GetBlockedUsers(string userId)
        {
            try
            {
                return _graphDB.GetBlockedUsers(userId);
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
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
    }
}
