using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMstSchoolManagement
    {
        public Int64 SlNo { get; set; }
        public string SchoolManagementId { get; set; }
        public string SchoolManagement { get; set; }
        public string EntType { get; set; }
        public string MigYN { get; set; }
    }
}