using System;
using Common;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ViewModel
{
    public class VMContentDetails
    {
        public string SlNo { get; set; }
        [Required(ErrorMessage = Message.Content.ContentIdRequired)]
        public string ContentId { get; set; }
        //[Required(ErrorMessage = Message.Content.LinkTypeRequired)]
        public string LinkTypeId { get; set; }
        public string LinkType { get; set; }
        public string LinkTypeIcon { get; set; }
        public string MenuCode { get; set; }
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string HeaderText { get; set; }
        public string BodyText { get; set; }
        //[Required(ErrorMessage = Message.Content.ContentRequired)]
        [AllowHtml]
        public string BodyTextOriginal { get; set; }
        public string FooterText { get; set; }
        public string URL { get; set; }
        public string PDFName { get; set; }
        public string PdfFilePath { get; set; }
        public string EntType { get; set; }
        public string SystemYN { get; set; }
        public string UpdationDateTime { get; set; }
        public string NewYN { get; set; }
        public string ArchiveYN { get; set; }
        public string NotificationType { get; set; }
    }
}
