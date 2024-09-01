using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMProfileData
    {
        public string ProfileData { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string OldPwdList { get; set; }
        public string LoginPwd { get; set; }
        [Required(ErrorMessage = Message.ProfileVerification.SalutationIDRequired)]
        public string SalutationId { get; set; }
        [Required(ErrorMessage = Message.ProfileVerification.FirstNameRequired)]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = Message.ProfileVerification.LastNameRequired)]
        public string LastName { get; set; }
        [Required(ErrorMessage = Message.ProfileVerification.EmailIdRequired)]
        public string EmailId { get; set; }
        [Required(ErrorMessage = Message.ProfileVerification.ContactNoRequired)]
        public string MobileNo { get; set; }
    }
}