using IdentityCommon.Execeptions;
using IdentityCommon.Models;
using IdentityRepository.DynamoDb;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IdentityBL.Managers
{
    public class IdentityManager
    {
        DynamoService _dynamo;

        //Ctor
        public IdentityManager()
        {
            _dynamo = new DynamoService();
        }

        //Public Methods
        public UserIdentity UpdateUserIdentity(string userId, UserIdentity updatedUserIdentity)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                updatedUserIdentity.UserId = userId;
                UserIdentity currentUserIdentity = _dynamo.GetItem<UserIdentity>(userId);

                if (currentUserIdentity != null)
                {
                    try
                    {
                        _dynamo.UpdateItem(updatedUserIdentity);
                    }
                    catch (Exception)
                    {
                        throw new FaildToConnectDbException();
                    }
                }
                else
                {
                    CreateUserIdentity(updatedUserIdentity);
                }

                updatedUserIdentity.UserId = null;
                return updatedUserIdentity;
            }
            else
            {
                throw new NotMatchException("UserId");
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

        public UserIdentity GetUserIdentity(string userId)
        {

            if (!string.IsNullOrEmpty(userId))
            {
                UserIdentity foundUserIdentity;

                try
                {
                    foundUserIdentity = _dynamo.GetItem<UserIdentity>(userId);
                }
                catch (Exception)
                {
                    throw new FaildToConnectDbException();
                }

                if (foundUserIdentity != null)
                {
                    foundUserIdentity.UserId = null;
                    return foundUserIdentity;
                }
                else
                {
                    throw new NotFoundException("User");
                }
            }
            else
            {
                throw new NotMatchException("UserId");
            }
        }


        //Private Methods
        private void CreateUserIdentity(UserIdentity updatedUserIdentity)
        {
            try
            {
                _dynamo.Store(updatedUserIdentity);
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }
        }

      
    }
}
