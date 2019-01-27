using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using UI.Models;
using Amazon.S3;
using UI.Storage;
using System.Threading.Tasks;
using UI.SignalR;

namespace UI.Controllers
{
    public class SocialController : Controller
    {
        [HttpGet]
        public ActionResult CreatePost()
        {
            SocialViewModel s = new SocialViewModel
            {
                PostDTO = new Models.PostDTO()
            };
            return PartialView(s);
        }

        [HttpPost]
        public ActionResult CreatePost(SocialViewModel socialViewModel)
        {
            string token = Request.Cookies["UserToken"].Value;
            string imageURL;
            if (socialViewModel.PostDTO.Post.Picture1 != null)
            {
                imageURL = UploadImageToS3(socialViewModel.PostDTO.Post.Picture1);
                socialViewModel.PostDTO.Post.ImageUrl = imageURL;
            }
            socialViewModel.PostDTO.Post.Picture1 = null;

            if(socialViewModel.PostDTO.Post.Permission == null)
            {
                socialViewModel.PostDTO.Post.Permission = "Public";
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var task = client.PostAsJsonAsync("api/SocialPost/addPost", socialViewModel.PostDTO);
                task.Wait();
                var res = task.Result;
                return RedirectToAction("MainPageAfterLogin","Home");
            }
        }

        [HttpGet]
        public ActionResult CreateComment()
        {
            SocialViewModel s = new SocialViewModel
            {
                CommentDTO = new Models.CommentDTO()
            };
            return PartialView(s); //need to add which page to return
        }

        [HttpPost]
        public ActionResult CreateComment(SocialViewModel socialViewModel)
        {
            string postId = socialViewModel.CommentDTO.Comment.PostID;
            string token = Request.Cookies["UserToken"].Value;
            string imageURL;
            if (socialViewModel.CommentDTO.Comment.Picture1 != null)
            {
                imageURL = UploadImageToS3(socialViewModel.CommentDTO.Comment.Picture1);
                socialViewModel.CommentDTO.Comment.ImageUrl = imageURL;
            }
            socialViewModel.CommentDTO.Comment.Picture1 = null;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var task = client.PostAsJsonAsync($"api/SocialPost/addComment/{postId}", socialViewModel.CommentDTO);
                task.Wait();
                var res = task.Result;
                return RedirectToAction("MainPageAfterLogin", "Home");
            }
        }

        [HttpGet]
        //Get post to main page
        public ActionResult GetFeed()
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.GetAsync($"api/SocialPost/getPosts").Result;

                if (res.IsSuccessStatusCode == true)
                {
                    var posts = res.Content.ReadAsAsync<List<ClientPost>>().Result;

                    SocialViewModel socialViewModel = new SocialViewModel();
                    socialViewModel.ClientPostFeed = posts;
                    return PartialView("GetFeed", socialViewModel);
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpGet]
        //Get profile and posts of other user
        public ActionResult GetUsersProfileAndFeeds(string targetUserId)
        {
            string token = Request.Cookies["UserToken"].Value;
            SocialViewModel socialViewModel = new SocialViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                string otherUserId = targetUserId;

                var res = client.GetAsync($"api/SocialPost/getMyPosts/{otherUserId}").Result;

                if (res.IsSuccessStatusCode == true)
                {
                    var posts = res.Content.ReadAsAsync<List<ClientPost>>().Result;
                    socialViewModel.ClientPostFeed = posts;
                    socialViewModel.OtherUserIdentityModel = GetUserIdentity(targetUserId);

                    socialViewModel.OtherUserIdentityModel.IsFollow = IsUserFollowUser(targetUserId);
                    return View(socialViewModel);
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        private bool IsUserFollowUser(string targetUserId)
        {
            string token = Request.Cookies["UserToken"].Value;
            SocialViewModel socialViewModel = new SocialViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.PostAsJsonAsync($"api/SocialUser/isUserFollowUser", targetUserId).Result;

                if (res.IsSuccessStatusCode == true)
                {
                    return res.Content.ReadAsAsync<bool>().Result;
                }
                else
                    return false;
            }
        }

        [HttpGet]
        //Get my post to my profile page
        public ActionResult GetMyPosts()
        {
            string token = Request.Cookies["UserToken"].Value;
            SocialViewModel socialViewModel = new SocialViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.GetAsync($"api/SocialPost/getMyPosts").Result;

                if (res.IsSuccessStatusCode == true)
                {
                    var posts = res.Content.ReadAsAsync<List<ClientPost>>().Result;
                    socialViewModel.ClientPostFeed = posts;
                    return View(socialViewModel); //need to add which page to return
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpGet]
        public bool AddLikeToPost(string postId)
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.PostAsJsonAsync($"api/SocialPost/addLikeToPost", postId).Result;
                //task.Wait();
                //var res = task.Result;
                if (res.IsSuccessStatusCode == true)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        [HttpGet]
        public bool UnlikePost(string postId)
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var task = client.PostAsJsonAsync($"api/SocialPost/unLikePost", postId);
                task.Wait();
                var res = task.Result;

                if (res.IsSuccessStatusCode == true)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        [HttpGet]
        public bool AddLikeToComment(string commentId)
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var task = client.PostAsJsonAsync($"api/SocialPost/addLikeToComment", commentId);
                task.Wait();
                var res = task.Result;

                if (res.IsSuccessStatusCode == true)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        [HttpGet]
        public bool UnlikeComment(string commentId)
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var task = client.PostAsJsonAsync($"api/SocialPost/unLikeComment", commentId);
                task.Wait();
                var res = task.Result;

                if (res.IsSuccessStatusCode == true)
                {
                    return true;
                }
                else
                    return false;
            }
        }


        //Private Methods
        private UserIdentityModel GetUserIdentity(string otherUserId)
        {
            string token = Request.Cookies["UserToken"].Value;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51639");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.GetAsync($"api/identity/getUserProfile/{otherUserId}").Result;

                if (res.IsSuccessStatusCode == true)
                {
                    return res.Content.ReadAsAsync<UserIdentityModel>().Result;
                }
                else
                    return null;
            }
        }

        private string UploadImageToS3(HttpPostedFileBase picture1)
        {
            string fileKey = DateTime.Now.ToString();
            string userId = GetUserId();
            StorageHelper storage = new StorageHelper();
            string imageUrl = storage.UploadImageToS3(picture1.InputStream, userId, fileKey);
            return imageUrl;
        }

        private string GetUserId()
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.GetAsync($"/api/SocialPost/getUserId").Result;

                if (res.IsSuccessStatusCode == true)
                {
                    var res2 = res.Content.ReadAsAsync<string>().Result;

                    return res2;
                }
                else
                    return null;
            }

        }
    }
}
