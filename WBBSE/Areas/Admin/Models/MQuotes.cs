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
    public class MQuotes
    {
        public List<VMQuotes> getQuotesList(int? page, int? limit, string sortBy, string direction, string menuCode, ref int total, string searchString)
        {
            DataTable dt = new DataTable();
            List<VMQuotes> result = new List<VMQuotes>();

            try
            {
                dt = new Quotes().GetQuotesList(searchString, menuCode, page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMQuotes
                               {
                                   ContentId = data.Field<string>("ContentId"),
                                   BodyText = data.Field<string>("BodyText"),
                                   FooterText = data.Field<string>("FooterText")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetQuotesList/getQuotesList(MQuotes)", "AdminWebOthersController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public int saveQuotes(VMQuotes data, string menuCode, ref string errDesc)
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
                string footerText = (Sanitizer.GetSafeHtmlFragment(data.FooterText) ?? DefaultSetting.EmptyVal).Trim();
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
                    if (footerText != "")
                    {
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, footerText))
                        {
                            err = 2;
                            errDesc = Message.Quote.InvalidQuotedBy;
                            return err;
                        }
                    }
                }

                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = new DAL.Common().SaveContentDetails(Convert.ToInt64(GblFunctions.Base64Decode(data.ContentId)), menuCode, Convert.ToInt32(LinkTypes.URL)
                                                        , string.Empty, bodyText, footerText, string.Empty, string.Empty, string.Empty, mode, ipAddress, createdBy, ref errDesc);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "Quotes/saveQuotes(MQuotes)", "AdminWebOthersController");
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "Quotes/saveQuotes(MQuotes)", "AdminWebOthersController");
            }

            return err;
        }
    }
}
