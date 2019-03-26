using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicatationPollService.Models;
using WebApplicatationPollService.Models.Entities;

namespace WebApplicatationPollService.Controllers
{
    public class PrivatePollController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ApplicationUserManager appUserManager;
        public PrivatePollController() {
            db = new ApplicationDbContext();
            appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }
        public ActionResult PrivatePollAuth(int id) {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public ActionResult PrivatePollAuht(int id, string password) {
            //check if poll exist
            var poll = db.Polls.Find(id);
            if (poll == null) return new HttpNotFoundResult();
            var privatePollManager = new PrivatePollManager();
            if (privatePollManager.VerifyPassword(poll.Password, password)) {
                Response.Cookies.Add(privatePollManager.GetSessionCookie(db, poll));//give user session that last 10 minutes
                return RedirectToAction("PollVote");
            } else {
                return RedirectToAction("PrivatePollAuth");
            }
        }

        [NonAction]
        public bool IsRequestAuthorised(PollEntity poll) {
            var user = appUserManager.FindById(User.Identity.GetUserId());
            if (poll.UserCreator == user) return true;
            var privatePollManager = new PrivatePollManager();
            if (!privatePollManager.IsAuthorisedByCookie(Request.Cookies["privPoll"].Value, db)) return false;
            else Response.Cookies["sesPrivPoll"].Expires = DateTime.Now.AddMinutes(10);
            
            return true;
        }
    }
}