using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UI.Models;

namespace UI.Controllers
{
    public class IdentityController : Controller
    {
        [HttpGet]
        public ActionResult GetUserProfile()
        {
            string token = Request.Cookies["UserToken"].Value;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:51639");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-token", token);

                var res = client.GetAsync($"/api/identity/getUserProfile").Result;

                if (res.IsSuccessStatusCode == true)
                {
                    var res2 = res.Content.ReadAsAsync<UserIdentityModel>().Result;
                    SocialViewModel res3 = new SocialViewModel();
                    res3.UserIdentityModel = res2;

                    return View(res3);
                }
                else
                    return Content("res.StatusCode = false :/");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetUserProfile(SocialViewModel identityModel)
        {
            string token = Request.Cookies["UserToken"].Value;
            if (!ModelState.IsValid)
            {
                return View(identityModel);
            }

            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:51639");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("x-token", token);

                    var res = client.PostAsJsonAsync($"/api/identity/updateUserProfile", identityModel.UserIdentityModel).Result;

                    if (res.IsSuccessStatusCode == true)
                    {
                        var res2 = res.Content.ReadAsAsync<UserIdentityModel>().Result;

                        identityModel.UserIdentityModel = res2;

                        //save Identity in cookies
                        HttpCookie NameIdentityProfile = new HttpCookie("My_Name");
                        NameIdentityProfile.Value = identityModel.UserIdentityModel.Name.ToString();
                        Response.Cookies.Add(NameIdentityProfile);///

                        HttpCookie AgeIdentityProfile = new HttpCookie("My_Age");
                        AgeIdentityProfile.Value = identityModel.UserIdentityModel.Age.ToString();
                        Response.Cookies.Add(AgeIdentityProfile);///

                        HttpCookie AddressIdentityProfile = new HttpCookie("My_Address");
                        AddressIdentityProfile.Value = identityModel.UserIdentityModel.Address.ToString();
                        Response.Cookies.Add(AddressIdentityProfile);///

                        HttpCookie WorkPlaceIdentityProfile = new HttpCookie("My_WorkPlace");
                        WorkPlaceIdentityProfile.Value = identityModel.UserIdentityModel.WorkPlace.ToString();
                        Response.Cookies.Add(WorkPlaceIdentityProfile);///

                        TempData.Add("social", identityModel);

                        return RedirectToAction("MainPageAfterLogin", "Home", identityModel);
                    }
                    else
                        return Content("res.StatusCode = false :/");
                }
            }
        }
    }
}
