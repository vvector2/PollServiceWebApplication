using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicatationPollService.Models.Entities {
    public class PollAnswersEntity {
        public int Id { get; set; }
        public string Answers { get; set; }
        public int Votes { get; set; }
        public virtual PollEntity Poll { get; set; }
    }
}