using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMBoardDesk
    {
        [Required(ErrorMessage = Message.Content.ContentIdRequired)]
        public string ContentId { get; set; }
        public string HeaderText { get; set; }
        public string BodyTextPartial { get; set; }
        [Required(ErrorMessage = Message.Content.ContentRequired)]
        public string BodyText { get; set; }
        public string ImagePath { get; set; }
        public string EntType { get; set; }
    }
}
