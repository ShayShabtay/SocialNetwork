using Amazon.Runtime;
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
        public IHttpActionResult AddPost([FromBody]Post post)
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
        [Route("api/SocialPost/addComment/{postId}")]
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
                    param.Add("Comment");
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
        [Route("api/SocialPost/addLikeToPost")]
        public IHttpActionResult AddLikeToPost([FromBody] string postId)
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
                _socialPostManager.AddLikeToPost(userId, postId);
                

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
                    param.Add("Like");
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
        [Route("api/SocialPost/unLikePost")]
        public IHttpActionResult UnLikePost([FromBody] string postId)
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
                _socialPostManager.UnLikePost(userId, postId);
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
        [Route("api/SocialPost/addLikeToComment")]
        public IHttpActionResult AddLikeToComment([FromBody] string commentId)
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
                _socialPostManager.AddLikeToComment(userId, commentId);

                ///////////////////////////////
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:51446/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // string from = userId;
                    string to = _socialPostManager.GetUserByPostID(commentId);
                    //string postid = postId;
                    List<string> param = new List<string>();
                    param.Add(userId);
                    param.Add(to);
                    param.Add(commentId);
                    param.Add("Like");
                    var res = client.PostAsJsonAsync("api/Notification/AddNotification", param);
                }
            }

            ////////////////////////////////////


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
        [Route("api/SocialPost/unLikeComment")]
        public IHttpActionResult UnLikeComment([FromBody] string commentId)
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
                _socialPostManager.UnLikeComment(userId, commentId);
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
        [Route("api/SocialPost/getMyPosts/{OtherUserId?}")]
        public IHttpActionResult GetMyPosts(string OtherUserId="")
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
            if(!string.IsNullOrEmpty(OtherUserId))
            {
                userId = OtherUserId;
            }

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

        //[HttpGet]
        //[Route("api/SocialPost/saveImage")]
        //public IHttpActionResult SaveImage([FromBody] string imageString)
        //{
        //    string token = Request.Headers.GetValues("x-token").First();

        //    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(imageString))
        //    {
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
        //    }
        //    string userId = null;

        //    try
        //    {
        //        userId = _socialPostManager.ValidateToken(token);
        //    }
        //    catch (Exception)
        //    {
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
        //    }

        //    if (userId == null)
        //    {
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
        //    }

        //    //byte[] image = Convert.FromBase64String(imageString);
        //    string imageUrl;
        //    try
        //    {
        //        //imageUrl = _socialPostManager.SaveImage(image, userId);
        //    }
        //    catch (FaildToConnectDbException)
        //    {
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
        //    }
        //    catch (Exception)
        //    {
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
        //    }

        //    //if (imageUrl == null)
        //    //{
        //    //    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
        //    //}

        //    return Ok(); //imageurl
        //}

        //[HttpGet]
        //[Route("api/SocialPost/getS3TemporaryToken")]
        //public IHttpActionResult GetS3TemporaryToken()
        //{
        //    //string token = Request.Headers.GetValues("x-token").First();

        //    //if (string.IsNullOrEmpty(token))
        //    //{
        //    //    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Sorry, we could not get the token"));
        //    //}
        //    //string SourceUserId = null;

        //    //try
        //    //{
        //    //    SourceUserId = _socialPostManager.ValidateToken(token);
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
        //    //}

        //    //if (SourceUserId == null)
        //    //{
        //    //    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid token"));
        //    //}

        //    //SessionAWSCredentials temporaryAWSCredentials;
        //    try
        //    {
        //        _socialPostManager.GetTemporaryToken();
        //    }
        //    catch (Exception)
        //    {
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
        //    }

        //    //if (temporaryAWSCredentials != null)
        //    //{
        //    //    return Ok(temporaryAWSCredentials);
        //    //}
        //    //else
        //    //{
        //    //    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong"));
        //    //}
        //    return Ok();
        //}

        [HttpGet]
        [Route("api/SocialPost/getUserId")]
        public IHttpActionResult GetUserId()
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
            return Ok(SourceUserId);
        }
    }
}
