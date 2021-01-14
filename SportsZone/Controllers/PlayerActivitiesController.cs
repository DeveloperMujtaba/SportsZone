using SportsZone.Helpers.Authority;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsZone.Controllers
{
    [Authorized]
    [OnlyForPlayer]
    public class PlayerActivitiesController : Controller
    {
        // GET: PlayerActivities
        public ActionResult Index()
        {
            return RedirectToAction("me", "account");
        }
        [ActionName("my-enrollments")]
        public ActionResult Enrollments()
        {
            List<users> ul = (List<users>)Session["Data"];
            int uid = ul[0].userid;
            using (var context = new Entities())
            {
                List<player_associations> pa = (from p in context.player_associations
                                                where p.players.users.userid == uid
                                                select p)
                                                .Include("players")
                                                .Include("clubs")
                                                .Include("clubs.users")
                                                .Include("teams")
                                                .Include("games_positions")
                                                .ToList();
                if (TempData["Message"] != null) ViewBag.Message = TempData["Message"];
                return View("Enrollments", pa);
            }
        }
        [HttpPost]
        public ActionResult RemoveEnrollment(string paid, string playerid)
        {
            try
            {
                int pid = int.Parse(playerid);
                int pasid = int.Parse(paid);
                List<users> ul = (List<users>)Session["Data"];
                using (var context = new Entities())
                {
                    int? userid = (from u in context.players where u.playerid == pid select u.userid).SingleOrDefault();
                    if (userid != null && userid == ul[0].userid)
                    {
                        var delas = context.player_associations.Find(pasid);
                        context.Entry(delas).State = EntityState.Deleted;
                        context.SaveChanges();
                        TempData["Message"] = "You have been removed from the club!";
                        return RedirectToAction("my-enrollments");
                    }
                    else
                    {
                        return RedirectToAction("error-401","global");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("my-enrollments");
            }
        }
        [ActionName("enrollment-requests")]
        public ActionResult EnrollmentRequests()
        {
                List<users> ul = (List<users>)Session["Data"];
                int uid = ul[0].userid;
            using (var context = new Entities())
            {
                List<player_associations_request> pa = (from p in context.player_associations_request
                                                        where p.players.users.userid == uid
                                                        select p)
                                                .Include("players")
                                                .Include("clubs")
                                                .Include("clubs.users")
                                                .Include("teams")
                                                .Include("games_positions")
                                                .ToList();
                if (TempData["Message"] != null) ViewBag.Message = TempData["Message"];
                return View("EnrollmentRequests", pa);
            }
        }
        [HttpPost]
        public ActionResult WithDrawEnrollmentRequest(string parid, string playerid)
        {
            try
            {
                int pid = int.Parse(playerid);
                int pasid = int.Parse(parid);
                List<users> ul = (List<users>)Session["Data"];
                using (var context = new Entities())
                {
                    int? userid = (from u in context.players where u.playerid == pid select u.userid).SingleOrDefault();
                    if (userid != null && userid == ul[0].userid)
                    {
                        var delas = context.player_associations_request.Find(pasid);
                        if (delas.parstatus == true)
                        {
                            delas.parstatus = false;
                            context.Entry(delas).State = EntityState.Modified;
                            context.SaveChanges();
                            TempData["Message"] = "Your request has been withdrawn!";
                        }
                        else
                        {
                            TempData["Message"] = "This request has already been witdrawn or processed!";
                        }
                        return RedirectToAction("enrollment-requests");
                    }
                    else
                    {
                        return RedirectToAction("error-401", "global");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("enrollment-requests");
            }
        }
        public ActionResult CreateEnrollmentRequest()
        {
            List<users> ul = (List<users>)Session["Data"];
            int userid = ul[0].userid;
            int playerid;
            using (var context = new Entities())
            {
                playerid = (from pi in context.players
                                where pi.userid == userid
                                select pi.playerid).SingleOrDefault();
            }
            if (TempData["Message"] != null) ViewBag.Message = TempData["Message"];
            return View("CreateEnrollmentRequest", playerid);
        }
        [HttpPost]
        public ActionResult CreateEnrollmentRequest(player_associations_request par)
        {
            if (ModelState.IsValid)
            {
                using (var context = new Entities())
                {
                    context.player_associations_request.Add(par);
                    context.SaveChanges();
                    TempData["Message"] = "New enrollment requests has been made!";
                }
            }
            else TempData["Message"] = "Please input valid data!";
            return RedirectToAction("enrollment-requests");
        }
    }
}