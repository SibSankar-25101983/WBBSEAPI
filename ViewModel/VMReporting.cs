using System;
using System.ComponentModel.DataAnnotations;
using Common;

namespace ViewModel
{
    public class VMReporting
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string ReportType { get; set; }
        public string CookieValue { get; set; }
    }
}
