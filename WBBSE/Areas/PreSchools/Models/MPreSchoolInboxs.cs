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

namespace WBBSE.Areas.PreSchools.Models
{
    public class MPreSchoolInboxs
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
                MCommon.saveExceptionLog(ex.Message, "GetCircularList/GetCircularList(MPreSchoolInboxs)", "PreSchoolInboxsController");
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
                MCommon.saveExceptionLog(ex.Message, "GetNotificationList/getNotificationList(MPreSchoolInboxs)", "PreSchoolInboxsController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMInbox> getAdminPreSchoolInboxList(int? page, int? limit, string sortBy, string direction, ref int total, string searchString, string archiveYN = "A")
        {
            DataTable dt = new DataTable();
            List<VMInbox> result = new List<VMInbox>();

            try
            {
                Int64 preSchoolId = Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId]);

                dt = new Inbox().GetAdminPreSchoolInboxList(searchString, page, limit, ref total, archiveYN, preSchoolId);

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
                MCommon.saveExceptionLog(ex.Message, "GetSchoolInboxList/getAdminPreSchoolInboxList(MPreSchoolInboxs)", "PreSchoolNoticeController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public string getPreSchoolUnreadNoticeList()
        {
            int err = 0;
            string data = string.Empty;

            try
            {
                Int64 preSchoolId = Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId]);
                int groupId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.GroupId]);

                err = new Inbox().GetPreSchoolUnreadNoticeList(preSchoolId, groupId, ref data);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "GetUnreadNoticeList/getPreSchoolUnreadNoticeList(MPreSchoolInboxs)", "PreSchoolNoticeController");
                    data = string.Empty;
                }

                //storing unread notice data
                HttpContext.Current.Session[SessionNames.UnreadNotice] = data;
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetUnreadNoticeList/getPreSchoolUnreadNoticeList(MPreSchoolInboxs)", "PreSchoolNoticeController");
            }

            return data;
        }

        public int updateAdminPreSchoolReadStatus()
        {
            int err = 0;
            string errDesc = string.Empty, data = string.Empty; ;

            try
            {
                //first check update count
                int updateCount = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UnreadNoticeUpdateCount]);

                if (updateCount == 1)
                {
                    Int64 preSchoolId = Convert.ToInt64(HttpContext.Current.Session[SessionNames.OrganizationId]);
                    int groupId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.GroupId]);

                    err = new Inbox().UpdateAdminPreSchoolReadStatus(preSchoolId, ref errDesc);

                    if (err == 1)
                    {
                        MCommon.saveExceptionLog(errDesc, "UnreadNoticeListStatus/updateAdminPreSchoolReadStatus(MPreSchoolInboxs)", "PreSchoolNoticeController");
                        errDesc = string.Empty;
                    }

                    if (err == 0)
                    {
                        //remake unread session data
                        err = new Inbox().GetPreSchoolUnreadNoticeList(preSchoolId, groupId, ref data);

                        if (err == 1)
                        {
                            MCommon.saveExceptionLog(data, "UnreadNoticeListStatus/updateAdminPreSchoolReadStatus(MPreSchoolInboxs)", "PreSchoolNoticeController");
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
                MCommon.saveExceptionLog(ex.Message, "UnreadNoticeListStatus/updateAdminPreSchoolReadStatus(MPreSchoolInboxs)", "PreSchoolNoticeController");
            }

            return err;
        }
    }
}