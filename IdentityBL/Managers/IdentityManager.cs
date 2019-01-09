using IdentityCommon.Execeptions;
using IdentityCommon.Models;
using IdentityRepository.DynamoDb;
using Jose;
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
        public UserIdentity UpdateUserIdentity(string token, UserIdentity updatedUserIdentity)
        {
            string userId = GetUserId(token);

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

        public bool ValidateToken(string token)
        {
            string url = "http://localhost:49884/api/token/validateManualToken";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-token", token);
                var task = client.GetAsync(url);
                task.Wait();
                if (task.Result.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public UserIdentity GetUserIdentity(string token)
        {
            string userId = GetUserId(token);

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

        private string GetUserId(string token)
        {
            string secretKey = "vCryTxAz8mvMbamPvRseh7Hov425kVectcGStY_il87aOjP3JQd3vAGTajiIY_kAgydOCso9j4z4GzqIK4Zb6Kt495TiSrZEn1iwTzZB3oioa8UwO9gqWX_DqNIAak8hUsAexWpOpxUWwakwmKA74pEpwDcvGTnHsGTkHFpEatuNuhLr6_gDlp7tzR9eCCfwd7PpsUbItHHc83crRmZuOhuWA_vzDuxiuWhCJ6QrFyN1M9T4kal1GPvptGwsWT9ywoKUTTfsiBkbNowYdUv4ZqfuqQNUTYbuye6DEsuo3WjaTAsbmobse3_pQGptC08ipk4V4yK-HSeBfW0twTcunQ";
            string payload = JWT.Decode(token, Encoding.ASCII.GetBytes(secretKey));
            dynamic data = JObject.Parse(payload);

            return data.sub;
        }
    }
}
