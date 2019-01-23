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

namespace UI.Controllers
{
    public class SocialController : Controller
    {
        [HttpGet]
        public ActionResult CreatePost()
        {
            SocialViewModel s = new SocialViewModel
            {
                Post = new Models.Post("")
            };
            return PartialView(s);
        }

        [HttpPost]
        public ActionResult CreatePost(SocialViewModel socialViewModel)
        {
            string token = Request.Cookies["UserToken"].Value;
            string imageURL;
            if (socialViewModel.Post.Picture1 != null)
            {
                imageURL = UploadImageToS3(socialViewModel.Post.Picture1);
                socialViewModel.Post.ImageUrl = imageURL;
            }
            socialViewModel.Post.Picture1 = null;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var task = client.PostAsJsonAsync("api/SocialPost/addPost", socialViewModel.Post);
                task.Wait();
                var res = task.Result;
                return View("MainPageAfterLogin");
            }
        }

        [HttpGet]
        public ActionResult CreateComment()
        {
            SocialViewModel s = new SocialViewModel
            {
                Comment = new Models.Comment("")
            };
            return PartialView(s); //need to add which page to return
        }

        [HttpPost]
        public ActionResult CreateComment(string postId, SocialViewModel socialViewModel)
        {
            string token = Request.Cookies["UserToken"].Value;
            string imageURL;
            if (socialViewModel.Comment.Picture1 != null)
            {
                imageURL = UploadImageToS3(socialViewModel.Comment.Picture1);
                socialViewModel.Comment.ImageUrl = imageURL;
            }
            socialViewModel.Comment.Picture1 = null;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var task = client.PostAsJsonAsync($"api/SocialPost/addComment/{postId}", socialViewModel.Comment);
                task.Wait();
                var res = task.Result;
                return View("MainPageAfterLogin"); //need to add which page to return
            }
        }

        [HttpGet]
        //Get post to main page
        public ActionResult GetFeed()

        {
            string token = Request.Cookies["UserToken"].Value;
            //List<SocialCommon.Models.ClientPost> listOfPosts = ClientPostFeed.ToList();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.GetAsync($"api/SocialPost/getPosts").Result;

                if (res.IsSuccessStatusCode == true)
                {
                   // var posts = res.Content.ReadAsAsync<List<ClientPost>>().Result;

                    SocialViewModel socialViewModel = new SocialViewModel();
                 //   socialViewModel.ClientPostFeed = posts;
                    return View("MainPageAfterLogin", socialViewModel);
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpGet]
        //Get profile and posts of other user
        public ActionResult GetUsersProfileAndFeeds(string userId)
        {
            string token = Request.Cookies["UserToken"].Value;
            SocialViewModel socialViewModel = new SocialViewModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                string otherUserId = userId;

                var res = client.GetAsync($"api/SocialPost/getMyPosts/{otherUserId}").Result;

                if (res.IsSuccessStatusCode == true)
                {
                   // var posts = res.Content.ReadAsAsync<List<ClientPost>>().Result;
                  //  socialViewModel.ClientPostFeed = posts;
                    socialViewModel.OtherUserIdentityModel = GetUserIdentity(userId);
                    return View("complete", socialViewModel);
                }
                else
                    return Content("res.StatusCode = false :/");
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
                 //   var posts = res.Content.ReadAsAsync<List<ClientPost>>().Result;
                 //   socialViewModel.ClientPostFeed = posts;
                    return View("x", socialViewModel); //need to add which page to return
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpPost]
        public ActionResult AddLikeToPost(string postId)
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var task = client.PostAsJsonAsync($"api/SocialPost/addLikeToPost", postId);
                task.Wait();
                var res = task.Result;
                if (res.IsSuccessStatusCode == true)
                {
                    return View(); //need to add which page to return
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpPost]
        public ActionResult UnlikePost(string postId)
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
                    return View(); //need to add which page to return
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpPost]
        public ActionResult AddLikeToComment(string commentId)
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
                    return View(); //need to add which page to return
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpPost]
        public ActionResult UnlikeComment(string commentId)
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
                    return View(); //need to add which page to return
                }
                else
                    return Content("res.StatusCode = false :/");
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
