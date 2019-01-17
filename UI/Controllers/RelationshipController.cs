using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using UI.Models;

namespace UI.Controllers
{
    public class RelationshipController : Controller
    {
        public ActionResult GetAllUsers()
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.GetAsync($"/api/SocialUser/getAllUsers").Result;

                if (res.IsSuccessStatusCode == true)
                {
                    var res2 = res.Content.ReadAsAsync<List<UserDTO>>().Result;
                    SocialViewModel s = new SocialViewModel();
                    s.UserDTO = res2;
                    return View(s);
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpGet]
        public ActionResult GetFollowing()
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.GetAsync($"/api/SocialUser/getFollowing").Result;

                if (res.IsSuccessStatusCode == true)
                {
                    var res2 = res.Content.ReadAsAsync<List<UserDTO>>().Result;
                    return View(res2);
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpGet]
        public ActionResult GetFollowers()
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.GetAsync($"/api/SocialUser/getFollowers").Result;

                if (res.IsSuccessStatusCode == true)
                {
                    var res2 = res.Content.ReadAsAsync<List<UserDTO>>().Result;
                    return View(res2);
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpGet]
        public ActionResult GetBlockUsers()
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.GetAsync($"/api/SocialUser/getBlockUsers").Result;

                if (res.IsSuccessStatusCode == true)
                {
                    var res2 = res.Content.ReadAsAsync<List<UserDTO>>().Result;
                    return View(res2);
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpGet]
        public ActionResult FollowUser(string targetUserId)
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.PostAsJsonAsync($"/api/SocialUser/follow", targetUserId).Result;

                if (res.IsSuccessStatusCode == true)
                {
                    return RedirectToAction("GetFollowing");
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpGet]
        public ActionResult UnFollowUser(string targetUserId)
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.PostAsJsonAsync($"/api/SocialUser/unFollow", targetUserId).Result;

                if (res.IsSuccessStatusCode == true)
                {
                    return RedirectToAction("GetFollowing");
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpGet]
        public ActionResult BlockUser(string targetUserId)
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.PostAsJsonAsync($"/api/SocialUser/blockUser", targetUserId).Result;

                if (res.IsSuccessStatusCode == true)
                {
                    return RedirectToAction("GetBlockUsers");
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpGet]
        public ActionResult UnBlockUser(string targetUserId)
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52536/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.PostAsJsonAsync($"/api/SocialUser/unblockUser", targetUserId).Result;

                if (res.IsSuccessStatusCode == true)
                {
                    return RedirectToAction("GetBlockUsers");
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }
    }
}