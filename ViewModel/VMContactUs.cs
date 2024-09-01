using System;
using System.ComponentModel.DataAnnotations;
using Common;

namespace ViewModel
{
    public class VMContactUs
    {
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Captcha { get; set; }        
        public string IpAddress { get; set; }
    }
}
