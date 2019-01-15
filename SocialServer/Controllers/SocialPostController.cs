using SocialBL.Interfaces;
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
        ISocialUserManager _socialManager;
        SocialPostManager SocialPostManager;

        public SocialPostController()
        {
            _socialManager = new SocialUserManager();
            SocialPostManager = new SocialPostManager();
        }

        [HttpPost]
        [Route("api/SocialPost/addPost")]
        public IHttpActionResult AddPost([FromBody]Post post)
        {
            string token = Request.Headers.GetValues("x-token").First();
            string SourceUserId = SocialPostManager.ValidateToken(token);
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
        [HttpPost]
        [Route("api/SocialPost/getPosts")]
        public IHttpActionResult getAllPosts()
        {
            string token = Request.Headers.GetValues("x-token").First();
            string UserId = SocialPostManager.ValidateToken(token);
            List<ClientPost> Posts=null;
            if (UserId != null)
            {
               Posts=SocialPostManager.getAllPosts(UserId);
            }

                return Ok(Posts);
        }
        [HttpPost]
        [Route("api/SocialPost/getMyPosts")]
        public IHttpActionResult getMyPosts()
        {
            string token = Request.Headers.GetValues("x-token").First();
            string UserId = SocialPostManager.ValidateToken(token);
            List<ClientPost> Posts = null;
            if (UserId != null)
            {
                try
                {
                //add await here
                Posts = SocialPostManager.getMyPosts(UserId);
                return Ok(Posts);

                }
                catch (Exception)
                {

                    throw;
                }
            }

            return BadRequest() ;
        }
        [HttpPost]
        [Route("api/SocialPost/addComment")]
        public IHttpActionResult addComment(Comment comment,string postId)
        {
            string token = Request.Headers.GetValues("x-token").First();
            string UserId = SocialPostManager.ValidateToken(token);
            if (UserId != null)
            {
                try
                {
                SocialPostManager.addComment(comment,UserId,postId);

                }
                catch (Exception e)
                {

                    throw e;
                }
            }
            return Ok();
        }
        [HttpPost]
        [Route("api/SocialPost/saveImage")]
        public IHttpActionResult SaveImage([FromBody] string imageString)
        {
            string token = Request.Headers.GetValues("x-token").First();
            string UserId = SocialPostManager.ValidateToken(token);
            if ((UserId != null)&&(imageString!=null))
            {
                byte[] image = Convert.FromBase64String(imageString);
                string imageUrl = SocialPostManager.SaveImage(image,UserId);

                return Ok(imageUrl);
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("api/SocialPost/addLike")]
        public IHttpActionResult addLike([FromBody] string postId)
        {
            string token = Request.Headers.GetValues("x-token").First();
            string UserId = SocialPostManager.ValidateToken(token);
            if (UserId != null)
            {
                try
                {
                    SocialPostManager.addLike(UserId, postId);
                }
                catch (Exception)
                {

                    throw;
                }

            }

            return null;
        } 

         


        





    }
}
