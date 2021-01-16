﻿using SportsZone.Helpers.Authority;
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
        private List<GamePositionsHolder> GamesPositions()
        {
            using (Entities context = new Entities())
            {
                List<games_positions> games = (from g in context.games_positions select g).ToList();
                List<GamePositionsHolder> holder = new List<GamePositionsHolder>();
                for (int i = 0; i < games.Count; i++)
                {
                    int id = games[i].gameid;
                    GamePositionsHolder hl = new GamePositionsHolder
                    {
                        pid = games[i].positionid,
                        gid = id,
                        positionname = games[i].position,
                        gamename = (from a in context.games where a.gameid == id select a.gamename).SingleOrDefault()
                    };
                    holder.Add(hl);
                }
                return holder;
            }
        } 
        Helpers.IsExist _IsExist = new Helpers.IsExist();
        Authenticate _auth = new Authenticate();
        // GET: Account
        [ActionName("register")]
        public ActionResult Register()
        {
            return View(GamesPositions());
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
                                userid = (from u in context.users where u.email == rm.Email select u.userid).SingleOrDefault(),
                                roleid = rm.RoleId,
                                photo="no-image.jpg",
                                cover="no-cover.jpg",
                                playername="",
                                age=0,
                                bio=""
                            };
                            context.players.Add(_p);
                            context.SaveChanges();
                        }
                        else if (rm.Type == "Coach")
                        {
                            coachs _c = new coachs
                            {
                                userid = (from u in context.users where u.email == rm.Email select u.userid).SingleOrDefault(),
                                name = rm.Username,
                                age = 40,
                                picture = "no-image.jpg",
                                cover = "no-cover.jpg",
                                positionid = rm.RoleId,
                                bio="Hey, I am coach!"
                                
                            };
                            context.coachs.Add(_c);
                            context.SaveChanges();
                        }
                        else
                        {
                            clubs _c = new clubs
                            {
                                userid = (from u in context.users where u.email == rm.Email select u.userid).SingleOrDefault(),
                                logo = "no-image.jpg",
                                cover = "no-cover.jpg"
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
                    return View("register", GamesPositions());
                }
            }
            else
            {
                ViewBag.Message = "Input valid data!";
                return View("register", GamesPositions());
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
            using (Entities context = new Entities())
            {
                List<users> u = new List<users>();
                u = (List<users>)Session["Data"];
                int uid = u[0].userid;
                if (TempData["Message"] != null)
                    ViewBag.Message = TempData["Message"].ToString();
                return View("index", (from e in context.users where e.userid == uid select e).ToList());
            }            
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
            try
            {
                using (var context = new Entities())
                {
                    var update = context.users.Find(um.userid);
                    if (um.passwd!=null && um.passwd.Length >= 6)
                    {
                        update.passwd = _auth.GenPassword(um.passwd);
                    }
                    update.email = um.email;
                    update.phone = um.phone;
                    context.Entry(update).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    TempData["Message"] = "Changes have been saved!";
                    return RedirectToAction("me");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("me");
            }
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
                    int pid = pp.roleid;
                    List<games_positions> gp = (from g in ctx.games_positions where g.positionid == pid select g).ToList();
                    int gid = gp[0].gameid;
                    string pname = gp[0].position;
                    string gname = (from g in ctx.games where g.gameid == gid select g.gamename).SingleOrDefault();
                    ViewBag.GameName = gid + " | " + pid + " | " + gname + " | " + pname;
                    return PartialView("~/Views/Shared/Partial/playerdetail.cshtml", pp);
                }
            }
            else if (d[0].usertype == "Coach")
            {
                using (var ctx = new Entities())
                {
                    List<coachs> p = (from _p in ctx.coachs where _p.userid == uid select _p).ToList();
                    coachs pp = p[0];
                    int? pid = pp.positionid;
                    List<games_positions> gp = (from g in ctx.games_positions where g.positionid == pid select g).ToList();
                    int gid = gp[0].gameid;
                    string pname = gp[0].position;
                    string gname = (from g in ctx.games where g.gameid == gid select g.gamename).SingleOrDefault();
                    ViewBag.GameName = gid + " | " + pid + " | " + gname + " | " + pname;
                    return PartialView("~/Views/Shared/Partial/coachdetail.cshtml", pp);
                }
            }
            else
            {
                using (var ctx = new Entities())
                {
                    List<clubs> p = (from _p in ctx.clubs where _p.userid == uid select _p).ToList();
                    clubs pp = p[0];
                    return PartialView("~/Views/Shared/Partial/clubdetail.cshtml", pp);
                }
            }
        }
        [Authorized]
        [HttpPost]
        public ActionResult SavePlayers(players pl, HttpPostedFileBase photo1, HttpPostedFileBase photo2)
        {
            try
            {
                using (var context = new Entities())
                {
                    var update = context.players.Find(pl.playerid);
                    if (photo1 != null)
                    {
                        string pic = System.IO.Path.GetFileName(photo1.FileName);
                        string newname = "spz-" + pic.Split('.')[0] + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss")+ "." + pic.Split('.')[1];
                        string path = System.IO.Path.Combine(
                        Server.MapPath("~/uploads/media"), newname);
                        // file is uploaded
                        photo1.SaveAs(path);
                        update.photo = newname;
                    }
                    if (photo2 != null)
                    {
                        string pic = System.IO.Path.GetFileName(photo2.FileName);
                        string newname = "spz-" + pic.Split('.')[0] + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + "." + pic.Split('.')[1];
                        string path = System.IO.Path.Combine(
                        Server.MapPath("~/uploads/media"), newname);
                        // file is uploaded
                        photo2.SaveAs(path);
                        update.cover = newname;
                    }
                    update.age = pl.age;
                    update.bio = pl.bio;
                    update.height = pl.height;
                    update.playername = pl.playername;
                    context.Entry(update).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    TempData["Message"] = "Changes have been saved!";
                    return RedirectToAction("me");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("me");
            }
        }
        [Authorized]
        [HttpPost]
        public ActionResult SaveCoach(coachs pl, HttpPostedFileBase photo1, HttpPostedFileBase photo2)
        {
            try
            {
                using (var context = new Entities())
                {
                    var update = context.coachs.Find(pl.coachid);
                    if (photo1 != null)
                    {
                        string pic = System.IO.Path.GetFileName(photo1.FileName);
                        string newname = "spz-" + pic.Split('.')[0] + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + "." + pic.Split('.')[1];
                        string path = System.IO.Path.Combine(
                        Server.MapPath("~/uploads/media"), newname);
                        // file is uploaded
                        photo1.SaveAs(path);
                        update.picture = newname;
                    }
                    if (photo2 != null)
                    {
                        string pic = System.IO.Path.GetFileName(photo2.FileName);
                        string newname = "spz-" + pic.Split('.')[0] + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + "." + pic.Split('.')[1];
                        string path = System.IO.Path.Combine(
                        Server.MapPath("~/uploads/media"), newname);
                        // file is uploaded
                        photo2.SaveAs(path);
                        update.cover = newname;
                    }
                    update.age = pl.age;
                    update.bio = pl.bio;
                    update.name = pl.name;
                    context.Entry(update).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    TempData["Message"] = "Changes have been saved!";
                    return RedirectToAction("me");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("me");
            }
        }
        [Authorized]
        [HttpPost]
        public ActionResult SaveClub(clubs pl, HttpPostedFileBase photo1, HttpPostedFileBase photo2)
        {
            try
            {
                using (var context = new Entities())
                {
                    var update = context.clubs.Find(pl.clubid);
                    if (photo1 != null)
                    {
                        string pic = System.IO.Path.GetFileName(photo1.FileName);
                        string path = System.IO.Path.Combine(
                        Server.MapPath("~/uploads/media"), "spz-" + pic.Split('.')[0] + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + "." + pic.Split('.')[1]);
                        // file is uploaded
                        photo1.SaveAs(path);
                        update.logo = "spz-" + pic.Split('.')[0] + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + "." + pic.Split('.')[1];
                    }
                    if (photo2 != null)
                    {
                        string pic = System.IO.Path.GetFileName(photo2.FileName);
                        string newname = "spz-" + pic.Split('.')[0] + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + "." + pic.Split('.')[1];
                        // file is uploaded
                        photo2.SaveAs(System.IO.Path.Combine(
                        Server.MapPath("~/uploads/media"), path2: newname));
                        update.cover = newname;
                    }
                    update.clubname = pl.clubname;
                    update.city = pl.city;
                    update.C_state = pl.C_state;
                    update.C_address = pl.C_address;
                    update.lat = pl.lat;
                    update.@long = pl.@long;
                    context.Entry(update).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    TempData["Message"] = "Changes have been saved!";
                    return RedirectToAction("me");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("me");
            }
        }
        public ActionResult logout()
        {
            Session.RemoveAll();
            return RedirectToAction("index","home", new { state="logged-out" });
        }
    }
}