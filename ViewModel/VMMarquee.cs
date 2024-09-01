using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMarquee
    {
        [Required(ErrorMessage = Message.Content.ContentIdRequired)]
        public string ContentId { get; set; }
        [Required(ErrorMessage = Message.Content.ContentRequired)]
        public string BodyText { get; set; }
        public string EntType { get; set; }
    }
}
