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
        ISocialManager socialManager;

     
        public SocialController()
        {
            socialManager = new SocialManager();
            //string uri = "bolt://ec2-34-244-123-17.eu-west-1.compute.amazonaws.com:7687";
            //_driver = GraphDatabase.Driver(uri, AuthTokens.Basic("neo4j", "skay1414"));
            
        }
        [HttpPost]
        [Route("api/Social/addUser")]
        public IHttpActionResult addUser([FromBody] User user)
        {
            
            socialManager.addUser(user);
            return Ok("good");
        }

        [HttpPost]
        [Route("api/Social/addPost")]
        public IHttpActionResult addPost([FromBody]Post post)
        {
            socialManager.addPost(post,new User("omer","omer"));
            return Ok("add post ok"); 
        }
        

    }
        

}
