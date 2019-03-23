﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicatationPollService.Models;
using WebApplicatationPollService.Models.Entities;
using WebApplicatationPollService.Models.ViewModels;

namespace WebApplicatationPollService.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly AdminPollManager AdminPollManager;
        private readonly AdminApplicationUserMenager appUserManager;
        public AdminController() {
            db = new ApplicationDbContext();
            AdminPollManager = new AdminPollManager();
            appUserManager = new AdminApplicationUserMenager(new UserStore<ApplicationUser>(db));
        }

        //show list of all polls
        public ActionResult Polls(FilterOptionModelView filterOptionModelView){
            if (!ModelState.IsValid) return View();
            return View(AdminPollManager.GetPollsFromFilterOption(filterOptionModelView, db));
        }
        //show list of all user
        public ActionResult Users(FilterOptionModelView filterOptionModelView) {
            if (!ModelState.IsValid) return View();
            return View(appUserManager.GetListUser(filterOptionModelView, db));
        }
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePoll( IEnumerable<PollEntity> polls) {
            db.Polls.RemoveRange(polls);
            db.SaveChanges();
            return RedirectToAction("Polls");
        }
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(ApplicationUser user) {
            appUserManager.Delete(user);
            //db.SaveChanges();
            return RedirectToAction("Users");
        }
    }
}