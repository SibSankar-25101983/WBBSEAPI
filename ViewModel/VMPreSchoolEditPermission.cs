using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class VMPreSchoolEditPermission
    {
        public Int64 SlNo { get; set; }
        public string PreSchoolId { get; set; }
        public string SchoolName { get; set; }
        //public string SchoolCode { get; set; }        
        public string DISECode { get; set; }
        public string EntType { get; set; }
        public bool EditPermissionYN { get; set; }
    }
}
