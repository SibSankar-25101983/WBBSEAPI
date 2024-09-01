﻿using System;
using Common;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ViewModel
{
    public class VMInbox
    {
        public string SlNo { get; set; }
        [Required(ErrorMessage = Message.Inbox.InboxIdRequired)]
        public string InboxId { get; set; }
        public string LinkTypeId { get; set; }
        public string LinkType { get; set; }
        public string LinkTypeIcon { get; set; }
        public string HeaderText { get; set; }
        public string BodyText { get; set; }
        [AllowHtml]
        public string BodyTextOriginal { get; set; }
        public string BodyTextUnread { get; set; }
        public string FooterText { get; set; }
        public string URL { get; set; }
        public string PDFName { get; set; }
        public string PdfFilePath { get; set; }
        public string EntType { get; set; }
        public string UpdationDateTime { get; set; }
        public string NewYN { get; set; }
        public string ArchiveYN { get; set; }
        public string ReadYN { get; set; }
        public string SchoolCount { get; set; }
        public string SchoolIdList { get; set; }
    }
}