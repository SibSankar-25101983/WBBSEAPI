using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using ViewModel;
using DAL;
using Common;

namespace WBBSE.Models
{
    public class MMstSubDivision
    {
        public List<VMMstSubDivision> getMstSubDivisionListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstSubDivision> result = new List<VMMstSubDivision>();

            try
            {
                dt = new SubDivision().GetMstSubDivisionListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSubDivision
                               {
                                   SubDivisionId = data.Field<string>("SubDivisionId"),
                                   SubDivisionName = data.Field<string>("SubDivisionName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSubDivision/getMstSubDivisionListDropDown(Model)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstSubDivision> getMstSubDivisionListDropDown(int districtId)
        {
            DataTable dt = new DataTable();
            List<VMMstSubDivision> result = new List<VMMstSubDivision>();

            try
            {
                dt = new SubDivision().GetMstSubDivisionListDropDownByDistrictId(districtId);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSubDivision
                               {
                                   SubDivisionId = data.Field<string>("SubDivisionId"),
                                   SubDivisionName = data.Field<string>("SubDivisionName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSubDivision/getMstSubDivisionListDropDown(Model)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstSubDivision> getMstSubDivisionList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total)
        {
            DataTable dt = new DataTable();
            SubDivision oSD = new SubDivision();
            List<VMMstSubDivision> result = new List<VMMstSubDivision>();

            try
            {
                dt = oSD.getMstSubDivisionList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSubDivision
                               {
                                   SlNo = data.Field<Int64>("SlNo"),
                                   SubDivisionId = data.Field<string>("SubDivisionId"),
                                   SubDivisionName = data.Field<string>("SubDivisionName"),
                                   DistrictId = data.Field<string>("DistrictId"),
                                   DistrictName = data.Field<string>("DistrictName"),
                                   IndexInitial = data.Field<string>("IndexInitial"),
                                   MaxIndexNo = data.Field<string>("MaxIndexNo"),
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
                oSD = null;
                dt = null;
            }
            return result;
        }

        public int saveMstSubDivision(VMMstSubDivision data, ref string errDesc)
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

                    error = new SubDivision().saveMstSubDivision(data, mode, ipAddress, createdBy, ref errDesc);
                }
            }
            catch (Exception ex)
            {
                error = 1;
                errDesc = ex.Message;
            }

            if (error == 1)
            {
                MCommon.saveExceptionLog(errDesc, "SubDivisions/saveMstSubDivision(MMstSubDivision)", "AdminSubDivisionController");
            }

            return error;
        }

        private bool validateSave(VMMstSubDivision data, string mode, ref string errDesc)
        {
            if (mode != Mode.DELETE)
            {
                string subDivisionName = string.Empty;
                subDivisionName = Sanitizer.GetSafeHtmlFragment((data.SubDivisionName ?? "").Trim());

                if ((string.IsNullOrEmpty(data.DistrictId)) || Sanitizer.GetSafeHtmlFragment(data.DistrictId) == DefaultSetting.DefaultValEnc)
                {
                    errDesc = Message.District.DistrictNameRequired;
                    return false;
                }

                if (string.IsNullOrEmpty(subDivisionName))
                {
                    errDesc = Message.SubDivision.SubDivisionNameRequired;
                    return false;
                }
                if (!string.IsNullOrEmpty(subDivisionName))
                {
                    if (!GblFunctions.chkDataFormat(RegexType.Alpha, subDivisionName))
                    {
                        errDesc = Message.SubDivision.SubDivisionNameInvalid;
                        return false;
                    }
                }
            }
            return true;
        }

        public DataTable DownloadMstSubDivisionList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, int reportFormat)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = new SubDivision().DownloadMstSubDivisionList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total, reportFormat);
            }
            catch
            {
                dt = null;
            }

            return dt;
        }

        public List<VMMstSubDivision> getMstSubDivisionListSchoolTransferDropDown(Int64 schoolId)
        {
            DataTable dt = new DataTable();
            List<VMMstSubDivision> result = new List<VMMstSubDivision>();

            try
            {
                dt = new SubDivision().GetMstSubDivisionListSchoolTransferDropDown(schoolId);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSubDivision
                               {
                                   SubDivisionId = data.Field<string>("SubDivisionId"),
                                   SubDivisionName = data.Field<string>("SubDivisionName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSubDivision/getMstSubDivisionListSchoolTransferDropDown(Model)", "AdminSchoolTransferController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }
    }
}
