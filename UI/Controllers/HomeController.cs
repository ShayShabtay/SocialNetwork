using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using UI.Models;
using System.Net.Http.Headers;
using AuthenticationCommon.Models;
using static System.Net.WebRequestMethods;
using System.Net.Http;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult MainPageAfterLogin()
        {
            string UserCookie = Request.Cookies["UserProfile"].Value;
            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}