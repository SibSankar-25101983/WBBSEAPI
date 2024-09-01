using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ViewModel;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using DAL;
using Common;

namespace WBBSE.Models
{
    public class MSchoolParameters
    {
        SchoolParameters sp = new SchoolParameters();

        public List<VMMstSchoolCategory> geMstSchoolCategoryListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstSchoolCategory> result = new List<VMMstSchoolCategory>();

            try
            {
                dt = sp.GetMstSchoolCategoryListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchoolCategory
                               {
                                   SchoolCategoryId = data.Field<string>("SchoolCategoryId"),
                                   SchoolCategoryName = data.Field<string>("SchoolCategoryName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolCategoryList/geMstSchoolCategoryListDropDown(MSchoolParameters)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstSchoolType> getMstSchoolTypeListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstSchoolType> result = new List<VMMstSchoolType>();

            try
            {
                dt = sp.GetMstSchoolTypeListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchoolType
                               {
                                   SchoolTypeId = data.Field<string>("SchoolTypeId"),
                                   SchoolTypeName = data.Field<string>("SchoolTypeName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolTypeList/getMstSchoolTypeListDropDown(MSchoolParameters)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstSchoolStatus> getMstSchoolStatusListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstSchoolStatus> result = new List<VMMstSchoolStatus>();

            try
            {
                dt = sp.GetMstSchoolStatusListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchoolStatus
                               {
                                   SchoolStatusId = data.Field<string>("SchoolStatusId"),
                                   SchoolStatusName = data.Field<string>("SchoolStatusName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolStatusList/getMstSchoolStatusListDropDown(MSchoolParameters)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstSchoolMedium> getMstSchoolMediumListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstSchoolMedium> result = new List<VMMstSchoolMedium>();

            try
            {
                dt = sp.GetMstSchoolMediumListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchoolMedium
                               {
                                   SchoolMediumId = data.Field<string>("SchoolMediumId"),
                                   SchoolMediumName = data.Field<string>("SchoolMediumName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolMediumList/getMstSchoolMediumListDropDown(MSchoolParameters)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstSchoolRecognition> getMstSchoolRecognitionListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstSchoolRecognition> result = new List<VMMstSchoolRecognition>();

            try
            {
                dt = sp.GetMstSchoolRecognitionListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchoolRecognition
                               {
                                   SchoolRecognitionId = data.Field<string>("SchoolRecognitionId"),
                                   RecognitionStatus = data.Field<string>("RecognitionStatus")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolMediumList/getMstSchoolRecognitionListDropDown(MSchoolParameters)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstSchoolManagement> getMstSchoolManagementListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstSchoolManagement> result = new List<VMMstSchoolManagement>();

            try
            {
                dt = sp.GetMstSchoolManagementListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchoolManagement
                               {
                                   SchoolManagementId = data.Field<string>("SchoolManagementId"),
                                   SchoolManagement = data.Field<string>("SchoolManagement")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetSchoolManagementList/getMstSchoolManagementListDropDown(MSchoolParameters)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstGeographicalDistrict> getMstGeographicalDistrictListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstGeographicalDistrict> result = new List<VMMstGeographicalDistrict>();

            try
            {
                dt = sp.GetMstGeographicalDistrictListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstGeographicalDistrict
                               {
                                   DistrictId = data.Field<string>("DistrictId"),
                                   DistrictName = data.Field<string>("DistrictName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetDistrictList/getMstGeographicalDistrictListDropDown(MSchoolParameters)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public void getZoneDistrictBySubDivisionId(int subDivisionid, ref string zoneName, ref string districtName, ref string indexInitial)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = sp.GetZoneDistrictBySubDivisionId(subDivisionid);

                if (dt != null && dt.Rows.Count == 1)
                {
                    zoneName = dt.Rows[0]["ZoneName"].ToString();
                    districtName = dt.Rows[0]["DistrictName"].ToString();
                    indexInitial = dt.Rows[0]["IndexInitial"].ToString();
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetZoneDistrict/getZoneDistrictBySubDivisionId(MSchoolParameters)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }
        }

        public string getBlockByCircleId(int circleId)
        {
            string data = string.Empty;
            DataTable dt = new DataTable();

            try
            {
                dt = sp.GetBlockByCircleId(circleId);

                if (dt != null && dt.Rows.Count == 1)
                {
                    data = dt.Rows[0]["BlockName"].ToString();
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetBlock/getBlockByCircleId(MSchoolParameters)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }
            return data;
        }

        public List<VMMstSchoolRecognition> getMstSchoolRecognitionList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total)
        {
            DataTable dt = new DataTable();
            SchoolParameters oSch = new SchoolParameters();
            List<VMMstSchoolRecognition> result = new List<VMMstSchoolRecognition>();

            try
            {
                dt = oSch.getMstSchoolRecognitionList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchoolRecognition
                               {
                                   SlNo = data.Field<Int64>("SlNo"),
                                   SchoolRecognitionId = data.Field<string>("SchoolRecognitionId"),
                                   RecognitionStatus = data.Field<string>("RecognitionStatus"),
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
                oSch = null;
                dt = null;
            }
            return result;
        }

        public int saveMstSchoolRecognition(VMMstSchoolRecognition data, ref string errDesc)
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

                    SchoolParameters oSch = new SchoolParameters();
                    error = oSch.saveMstSchoolRecognition(data, mode, ipAddress, createdBy, ref errDesc);
                }
            }
            catch (Exception ex)
            {
                error = 1;
                errDesc = ex.Message;
            }

            if (error == 1)
            {
                MCommon.saveExceptionLog(errDesc, "SchoolRecognition/saveMstSchoolRecognition(MSchoolParameters)", "AdminSchoolRecognitionController");
            }

            return error;
        }

        public List<VMMstSchoolManagement> getMstSchoolManagementList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total)
        {
            DataTable dt = new DataTable();
            SchoolParameters oSp = new SchoolParameters();
            List<VMMstSchoolManagement> result = new List<VMMstSchoolManagement>();

            try
            {
                dt = oSp.getMstSchoolManagementList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstSchoolManagement
                               {
                                   SlNo = data.Field<Int64>("SlNo"),
                                   SchoolManagementId = data.Field<string>("SchoolManagementId"),
                                   SchoolManagement = data.Field<string>("SchoolManagement"),
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
                oSp = null;
                dt = null;
            }
            return result;
        }

        public int saveMstSchoolManagement(VMMstSchoolManagement data, ref string errDesc)
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
                    if (!validateSaveForManagement(data, mode, ref errDesc))
                    {
                        error = 2;
                        return error;
                    }
                    #endregion

                    ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                    SchoolParameters oSp = new SchoolParameters();
                    error = oSp.saveMstSchoolManagement(data, mode, ipAddress, createdBy, ref errDesc);
                }
            }
            catch (Exception ex)
            {
                error = 1;
                errDesc = ex.Message;
            }

            if (error == 1)
            {
                MCommon.saveExceptionLog(errDesc, "SchoolManagement/saveMstSchoolManagement(MSchoolParameters)", "AdminSchoolManagementController");
            }

            return error;
        }

        private bool validateSave(VMMstSchoolRecognition data, string mode, ref string errDesc)
        {
            if (mode != Mode.DELETE)
            {
                string recognitionStatus = string.Empty;
                recognitionStatus = Sanitizer.GetSafeHtmlFragment((data.RecognitionStatus ?? "").Trim());

                if (string.IsNullOrEmpty(recognitionStatus))
                {
                    errDesc = Message.SchoolParameter.RecognitionStatusRequired;
                    return false;
                }
                
                if (!GblFunctions.chkDataFormat(RegexType.Alpha, recognitionStatus))
                {
                    errDesc = Message.SchoolParameter.RecognitionStatusInvalid;
                    return false;
                }
            }
            return true;
        }

        private bool validateSaveForManagement(VMMstSchoolManagement data, string mode, ref string errDesc)
        {
            if (mode != Mode.DELETE)
            {
                string schoolManagement = string.Empty;
                schoolManagement = Sanitizer.GetSafeHtmlFragment((data.SchoolManagement ?? "").Trim());

                if (string.IsNullOrEmpty(schoolManagement))
                {
                    errDesc = Message.SchoolParameter.SchoolManagementRequired;
                    return false;
                }
                //if (!string.IsNullOrEmpty(schoolManagement))
                //{
                //    Match match = Regex.Match(schoolManagement, @"^([A-Za-z0-9 \/\-()_,.]+)$", RegexOptions.IgnoreCase);
                //    if (!match.Success)
                //    {
                //        errDesc = Message.SchoolParameter.SchoolManagementInvalid;
                //        return false;
                //    }
                //}

                if (!GblFunctions.chkDataFormat(RegexType.Alpha, schoolManagement))
                {
                    errDesc = Message.SchoolParameter.SchoolManagementInvalid;
                    return false;
                }
            }
            return true;
        }

        public DataTable DownloadMstSchoolRecognitionList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, int reportFormat)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = new SchoolParameters().DownloadMstSchoolRecognitionList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total, reportFormat);
            }
            catch
            {
                dt = null;
            }

            return dt;
        }

        public DataTable DownloadMstSchoolManagementList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, int reportFormat)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = new SchoolParameters().DownloadMstSchoolManagementList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total, reportFormat);
            }
            catch
            {
                dt = null;
            }

            return dt;
        }
    
    }
}
