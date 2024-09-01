using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMstUser
    {
        public string SlNo { get; set; }
        [Required(ErrorMessage = Message.User.UserIdRequired)]
        public string UserId { get; set; }
        public string SalutationId { get; set; }
        [Required(ErrorMessage = Message.User.FirstNameRequired)]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = Message.User.LastNameRequired)]
        public string LastName { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        [Required(ErrorMessage = Message.User.DesignationRequired)]
        public string DesignationId { get; set; }
        public string Designation { get; set; }
        public string DesignEditableYN { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string EntType { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string UserTypeId { get; set; }
        public string UserType { get; set; }
        public string UserTypeName { get; set; }
        public string IsActive { get; set; }
        public string ActiveYN { get; set; }
    }
}
