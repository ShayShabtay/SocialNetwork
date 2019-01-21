using SocialCommon.Models;
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
                    return View("MainPageAfterLogin", socialViewModel);
                }
                else
                    return Content("res.StatusCode = false :/");
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
