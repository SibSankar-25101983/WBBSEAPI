using System;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections.Generic;
using Microsoft.Security.Application;
using WBBSE.Models;
using ViewModel;
using Common;
using DAL;

namespace WBBSE.Areas.Admin.Models
{
    public class MMarquee
    {
        public List<VMMarquee> getMarqueeList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total)
        {
            DataTable dt = new DataTable();
            List<VMMarquee> result = new List<VMMarquee>();

            try
            {
                dt = new Marquee().GetMarqueeList(menuCode, page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMMarquee
                               {
                                   ContentId = data.Field<string>("ContentId"),
                                   BodyText = data.Field<string>("BodyText")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetMarqueeList/getMarqueeList(MMarquee)", "AdminWebOthersController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public int saveMarquee(VMMarquee data, string menuCode, ref string errDesc)
        {
            int err = 0, createdBy = 0;
            string mode = string.Empty, ipAddress = string.Empty;

            try
            {
                mode = ((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));

                if (mode == Mode.ERROR)
                {
                    err = 1;
                    return err;
                }

                string bodyText = Sanitizer.GetSafeHtmlFragment((data.BodyText ?? string.Empty).Trim());
                if (mode != Mode.DELETE)
                {
                    if (bodyText == string.Empty)
                    {
                        err = 2;
                        errDesc = Message.Content.ContentRequired;
                        return err;
                    }
                    if (!GblFunctions.chkDataFormat(RegexType.Alpha, bodyText))
                    {
                        err = 2;
                        errDesc = Message.Content.InvalidContent;
                        return err;
                    }
                }

                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = new DAL.Common().SaveContentDetails(Convert.ToInt64(GblFunctions.Base64Decode(data.ContentId)), menuCode, Convert.ToInt32(LinkTypes.URL)
                                                        , string.Empty, bodyText.ToUpper(), string.Empty, string.Empty, string.Empty, string.Empty, mode
                                                        , ipAddress, createdBy, ref errDesc);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "Marquee/saveMarquee(MMarquee)", "AdminWebOthersController");
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "Marquee/saveMarquee(MMarquee)", "AdminWebOthersController");
            }

            return err;
        }
    }
}
