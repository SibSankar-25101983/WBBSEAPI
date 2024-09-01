using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SessionNames
    {
        public const string URL = "URL";
        public const string GroupId = "GroupId";
        public const string UserId = "UserId";
        public const string OrganizationId = "OrganizationId";
        public const string OrganizationCode = "OrganizationCode";
        public const string OrganizationName = "OrganizationName";
        public const string SubDivisionId = "SubDivisionId";
        public const string Salt = "Salt";
        public const string Identity = "Identity";
        public const string Captcha = "Captcha";
        public const string RoleDetails = "RoleDetails";
        public const string GroupName = "GroupName";
        public const string Name = "Name";
        public const string RoleDetailsPermissions = "RoleDetailsPermissions";
        public const string NewPasswordChangedYN = "NewPasswordChangedYN";
        public const string EmailVerifiedYN = "EmailVerifiedYN";
        public const string ContactNoVerifiedYN = "ContactNoVerifiedYN";
        public const string ProfileVerificationData = "ProfileVerificationData";
        public const string RoleId = "RoleId";
        public const string UserTypeId = "UserTypeId";
        public const string MenuDetails = "MenuDetails";
        public const string ProfileImage = "ProfileImage";
        public const string WebsiteHeaderData = "WebsiteHeaderData";
        public const string WebsiteMenuData = "WebsiteMenuData";
        public const string ContentType = "ContentType";
        public const string HomeLink = "HomeLink";
        public const string SchoolLoginURL = "SchoolLoginURL";
        public const string MachineId = "MachineId";
        public const string MachineName = "MachineName";
        public const string MACActivatedYN = "MACActivatedYN";
        public const string MACAuthorizationToken = "AuthorizationToken";
        public const string MACDuplicateYN = "MADuplicateYN";
        public const string MACTimeStamp = "MACTimeStamp";
        public const string Role = "Role";
        public const string DeleteYN = "DeleteYN";
        public const string NoOnce = "NoOnce";
        public const string UnreadNotice = "UnreadNotice";
        public const string UnreadNoticeUpdateCount = "UnreadNoticeUpdateCount";
        public const string PostPublicationType = "PostPublicationType";
        public const string PostPublicationPrice = "PostPublicationPrice";
        public const string PostPublicationPaymentPageLink = "PostPublicationPaymentPageLink";
        public const string NotificationType = "NotificationType";
        public const string Nonce = "Nonce";
    }

    public class ViewDataNames
    {
        public const string Salt = "Salt";
        public const string LoginInfo = "LoginInfo";
        public const string InvalidPasswordLength = "InvalidPasswordLength";
        public const string InvalidChar = "InvalidChar";
        public const string InvalidCaptcha = "InvalidCaptcha";
        public const string divLoginAlertVisibility = "divLoginAlertVisibility";
        public const string GridColumns = "GridColumns";
        public const string GridColumnsAlternative = "GridColumnsAlternative";
        public const string RoleDetailsGridColumns = "RoleDetailsGridColumns";
        public const string AddYN = "AddYN";
        public const string EditYN = "EditYN";
        public const string DeleteYN = "DeleteYN";
        public const string ReportYN = "ReportYN";
        public const string ErrorVisibility = "ErrorVisibility";
        public const string SucessVisibility = "SucessVisibility";
        public const string SaveInfo = "SaveInfo";
        public const string ErrorAlertVisibility = "ErrorAlertVisibility";
        public const string EmailOTPVisibleYN = "EmailOTPVisibleYN";
        public const string ContactOTPVisibleYN = "ContactOTPVisibleYN";
        public const string ShowHomeLinksYN = "ShowHomeLinksYN";
        public const string EmailId = "EmailId";
        public const string ContactNo = "ContactNo";
        public const string Err = "Err";
        public const string ErrDesc = "ErrDesc";
        public const string WebsiteEditingMenuName = "WebsiteEditingMenuName";
        public const string ActiveLinkA = "ActiveLinkA";
        public const string ActiveLinkLI = "ActiveLinkLI";
        public const string ActiveLinkTab = "ActiveLinkTab";
        public const string RawData = "RawData";
        public const string RawDataHeader = "RawDataHeader";
        public const string Section1 = "Section1";
        public const string Section2 = "Section2";
        public const string Section3 = "Section3";
        public const string Section4 = "Section4";
        public const string Section5 = "Section5";
        public const string Section6 = "Section6";
        public const string PageLink = "PageLink";
        public const string SchoolIndexNo = "SchoolIndexNo";

    }

    public class TempDataNames
    {
        public const string SaveStatus = "SaveStatus";
        public const string ErrDesc = "ErrDesc";
        public const string SaveInfo = "SaveInfo";
        public const string ProfileVerificationDataFetchMode = "ProfileVerificationDataFetchMode";
    }

    public class SchoolHomeMessage
    {
        public const string ProfileVerficationSuccess = "Profile Verfied Successfully";
    }

    public class Message
    {
        /*COMMON MESSAGES*/
        public const string InvalidRequest = "Invalid Request";
        public const string OperationError = "Operation Error. Current Operation Cannot Be Performed.";
        public const string CaptchaError = "Captcha Required";
        public const string Required = "Required";
        public const string ValidateLoginUserName = "User name required";
        public const string ValidateLoginPassword = "Password required";
        public const string InvalidUser = "Invalid User Name/Password";
        public const string InvalidPasswordLength = @"Operation can't be performed.Your browser is not supporting Password Encription.";
        public const string InvalidChar = "Special character (e.g., Delete, Drop, Truncate, Or,* , &, $, #, <, >, script, html) are not allowed.";
        public const string InvalidCaptcha = "Invalid Captcha";
        public const string LoggedOut = "Logged Out Successfully";
        public const string SaveMsg = "Data Saved Successfully";
        public const string ErrorMsg = "Operation Error. Data Not Saved";
        public const string DeleteMsg = "Data Deleted Successfully";
        public const string GenRequired = "*Input can not be blank";
        public const string NameRequired = "Name Required";
        public const string AbbreviationNameRequired = "Abbreviation Name Required";
        public const string StateSelectionRequired = "State Selection Required";
        public const string DuplicateEmailId = "Duplicate Email Id";
        public const string DuplicateContactNo = "Duplicate Contact No";
        public const string DuplicateAdminUser = "Duplicate Admin User";
        public const string FileSizeExceeded = "File size should not exceeds 10 MB";
        public const string EmailIdRequired = "Email Id Required";
        public const string ContactNoRequired = "Contact No Required";
        public const string UnauthorizedLoginAttempt = "Contact No Required";
        public const string ReportGenerationFailed = "No Record Found. Report Generation Failed.";
        public const string SendMsg = "Thank you for contacting us. We will respond as soon as possible.";
        public const string FailedMsg = "Message send failed. Please retry again.";
        public const string UnauthorizedAccess = "Un-authorized Access For Current Module";

        public class RegexMsg
        {
            public const string AlphaWithSpace = "Only Alphabets & Space Allowed";
            public const string InvalidEmailId = "Invalid Email Id";
            public const string InvalidWebsite = "Invalid Website";
            public const string InvalidContactNo = "Invalid Phone No";
            public const string InvalidMobileNo = "Invalid Mobile No";
            public const string InvalidFaxNo = "Invalid Fax No";
            public const string InvalidCharacter = "Invalid Character";
            public const string InvalidPinCode = "Invalid Pin Code";
            public const string InvalidSTDCode = "Invalid STD Code";
            public const string InvalidDISECode = "Invalid DISE Code";
            public const string AlphaWithSpecialChar = "Allowed :: Alphabets Space , - () . &";
            public const string InvalidURL = "Invalid Website/URL";
        }

        public class User
        {
            public const string UserIdRequired = "User Id Required";
            public const string FirstNameRequired = "First Name Required";
            public const string LastNameRequired = "Last Name Required";
            public const string SalutationRequired = "Salutation Required";
            public const string AddressLine1Required = "Address Line1 Required";
            public const string CityRequired = "City Required";
            public const string PinCodeRequired = "Pin Code Required";
            public const string DesignationRequired = "Designation Selection Required";
            public const string InvalidFirstName = "Invalid First Name";
            public const string InvalidMiddleName = "Invalid Middle Name";
            public const string InvalidLastName = "Invalid Last Name";
        }

        public class School
        {
            public const string PreSchoolIdRequired = "Junior School Id Required";
            public const string SchoolIdRequired = "School Id Required";
            public const string SchoolNameRequired = "School Name Required";
            public const string DISECodeRequired = "DISE Code Required";
            public const string SubDivisionRequired = "Sub-division Selection Required";
            public const string SchoolHeadNameRequired = "School Head Name Required";
            public const string DesignationRequired = "Designation Selection Required";
            public const string InvalidIndexNo = "Invalid Index No";
            public const string InvalidSchoolName = "Invalid School Name";
            public const string InvalidStreetName = "Invalid Street/Locality Name";
            public const string InvalidAreaName = "Invalid Area Name";
            public const string InvalidGramPanchayetName = "Invalid Gram Panchayet Name";
            public const string InvalidPOName = "Invalid Post Office Name";
            public const string InvalidPSName = "Invalid Police Station Name";
            public const string InvalidCityName = "Invalid City Name";
            public const string EditNotPermitted = "Selected School Edit Not Permitted";
            public const string DeleteNotPermitted = "Delete Not Permitted for this school. Reason: Registration Data Present for Selected School.";
            public const string DeleteNotPermittedMigration = "Delete Not Permitted for this school. Reason: Selected School is Migrated.";
            public const string RegPermission = "You are not permitted to perform this action.";
            public const string StreetNameRequired = "Street/Locality Name Required";
            public const string AreaNameRequired = "Area Name Required";
            public const string CityRequired = "City Required";
            public const string PinCodeRequired = "Pin Code Required";
            public const string MobileNoRequired = "Mobile No Required";
            public const string EmailIdRequired = "Email Id Required";
            public const string PhoneNoOrFaxNoRequired = "Either Phone No/Fax No Required (STD Code Present)";
            public const string STDCodePhoneNoRequired = "STD Code Required (Phone No Present)";
            public const string STDCodeFaxNoRequired = "STD Code Required (Fax No Present)";
            public const string SchoolTypeRequired = "School Type Selection Required";
            public const string SchoolCategoryRequired = "School Category Selection Required";
            public const string SchoolStatusRequired = "School Status Selection Required";
            public const string SchoolMediumRequired = "School Medium Selection Required";
            public const string SchoolRecognizationRequired = "School Recognization Selection Required";
            public const string SchoolManagementRequired = "School Management Selection Required";
            public const string OrderNoRequired = "Order No Required";
            public const string OrderDateRequired = "Order Date Required";
            public const string InvalidOrderNo = "Invalid Order No";
            public const string InvalidOrderDate = "Invalid Order Date (Date Format : dd-MM-yyyy)";
            public const string UnLockNotPermitted = "School Edit un-lock not permitted with current priviledge";
        }

        public class SchoolParameter
        {
            public const string SchoolRecognitionIdRequired = "School Recognition Id Required";
            public const string RecognitionStatusRequired = "School Recognition Status Required";
            public const string RecognitionStatusInvalid = "Recognition Status is Invalid.";
            public const string RecognitionStatusDeleteNotPermitted = "School Recognition data is mapped with school/Junior School. School Recognition data cannot be deleted.";
            public const string SchoolManagementIdRequired = "School Management Id Required";
            public const string SchoolManagementRequired = "School Management Required";
            public const string SchoolManagementDeleteNotPermitted = "School Management data is mapped with school/Junior School. School management data cannot be deleted.";
            public const string SchoolManagementInvalid = "School Management Name is Invalid.";
            public const string SchoolManagementDeleteNotPermittedMigration = "School management data cannot be deleted. Reason : School management data is migrated.";
            public const string SchoolRecognitionDeleteNotPermittedMigration = "School recognition data cannot be deleted. Reason : School recognition data is migrated.";
        }

        public class Block
        {
            public const string BlockIdRequired = "Block Id Required";
            public const string BlockNameRequired = "Block Name Required";
            public const string BlockDeleteNotPermitted = "Block is mapped with circle. Block cannot be deleted.";
            public const string BlockDeleteNotPermittedMigration = "Block cannot be deleted. Reason : Block data is migrated.";
            public const string BlockNameInvalid = "Block Name is Invalid.";
        }

        public class Circle
        {
            public const string CircleIdRequired = "Circle Id Required";
            public const string CircleNameRequired = "Circle Name Required";
            public const string CircleDeleteNotPermitted = "Circle is mapped with school. Circle cannot be deleted.";
            public const string CircleDeleteNotPermittedMigration = "Circle cannot be deleted. Reason : Circle data is migrated.";
            public const string CircleNameInvalid = "Circle Name is Invalid.";
        }

        public class Zone
        {
            public const string ZoneIdRequired = "Zone Id Required";
            public const string ZoneNameRequired = "Zone Name Required";
            public const string ZoneDeleteNotPermitted = "Zone is mapped with district. Zone cannot be deleted.";
            public const string ZoneNameInvalid = "Zone Name is Invalid.";
            public const string ZoneDeleteNotPermittedMigration = "Zone cannot be deleted. Reason : Zone data is migrated.";
        }

        public class State
        {
            public const string StateIdRequired = "State Id Required";
            public const string StateNameRequired = "State Name Required.";
        }

        public class District
        {
            public const string DistrictRequired = "District Id Required";
            public const string DistrictNameRequired = "District Name Required";
            public const string IndexInitial = "Index Initial Name Required.";
            public const string DistrictDeleteNotPermitted = "District is mapped with sub-division. District cannot be deleted.";
            public const string DistrictNameInvalid = "District Name is Invalid.";
            public const string DistrictDeleteNotPermittedMigration = "District cannot be deleted. Reason : District data is migrated.";
        }

        public class SubDivision
        {
            public const string SubDivisionIdRequired = "SubDivision Id Required";
            public const string SubDivisionNameRequired = "SubDivision Name Required";
            public const string SubDivisionDeleteNotPermitted = "SubDivision is mapped with school. SubDivision cannot be deleted.";
            public const string SubDivisionNameInvalid = "SubDivision Name is Invalid.";
            public const string SubDivisionDeleteNotPermittedMigration = "SubDivision cannot be deleted. Reason : SubDivision data is migrated.";
        }

        /*USER GROUP*/
        public class UserGroup
        {
            public const string UserGroupNameRequired = "Group Name Required";
            public const string UserGroupNameMaxLength = "Group Name Maximum Length 100 Characters";
            public const string UserGroupDeleteNotPermitted = "User Is Assigned Against This Group. User Group Cannot Be Deleted.";
            public const string InvalidUserGroupName = "Invalid Group Name";
        }
        /************/

        public class ContactUs
        {
            public const string ContactUsSubject = "New Enquery from Contact Us WBBSE";
            public const string SenderName = "Name Required.";
            public const string SenderEmailId = "Email Id Required.";
            public const string SenderMobileNo = "Mobile No Required.";
            public const string SenderSubject = "Mail Subject Required.";
            public const string SenderMessage = "Mail Body Required.";
            public const string InvalidName = "Invalid Name";
            public const string InvalidSubject = "Invalid Subject";
            public const string InvalidMessage = "Invalid Message";
        }

        public class ProfileVerification
        {
            public const string OldPasswordRequired = "Old Password Required.";
            public const string NewPasswordRequired = "New Password Required.";
            public const string ConfirmPasswordRequired = "Confirm Password Required.";
            public const string OldPasswordMismatch = "Old Password Mismatch.";
            public const string PasswordMismatch = "Password Mismatch.";
            public const string PasswordPolicyMismatch = "Password Policy Mismatch.";
            public const string LastThreePasswordNotAllowed = "New Password Cannot Be Same As Last Three Password(s).";
            public const string PasswordSaveMsg = "Password Saved Successfully.";
            public const string SchoolProfileVerificationDataSaveMsg = "Profile Data Saved Successfully.";
            public const string OTPRequired = "OTP Required.";
            public const string OTPMismatch = "OTP Mismatch.";
            public const string EmailVerificationOTPSent = "OTP Sent To Your Email Id.";
            public const string EmailVerificationOTPSentFailed = "Operation Error. OTP Sending To Email Id Failed.";
            public const string EmailVerificationSaveMsg = "Email Id Verified Successfully.";
            public const string EmailVerificationErrorMsg = "Email Id Verification Failed.";
            public const string ContactNoVerificationOTPSent = "OTP Sent To Your Contact No.";
            public const string ContactNoVerificationOTPSentFailed = "Operation Error. OTP Sending To Contact No Failed.";
            public const string ContactNoVerificationSaveMsg = "Contact No Verified Successfully.";
            public const string ContactNoVerificationErrorMsg = "Contact No Verification Failed.";
            public const string FirstNameRequired = "First Name Required.";
            public const string LastNameRequired = "Last Name Required.";
            public const string EmailIdRequired = "Email Id. Required.";
            public const string ContactNoRequired = "Mobile No. Required.";
            public const string SalutationIDRequired = "Salutation Required.";
        }

        public class CandidateVerification
        {
            public const string RollNoRequired = "Roll No Required.";
            public const string InvalidRollNo = "Invalid Roll No.";
            public const string DOBRequired = "Date of Birth Required.";
            public const string InvalidDOB = "Invalid Date of Birth.";
            public const string CanidateDetailsUpdateSuccess = "Candidate Details Updated Successfully.";
            public const string CanidateDetailsUpdateFailure = "Candidate Details Update Failed.";

            public const string CanidateVerificationSaveSuccess = "Candidate Verification Successfully Completed.";
            public const string CanidateVerificationSaveFailure = "Operation Error. Candidate Verification Failed.";
        }

        public class FileUpload
        {
            public const string SelfieRequired = "Profile image required.";
            public const string TwoFilesRequired = "Photograph & signature required.";
            public const string ImageRequired = "Image Selection Required.";
            public const string InvalidContentTypeImage = "Photograph Allowed Extension : jpg/jpeg.";
            public const string InvalidContentTypeSignature = "Signature Allowed Extension : jpg/jpeg.";
            public const string InvalidSizeImage = "Photograph is not within allowed size.";
            public const string InvalidSizeSignature = "Signature is not within allowed size.";
            public const string ImageUploadSuccessful = "Image(s) Uploaded Successfully.";
            public const string ImageDeleteSuccessful = "Image(s) Deleted Successfully.";
            public const string ImageUploadErr = "Operation Error. Image(s) Uploaded Failed.";
            public const string ImageDeleteErr = "Operation Error. Image Delete Failed.";
            public const string InvalidImageExt = "Allowed Image Types : jpg/jpeg/png.";
            public const string PdfFileRequired = "Pdf file required.";
            public const string InvalidContentTypePdf = "Allowed File Type: pdf.";
            public const string MaxFileSizePdf = "Maximum file size allowed : 4mb.";
            public const string MaxFileSizeBookPdf = "Maximum file size allowed : 100mb.";
            public const string StudentRegImageMaxMinSize = "Image is not within allowed size.";
        }

        public class Content
        {
            public const string ContentIdRequired = "Content Id Required";
            public const string ContentRequired = "Content Required";
            public const string InvalidContent = "Invalid Content";
            public const string LinkTypeRequired = "Link Type Required";
            public const string URLRequired = "URL Required";
            public const string InvalidURL = "Invalid URL";
        }

        public class Inbox
        {
            public const string InboxIdRequired = "Inbox Id Required";
            public const string ContentRequired = "Content Required";
            public const string InvalidContent = "Invalid Content";
            public const string LinkTypeRequired = "Link Type Required";
            public const string URLRequired = "URL Required";
            public const string InvalidURL = "Invalid URL";
            public const string SchoolListRequired = "School List Required";
        }

        public class MAC
        {
            public const string MachineIdRequired = "Machine Id Required";
            public const string MachineNameRequired = "Machine Name Required";
            public const string AuthorizationStatusRequired = "Authorization Status Required";
            public const string DuplicateAuthorizationRequest = "Authorization Request is Already Done for Current Device";
            public const string AuthorizationRequestSuccessful = "Authorization Request for Current Device Sent Successfully";
            public const string AuthorizationRequestFailure = "Authorization Request for Current Device Failed";
            public const string SessionExpiredCurrentRequest = "Session Expired for Current Request";
            public const string AuthorizationAssignSuccessful = "Selected Computer(s) Authorized Successfully";
            public const string AuthorizationAssignFailure = "Selected Computer(s) Authorization Failed";
        }

        public class SchoolTransfer
        {
            public const string SaveMsg = "School transfered successfully";
            public const string DeleteMsg = "School Transfer Data Deleted Successfully";
            public const string ErrorMsg = "School Transfer Operation Failed";
        }
        public class Quote
        {
            public const string InvalidQuotedBy = "Invalid Quoted By data";
        }

        public class Candidate
        {
            public const string RollNoRequired = "Roll No Required";
            public const string InvalidRollNo = "Invalid Roll No. Please Check Roll No Printed On Admit Card";
            public const string SchoolIndexNoRequired = "Index No Required";
            public const string InvalidSchoolIndexNo = "Invalid Index No. Please Check Index No Printed On Admit Card";
            public const string NameRequired = "Name Required";
            public const string InvalidName = "Invalid Name. Please Check Name Printed On Admit Card";
            public const string DOBRequired = "Date of Birth Required";
            public const string InvalidDOB = "Invalid Date of Birth. Date of Birth Should Be in DDMMYY Format";
            public const string InvalidLoginAttempt = "Invalid Roll No/ Index No/ Name/ Date of Birth. Please Check Your Admit Card & Enter All Fields Properly";
            public const string SubjectSelectionRequired = "At Least One Subject Selection Required Before Apply";
            public const string PPApplicationSaveSuccess = "Application Form Saved Successfully";
            public const string PPApplicationSaveError = "Operation Error. Application Form Save Failed";
        }

        public class Reporting
        {
            public const string InvalidFromDate = "Invalid From Date";
            public const string InvalidToDate = "Invalid To Date";
            public const string InvalidReportFormat = "Invalid Report Format";
            public const string ReportGenerationFailed = "No Record Found With Given Search Criteria";
            public class PostPublicationReport
            {
                public const string InvalidScrutinyType = "Invalid Scrutiny Type";
            }
        }

        public class Books
        {
            public const string BookIdRequired = "Book Id Required";
            public const string BookNameRequired = "Book Name Required";
            public const string InvalidBookName = "Invalid Book Name";
            public const string BookCodeRequired = "Book Code Required";
            public const string InvalidBookCode = "Invalid Book Code";
            public const string SubjectClassRequired = "Class Selection Required";
            public const string InvalidSubjectClass = "Invalid Class";
            public const string SchoolMediumRequired = "Medium Selection Required";
            public const string InvalidSchoolMedium = "Invalid Medium";
            public const string BookPriceRequired = "Book Price Required";
            public const string InvalidBookPrice = "Invalid Book Price";
        }
    }

    public class LogCategory
    {
        public const string AdminLogin = "Admin Login";
        public const string SchoolLogin = "School Login";
        public const string PreSchoolLogin = "Pre-School Login";
        public const string CandidateLogin = "Candidate Login";
    }

    public class UserType
    {
        public const string ADMIN = "ADMIN";
        public const string SCHOOL = "SCHOOL";
        public const string PRESCHOOL = "PRESCHOOL";
        public const string CANDIDATE = "CANDIDATE";

        public class UserTypeID
        {
            public const string ADMIN = "1";
            public const string SCHOOL = "2";
            public const string PRESCHOOL = "3";
            public const string CANDIDATE = "4";
        }
    }

    public class BoardInfo
    {
        public const string BoardName = "WEST BENGAL BOARD OF SECONDARY EDUCATION";
        public const string BoardAddress = "Nivedita Bhavan, Sector II, DJ-8, Saltlake, Kolkata, West Bengal - 700091";
    }

    public class ForSite
    {
        public const string Admin = "A";
        public const string School = "S";
        public const string PreSchool = "P";
        public const string Candidate = "C";
    }

    public class DefaultSetting
    {
        public static string EmptyVal = string.Empty;
        public const string DefaultVal = "-1";
        public const string DefaultValEnc = "LTE="; // = -1
        public const string DefaultPwdHash = "8776F108E247AB1E2B323042C049C266407C81FBAD41BDE1E8DFC1BB66FD267E";
        public const string DefaultValY = "Y";
        public const string DefaultValN = "N";
        public const bool DefaultValT = true;
        public const bool DefaultValF = false;
        public const string NotFound = "NF";
    }

    public class Mode
    {
        public const string ERROR = "ERROR";
        public const string ADD = "ADD";
        public const string EDIT = "EDIT";
        public const string DELETE = "DELETE";
        public const string LOCK = "LOCK";
        public const string UNLOCK = "UNLOCK";
    }

    public class EntType
    {
        public const string ADD = "I";
        public const string EDIT = "E";
        public const string DELETE = "D";
        public const string LOCK = "L";
        public const string UNLOCK = "U";
    }

    public class SaveStatus
    {
        public const string SaveDelete = "SD";
        public const string SaveSuccess = "SS";
        public const string Failed = "F";
        public const string ValidationFailed = "VF";
        public const string SaveApproveData = "SaveApproveData";
        public const string FailedApproveData = "FailedApproveData";
    }

    public class MenuCode
    {
        public const string PhotoGallery = "PG";
        public const string Circulars = "CR";
        public const string Notification = "N2";
        public const string Tender = "TD";
        public const string Appointment = "AN";
        public const string PresidentDesk = "PD";
        public const string SecretaryDesk = "SD";
        public const string ImageSlider = "IS";
        public const string Marquee = "MR";
        public const string Quotes = "QT";
        public const string ExamSchedule = "MS";
        public const string DownloadForms = "DF";
        public const string RequisitionSlip = "RS";
        public const string Syllabus = "SB";
        public const string SyllabusCurriculum = "SC";
        public const string ModelQuestions = "MQ";
        public const string RTI = "RT";
        public const string EBooks = "EB";
        public const string GeneralSection = "GS";
        public const string Establishment = "ES";
        public const string DuplicateRecords = "DR";
        public const string RecordsVerification = "RV";
        public const string RecordsCorrection = "RC";
        public const string Recognition = "RN";
        public const string Law = "LW";
        public const string AcademicSection = "AC";
        public const string Administration = "AM";
        public const string ExaminationSection = "EM";
        public const string BookSalesCounter = "BC";
        public const string AppointmentCell = "AO";
        public const string Profile = "PR";
        public const string VisionMission = "VM";
        public const string MainObjectives = "MO";
        public const string Authorities = "AR";
        public const string PastAuthorities = "PT";
        public const string RegionalOffice = "RO";
        public const string Structure = "ST";
        //public const string Gallery = "GL";
        public const string Registration = "RG";
        public const string Enrollment = "EL";
        public const string PPRPPS = "PS";
        public const string Duplicate = "DC";
        public const string Migration = "MG";
        public const string Correction = "CT";
        public const string Verification = "VC";
        public const string RTIMP = "RI";
        //public class Service
        //{
        //    public const string Recognition = "OR";
        //}
        public const string StudyLeave = "SL";
        public const string SpecialLeave = "SP";
        public const string ExtraTime = "ET";
        public const string DutyLeave = "DL";
        public const string ExtraordinaryLeave = "EO";
        public const string SchoolDirectory = "SR";
        public const string OrganizationStructure = "OS";
        public const string ExaminationSystem = "EI";
        public const string AcademicCalender = "CA";
        public const string ExaminationRoutine = "ER";
        public const string CampSchedule = "CS";
        public const string History = "HS";
        public const string ResultsAbstract = "RA";
    }

    public class MenuType
    {
        public const string MainMenu = "MM";
        public const string Other = "OT";
    }

    public class ImagePath
    {
        public const string ImageSlider = "../../../../ReadWriteData/SliderImages/";
        public const string PhotoGallery = "../../../../ReadWriteData/PhotoGallery/";
        public const string BoardDesk = "../../../../ReadWriteData/BoardDesk/";
        public const string StudentRegImage = "../../../../ReadWriteData/StudentRegImage/";
    }

    public class FilePath
    {
        public const string Circular = "../../../../ReadWriteData/Circular/";
        public const string Notification = "../../../../ReadWriteData/Notification/";
        public const string Content = "../../../../ReadWriteData/Content/";
        public const string ExamSchedule = "../../../../ReadWriteData/ExamSchedule/";
        public const string DownloadForms = "../../../../ReadWriteData/DownloadForms/";
        public const string SyllabusCurriculum = "../../../../ReadWriteData/SyllabusCurriculum/";
        public const string ModelQuestions = "../../../../ReadWriteData/ModelQuestions/";
        public const string RTI = "../../../../ReadWriteData/RTI/";
        public const string RTIMPSE = "../../ReadWriteData/RTI/RTI_MP_2020.pdf";
        public const string EBooks = "../../../../ReadWriteData/EBooks/";
        public const string MACToken = "~/Files/Token/WBBSEToken.zip";
        public const string MACTokenManual = "~/Files/Token/WBBSETokenSoftwareManual.docx";
        public const string MACAuthorization = "~/Files/Authorization/WBBSEAuthorization.zip";
        public const string MACAuthorizationManual = "~/Files/Authorization/WBBSEAuthorizationSoftwareManual.docx";
        public const string PublicKey = "~/Files/Public Key/NIC Kolkata - WBBSE Public Key.asc";
        public const string Notice = "../../../../ReadWriteData/Notice/";
        public const string RequisitionSlip = "../../../../ReadWriteData/RequisitionSlip/";
        public const string Syllabus = "../../../../ReadWriteData/Syllabus/";
        public const string ResultsAbstract = "../../ReadWriteData/ResultsAbstract/";
        public const string OrganizationStructure = "../../ReadWriteData/OrganizationStructure/OrganizationStructure.pdf";
        public const string ExaminationSystem = "../../ReadWriteData/ExaminationSystem/ExaminationSystem.pdf";
    }

    public class ImageExtension
    {
        public const string JPG = ".jpg";
        public const string JPEG = ".jpeg";
        public const string PNG = ".png";
    }

    public class MIMETypes
    {
        public const string JPG = "image/jpeg";
        public const string JPEG = "image/jpeg";
        public const string PJPEG = "image/pjpeg";
        public const string PNG = "image/png";
        public const string PDF = "application/pdf";
    }

    public class LinkTypes
    {
        public const string URL = "1";
        public const string PDF = "2";
        public const string CONTENT = "3";
        public const string IMAGE = "4";
    }

    public class MaxFileSize
    {
        public const int PDF = 4194304; //4 MB(IN BYTE)
        public const int PDFEBook = 104857600; //100 MB(IN BYTE)
        public const int RegImgMin = 10240; //30 KB(IN BYTE)
        public const int RegImgMax = 30720; //30 KB(IN BYTE)
        public const int RecordCount = 99999999;
    }

    public class FileExtension
    {
        public const string PDF = ".pdf";
    }

    public class ContentType
    {
        public const string MAIN = "MAIN";
        public const string ARCHIVE = "ARCHIVE";
    }

    public class BoardDeskType
    {
        public const string President = "President's Desk";
        public const string Secretary = "Secretary's Desk";
    }

    public class SearchType
    {
        public class SchoolProfile
        {
            public const string All = "A";
            public const string Locked = "Y";
            public const string UnLocked = "N";
        }
        public class School
        {
            public const string SchoolName = "N";
            public const string IndexNo = "I";
            public const string SchoolId = "S";
        }
        public class PreSchool
        {
            public const string SchoolName = "N";
            //public const string IndexNo = "I";
            public const string DISECode = "I";
            public const string PreSchoolId = "P";
        }
        public class MAC
        {
            public const string ComputerName = "1";
            public const string MachineId = "2";
        }
        public class PostPublication
        {
            public const string RollNo = "1";
            public const string CandidateName = "2";
            public const string SchoolName = "3";
        }
    }

    public class RegexType
    {
        public const string Alpha = "Alpha";
        public const string AlphaWithoutSpace = "AlphaWithoutSpace";
        public const string Numeric = "Numeric";
        public const string EmailId = "EmailId";
        public const string MobileNo = "MobileNo";
        public const string URL = "URL";
        public const string OnlyAlpha = "OnlyAlpha";
        public const string PinCode = "PinCode";
        public const string StdCode = "StdCode";
        public const string PhoneNo = "PhoneNo";
        public const string Date = "Date";
        public const string ContentEdit = "ContentEdit";
        public const string CustomDOB = "CustomDOB";
        public const string RollNo = "RollNo";
        public const string SchoolIndexNo = "SchoolIndexNo";
        public const string Price = "Price";
    }

    public class OTPType
    {
        public const string AdminEmail = "AE";
        public const string SchoolEmail = "SE";
        public const string PreSchoolEmail = "PE";
        public const string AdminMobile = "AM";
        public const string SchoolMobile = "SM";
        public const string PreSchoolMobile = "PM";
    }

    public class ProfileVerificationOTPMsg
    {
        public const string Email = "Hi,<br/>Below is your OTP for Email Id Verification-<br/><br/><b>#</b><br/><br/>* <b>NOTE</b> : OTP is only valid for 30 minutes.<br/><br/>Regards,<br/>WBBSE";
    }

    public class SchoolProfileEditFor
    {
        public const string SuperAdmin = "SUPERADMIN";
        public const string Admin = "ADMIN";
        public const string Others = "OTHERS";
    }

    public class StudentRegistrationApproval
    {
        public const string SuperAdmin = "SUPERADMIN";
        public const string Admin = "ADMIN";
        public const string Others = "OTHERS";
    }

    public class GroupType
    {
        public const string NICADMIN = "1";
    }

    public class FileDownloadOption
    {
        public const string MACToken = "T";
        public const string MACTokenManual = "TM";
        public const string MACAuthorization = "A";
        public const string MACAuthorizationManual = "AM";
    }

    public class MACSearchParameters
    {
        public const string FromDate = "FromDate";
        public const string ToDate = "ToDate";
        public const string SearchString = "SearchString";
        public const string SearchType = "SearchType";
    }

    public class ReportFormat
    {
        public const string Excel = "1";
        public const string Pdf = "2";
    }

    public class ProfileVerificationDataFetchMode
    {
        public const string New = "1";
        public const string Session = "2";
    }

    public class InboxOptions
    {
        public class Mode
        {
            public const string All = "A";
        }
        public class Sender
        {
            public const string Admin = "A";
            public const string School = "S";
            public const string PreSchool = "P";
        }
        public class Receiver
        {
            public const string Admin = "A";
            public const string School = "S";
            public const string PreSchool = "P";
        }
    }

    public class PostPublicationMode
    {
        public const int ApplicationForm = 1;
        public const int Payment = 2;
    }

    public class SubjectClass
    {
        public const string V = "V";
        public const string VI = "VI";
        public const string VII = "VII";
        public const string VIII = "VIII";
        public const string IX = "IX";
        public const string X = "X";
    }
}
