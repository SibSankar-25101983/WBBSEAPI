using System;
using System.ComponentModel.DataAnnotations;
using Common;

namespace ViewModel
{
    public class VMMstBlock
    {
        public Int64 SlNo { get; set; }
        public string BlockId { get; set; }
        public string BlockName { get; set; }
        public string IpAddress { get; set; }
        public string EntType { get; set; }
        public string CreatedBy { get; set; }
        public string CreationDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdationDateTime { get; set; }
        public string MigYN { get; set; }
    }
}