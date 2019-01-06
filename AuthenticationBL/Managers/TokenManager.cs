using AuthenticationCommon.Execeptions;
using AuthenticationCommon.Models;
using AuthenticationRepository.DynamoDb;
using Jose;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationBL.Managers
{
    public class TokenManager
    {
        DynamoService _dynamo;

        //Ctor
        public TokenManager()
        {
            _dynamo = new DynamoService();
        }


        //Public Methods
        internal string GenerateToken(string userId, string email)
        {
            long exp = (long)(DateTime.UtcNow.AddMinutes(15) - new DateTime(1970, 1, 1)).TotalSeconds;
            long iat = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

            var payload = new Dictionary<string, object>()
            {
                ["sub"] = userId,
                ["exp"] = exp,
                ["iat"] = iat,
                ["aud"] = "facelook social",
                ["username"] = email
            };

            byte[] secretKey = Encoding.ASCII.GetBytes("vCryTxAz8mvMbamPvRseh7Hov425kVectcGStY_il87aOjP3JQd3vAGTajiIY_kAgydOCso9j4z4GzqIK4Zb6Kt495TiSrZEn1iwTzZB3oioa8UwO9gqWX_DqNIAak8hUsAexWpOpxUWwakwmKA74pEpwDcvGTnHsGTkHFpEatuNuhLr6_gDlp7tzR9eCCfwd7PpsUbItHHc83crRmZuOhuWA_vzDuxiuWhCJ6QrFyN1M9T4kal1GPvptGwsWT9ywoKUTTfsiBkbNowYdUv4ZqfuqQNUTYbuye6DEsuo3WjaTAsbmobse3_pQGptC08ipk4V4yK-HSeBfW0twTcunQ");
            string token = Jose.JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);

            AddToTokenHistory(userId, token);

            return token;
        }

        public bool ValidateManualToken(string token)
        {
            string secretKey = "vCryTxAz8mvMbamPvRseh7Hov425kVectcGStY_il87aOjP3JQd3vAGTajiIY_kAgydOCso9j4z4GzqIK4Zb6Kt495TiSrZEn1iwTzZB3oioa8UwO9gqWX_DqNIAak8hUsAexWpOpxUWwakwmKA74pEpwDcvGTnHsGTkHFpEatuNuhLr6_gDlp7tzR9eCCfwd7PpsUbItHHc83crRmZuOhuWA_vzDuxiuWhCJ6QrFyN1M9T4kal1GPvptGwsWT9ywoKUTTfsiBkbNowYdUv4ZqfuqQNUTYbuye6DEsuo3WjaTAsbmobse3_pQGptC08ipk4V4yK-HSeBfW0twTcunQ";

            try
            {
                string payload = JWT.Decode(token, Encoding.ASCII.GetBytes(secretKey));
                dynamic data = JObject.Parse(payload);

                long now = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                if (data.iat > now || data.exp < now || data.aud != "facelook social")
                {
                    return false;
                }
                return true;
            }
            catch (InvalidAlgorithmException)
            {
                return false;
            }
            catch (IntegrityException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }


        //Private Methods
        private void AddToTokenHistory(string userId, string token)
        {
            TokenHistory tokenHistory = new TokenHistory()
            {
                UserId = userId,
                TimeStamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds,
                Token = token
            };

            try
            {
                _dynamo.Store(tokenHistory);
            }
            catch (Exception)
            {
                throw new FaildToConnectDbException();
            }
        }
    }
}
