using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMstScrutiny
    {
        public string ScrutinyTypeId { get; set; }
        public string ScrutinyType { get; set; }
        public float ScrutinyPrice { get; set; }
        public string EntType { get; set; }
    }
}
