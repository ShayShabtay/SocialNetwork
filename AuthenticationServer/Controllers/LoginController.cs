using AuthenticationBL.Managers;
using AuthenticationCommon.Execeptions;
using AuthenticationCommon.Models;
using AuthenticationCommon.ModelsDTO;
using AuthenticationServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AuthenticationServer.Controllers
{
    public class LoginController : ApiController, ILoginApi
    {
        LoginManager _loginManager;
        public LoginController()
        {
            _loginManager = new LoginManager();
        }

        [HttpPost]
        [Route("api/login/register")]
        public IHttpActionResult Register([FromBody] UserLoginDTO userLoginDTO)
        {
            string token;

            try
            {
                token = _loginManager.Register(userLoginDTO);
            }
            catch (AlreadyExistException e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, e.Message));

            }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong"));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            if (string.IsNullOrEmpty(token))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User has not been added, please try again"));
            }
            else
            {
                return Ok(token);
            }
        }

        [HttpPost]
        [Route("api/login/loginManual")]
        public IHttpActionResult LoginManual([FromBody] UserLoginDTO userLoginDTO)
        {
            string token;

            try
            {
                token = _loginManager.LoginManual(userLoginDTO);
            }
            catch (NotMatchException e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, e.Message));

            }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong"));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            if (string.IsNullOrEmpty(token))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User can not been logged in, please try again"));
            }
            else
            {
                return Ok(token);
            }
        }

        [HttpGet]
        [Route("api/login/loginFacebook")]
        public IHttpActionResult LoginFacebook()
        {
            string facebookToken = Request.Headers.GetValues("x-auth-token").First();

            if(string.IsNullOrEmpty(facebookToken))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the data"));
            }

            string customToken;

            try
            {
                customToken =_loginManager.LoginFacebook(facebookToken);
            }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong"));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            if (string.IsNullOrEmpty(customToken))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Token is invalid"));
            }
            else
            {
                return Ok(customToken);
            }
        }

        [HttpPost]
        [Route("api/login/resetPassword")]
        public IHttpActionResult ResetPassword([FromBody]ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                _loginManager.ResetPassword(resetPasswordDTO);
            }
            catch (NotMatchException e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, e.Message));

            }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Something went wrong"));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            return Ok();
        }
    }
}
