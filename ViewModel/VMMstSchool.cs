﻿using System;
using Common;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class VMMstSchool
    {
        public string SlNo { get; set; }
        [Required(ErrorMessage = Message.School.SchoolIdRequired)]
        public string SchoolId { get; set; }
        public string IndexNo { get; set; }
        //[Required(ErrorMessage = Message.School.DISECodeRequired)]
        public string DISECode { get; set; }
        [Required(ErrorMessage = Message.School.SchoolNameRequired)]
        public string SchoolName { get; set; }
        [Required(ErrorMessage = Message.School.SubDivisionRequired)]
        public string SubDivisionId { get; set; }
        public string SubDivisionName { get; set; }
        public string DistrictName { get; set; }
        public string ZoneName { get; set; }
        public string CircleId { get; set; }
        public string SchoolTypeId { get; set; }
        public string SchoolCategoryId { get; set; }
        public string SchoolStatusId { get; set; }
        public string SchoolMediumId { get; set; }
        public string SchoolRecognitionId { get; set; }
        public string SchoolManagementId { get; set; }
        //[Required(ErrorMessage = Message.School.SchoolHeadNameRequired)]
        public string SchoolHeadSalutationId { get; set; }
        public string SchoolHeadFirstName { get; set; }
        public string SchoolHeadMiddleName { get; set; }
        public string SchoolHeadLastName { get; set; }
        [Required(ErrorMessage = Message.School.DesignationRequired)]
        public string DesignationId { get; set; }
        public string Designation { get; set; }
        public string PreSchoolId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string GramPanchayet { get; set; }
        public string StdCode { get; set; }
        public string PostOffice { get; set; }
        public string PoliceStation { get; set; }
        public string City { get; set; }
        public string DistrictId { get; set; }
        public string PinCode { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string Website { get; set; }
        public string EmailId { get; set; }
        public string EntType { get; set; }
        public string SchoolEditPermissionStatus { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string OrderDetails { get; set; }
        public int DeletePermissionCount { get; set; }
        public string MigYN { get; set; }
    }
}