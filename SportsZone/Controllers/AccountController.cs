using SportsZone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsZone.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [ActionName("register")]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel rm)
        {
            try
            {

                ViewBag.Message = "You account has been created, please login to continue!";
                return View("login");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("register");
            }
        }
        public ActionResult Login()
        {
            return View();
        }
    }
}