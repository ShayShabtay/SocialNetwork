﻿using System;
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
        // GET: Identity
        [HttpGet]
        public ActionResult GetUserProfile(string token)
        {
            return View(token);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetUserProfile(UserIdentityModel model, string token)//, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:51639");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("x-auth-token", token);

                    var res = client.GetAsync($"/api/identity/updateUserProfile").Result;

                    if (res.IsSuccessStatusCode == true)
                    {
                        token = res.Content.ReadAsAsync<string>().Result;

                        ////set token  in cookie
                        //HttpCookie userTokenCookie = new HttpCookie("UserToken");
                        //userTokenCookie.Value = token.ToString();
                        //Response.Cookies.Add(userTokenCookie);///

                        return RedirectToAction("MainPageAfterLogin", "Home");
                    }
                    else
                        return Content("res.StatusCode = false :/");
                }
            }
        }





        // GET: Identity/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Identity/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Identity/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Identity/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Identity/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Identity/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Identity/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
