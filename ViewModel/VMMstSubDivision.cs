using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMstSubDivision
    {
        public Int64 SlNo { get; set; }
        public string SubDivisionId { get; set; }
        public string SubDivisionName { get; set; }
        public string DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string EntType { get; set; }
        public string IndexInitial { get; set; }
        public string MaxIndexNo { get; set; }
        public string MigYN { get; set; }
    }
}