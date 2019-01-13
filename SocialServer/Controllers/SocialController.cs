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
    public class SocialController : ApiController
    {
        ISocialManager _socialManager;

     
        public SocialController()
        {
            _socialManager = new SocialManager();
        }

        [HttpPost]
        [Route("api/Social/addUser")]
        public IHttpActionResult AddUser([FromBody] User user)
        {
            
            _socialManager.AddUser(user);
            return Ok("good");
        }

        [HttpPost]
        [Route("api/Social/addPost")]
        public IHttpActionResult AddPost([FromBody]Post post)
        {
            _socialManager.AddPost(post,new User("omer","omer"));
            return Ok("add post ok"); 
        }

        [HttpGet]
        [Route("api/Social/follow")]
        public IHttpActionResult Follow()
        {
            _socialManager.Follow();
            return Ok();
        }


    }
        

}
