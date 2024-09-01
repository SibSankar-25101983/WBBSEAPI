using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMstBooks
    {
        public Int64 SlNo { get; set; }
        [Required(ErrorMessage = Message.Books.BookIdRequired)]
        public string BookId { get; set; }
        public string BookName { get; set; }
        public string BookCode { get; set; }
        public string Class { get; set; }
        public string SchoolMediumId { get; set; }
        public string SchoolMediumName { get; set; }
        public string BookPrice { get; set; }
        public string EntType { get; set; }
        public string MigYN { get; set; }
    }
}
