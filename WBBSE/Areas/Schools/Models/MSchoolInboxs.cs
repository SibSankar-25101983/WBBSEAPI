using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections.Generic;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using WBBSE.Models;
using ViewModel;
using Common;
using DAL;

namespace WBBSE.Areas.Schools.Models
{
    public class MSchoolInboxs
    {
        public List<VMContentDetails> getCircularList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new Circular().GetCircularList(searchString, menuCode, page, limit, ref total, archiveYN);

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
                MCommon.saveExceptionLog(ex.Message, "GetCircularList/GetCircularList(MSchoolInboxs)", "SchoolInboxsController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMContentDetails> getNotificationList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new Notification().GetNotificationList(searchString, menuCode, page, limit, ref total, archiveYN);

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
                                   LinkTypeId = data.Field<string>("LinkTypeId"),
                                   NotificationType = data.Field<string>("NotificationType"),
                                   MenuCode = data.Field<string>("MenuCode")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetNotificationList/getNotificationList(MSchoolInboxs)", "SchoolInboxsController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMInbox> getAdminSchoolInboxList(int? page, int? limit, string sortBy, string direction, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMInbox> result = new List<VMInbox>();

            try
            {
                Int64 schoolId = Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId]);

                dt = new Inbox().GetAdminSchoolInboxList(searchString, page, limit, ref total, archiveYN, schoolId);

                var records = (from data in dt.AsEnumerable()
                               select new VMInbox
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   InboxId = data.Field<string>("InboxId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   BodyTextOriginal = data.Field<string>("BodyTextOriginal"),
                                   BodyTextUnread = data.Field<string>("BodyTextUnread"),
                                   URL = data.Field<string>("URL"),
                                   PdfFilePath = data.Field<string>("PdfFilePath"),
                                   LinkType = data.Field<string>("LinkType"),
                                   LinkTypeIcon = data.Field<string>("LinkTypeIcon"),
                                   UpdationDateTime = data.Field<string>("UpdationDateTime"),
                                   NewYN = data.Field<string>("NewYN"),
                                   ArchiveYN = data.Field<string>("ArchiveYN"),
                                   ReadYN = data.Field<string>("ReadYN"),
                                   SchoolCount = data.Field<string>("SchoolCount"),
                                   LinkTypeId = data.Field<string>("LinkTypeId"),
                                   SchoolIdList = data.Field<string>("SchoolIdList")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolInboxList/getAdminSchoolInboxList(MSchoolInboxs)", "SchoolNoticeController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public string getSchoolUnreadNoticeList()
        {
            int err = 0;
            string data = string.Empty;

            try
            {
                Int64 schoolId = Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId]);
                int groupId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.GroupId]);

                err = new Inbox().GetSchoolUnreadNoticeList(schoolId, groupId, ref data);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "GetUnreadNoticeList/getSchoolUnreadNoticeList(MSchoolInboxs)", "SchoolHomeController");
                    data = string.Empty;
                }

                //storing unread notice data
                HttpContext.Current.Session[SessionNames.UnreadNotice] = data;
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetUnreadNoticeList/getSchoolUnreadNoticeList(MSchoolInboxs)", "SchoolHomeController");
            }

            return data;
        }

        public int updateAdminSchoolReadStatus()
        {
            int err = 0;
            string errDesc = string.Empty, data = string.Empty; ;

            try
            {
                //first check update count
                int updateCount = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UnreadNoticeUpdateCount]);

                if (updateCount == 1)
                {
                    Int64 schoolId = Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId]);
                    int groupId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.GroupId]);

                    err = new Inbox().UpdateAdminSchoolReadStatus(schoolId, ref errDesc);

                    if (err == 1)
                    {
                        MCommon.saveExceptionLog(errDesc, "UnreadNoticeListStatus/updateAdminSchoolReadStatus(MSchoolInboxs)", "SchoolNoticeController");
                        errDesc = string.Empty;
                    }

                    if (err == 0)
                    {
                        //remake unread session data
                        err = new Inbox().GetSchoolUnreadNoticeList(schoolId, groupId, ref data);

                        if (err == 1)
                        {
                            MCommon.saveExceptionLog(data, "UnreadNoticeListStatus/updateAdminSchoolReadStatus(MSchoolInboxs)", "SchoolNoticeController");
                            data = string.Empty;
                        }

                        //storing unread notice data
                        HttpContext.Current.Session[SessionNames.UnreadNotice] = data;
                    }
                }

                HttpContext.Current.Session[SessionNames.UnreadNoticeUpdateCount] = updateCount + 1;
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "UnreadNoticeListStatus/updateAdminSchoolReadStatus(MSchoolInboxs)", "SchoolNoticeController");
            }

            return err;
        }
    }
}
