using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Neo4j.Driver.V1;
using SocialBL.Interfaces;
using SocialBL.Managers;
using SocialCommon.Models;

namespace SocialServer.Controllers
{
    public class SocialUserController : ApiController
    {
        ISocialUserManager _sociaUserlManager;


        public SocialUserController()
        {
            _sociaUserlManager = new SocialUserManager();
        }

        [HttpPost]
        [Route("api/SocialUser/addUser")]
        public IHttpActionResult AddUser([FromBody]User user)
        {
            _socialManager.AddUser(user);
            return Ok();
        }

        [HttpGet]//chenge to post
        [Route("api/SocialUser/follow")]
        public IHttpActionResult Follow() //[FromBody]string targetUserId
        {
            //string token = Request.Headers.GetValues("x-token").First();

            //if (string.IsNullOrEmpty(token))
            //{
            //    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
            //}

            //string SourceUserId = _sociaUserlManager.ValidateToken(token);

            string SourceUserId = "c74727fe-d410-4c50-ac78-cc01262a58b8";
            string targetUserId = "5c05e797-fb5a-4ef8-b463-e32073f7e4da";

            if (SourceUserId == null)
            {

            }
                try
                {
                _sociaUserlManager.Follow(SourceUserId, targetUserId);
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
            string SourceUserId = _sociaUserlManager.ValidateToken(token);
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
            string SourceUserId = _sociaUserlManager.ValidateToken(token);

            return Ok();
        }

        [HttpPost]
        [Route("api/SocialUser/unblockUser")]
        public IHttpActionResult UnblockUser()
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = _sociaUserlManager.ValidateToken(token);

            return Ok();
        }

        [HttpGet]
        [Route("api/SocialUser/getFollowers")]
        public IHttpActionResult GetFollowers()
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = _sociaUserlManager.ValidateToken(token);

            return Ok();
        }

        [HttpGet]
        [Route("api/SocialUser/getFollowing")]
        public IHttpActionResult GetFollowing()
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = _sociaUserlManager.ValidateToken(token);

            return Ok();
        }


    }


}
