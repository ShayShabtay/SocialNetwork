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


namespace UI.Controllers
{
    public class SocialController : Controller
    {
        //public ActionResult MainPageAfterLogin(UserIdentityModel userModel)
        //{
        //    //string UserCookie = Request.Cookies["UserProfile"].Value;
        //    return View(userModel);
        //}
        [HttpPost]
        public ActionResult CreatePost(string postContent, string imagePath=null)
        {
            string token = Request.Cookies["UserToken"].Value;
            string imageBase64String = null;
            List<string> param = new List<string>();
            using (var client = new HttpClient())
            { 
                client.BaseAddress = new Uri("http://localhost:51639");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                param.Add(postContent);
                if (imagePath != null)
                {
                    byte[] imageData = System.IO.File.ReadAllBytes(imagePath);
                   imageBase64String = Convert.ToBase64String(imageData);
                    param.Add(imageBase64String);
                }
                    var res = client.PostAsJsonAsync("api/SocialPost/addPost", param).Result;
                return PartialView();
            }
        }

        // GET: Social
        public ActionResult GetFeed()
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51639");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.GetAsync($"api/SocialPost/getPosts").Result;
                
                if (res.IsSuccessStatusCode == true)
                {
                    // var res2 = res.Content.ReadAsAsync<UserIdentityModel>().Result;

                    var posts = res.Content.ReadAsAsync<List<ClientPost>>();
                    return View(posts);// View(res2);
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        // GET: Social/GetProfileAndFeeds/5
        [HttpGet]
        public ActionResult GetUsersProfileAndFeeds()
        {
            return View();
        }

    }
}
