using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicatationPollService.Models;

namespace WebApplicatationPollService.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ApplicationUserManager appUserManager;
        public UserController() {
            db = new ApplicationDbContext();
            appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        //show user        
        public ActionResult UserProfile(string name){            
            return View(appUserManager.FindByName(name));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public ActionResult DeletePoll(int id) {
            var user = appUserManager.FindById(User.Identity.GetUserId());
            var poll = db.Polls.Find(id);
            if (poll == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (user.CreatedPoll.Any(x => x.Id == id))
                db.Polls.Remove(poll);
            else return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            db.SaveChanges();
            return RedirectToAction("UserProfile", new { name = user.UserName });
            
        }
    }
}