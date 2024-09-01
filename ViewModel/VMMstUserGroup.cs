using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common;

namespace ViewModel
{
    public class VMAdminUserGroup
    {
        public VMMstUserGroup VMMstUserGroup { get; set; }
        public List<VMRoleDetails> VMRoleDetails { get; set; }
    }

    public class VMMstUserGroup
    {
        public string GroupId { get; set; }
        [Required(ErrorMessage = Message.UserGroup.UserGroupNameRequired)]
        [StringLength(100, ErrorMessage = Message.UserGroup.UserGroupNameMaxLength)]
        public string GroupName { get; set; }
        public string AccessType { get; set; }
        public string SystemYN { get; set; }
        public string ActiveYN { get; set; }
        public string UserTypeId { get; set; }
        public string UserType { get; set; }
        public string IpAddress { get; set; }
        public string EntType { get; set; }
        public string CreatedBy { get; set; }
        public string CreationDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdationDateTime { get; set; }
    }

    public class VMRoleDetails
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string GroupId { get; set; }
        public string ViewYN { get; set; }
        public string AddYN { get; set; }
        public string EditYN { get; set; }
        public string DeleteYN { get; set; }
        public string ReportYN { get; set; }
        public string SystemYN { get; set; }
        public string EntType { get; set; }
        public string CreatedBy { get; set; }
        public string CreationDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdationDateTime { get; set; }
    }
}
