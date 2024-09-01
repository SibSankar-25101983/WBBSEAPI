using System;
using System.Data;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using ViewModel;
using Common;
using DAL;

namespace WBBSE.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }

    public class MCommon
    {
        public List<VMMstUserType> getUserTypeList()
        {
            DataTable dt = new DataTable();
            List<VMMstUserType> result = new List<VMMstUserType>();

            try
            {
                int groupId = (HttpContext.Current.Session[SessionNames.GroupId] == null) ? -1 : Convert.ToInt32(HttpContext.Current.Session[SessionNames.GroupId]);

                dt = new DAL.Common().GetUserTypeList(groupId);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstUserType
                               {
                                   UserTypeId = data.Field<string>("UserTypeId"),
                                   UserType = data.Field<string>("UserType")
                               });

                result = records.ToList();
            }
            catch
            {
                result = null;
            }
            finally
            {
                dt = null;
            }
            return result;
        }

        public int chkDelete(string tableName, string fieldName, string fieldValue)
        {
            int err = 0;
            DAL.Common cm = new DAL.Common();

            try
            {
                err = cm.ChkDelete(tableName, fieldName, fieldValue);
            }
            catch
            {
                err = 1;
            }

            return err;
        }

        public int chkMigration(string tableName, string fieldName, string fieldValue)
        {
            int err = 0;
            DAL.Common cm = new DAL.Common();

            try
            {
                err = cm.ChkMigration(tableName, fieldName, fieldValue);
            }
            catch
            {
                err = 1;
            }

            return err;
        }

        public int chkTransfer(string tableName, string fieldName, string fieldValue)
        {
            int err = 0;
            DAL.Common cm = new DAL.Common();

            try
            {
                err = cm.ChkTransfer(tableName, fieldName, fieldValue);
            }
            catch
            {
                err = 1;
            }

            return err;
        }

        public List<VMMstDesignation> getMstDesignationListDropDown(string userType)
        {
            DataTable dt = new DataTable();
            List<VMMstDesignation> result = new List<VMMstDesignation>();

            try
            {
                dt = new DAL.Common().GetMstDesignationListDropDown(userType);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstDesignation
                               {
                                   DesignationId = data.Field<string>("DesignationId"),
                                   Designation = data.Field<string>("Designation")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "getMstDesignationListDropDown", "MCommon");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstSalutation> getMstSalutationListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstSalutation> result = new List<VMMstSalutation>();

            try
            {
                dt = new DAL.Common().GetMstSalutationListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSalutation
                               {
                                   SalutationId = data.Field<string>("SalutationId"),
                                   SalutationName = data.Field<string>("SalutationName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "getMstSalutationListDropDown", "MCommon");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public static int saveExceptionLog(string exMessage, string actionName, string controllerName)
        {
            int error = 0;
            try
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                error = new DAL.Common().SaveExceptionLog(exMessage, actionName, controllerName, ipAddress);
            }
            catch
            {
                error = 1;
            }
            return error;
        }

        public static byte[] GetPdfFromDB(string ContentId)
        {
            byte[] StrFile;
            try
            {

                StrFile = DAL.Common.GetBinaryFileById(ContentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return StrFile;
        }

        public string getContentLink(Int64 contentId, int linkType, ref string headerData)
        {
            string data = string.Empty;

            try
            {
                int err = 0;

                data = new DAL.Common().GetContentLink(contentId, linkType, ref err, ref headerData);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "getContentLink", "MCommon");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "getContentLink", "MCommon");
            }

            return data;
        }

        public string getInboxLink(Int64 inboxId, int linkType, ref string headerData)
        {
            string data = string.Empty;

            try
            {
                int err = 0;

                data = new Inbox().GetInboxLink(inboxId, linkType, ref err, ref headerData);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(data, "getInboxLink", "MCommon");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "getInboxLink", "MCommon");
            }

            return data;
        }

        public List<VMContentDetails> getNotificationTypeListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMContentDetails> result = new List<VMContentDetails>();

            try
            {
                dt = new Notification().GetNotificationTypeListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMContentDetails
                               {
                                   MenuCode = data.Field<string>("MenuCode"),
                                   NotificationType = data.Field<string>("NotificationType")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetNotificationTypeList/getNotificationTypeListDropDown(MCommon)", "CommonController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }
    }
}
