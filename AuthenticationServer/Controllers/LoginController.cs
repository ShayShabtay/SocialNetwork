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

        [HttpGet]
        [Route("api/login/register")] //v
        public IHttpActionResult Register()
        {
            UserLoginDTO userLoginDTO = new UserLoginDTO() { Email = "test4@gmail.com", Password = "123456" }; //for testing

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

        [HttpGet]
        [Route("api/login/loginManual")] //v
        public IHttpActionResult LoginManual()
        {
            UserLoginDTO userLoginDTO = new UserLoginDTO() { Email = "test1@gmail.com", Password = "123456" }; //for testing

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
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("api/login/logout")]
        public IHttpActionResult Logout()
        {
            throw new NotImplementedException();
        }


        [HttpGet]
        [Route("api/login/resetPassword")] //v
        public IHttpActionResult ResetPassword()
        {
            ResetPasswordDTO resetPasswordDTO = new ResetPasswordDTO()
            {
                Email = "test1@gmail.com",
                OldPassword = "111111",
                NewPassword = "1111112"
            };

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
