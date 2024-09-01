using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ViewModel;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using Common;
using DAL;

namespace WBBSE.Models
{
    public class MMstZone
    {
        public List<VMMstZone> getMstZoneListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstZone> result = new List<VMMstZone>();

            try
            {
                dt = new Zone().getMstZoneListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstZone
                               {
                                   ZoneId = data.Field<string>("ZoneId"),
                                   ZoneName = data.Field<string>("ZoneName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetZoneList/getMstZoneListDropDown(MMstZone)", "AdminDistrictController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }
        
        public List<VMMstZone> getMstZoneList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total)
        {
            DataTable dt = new DataTable();
            Zone oZn = new Zone();
            List<VMMstZone> result = new List<VMMstZone>();

            try
            {
                dt = oZn.getMstZoneList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstZone
                               {
                                   SlNo = data.Field<Int64>("SlNo"),
                                   ZoneId = data.Field<string>("ZoneId"),
                                   ZoneName = data.Field<string>("ZoneName"),
                                   StateId = data.Field<string>("StateId"),
                                   StateName = data.Field<string>("StateName"),
                                   MigYN = data.Field<string>("MigYN"),
                               }).AsQueryable();

                result = records.ToList();
            }
            catch
            {
                result = null;
            }
            finally
            {
                oZn = null;
                dt = null;
            }
            return result;
        }

        public int saveMstZone(VMMstZone data, ref string errDesc)
        {
            int error = 0, createdBy = 0;
            string ipAddress = string.Empty, mode = string.Empty;
            try
            {
                mode = ((data.EntType == EntType.ADD) ? Mode.ADD : ((data.EntType == EntType.EDIT) ? Mode.EDIT : ((data.EntType == EntType.DELETE) ? Mode.DELETE : Mode.ERROR)));
                if (mode == Mode.ERROR)
                {
                    error = 1;
                }
                else
                {
                    #region VALIDATIONS
                    if (!validateSave(data, mode, ref errDesc))
                    {
                        error = 2;
                        return error;
                    }
                    #endregion

                    ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                    Zone oZn = new Zone();
                    error = oZn.saveMstZone(data, mode, ipAddress, createdBy, ref errDesc);
                }
            }
            catch (Exception ex)
            {
                error = 1;
                errDesc = ex.Message;
            }

            if (error == 1)
            {
                MCommon.saveExceptionLog(errDesc, "Zones/saveMstZone(MMstZone)", "AdminZoneController");
            }

            return error;
        }

        private bool validateSave(VMMstZone data, string mode, ref string errDesc)
        {
            if (mode != Mode.DELETE)
            {
                string zoneName = string.Empty;
                zoneName = Sanitizer.GetSafeHtmlFragment((data.ZoneName ?? "").Trim());

                if ((string.IsNullOrEmpty(data.StateId)) || Sanitizer.GetSafeHtmlFragment(data.StateId) == DefaultSetting.DefaultValEnc)
                {                    
                    errDesc = Message.State.StateNameRequired;
                    return false;
                }
                if (string.IsNullOrEmpty(zoneName))
                {                   
                    errDesc = Message.Zone.ZoneNameRequired;
                    return false;
                }
                if (zoneName != "")
                {
                    Match match = Regex.Match(zoneName, @"^([A-Za-z0-9 \/\-()_,.]+)$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        errDesc = Message.Zone.ZoneNameInvalid;
                        return false;
                    }
                }
            }
            return true;
        }

        public DataTable DownloadMstZoneList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, int reportFormat)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = new Zone().DownloadMstZoneList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total, reportFormat);
            }
            catch
            {
                dt = null;
            }

            return dt;
        }
    }
}