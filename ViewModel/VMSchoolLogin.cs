using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMSchoolLogin
    {
        public int? UserId { get; set; }
        [Required(ErrorMessage = Message.ValidateLoginUserName)]
        public string UserName { get; set; }
        [Required(ErrorMessage = Message.ValidateLoginPassword)]
        public string LoginPwd { get; set; }
        public int? GroupId { get; set; }
        public string ActiveYN { get; set; }
        public string IpAddress { get; set; }
        public string LastPwdChangeDate { get; set; }
        public string OldPwdList { get; set; }
        [Required(ErrorMessage = Message.CaptchaError)]
        public string Captcha { get; set; }
        public string IsPwdChange { get; set; }
    }
}
