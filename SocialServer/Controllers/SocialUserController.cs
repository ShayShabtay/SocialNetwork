using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Neo4j.Driver.V1;
using SocialBL.Interfaces;
using SocialBL.Managers;
using SocialCommon.Exceptions;
using SocialCommon.Models;

namespace SocialServer.Controllers
{
    public class SocialUserController : ApiController
    {
        ISocialUserManager _sociaUserManager;


        public SocialUserController()
        {
            _sociaUserManager = new SocialUserManager();
        }

        [HttpPost]
        [Route("api/SocialUser/addUser")]
        public IHttpActionResult AddUser([FromBody]User user)
        {
            if (user == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
            }

            try
            {
                _sociaUserManager.AddUser(user);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong"));
            }

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
            string SourceUserId = null;

            try
            {
                SourceUserId = _sociaUserManager.ValidateToken(token);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            if (SourceUserId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }
            try
            {
                _sociaUserManager.BlockUser(SourceUserId, targetUserId);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/SocialUser/unFollow")]
        public IHttpActionResult UnFollow([FromBody]string targetUserId)
        {
            string token = Request.Headers.GetValues("x-token").First();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(targetUserId))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
            }
            string SourceUserId = null;

            try
            {
                SourceUserId = _sociaUserManager.ValidateToken(token);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            if (SourceUserId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }
            //_socialManager.UnFollow();
            return Ok();
        }

        [HttpPost]
        [Route("api/SocialUser/blockUser")]
        public IHttpActionResult BlockUser([FromBody]string targetUserId)
        {
            string token = Request.Headers.GetValues("x-token").First();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(targetUserId))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
            }
            string SourceUserId = null;

            try
            {
                SourceUserId = _sociaUserManager.ValidateToken(token);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            if (SourceUserId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            try
            {
                _sociaUserManager.BlockUser(SourceUserId, targetUserId);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/SocialUser/unblockUser")]
        public IHttpActionResult UnblockUser([FromBody]string targetUserId)
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = _sociaUserManager.ValidateToken(token);

            return Ok();
        }

        [HttpGet]
        [Route("api/SocialUser/getFollowers")]
        public IHttpActionResult GetFollowers()
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = null;

            try
            {
                SourceUserId = _sociaUserManager.ValidateToken(token);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            if (SourceUserId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            return Ok();
        }

        [HttpGet]
        [Route("api/SocialUser/getFollowing")]
        public IHttpActionResult GetFollowing()
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = null;

            try
            {
                SourceUserId = _sociaUserManager.ValidateToken(token);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            if (SourceUserId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            return Ok();
        }


    }


}
