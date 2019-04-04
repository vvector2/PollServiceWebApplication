using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplicatationPollService.Models;
using WebApplicatationPollService.Models.Entities;

namespace WebApplicationPollServiceTest.UnitTest {
    [TestClass]
    public class PrivatePollManagerTets {
        private PrivatePollManager manager;
        public PrivatePollManagerTets() {
            manager = new PrivatePollManager();
        }

        [TestMethod]
        public void IsAuthorisedByCookie_SessionDoNoExistInDatabase() {
            var dbMock = new Mock<ApplicationDbContext>();
            dbMock.Setup(x => x.SessionPrivatePoll.Add(It.IsAny<SessionUserPrivatePollEntity>()));
            dbMock.Setup(x => x.SessionPrivatePoll.Find(It.IsAny<string>())).Returns( (SessionUserPrivatePollEntity) null);
            dbMock.Setup(x => x.SaveChanges());
            Assert.AreEqual(false, manager.IsAuthorisedByCookie("some id of session that does not exists", dbMock.Object));
        }
        [TestMethod]
        public void IsAuthorisedByCookie_SessionIsOld() {
            var dbMock = new Mock<ApplicationDbContext>();
            dbMock.Setup(x => x.SessionPrivatePoll.Add(It.IsAny<SessionUserPrivatePollEntity>()));
            dbMock.Setup(x => x.SessionPrivatePoll.Find(It.IsAny<string>())).Returns(new SessionUserPrivatePollEntity() { DateTime=DateTime.Now.Subtract(new TimeSpan(0,11,0)) });
            dbMock.Setup(x => x.SaveChanges());
            Assert.AreEqual(false, manager.IsAuthorisedByCookie("some id of session that exists", dbMock.Object));
        }
        [TestMethod]
        public void IsAuthorisedByCookie_Success() {
            var dbMock = new Mock<ApplicationDbContext>();
            dbMock.Setup(x => x.SessionPrivatePoll.Add(It.IsAny<SessionUserPrivatePollEntity>()));
            dbMock.Setup(x => x.SessionPrivatePoll.Find(It.IsAny<string>())).Returns(new SessionUserPrivatePollEntity() { DateTime = DateTime.Now.Subtract(new TimeSpan(0, 9, 0)) });
            dbMock.Setup(x => x.SaveChanges());
            Assert.AreEqual(true, manager.IsAuthorisedByCookie("some id of session that exists", dbMock.Object));
        }
        [TestMethod]
        public void HashingPassword() {
            string passwd = "ABCDE";
            string hashed = manager.HashPassword(passwd);

            Assert.AreEqual(true, manager.VerifyPassword(hashed, passwd));
            Assert.AreEqual(false, manager.VerifyPassword(hashed, "ABCDEF"));
        }

    }
}
