using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplicatationPollService.Models;
using Microsoft.AspNet.Identity;
using WebApplicatationPollService.Models.Entities;
using WebApplicatationPollService.Models.ViewModels;

namespace WebApplicatationPollService.Controllers {
    public class HomeController : Controller {

        private readonly ApplicationDbContext db;
        private readonly PollManager pollManager;

        private readonly ApplicationUserManager appUserManager;
        public HomeController() {
            db = new ApplicationDbContext();
            pollManager = new PollManager();
            appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        //home page
        public ActionResult Index() {
            return View();
        }

        //list of all polls 
        public ActionResult Explore(FilterOptionModelView filterOptionModelView) {           
            if (ModelState.IsValid) return View(pollManager.GetPollsFromFilterOption(filterOptionModelView, db));
            else return View("Error");
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreatePoll (PollModelView pollModelView) {
            if (!ModelState.IsValid) return View("error");           
            var user =  appUserManager.FindById(User.Identity.GetUserId());
            if (user.CreatedPoll.Count <= 10) db.Polls.Add(new PollEntity(pollModelView,user));
            else return View("error");//user can't create more than 10 poll. He must delete some old one
            db.SaveChanges();
            return RedirectToAction("PollVote");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VotePoll(VotePollModelView votePollModelView) {
            if (!ModelState.IsValid) return View("erorr");
            var poll = db.Polls.First(x => x.Id == votePollModelView.IdPoll);
            var pollAnswer = db.PollAnswers.First(x=> x.Id== votePollModelView.IdAnswer);
            if (poll == null) return new HttpNotFoundResult();
            if (!IsPrivPollAuthorised(poll)) return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
            
            if (poll.UserChecking ) {//checking if user is logged in
                if (!User.Identity.IsAuthenticated) return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
                else {
                    var user =appUserManager.FindById(User.Identity.GetUserId());
                    if (user.VotedPoll.Any(x => x.Poll == poll)) return View("erorr");//if user have already voted on this poll return error
                    else user.VotedPoll.Add(pollAnswer);//adding pollAnswer entity to list of Votedpoll of user
                }
            }
            //to do ip or cookie
            pollManager.Vote(votePollModelView, db);
            db.SaveChanges();
            return RedirectToAction("PollResult");
        }
        public ActionResult PollResult(int id) {
            return GetPollAndReturnView("PollResult", id);
        }
        public ActionResult PollVote(int id) {
            return GetPollAndReturnView("PollVote", id);
        }

        //Get access to the poll and put as a model in View 
        [NonAction]
        private ActionResult GetPollAndReturnView(string nameOfView, int id) {
            PollEntity poll = db.Polls.First(x => x.Id == id);
            if (poll == null) return new HttpNotFoundResult();//check if the poll exists
            if (!IsPrivPollAuthorised(poll)) return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
            return View(nameOfView, poll);
        }
        
        [NonAction]
        private bool IsPrivPollAuthorised(PollEntity poll) {
            if (!string.IsNullOrEmpty(poll.Password)) {
                var controller = DependencyResolver.Current.GetService<PrivatePollController>();
                controller.ControllerContext = new ControllerContext(Request.RequestContext, this);
                return controller.IsRequestAuthorised(poll);
            } else return true;
        }


    }
}