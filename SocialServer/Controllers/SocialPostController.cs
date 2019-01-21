using SocialBL.Interfaces;
using SocialBL.Managers;
using SocialCommon.Exceptions;
using SocialCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SocialServer.Controllers
{
    public class SocialPostController : ApiController
    {
        ISocialPostManager _socialPostManager;

        //Ctor
        public SocialPostController()
        {
            _socialPostManager = new SocialPostManager();
        }

        [HttpPost]
        [Route("api/SocialPost/addPost")]
        public IHttpActionResult AddPost([FromBody]List<string> param)
        {
            string token = Request.Headers.GetValues("x-token").First();

            if (string.IsNullOrEmpty(token))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
            }
            string SourceUserId = null;

            try
            {
                SourceUserId = _socialPostManager.ValidateToken(token);
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
                byte[] imageAsBytes = Convert.FromBase64String(param[1]);

               string imagePath= _socialPostManager.SaveImage(imageAsBytes, SourceUserId);
                Post post = new Post(param[0]);
                post.ImageUrl = imagePath;

                _socialPostManager.AddPost(post, SourceUserId);
            }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/SocialPost/addComment")]
        public IHttpActionResult AddComment(Comment comment, string postId)
        {
            string token = Request.Headers.GetValues("x-token").First();

            if (string.IsNullOrEmpty(token))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
            }
            string userId = null;

            try
            {
                userId = _socialPostManager.ValidateToken(token);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            if (userId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            try
            {
                _socialPostManager.AddComment(comment, userId, postId);
         
                }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/SocialPost/addLike")]
        public IHttpActionResult AddLike([FromBody] string postId)
        {
            string token = Request.Headers.GetValues("x-token").First();

            if (string.IsNullOrEmpty(token))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
            }
            string userId = null;

            try
            {
                userId = _socialPostManager.ValidateToken(token);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            if (userId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            try
            {
                _socialPostManager.AddLike(userId, postId);

                ///////////////////////////////
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:51446/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                   // string from = userId;
                    string to = _socialPostManager.GetUserByPostID(postId);
                   //string postid = postId;
                    List<string> param = new List<string>();
                    param.Add(userId);
                    param.Add(to);
                    param.Add(postId);
                    var res = client.PostAsJsonAsync("api/Notification/AddNotification", param);
                }

                ////////////////////////////////////

            }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/SocialPost/addLike")]
        public IHttpActionResult UnLike([FromBody] string postId)
        {
            string token = Request.Headers.GetValues("x-token").First();

            if (string.IsNullOrEmpty(token))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
            }
            string userId = null;

            try
            {
                userId = _socialPostManager.ValidateToken(token);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            if (userId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            try
            {
                _socialPostManager.UnLike(userId, postId);
            }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            return Ok();
        }

        [HttpGet]
        [Route("api/SocialPost/getPosts")]
        public IHttpActionResult GetAllPosts()
        {
            string token = Request.Headers.GetValues("x-token").First();

            if (string.IsNullOrEmpty(token))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
            }
            string userId = null;

            try
            {
                userId = _socialPostManager.ValidateToken(token);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            if (userId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            List<ClientPost> Posts;
            try
            {
                Posts = _socialPostManager.GetAllPosts(userId);
            }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            return Ok(Posts);
        }

        [HttpGet]
        [Route("api/SocialPost/getMyPosts")]
        public IHttpActionResult GetMyPosts()
        {
            string token = Request.Headers.GetValues("x-token").First();

            if (string.IsNullOrEmpty(token))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
            }
            string userId = null;

            try
            {
                userId = _socialPostManager.ValidateToken(token);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            if (userId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            List<ClientPost> Posts = null;
            try
            {
                //add await here
                Posts = _socialPostManager.GetMyPosts(userId);
            }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            return Ok(Posts);
        }

        [HttpGet]
        [Route("api/SocialPost/saveImage")]
        public IHttpActionResult SaveImage([FromBody] string imageString)
        {
            string token = Request.Headers.GetValues("x-token").First();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(imageString))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
            }
            string userId = null;

            try
            {
                userId = _socialPostManager.ValidateToken(token);
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            if (userId == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
            }

            byte[] image = Convert.FromBase64String(imageString);
            string imageUrl;
            try
            {
                imageUrl = _socialPostManager.SaveImage(image, userId);
            }
            catch (FaildToConnectDbException)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }
            catch (Exception)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            if (imageUrl == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
            }

            return Ok(imageUrl);
        }
    }
}
