using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMstCircle
    {
        public Int64 SlNo { get; set; }
        public string CircleId { get; set; }
        public string CircleName { get; set; }
        public string BlockId { get; set; }
        public string BlockName { get; set; }
        public string EntType { get; set; }
        public string MigYN { get; set; }
    }
}