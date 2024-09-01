using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMstSchoolRecognition
    {
        public Int64 SlNo { get; set; }
        public string SchoolRecognitionId { get; set; }
        public string RecognitionStatus { get; set; }
        public string EntType { get; set; }
        public string MigYN { get; set; }
    }
}
