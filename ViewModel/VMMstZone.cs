using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMstZone
    {
        public Int64 SlNo { get; set; }
        public string ZoneId { get; set; }
        public string ZoneName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string EntType { get; set; }
        public string MigYN { get; set; }
    }
}
