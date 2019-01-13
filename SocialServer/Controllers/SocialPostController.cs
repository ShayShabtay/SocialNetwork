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

        public SocialPostController()
        {
            _socialManager = new SocialManager();
        }

        [HttpPost]
        [Route("api/SocialUser/addPost")]
        public IHttpActionResult AddPost([FromBody]Post post)
        {
            _socialManager.AddPost(post, new User("omer", "omer"));
            return Ok("add post ok");
        }
    }
}
