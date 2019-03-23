using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicatationPollService.Models.Entities {
    public class SessionUserPrivatePollEntity {
        public DateTime DateTime { get; set; }
        public string sessionID { get; set; }

        public virtual PollEntity Poll { get; set; }
    }
}