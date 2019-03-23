using System.ComponentModel.DataAnnotations;

namespace WebApplicatationPollService.Models.ViewModels {
    public class FilterOptionModelView {
        [StringLength(50,ErrorMessage = "Phrase is too long!",MinimumLength =3  )]
        public string phrase { get; set; }       
        public bool orderMode { get; set; }
        public string nameSort { get; set; }
        [Range(1,50)]
        public int elements { get; set; }
        [Range(1, int.MaxValue)]
        public int page { get; set; }
    }
}