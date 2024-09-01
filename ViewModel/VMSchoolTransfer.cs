using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMSchoolTransfer
    {
        public string SlNo { get; set; }
        [Required(ErrorMessage = Message.School.SchoolIdRequired)]
        public string SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string TransferDate { get; set; }
        public string SubDivisionId { get; set; }
        public string SubDivisionName { get; set; }
        public string EntType { get; set; }
    }
}
