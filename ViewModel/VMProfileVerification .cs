using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class VMProfileVerification
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string OldPwdList { get; set; }
        public string IsPwdChange { get; set; }
        public string EMailId { get; set; }
        public string EmailVerifiedYN { get; set; }
        public string ContactNo { get; set; }
        public string PhoneNo { get; set; }
        public string StdCode { get; set; }
        public string ContactNoVerifiedYN { get; set; }
        public string OTP { get; set; }
        public string OTPType { get; set; }
        public string OTPEmailSentCount { get; set; }
        public string OTPContactNoSentCount { get; set; }
        public string OperationType { get; set; }
        public string SalutationId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string VerificationFor { get; set; }
    }
}
