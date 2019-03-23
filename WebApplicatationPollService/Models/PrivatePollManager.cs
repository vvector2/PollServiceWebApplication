using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebApplicatationPollService.Models.Entities;

namespace WebApplicatationPollService.Models {
    public class PrivatePollManager {
        public SessionUserPrivatePollEntity session;

        public bool IsAuthorisedByCookie(string cookieCode, ApplicationDbContext db) {
            var session = db.SessionPrivatePoll.Find(cookieCode);
            this.session = session;
            if (session == null || session.DateTime.AddMinutes(10) < DateTime.Now) return false;
            else {
                session.DateTime = DateTime.Now;
                db.Entry(session).State = System.Data.Entity.EntityState.Modified;
                return true;
            }
        }
        public HttpCookie GetSessionCookie(ApplicationDbContext db , PollEntity poll) {
            string idSession = new Guid().ToString();
            db.SessionPrivatePoll.Add(new SessionUserPrivatePollEntity() { sessionID = idSession, DateTime = DateTime.Now, Poll = poll });
            var cookie = new HttpCookie("sesPrivPoll");
            cookie.Expires = DateTime.Now.AddMinutes(10);
            cookie.Value = idSession;
            return cookie;
        }
        //more secure hashing !!
        public string HashPassword(string password) {
            var sha1 = new SHA1CryptoServiceProvider();
            return Convert.ToBase64String(sha1.ComputeHash(Encoding.ASCII.GetBytes(password)));
        } 
        public bool VerifyPassword(string password , string notHashedPassword) {
            var sha1 = new SHA1CryptoServiceProvider();
            if (password == HashPassword(password)) return true;
            else return false;
        }

    }
}