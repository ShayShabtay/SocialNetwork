using SocialBL.Managers;
using SocialCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SocialServer.Controllers
{
    public class SocialPostController : ApiController
    {
        ISocialManager _socialManager;
        SocialPostManager SocialPostManager;

        public SocialPostController()
        {
            _socialManager = new SocialManager();
            SocialPostManager = new SocialPostManager();
        }

        [HttpPost]
        [Route("api/SocialUser/addPost")]
        public IHttpActionResult AddPost([FromBody]Post post)
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = _socialManager.ValidateToken(token);
            if (SourceUserId != null)
            {
                try
                {
                    SocialPostManager.AddPost(post, SourceUserId);
                    return Ok("add post ok");
                }
                catch (Exception)
                {

                    return BadRequest();
                } 
            }
            return null;

        }
    }
}
