using AuthenticationBL.Managers;
using AuthenticationServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AuthenticationServer.Controllers
{
    public class TokenController : ApiController, ITokenApi
    {
        TokenManager _tokenManager;

        public TokenController()
        {
            _tokenManager = new TokenManager();
        }

        [HttpGet]
        [Route("api/token/validateManualToken")]
        public IHttpActionResult ValidateToken()
        {
            string token = Request.Headers.GetValues("x-token").First();

            bool isValidToken;
            try
            {
                isValidToken = _tokenManager.ValidateManualToken(token);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            if(isValidToken)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
