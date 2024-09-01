using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMstGeographicalDistrict
    {
        public string DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string AbbreviationName { get; set; }
        public string StateId { get; set; }        
        public string EntType { get; set; }
    }
}
