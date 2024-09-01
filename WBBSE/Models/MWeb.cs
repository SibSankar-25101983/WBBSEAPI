using System;
using System.Data;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using WBBSE.Models;
using ViewModel;
using Common;
using DAL;

namespace WBBSE.Models
{
    public class MWeb
    {
        public int getWebsiteHeader()
        {
            int err = 0;
            string headerData = string.Empty, menuDetails = string.Empty;

            try
            {
                err = new Menu().GetWebsiteHeader(ref headerData, ref menuDetails);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(headerData, "getWebsiteHeader(MCommon)", "ApplicationController");
                }
                else
                {
                    HttpContext.Current.Session[SessionNames.WebsiteHeaderData] = headerData;
                    HttpContext.Current.Session[SessionNames.WebsiteMenuData] = menuDetails;
                }


                //if (HttpContext.Current.Session[SessionNames.WebsiteHeaderData] == null || HttpContext.Current.Session[SessionNames.WebsiteMenuData] == null)
                //{
                //    err = new Menu().GetWebsiteHeader(ref headerData, ref menuDetails);

                //    if (err == 1)
                //    {
                //        MCommon.saveExceptionLog(headerData, "getWebsiteHeader(MCommon)", "ApplicationController");
                //    }
                //    else
                //    {
                //        HttpContext.Current.Session[SessionNames.WebsiteHeaderData] = headerData;
                //        HttpContext.Current.Session[SessionNames.WebsiteMenuData] = menuDetails;
                //    }
                //}
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "getWebsiteHeader(MWeb)", "ApplicationController");
                err = 1;
            }

            return err;
        }

        public int getWebsiteHomePageContent(ref string section1, ref string section2, ref string section3, ref string section4, ref string section5, ref string section6)
        {
            int err = 0;

            try
            {
                err = new Home().GetWebsiteHomePageContent(ref section1, ref section2, ref section3, ref section4, ref section5, ref section6);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(section1, "getWebsiteHomePageContent(MWeb)", "WebController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "getWebsiteHomePageContent(MWeb)", "WebController");
                err = 1;
            }

            return err;
        }

        public List<VMContentDetails> getSyllabusCurriculumList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new SyllabusCurriculum().GetSyllabusCurriculumList(searchString, menuCode, page, limit, ref total, archiveYN);

                var records = (from data in dt.AsEnumerable()
                               select new VMContentDetails
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   ContentId = data.Field<string>("ContentId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   LinkTypeId = data.Field<string>("LinkTypeId")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSyllabusCurriculumList/getSyllabusCurriculumList(MWeb)", "WebController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMContentDetails> getModelQuestionsList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new ModelQuestions().GetModelQuestionsList(searchString, menuCode, page, limit, ref total, archiveYN);

                var records = (from data in dt.AsEnumerable()
                               select new VMContentDetails
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   ContentId = data.Field<string>("ContentId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   LinkTypeId = data.Field<string>("LinkTypeId")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetModelQuestionsList/getModelQuestionsList(MWeb)", "WebController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMContentDetails> getDownloadFormsList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new DownloadForms().GetDownloadFormsList(searchString, menuCode, page, limit, ref total, archiveYN);

                var records = (from data in dt.AsEnumerable()
                               select new VMContentDetails
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   ContentId = data.Field<string>("ContentId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   LinkTypeId = data.Field<string>("LinkTypeId")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetDownloadFormsList/GetDownloadFormsList(MWeb)", "WebController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMContentDetails> getRTIList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new RTI().GetRTIList(searchString, menuCode, page, limit, ref total, archiveYN);

                var records = (from data in dt.AsEnumerable()
                               select new VMContentDetails
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   ContentId = data.Field<string>("ContentId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   LinkTypeId = data.Field<string>("LinkTypeId")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetRTIList/GetRTIList(MWeb)", "WebController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public string getDisclaimer()
        {
            string data = string.Empty;

            try
            {
                data = new Content().GetDisclaimer();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "getWebsiteHeader(MWeb)", "ApplicationController");
                data = string.Empty;
            }

            return data;
        }

        public List<VMContentDetails> getTenderList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new Tender().GetTenderList(searchString, menuCode, page, limit, ref total, archiveYN);

                var records = (from data in dt.AsEnumerable()
                               select new VMContentDetails
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   ContentId = data.Field<string>("ContentId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   LinkTypeId = data.Field<string>("LinkTypeId")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetTenderList/getTenderList(MWeb)", "AdminWebController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstSchool> getSchoolDirectory(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, string searchType, int subdivisionId)
        {
            DataTable dt = new DataTable();
            List<VMMstSchool> result = new List<VMMstSchool>();

            try
            {
                dt = new School().GetSchoolDirectory((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString, subdivisionId, page, limit, ref total, (string.IsNullOrEmpty(searchType) ? "" : searchType));

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchool
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   SchoolId = data.Field<string>("SchoolId"),
                                   IndexNo = data.Field<string>("IndexNo"),
                                   DISECode = data.Field<string>("DISECode"),
                                   SchoolName = data.Field<string>("SchoolName"),
                                   SubDivisionName = data.Field<string>("SubDivisionName"),
                                   DistrictName = data.Field<string>("DistrictName"),
                                   ZoneName = data.Field<string>("ZoneName"),
                                   Designation = data.Field<string>("Designation"),
                                   PhoneNo = data.Field<string>("ContactNo")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolDirectoryList/getSchoolDirectory(MWeb)", "WebController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public string getMstSchoolView(string schoolId)
        {
            int err = 0;
            string data = string.Empty;

            try
            {
                err = new School().GetSchoolView(schoolId, ref data);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "GetSchoolView/getMstSchoolView(MWeb)", "WebController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetMstSchoolView/getMstSchoolView(MWeb)", "WebController");
            }

            return data;
        }

        public List<VMContentDetails> getEBooksList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new EBooks().GetEBooksList(searchString, menuCode, page, limit, ref total, archiveYN);

                var records = (from data in dt.AsEnumerable()
                               select new VMContentDetails
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   ContentId = data.Field<string>("ContentId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   LinkTypeId = data.Field<string>("LinkTypeId")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetEBooksList/getEBooksList(MWeb)", "WebController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public string getBoardDeskDetails(string menuCode, ref string headerText, ref string img)
        {
            string data = string.Empty;

            try
            {
                data = new Content().GetBoardDeskDetails(menuCode, ref headerText, ref img);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "getBoardDeskDetails(MWeb)", "WebController");
                data = string.Empty;
            }

            return data;
        }

        public int sendMailForContactUs(VMContactUs data, ref string errDesc)
        {
            int err = 0;
            string name = string.Empty, emailId = string.Empty, mobileNo = string.Empty, subject = string.Empty, message = string.Empty, contactUsSubject = string.Empty;

            name = Sanitizer.GetSafeHtmlFragment((data.Name ?? "").Trim());
            if (string.IsNullOrEmpty(name))
            {
                err = 2;
                errDesc = Message.ContactUs.SenderName;
                return err;
            }
            if (!GblFunctions.chkDataFormat(RegexType.OnlyAlpha, name))
            {
                err = 2;
                errDesc = Message.ContactUs.InvalidName;
                return err;
            }

            emailId = Sanitizer.GetSafeHtmlFragment((data.EmailId ?? "").Trim());
            if (string.IsNullOrEmpty(emailId))
            {
                err = 2;
                errDesc = Message.ContactUs.SenderEmailId;
                return err;
            }
            if (!string.IsNullOrEmpty(emailId))
            {
                if (!GblFunctions.chkDataFormat(RegexType.EmailId, emailId))
                {
                    err = 2;
                    errDesc = Message.RegexMsg.InvalidEmailId;
                    return err;
                }
            }

            mobileNo = Sanitizer.GetSafeHtmlFragment((data.MobileNo ?? "").Trim());
            if (string.IsNullOrEmpty(mobileNo))
            {
                err = 2;
                errDesc = Message.ContactUs.SenderMobileNo;
                return err;
            }
            //mobile no: Special character checking (if not blank or empty)
            if (!string.IsNullOrEmpty(mobileNo))
            {
                if (!GblFunctions.chkDataFormat(RegexType.MobileNo, mobileNo))
                {
                    err = 2;
                    errDesc = Message.RegexMsg.InvalidMobileNo;
                    return err;
                }
            }

            subject = Sanitizer.GetSafeHtmlFragment((data.Subject ?? "").Trim());
            if (string.IsNullOrEmpty(subject))
            {
                err = 2;
                errDesc = Message.ContactUs.SenderSubject;
                return err;
            }
            if (!GblFunctions.chkDataFormat(RegexType.Alpha, subject))
            {
                err = 2;
                errDesc = Message.ContactUs.InvalidSubject;
                return err;
            }

            message = Sanitizer.GetSafeHtmlFragment((data.Message ?? "").Trim());
            if (string.IsNullOrEmpty(message))
            {
                err = 2;
                errDesc = Message.ContactUs.SenderMessage;
                return err;
            }
            if (!GblFunctions.chkDataFormat(RegexType.Alpha, message))
            {
                err = 2;
                errDesc = Message.ContactUs.InvalidMessage;
                return err;
            }

            contactUsSubject = Message.ContactUs.ContactUsSubject;

            string mailBody = "<table>";
            mailBody += "<tr><td colspan='2'>Hi Admin,</td></tr>";
            mailBody += "<tr><td>Name</td><td>:</td><td>" + name + "</td></tr>";
            mailBody += "<tr><td>Mail Id</td><td>:</td><td>" + emailId + "</td></tr>";
            mailBody += "<tr><td>Mobile No</td><td>:</td><td>" + mobileNo + "</td></tr>";
            mailBody += "<tr><td>Subject</td><td>:</td><td>" + subject + "</td></tr>";
            mailBody += "<tr><td>Message</td><td>:</td><td>" + message + "</td></tr>";
            mailBody += "</table>";

            err = GblFunctions.sendMailForContactUs(contactUsSubject, mailBody);
            return err;
        }

        public string getWebsiteMenuLink(string menuCode)
        {
            string data = string.Empty;

            try
            {
                data = new Web().GetWebsiteMenuLink(menuCode);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "getWebsiteMenuLink(MWeb)", "WebController");
                data = string.Empty;
            }

            return data;
        }

        public string getPdfView(string menuCode)
        {
            string data = string.Empty;

            try
            {
                data = new Web().GetPdfView(menuCode);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "getPdfView(MWeb)", "WebController");
                data = string.Empty;
            }

            return data;
        }

        public List<VMMstBooks> getMstBookList(int? page, int? limit, string sortBy, string direction, ref int total, string searchString)
        {
            DataTable dt = new DataTable();
            List<VMMstBooks> result = new List<VMMstBooks>();

            try
            {
                dt = new Books().GetMstBookList(searchString, page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstBooks
                               {
                                   SlNo = data.Field<Int64>("SlNo"),
                                   BookId = data.Field<string>("BookId"),
                                   BookName = data.Field<string>("BookName"),
                                   BookCode = data.Field<string>("BookCode"),
                                   Class = data.Field<string>("Class"),
                                   SchoolMediumId = data.Field<string>("SchoolMediumId"),
                                   SchoolMediumName = data.Field<string>("SchoolMediumName"),
                                   BookPrice = data.Field<string>("BookPrice")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetBookList/getMstBookList(MWeb)", "WebController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMContentDetails> getRequisitionSlipList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new RequisitionSlip().GetRequisitionSlipList(searchString, menuCode, page, limit, ref total, archiveYN);

                var records = (from data in dt.AsEnumerable()
                               select new VMContentDetails
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   ContentId = data.Field<string>("ContentId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   LinkTypeId = data.Field<string>("LinkTypeId")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetRequisitionSlipList/getRequisitionSlipList(MWeb)", "WebController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMContentDetails> getSyllabusList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new Syllabus().GetSyllabusList(searchString, menuCode, page, limit, ref total, archiveYN);

                var records = (from data in dt.AsEnumerable()
                               select new VMContentDetails
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   ContentId = data.Field<string>("ContentId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   LinkTypeId = data.Field<string>("LinkTypeId")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSyllabusList/getSyllabusList(MWeb)", "WebController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMContentDetails> getResultsAbstract(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new Result().GetResultsAbstractList(searchString, menuCode, page, limit, ref total, archiveYN);

                var records = (from data in dt.AsEnumerable()
                               select new VMContentDetails
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   ContentId = data.Field<string>("ContentId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   LinkTypeId = data.Field<string>("LinkTypeId")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetResultsAbstract/getResultsAbstract(MWeb)", "WebController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }
    }
}
