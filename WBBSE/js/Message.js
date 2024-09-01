/****COMMON MESSAGES*/
function setET(type) {
    var mode = "";
    switch (type) {
        case 1:
            mode = "I";
            break;
        case 2:
            mode = "E";
            break;
        case 3:
            mode = "D";
            break;
        case 4:
            mode = "L";
            break;
        case 5:
            mode = "U";
            break;
    }
    return mode;
}
function DefaultVal() {
    return "-1";
}
function EmptyVal() {
    return "";
}
function Msg_Confirm() {
    return "Do You Want To Proceed?";
}
function RequiredMsg() {
    return "This field can not be blank.";
}
function Default_Password() {
    return "123456";
}
function Password_Policy() {
    return "<span class='text-dark'># Password should contain at least:- <br/>* One Upper Case Letter<br/>* One Lower Case Letter<br/>* One Digit<br/>* One Special Character.<br/># Minimum Password Length : 8.<br/># Last Three Passwords Not Allowed.<br/># Allowed Special Characters :- <br/>@ $ # ! % * ? & - _</span>";
}
function ErrVal() {
    return "1";
}
function isDuplicateEmailId() {
    return "1";
}
function isDuplicateContactNo() {
    return "2";
}
function isDuplicateAdminUser() {
    return "3";
}
function isDuplicateSeatEntry() {
    return "1";
}
function isErrorC() {
    return "-1";
}
function OperationError() {
    return "Something Went Wrong. Current Operation Cannot Be Performed.";
}
function FileSizeExceeded() {
    return "File size should not exceeds 10 MB !";
}

function ConfirmDelete() {
    return "Do You Want To Proceed?";
}
function Required() {
    return "Required.";
}
function OTP_Required() {
    return "OTP Required.";
}
function Old_Password_Required() {
    return "Old Password Required.";
}
function New_Password_Required() {
    return "New Password Required.";
}
function Confirm_Password_Required() {
    return "Confirm Password Required.";
}
function Password_Mismatch() {
    return "Password Mismatch.";
}
function Password_Policy_Required() {
    return "Password Policy Not Met.";
}
function Old_New_Password() {
    return "Old Password Not Allowed.";
}
function Name_Required() {
    return "Name Required.";
}
function FirstName_Required() {
    return "First Name Required.";
}
function LastName_Required() {
    return "Last Name Required.";
}
function EmailId_Required() {
    return "Email Id Required.";
}
function ContactNo_Required() {
    return "Contact No Required.";
}
function Address_Required() {
    return "Address Required.";
}
function UserType_Sel_Required() {
    return "Group Type Selection Required.";
}
function Desig_Sel_Required() {
    return "Designation Selection Required.";
}
function Group_Sel_Required() {
    return "Group Selection Required.";
}
function State_Sel_Required() {
    return "State Selection Required.";
}
function Invalid_EmailId() {
    return "Invalid Email Id.";
}
function Invalid_ContactNo() {
    return "Mobile No should be 10 digit.";
}
function Invalid_StdCode() {
    return "Enter valid Std Code.";
}
function Invalid_PhoneNo() {
    return "Enter valid Phone No.";
}
function Invalid_OTP() {
    return "Invalid OTP.";
}
function Invalid_Pincode() {
    return "Pin code should be 6 digit.";
}
function Duplicate_EmailId() {
    return "Duplicate Email Id.";
}
function Duplicate_ContactNo() {
    return "Duplicate Contact No.";
}
function Duplicate_AdminUser() {
    return "Duplicate Institute Admin User.";
}
function Duplicate_Seat() {
    return "Duplicate Seat Entry.";
}
/**************************************/

/****LOGIN RELATED MESSAGES****/
function UserName_Required() {
    return "*";
}
function Password_Required() {
    return "*";
}
function Captcha_Required() {
    return "*";
}
/******************************/

/****ADMIN USER GROUP RELATED MESSAGES*/
function AUG_GroupName_Required() {
    return "Group Name Cannot Be Empty.";
}
function AUG_GroupName_Max_Length() {
    return "Group Name Maximum Length 100 Characters.";
}
function AUG_Invalid_GroupName() {
    return "Invalid Group Name";
}
function AUG_DeleteNotPermitted() {
    return "User Is Assigned Against This Group. User Group Cannot Be Deleted.";
}
/**************************************/
/*ROLE MASTER RELATED MESSAGES*/
function ARM_Rolename_Required() {
    return "Role Name Required.";
}
function ARM_URL_Required() {
    return "URL Required.";
}
function ARM_Parent_Required() {
    return "Parent Selection Required.";
}
/******************************/

/*SCHOOL RELATED WORK*/
function SchoolName_Required() {
    return "School Name Required.";
}
function SubDivision_Selection_Required() {
    return "Sub-Division Selection Required.";
}
/*********************/

/*MARQUEE RELATED MESSAGES*/
function Marquee_Text_Required() {
    return "Marquee Text Required.";
}
function ForSite_Required() {
    return "For Site Required.";
}
/******************************/

function UI_Image_Signature_Required() {
    return "Photograph & signature required.";
}
function UI_Image_Required() {
    return "Photograph required.";
}
function UI_Signature_Required() {
    return "Signature required.";
}
function UI_Image_Ext() {
    return "Photograph Allowed Extension : jpg/jpeg.";
}
function UI_Signature_Ext() {
    return "Signature Allowed Extension : jpg/jpeg.";
}
function UI_Image_Size() {
    return "Photograph is not within allowed size.";
}
function UI_Signature_Size() {
    return "Signature is not within allowed size.";
}

/*************************NEWER APPROACH FOR MESSAGE HANDLING***************************/
var GeneralMsg = {
    Validation: {
        InvalidMobileNo: "Invalid Mobile No",
        InvalidPhoneNo: "Invalid Phone No",
        InvalidSTDCode: "Invalid STD Code",
        InvalidFaxNo: "Invalid Fax No",
        InvalidEmailId: "Invalid Email Id",
        InvalidURL: "Invalid Website/URL",
        InvalidOTP: "Invalid OTP",
        MigDataDelete: "Migrated Data Delete Not Allowed."
    },
    Error: {
        DashboardStatistics: "ERROR. REGISTRATION STATISTICS LOAD FAILED.",
        SchoolProfileData: "ERROR. PROFILE LOAD FAILED."
    }
};
//school related messages
var School = {
    Required: {
        SchoolName: "School Name Required",
        DISECode: "DISE Code Required",
        IndexNo: "Index No Required",
        SubDivision: "Sub-Division Selection Required",
        SchoolType: "School Type Selection Required",
        SchoolCategory: "School Category Selection Required",
        SchoolStatus: "School Status Selection Required",
        SchoolMedium: "School Medium Selection Required",
        SchoolRecognization: "School Recognization Selection Required",
        SchoolManagement: "School Management Selection Required",
        Designation: "Designation Selection Required",
        SchoolDirectory: "School Name/Index No Required",
        PreSchoolId: "Junior School Id Required",
        PreSchoolName: "Junior School Name Required",
        StreetName: "Street/Locality Name Required",
        AreaName: "Area Name Required",
        City: "City Required",
        PinCode: "Pin Code Required",
        MobileNo: "Mobile No Required",
        EmailId: "Email Id Required",
        PhoneNoOrFaxNo: "Either Phone No/Fax No Required (STD Code Present)",
        STDCodePhoneNo: "STD Code Required (Phone No Present)",
        STDCodeFaxNo: "STD Code Required (Fax No Present)",
        PhoneNoSTDCode: "Phone No Required (STD Code Present)",
        OrderNo: "Order No Required",
        OrderDate: "Order Date Required",
        SchoolSelectionTransferView: "Select a school to view transfer record",
        SchoolSelectionTransferDelete: "Select a school before transfer delete",
        SchoolSelectionTransferSave: "School selection required before transfer",
        SubDivisionSelectionTransferSave: "Sub-division selection required before transfer"
    },
    Validation: {
        InvalidDISECode: "Invalid DISE Code.",
        InvalidIndexNo: "Invalid Index No.",
        InvalidSchoolName: "Invalid School Name",
        InvalidStreetName: "Invalid Street/Locality Name",
        InvalidAreaName: "Invalid Area Name",
        InvalidGramPanchayetName: "Invalid Gram Panchayet Name",
        InvalidPOName: "Invalid Post Office Name",
        InvalidPSName: "Invalid Police Station Name",
        InvalidCityName: "Invalid City Name",
        InvalidPinCode: "Invalid Pin Code",
        EditNotPermitted: "School Not Locked. Selected School Edit Not Permitted",
        DeleteNotPermitted: "Delete Not Permitted for this school. Reason: Registration Data Present for Selected School.",
        DeleteNotPermittedCountMsg: "Delete Not Permitted for this school. Reson: You have reached maximum count.",
        UnLockNotPermitted: "Un-lock not permitted for the selected school",
        InvalidOrderNo: "Invalid Order No",
        InvalidOrderDate: "Invalid Order Date (Date Format : dd-MM-yyyy)"
    },
    Info: {
        SchoolHeadNameMissing: "School Head name not present. \nPlease note that School User will be created without name. \nDo you want to proceed?",
        SchoolDataMissing: "Some of the field(s) are missing data. \nOnce the school profile is locked, you will not be able to make further change. \nDo you want to proceed anyway?",
        SchoolProfileLock: "Once the school profile is locked, you will not be able to make further change \nDo you want to proceed anyway?",
        UserCreation: "<strong>INFO : </strong>School Head login will be created with school creation. User Name : Index No & Password : Test@123",
        UnLockMsg: "<strong>NOTE :</strong> Once you click to <strong>Un-Lock</strong> button, the school head will be able to edit school profile.",
        PreSchoolUserCreation: "<strong>INFO : </strong>School Head login will be created with school creation. User Name : School Code & Password : Test@123",
        SchoolProfileUnlockAdmin: "Once un-locked, selected school will not be able to edit school profile. Instead, WBBSE admin will be able to edit school profile. \nDo you want to proceed anyway?",
        OrderDetails: "Enter Government order no. & date or Gazette details.",
        SchoolTransferSave: "<strong>WARNING! </strong>Please check selected sub-division carefully before clicking on <b>Save</b> button. Transfer may have advarse effect on school data.",
        SchoolTransferSaveWarning: "You are about to transfer sub-division of # to $. \nPlease check selected sub-division carefully. Transfer may have advarse effect on school data. \nDo you want to proceed?",
        SchoolTransferDeleteWarning: "You are about to delete last transfer record of #. \nPlease check necessary details carefully. Delete may have advarse effect on school data. \nDo you want to proceed?",
    }
};

//user related messages
var User = {
    Required: {
        Salutation: "Salutation Selection Required",
        FirstName: "First Name Required.",
        LastName: "Last Name Required.",
        Designation: "Designation Selection Required.",
        ContactNo: "Mobile No. Required.",
        EmailId: "Email Id Required."
    },
    Validation: {
        InvalidFirstName: "Invalid First Name",
        InvalidMiddleName: "Invalid Middle Name",
        InvalidLastName: "Invalid Last Name"
    }
};

/*BLOCK MASTER RELATED MESSAGES*/
var Block = {
    Required: {
        BlockName: "Block Name Required."
    },
    Validation: {
        BlockDeleteNotPermitted: "Block is mapped with circle. Block cannot be deleted."
    }
};
/******************************/

//Circle related messages
var Circle = {
    Required: {
        BlockName: "Block Name Required.",
        CircleName: "Circle Name Required."
    },
    Validation: {
        CircleDeleteNotPermitted: "Circle is mapped with school. Circle cannot be deleted."
    }
};

//Zone related messages
var Zone = {
    Required: {
        StateName: "State Name Required.",
        ZoneName: "Zone Name Required."
    },
    Validation: {
        ZoneDeleteNotPermitted: "Zone is mapped with district. Zone cannot be deleted.",
        ZoneTransferNotPermitted: "Zone is transfered. State name cannot be changed."
    }
};

//District related messages
var District = {
    Required: {
        ZoneName: "Zone Selection Required.",
        IndexInitial: "Index Initial Selection Required.",
        DistrictName: "District Name Required."
    },
    Validation: {
        DistrictDeleteNotPermitted: "District is mapped with sub-division. District cannot be deleted.",
        DistrictTransferNotPermitted: "District is transfered. Zone name cannot be changed."
    }
};

//SubDivision related messages
var SubDivision = {
    Required: {
        DistrictName: "District Selection Required.",
        SubDivisionName: "Sub-Division Name Required."
    },
    Validation: {
        SubDivisionDeleteNotPermitted: "Sub-Division is mapped with School. Sub-Division cannot be deleted.",
        SubDivisionTransferNotPermitted: "Sub-Division is transfered. District name cannot be changed."
    }
};

//School Recognition related messages
var SchoolParameter = {
    Required: {
        RecognitionStatus: "School Recognition Status Required.",
        SchoolManagement: "School Management Required."
    },
    Validation: {
        RecognitionStatusDeleteNotPermitted: "School Recognition Status is mapped with School/Junior School. School Recognition Status cannot be deleted.",
        SchoolManagementDeleteNotPermitted: "School Management is mapped with School/Junior School. School Management cannot be deleted."
    }
};

//Contact Us related messages
var ContactUs = {
    Required: {
        Name: "Name Required.",
        EmailId: "Email Id Required",
        MobileNo: "Mobile No Required",
        Subject: "Subject Required",
        Message: "Message Required"

    },
    Validation: {
        InvalidName: "Invalid Name",
        InvalidSubject: "Invalid Subject",
        InvalidMessage: "Invalid Message"
    }
};

var Image = {
    Required: {
        ImageSelection: "Image(s) Selection Required."
    },
    Validation: {
        ImageExt: "Allowed Image Type : jpg/jpeg/png.",
        ImageSize: "Maximum Allowed Size"
    }
};

var Content = {
    LinkType: {
        URL: "URL",
        PDF: "PDF",
        CONTENT: "CONTENT"
    },
    PdfAllowedSize: 4194304,
    EBookPdfAllowedSize: 104857600,
    Validation: {
        ContentRequired: "Content Required.",
        InvalidContent: "Invalid Content.",
        NotificationTypeRequired: "Notification Type Required.",
        LinkTypeRequired: "Link Type Required.",
        URLRequired: "URL Required.",
        InvalidURL: "Invalid URL.",
        PDFRequired: "PDF Selection Required.",
        PdfFileExt: "Allowed File Type : pdf.",
        PdfFileSize: "Maximum file size allowed : 4mb.",
        EBookPdfFileSize: "Maximum file size allowed for E-Book : 100mb.",
        MaxFileSizeBookPdf: "Maximum file size allowed : 100mb."        
    }
};

var Grid = {
    NoData: "No records found."
};

var Login = {
    Validation: {
        UserNameRequired: "User Name Required.",
        PasswordRequired: "Password Required.",
        CaptchaRequired: "Captcha Required."
    }
};

var StudentRegistration = {
    Required: {
        RegistrationYear: "Registration Year Required.",
        MadhyamikParikshaYear: "Madhyamik Pariksha Year Required.",
        SchoolIndex: "School Index Required.",
        FormNo: "Form No Required.",
        SchoolName: "School Name Required.",
        StudentName: "Student Name Required.",
        FathersName: "Fathers Name Required.",
        MothersName: "Mothers Name Required.",
        GuardiansName: "Guardians Name Required.",
        Address: "Address Required.",
        Pin: "Pin Required.",
        ContactNo: "Contact No Required.",
        NationalityCode: "Nationality Code Required.",
        ReligionCode: "Religion Code Required.",
        CasteCode: "Caste Code Required.",
        PhysicallychallengedCode: "Physically challenged Code Required.",
        SexCode: "Sex Code Required.",
        DateOfBirth: "Date Of Birth Required.",
        FirstLanguageId: "First Language Required.",
        FirstLanguageSymbol: "First Language Symbol Required.",
        FirstLanguageCode: "First Language Code Required.",
        SecondLanguageId: "Second Language Required.",
        SecondLanguageSymbol: "Second Language Symbol Required.",
        SecondLanguageCode: "Second Language Code Required.",
        OptionalElectiveId: "Optional Elective  Required.",
        OptionalElectiveSymbol: "Optional Elective Symbol Required.",
        OptionalElectiveCode: "Optional Elective Code Required.",
        PupilAdmitted: "Pupil Admitted Class is Required.",
        DateOfAdmission: "Date Of Admission Required.",
        StudentImage: "Student Photograph is Required.",
    },
    Info: {
        RegistrationFinalSubmit: "Registration is one time process, Please Check all data carefully before final submission of the Student Registration data as this data will be locked after submit. Are you sure to submit the data?",
        RegistrationApprovalFinalSubmit: "Registration is one time process, Please Check all data carefully before final approval of the Student Registration data as this data will be locked after submit. Do you want to proceed?"
    }
};

var MAC = {
    Validation: {
        DuplicateAuthorizationRequest: "Authorization Request is Already Done for Current Device",
        NoData: "There is not data to approve"
    },
    Info: {
        CheckApproval: "Be careful while authorizing any computer. Verify any request manually before approving. Do you want to proceed anyway?"
    }
};

var ReportFormat = {
    Excel: "1",
    Pdf: "2"
};

var SchoolInbox = {
    LinkType: {
        URL: "URL",
        PDF: "PDF",
        CONTENT: "CONTENT"
    },
    PdfAllowedSize: 4194304,
    All: 'QQ==',
    Validation: {
        ContentRequired: "Notice Description Required.",
        InvalidContent: "Invalid Notice Description.",
        //LinkTypeRequired: "Link Type Required.",
        URLRequired: "URL Required.",
        InvalidURL: "Invalid URL.",
        PDFRequired: "PDF Selection Required.",
        PdfFileExt: "Allowed File Type : pdf.",
        PdfFileSize: "Maximum file size allowed : 4mb."
    },
    Info: {
        RemoveSchool: "Are you sure you want to remove selected school?"
    }
};
/***************************************************************************************/

var CandidateLogin = {
    Required: {
        RollNo: "Roll No Required",
        SchoolIndexNo: "Index No Required",
        Name: "Name Required",
        DOB: "Date of Birth Required",
        Captcha: "Captcha Required"
    },
    Validation: {
        InvalidRollNo: "Invalid Roll No. Please Check Roll No Printed On Admit Card",
        InvalidSchoolIndexNo: "Invalid Index No. Please Check Index No Printed On Admit Card",
        InvalidName: "Invalid Name. Please Check Name Printed On Admit Card",
        InvalidDOB: "Invalid Date of Birth. Date of Birth Should Be in DDMMYY Format",
        InvalidCaptcha: "Invalid Captcha"
    },
    Info: {
        RollNoMsg: "Type In Roll & No Together",
        IndexNoMsg: "Type In Index No Without '-'",
        NameMsg: "Type In Your Name",
        DateOfBirthMsg: "Type In DDMMYY Format",
        CaptchaMsg: "Type In Captcha Shown Below"
    }
};

var Candidate = {
    Info: {
        DashBoardErrMsg: "Operation Error. Unable To Fetch Candidate Details."
    }
};

var PostPublication = {
    Required: {
        SubjectSelection: "At Least One Subject Selection Required Before Apply."
    },
    Validation: {
        InvalidFromDate: "Invalid From Date",
        InvalidToDate: "Invalid To Date"
    }
};

var Books = {
    Required: {
        BookName: "Book Name Required",
        BookCode: "Book Code Required",
        SubjectClass: "Class Selection Required",
        SchoolMedium: "Medium Selection Required",
        BookPrice: "Book Price Required",
    },
    Validation: {
        InvalidBookName: "Invalid Book Name",
        InvalidBookCode: "Invalid Book Code",
        InvalidSubjectClass: "Invalid Class",
        InvalidSchoolMedium: "Invalid Medium",
        InvalidBookPrice: "Invalid Book Price"
    }
};
