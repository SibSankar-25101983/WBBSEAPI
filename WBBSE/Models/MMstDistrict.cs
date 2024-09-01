using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using ViewModel;
using Common;
using DAL;

namespace WBBSE.Models
{
    public class MMstDistrict
    {
        public List<VMMstDistrict> getMstDistrictListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstDistrict> result = new List<VMMstDistrict>();

            try
            {
                dt = new District().getMstDistrictListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstDistrict
                               {
                                   DistrictId = data.Field<string>("DistrictId"),
                                   DistrictName = data.Field<string>("DistrictName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetDistrictList/getMstDistrictListDropDown(MMstDistrict)", "AdminSubDivisionController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstDistrict> getMstDistrictListDropDown(int zoneId)
        {
            DataTable dt = new DataTable();
            List<VMMstDistrict> result = new List<VMMstDistrict>();

            try
            {
                dt = new District().getMstDistrictListDropDownByZoneId(zoneId);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstDistrict
                               {
                                   DistrictId = data.Field<string>("DistrictId"),
                                   DistrictName = data.Field<string>("DistrictName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetDistrictList/getMstDistrictListDropDown(MMstDistrict)", "AdminSubDivisionController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }
        
        public List<VMMstDistrict> getMstDistrictList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total)
        {
            DataTable dt = new DataTable();
            District oDis = new District();
            List<VMMstDistrict> result = new List<VMMstDistrict>();

            try
            {
                dt = oDis.getMstDistrictList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstDistrict
                               {
                                   SlNo = data.Field<Int64>("SlNo"),
                                   DistrictId = data.Field<string>("DistrictId"),
                                   DistrictName = data.Field<string>("DistrictName"),
                                   ZoneId = data.Field<string>("ZoneId"),
                                   ZoneName = data.Field<string>("ZoneName"),
                                   IndexInitial = data.Field<string>("IndexInitial"),
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
                oDis = null;
                dt = null;
            }
            return result;
        }

        public int saveMstDistrict(VMMstDistrict data, ref string errDesc)
        {
            int error = 0, createdBy = 0;
            string ipAddress = string.Empty, mode = string.Empty;
            try
            {
                mode = ((data.EntType == EntType.ADD) ? Mode.ADD : ((data.EntType == EntType.EDIT) ? Mode.EDIT : ((data.EntType == EntType.DELETE) ? Mode.DELETE : Mode.ERROR)));
                if (mode == Mode.ERROR)
                {
                    error = 1;
                    return error;
                }

                #region VALIDATIONS
                if (!validateSave(data, mode, ref errDesc))
                {
                    errDesc = errDesc.Replace("<br/>", "");
                    error = 2;
                    return error;
                }
                #endregion

                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                District oDis = new District();
                error = oDis.saveMstDistrict(data, mode, ipAddress, createdBy, ref errDesc);
            }
            catch (Exception ex)
            {
                error = 1;
                errDesc = ex.Message;
            }

            if (error == 1)
            {
                MCommon.saveExceptionLog(errDesc, "Districts/saveMstDistrict(MMstDistrict)", "AdminDistrictController");
            }

            return error;
        }

        public List<VMMstDistrict> getIndexInitialList()
        {
            List<VMMstDistrict> result = new List<VMMstDistrict>();

            try
            {
                for (int i = 65; i < 91; i++)
                {
                    var x = new VMMstDistrict();
                    x.IndexInitial = ((char)i).ToString();
                    x.IndexInitialEnc = GblFunctions.Base64Encode(((char)i).ToString());
                    result.Add(x);
                }
            }
            catch
            {
                result = null;
            }
            finally
            {

            }
            return result;
        }

        public int chkDuplicateIndexInitial(string indexInitial, ref string errDesc)
        {
            int err = 0;

            try
            {
                District oDis = new District();

                if (indexInitial == DefaultSetting.DefaultValEnc)
                {
                    err = 1;
                    errDesc = Message.OperationError;
                }
                else
                {
                    err = oDis.chkDuplicateIndexInitial(GblFunctions.Base64Decode(indexInitial), ref errDesc);
                }

                errDesc = HttpContext.Current.Server.HtmlDecode(errDesc);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "Districts/chkDuplicateIndexInitial(MMstDistrict)", "AdminDistrictController");
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Districts/chkDuplicateIndexInitial(MMstDistrict)", "AdminDistrictController");
            }

            return err;
        }

        private bool validateSave(VMMstDistrict data, string mode, ref string errDesc)
        {
            if (mode != Mode.DELETE)
            {
                string districtName = string.Empty;
                districtName = Sanitizer.GetSafeHtmlFragment((data.DistrictName ?? "").Trim());

                if ((string.IsNullOrEmpty(data.ZoneId)) || Sanitizer.GetSafeHtmlFragment(data.ZoneId) == DefaultSetting.DefaultValEnc)
                {
                    errDesc = Message.Zone.ZoneNameRequired;
                    return false;
                }
                if (mode == Mode.ADD)
                {
                    if ((string.IsNullOrEmpty(data.IndexInitial)) || Sanitizer.GetSafeHtmlFragment(data.IndexInitial) == DefaultSetting.DefaultValEnc)
                    {
                        errDesc = Message.District.IndexInitial;
                        return false;
                    }

                    //duplicate index initial check
                    int err = chkDuplicateIndexInitial(Sanitizer.GetSafeHtmlFragment(data.IndexInitial), ref errDesc);

                    if (err > 0)
                    {
                        return false;
                    }
                }
                if (string.IsNullOrEmpty(districtName))
                {
                    errDesc = Message.District.DistrictNameRequired;
                    return false;
                }
                if (!string.IsNullOrEmpty(districtName))
                {
                    if (!GblFunctions.chkDataFormat(RegexType.Alpha, districtName))
                    {
                        errDesc = Message.District.DistrictNameInvalid;
                        return false;
                    }
                }
            }
            return true;
        }

        public DataTable DownloadMstDistrictList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, int reportFormat)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = new District().DownloadMstDistrictList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total, reportFormat);
            }
            catch
            {
                dt = null;
            }

            return dt;
        }
    }
}
