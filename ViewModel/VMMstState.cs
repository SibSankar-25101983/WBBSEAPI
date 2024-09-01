using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMstState
    {
        public Int64 SlNo { get; set; }        
        public string StateId { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string EntType { get; set; }
    }
}
