using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsZone.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [ActionName("pricing")]
        public ActionResult Pricing()
        {
            return View();
        }
        [ActionName("players")]
        public ActionResult Players(string id)
        {
            if (id == null)
                return View();
            else return View();
        }
        public ActionResult Coachs()
        {
            return View();
        }
        public ActionResult Clubs()
        {
            return View();
        }
    }
}