using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Neo4j.Driver.V1;
using SocialBL.Managers;
using SocialCommon.Models;

namespace SocialServer.Controllers
{
    public class SocialUserController : ApiController
    {
        ISocialManager _socialManager;


        public SocialUserController()
        {
            _socialManager = new SocialManager();
        }

        [HttpPost]
        [Route("api/SocialUser/addUser")]
        public IHttpActionResult AddUser([FromBody]User user)
        {
            _socialManager.AddUser(user);
            return Ok("good");
        }

        [HttpPost]
        [Route("api/SocialUser/follow")]
        public IHttpActionResult Follow([FromBody]string targetUserId)
        {
            string token = Request.Headers.GetValues("x-token").First();

            string SourceUserId = _socialManager.ValidateToken(token);

            if (SourceUserId != null)
            {
                try
                {
                _socialManager.Follow(SourceUserId, targetUserId);
                return Ok();

                }
                catch (Exception)
                {
                    return BadRequest();
                    throw;
                }
            }
            return null;
        }

        [HttpGet]
        [Route("api/SocialUser/unFollow")]
        public IHttpActionResult UnFollow(string targetUserId)
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = _socialManager.ValidateToken(token);
            if (SourceUserId != null)
            {

            }
            //_socialManager.UnFollow();
            return Ok();
        }


    }


}
