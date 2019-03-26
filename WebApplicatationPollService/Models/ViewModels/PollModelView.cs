using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicatationPollService.Models.ViewModels {
    public class PollModelView {
        [Required]
        [StringLength(50)]
        public string Question { get; set; }
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }        
        public bool UserChecking { get; set; }
        [Required]
        [ListAnswersValidationAtr]
        public List<string> Answers { get; set; }
    }

    public class ListAnswersValidationAtr : ValidationAttribute {
        public override bool IsValid(object obj) {
            List<string> list = obj as List<string>;
            if (list == null) return false;
            if (list.Count > 6) return false;//maximum number of answers is 7
            foreach(var item in list) {
                if (item.Length > 50) return false;//maximum number of letter is 50 
                else if (string.IsNullOrEmpty(item)) list.Remove(item);
            }
            if (list.Count < 2) return false;// In list must be atleast two not empty answers to choice
            return true;

        }
    } 
}

