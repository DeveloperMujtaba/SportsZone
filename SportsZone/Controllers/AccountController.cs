using SportsZone.Helpers.Authority;
using SportsZone.Models;
using SportsZone.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsZone.Controllers
{

    public class AccountController : Controller
    {
        Helpers.IsExist _IsExist = new Helpers.IsExist();
        Authenticate _auth = new Authenticate();
        // GET: Account
        [ActionName("register")]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel rm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var context = new Entities())
                    {
                        users _u = new users
                        {
                            usertype = rm.Type,
                            username = rm.Username,
                            passwd = _auth.GenPassword(rm.Password),
                            email = rm.Email,
                            phone = rm.Username,
                            C_date = DateTime.Now,
                            ispayment = false,
                            isemail = false,
                            isphone = false,
                            C_status = true
                        };
                        context.users.Add(_u);
                        context.SaveChanges();
                        if (rm.Type == "Player")
                        {
                            players _p = new players
                            {
                                userid = (from u in context.users where u.email == rm.Email select u.userid).SingleOrDefault()
                            };
                            context.players.Add(_p);
                            context.SaveChanges();
                        }
                        else if (rm.Type == "Coach")
                        {
                            coachs _c = new coachs
                            {
                                userid = (from u in context.users where u.email == rm.Email select u.userid).SingleOrDefault()
                            };
                            context.coachs.Add(_c);
                            context.SaveChanges();
                        }
                        else
                        {
                            clubs _c = new clubs
                            {
                                userid = (from u in context.users where u.email == rm.Email select u.userid).SingleOrDefault()
                            };
                            context.clubs.Add(_c);
                            context.SaveChanges();
                        }
                        ViewBag.Message = "Your account has been created, please login to continue!";
                        return RedirectToAction("login");
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    Console.WriteLine(ex);
                    throw;
                    //return View("register");
                }
            }
            else
            {
                ViewBag.Message = "input valid data!";
                return View("register");
            }
        }
        [ActionName("login")]
        public ActionResult Login()
        {
            return View();
        }
        [ActionName("login")]
        [HttpPost]
        public ActionResult Login(LoginModel lm)
        {
            if (ModelState.IsValid && _IsExist.User(lm.Email) && _auth.Verify(lm.Email, lm.Password))
            {
                try
                {
                    using (Entities context = new Entities())
                    {
                        List<users> data = new List<users>();
                        Session["IsLoggedIn"] = true;
                        data = (from e in context.users where e.email == lm.Email select e).ToList();
                        Session["Data"] = data;
                        return RedirectToAction("me");
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View("login");
                }
            }
            else
            {
                ViewBag.Message = "Invalid data or user doesn't exist!";
                return View("login");
            }
        }
        [Authorized]
        [ActionName("me")]
        public ActionResult Me(string id)
        {
            List<users> _u = new List<users>();
            _u = (List<users>)Session["Data"];
            return View("index", _u);
        }
        [Authorized]
        public ActionResult Index()
        {
            return RedirectToAction("me");
        }
        [Authorized]
        [HttpPost]
        public ActionResult UpdateUser(users um)
        {

            return View("me");
        }
        [Authorized]
        [HttpGet]
        public PartialViewResult GetPartial()
        {
            List<users> d = (List<users>)Session["Data"];
            int uid = d[0].userid;
            if (d[0].usertype == "Player")
            {
                using (var ctx = new Entities())
                {
                    List<players> p = (from _p in ctx.players where _p.userid == uid select _p).ToList();
                    players pp = p[0];
                    return PartialView("~/Views/Shared/Partial/playerdetail.cshtml", pp);
                }
            }
            else if (d[0].usertype == "Coach")
            {
                using (var ctx = new Entities())
                {
                    List<coachs> p = (from _p in ctx.coachs where _p.userid == uid select _p).ToList();
                    return PartialView("~/Views/Shared/Partial/coachdetail.cshtml", p);
                }
            }
            else
            {
                using (var ctx = new Entities())
                {
                    List<clubs> p = (from _p in ctx.clubs where _p.userid == uid select _p).ToList();
                    return PartialView("~/Views/Shared/Partial/clubdetail.cshtml", p);
                }
            }
        }
    }
}