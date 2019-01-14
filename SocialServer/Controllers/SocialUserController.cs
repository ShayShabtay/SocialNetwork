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
            return Ok();
        }

        [HttpPost]
        [Route("api/SocialUser/follow")]
        public IHttpActionResult Follow([FromBody]string targetUserId) 
        {
            string token = Request.Headers.GetValues("x-token").First();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(targetUserId))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
            }

            string SourceUserId = _socialManager.ValidateToken(token);

            if (SourceUserId == null)
            {

            }
                try
                {
                _socialManager.Follow(SourceUserId, targetUserId);
                return Ok();

                }
                catch (Exception)
                {
                    return BadRequest();
                }
        }

        [HttpPost]
        [Route("api/SocialUser/unFollow")]
        public IHttpActionResult UnFollow([FromBody]string targetUserId)
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = _socialManager.ValidateToken(token);
            if (SourceUserId != null)
            {

            }
            //_socialManager.UnFollow();
            return Ok();
        }

        [HttpPost]
        [Route("api/SocialUser/blockUser")]
        public IHttpActionResult BlockUser()
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = _socialManager.ValidateToken(token);

            return Ok();
        }

        [HttpPost]
        [Route("api/SocialUser/unblockUser")]
        public IHttpActionResult UnblockUser()
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = _socialManager.ValidateToken(token);

            return Ok();
        }

        [HttpGet]
        [Route("api/SocialUser/getFollowers")]
        public IHttpActionResult GetFollowers()
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = _socialManager.ValidateToken(token);

            return Ok();
        }

        [HttpGet]
        [Route("api/SocialUser/getFollowing")]
        public IHttpActionResult GetFollowing()
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = _socialManager.ValidateToken(token);

            return Ok();
        }


    }


}
