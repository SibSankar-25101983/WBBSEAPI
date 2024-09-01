using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMPostPublicationApplicationData
    {
        public Int64 SlNo { get; set; }
        public string RollNo { get; set; }
        public string StudentName { get; set; }
        public string SchoolName { get; set; }
        public string ScrutinyType { get; set; }
        public string TotalSubject { get; set; }
        public string TotalPrice { get; set; }
        public string ApplicationDateTime { get; set; }
        public string PaymentDateTime { get; set; }
    }
}
