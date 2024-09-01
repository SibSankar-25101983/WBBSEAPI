using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMCandidateLogin
    {
        [Required(ErrorMessage = Message.Candidate.RollNoRequired)]
        public string RollNo { get; set; }
        [Required(ErrorMessage = Message.Candidate.SchoolIndexNoRequired)]
        public string SchoolIndexNo { get; set; }
        [Required(ErrorMessage = Message.Candidate.NameRequired)]
        public string StudentName { get; set; }
        [Required(ErrorMessage = Message.Candidate.DOBRequired)]
        public string DOB { get; set; }
        [Required(ErrorMessage = Message.CaptchaError)]
        public string Captcha { get; set; }
    }
}
