using System.Collections.Generic;

namespace WebApplicatationPollService.Models.ViewModels {
    public class PollModelView {
        public string Question { get; set; }
        public string Password { get; set; }
        public bool UserChecking { get; set; }
        public List<string> Answers { get; set; }
    }
}