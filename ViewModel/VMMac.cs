using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMac
    {
        public string SlNo { get; set; }
        public string TblId { get; set; }
        public string MachineId { get; set; }
        public string ComputerName { get; set; }
        public string AuthorizedYN { get; set; }
        public string EntType { get; set; }
        public string IpAddress { get; set; }
        public string CreationDateTime { get; set; }
    }
}
