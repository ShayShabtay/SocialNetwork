using IdentityBL.Managers;
using IdentityCommon.Execeptions;
using IdentityCommon.Models;
using IdentityServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IdentityServer.Controllers
{
    public class IdentityController : ApiController, IIdentityApi
    {
        IdentityManager _identityManager;

        public IdentityController()
        {
            _identityManager = new IdentityManager();
        }

        [HttpGet]
        [Route("api/identity/getUserProfile")]
        public IHttpActionResult GetUserProfile()
        {
            string token = Request.Headers.GetValues("x-token").First();

            if (string.IsNullOrEmpty(token))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the data"));
            }

            string userId = _identityManager.ValidateToken(token);
            if (userId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, "Invalid token"));
            }

            UserIdentity foundUserIdentity;
            try
            {
                foundUserIdentity = _identityManager.GetUserIdentity(userId);
            }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong"));
            }
            catch (NotMatchException e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, e.Message));
            }
            catch (NotFoundException e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, e.Message));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }
            return Ok(foundUserIdentity);
        }

        [HttpPost]
        [Route("api/identity/updateUserProfile")]
        public IHttpActionResult UpdateUserProfile([FromBody]UserIdentity userIdentity)
        {
            string token = Request.Headers.GetValues("x-token").First(); 

            if (string.IsNullOrEmpty(token))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the data"));
            }

            string userId = _identityManager.ValidateToken(token);
            if (userId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, "Invalid token"));
            }

            UserIdentity updatedUser;

            try
            {
                updatedUser = _identityManager.UpdateUserIdentity(userId, userIdentity);
            }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong"));
            }
            catch(NotMatchException e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, e.Message));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            return Ok(updatedUser);
        }
    }
}
