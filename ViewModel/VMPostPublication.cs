using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMPostPublication
    {
        public int ScrutinySubjectId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectAbbreviation { get; set; }
        public string SubjectCode { get; set; }
        public float Marks { get; set; }
        public string Grade { get; set; }
        public string AppliedYN { get; set; }
    }
}
