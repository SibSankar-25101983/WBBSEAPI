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
    public class MMstCircle
    {
        public List<VMMstCircle> getMstCircleListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstCircle> result = new List<VMMstCircle>();

            try
            {
                dt = new Circle().GetMstCircleListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstCircle
                               {
                                   CircleId = data.Field<string>("CircleId"),
                                   CircleName = data.Field<string>("CircleName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetCircleList/getMstCircleListDropDown(Model)", "AdminSchoolController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }

        public List<VMMstCircle> getMstCircleList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total)
        {
            DataTable dt = new DataTable();
            Circle oCrcl = new Circle();
            List<VMMstCircle> result = new List<VMMstCircle>();

            try
            {
                dt = oCrcl.getMstCircleList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstCircle
                               {
                                   SlNo = data.Field<Int64>("SlNo"),
                                   CircleId = data.Field<string>("CircleId"),
                                   CircleName = data.Field<string>("CircleName"),
                                   BlockId = data.Field<string>("BlockId"),
                                   BlockName = data.Field<string>("BlockName"),
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
                oCrcl = null;
                dt = null;
            }
            return result;
        }

        public DataTable downloadMstCircleList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, int reportFormat)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = new Circle().downloadMstCircleList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total, reportFormat);
            }
            catch
            {
                dt = null;
            }

            return dt;
        }

        public int saveMstCircle(VMMstCircle crcl, ref string errDesc)
        {
            int error = 0;
            try
            {
                int circleId = 0, blockId = 0, createdBy = 0;
                string circleName = string.Empty, ipAddress = string.Empty;
                string mode = ((crcl.EntType == EntType.ADD) ? Mode.ADD : ((crcl.EntType == EntType.EDIT) ? Mode.EDIT : ((crcl.EntType == EntType.DELETE) ? Mode.DELETE : Mode.ERROR)));
                if (mode == Mode.ERROR)
                {
                    error = 1;
                }
                else
                {
                    if (mode != Mode.ADD)
                    {
                        if ((string.IsNullOrEmpty(crcl.CircleId)) || Sanitizer.GetSafeHtmlFragment(crcl.CircleId) == DefaultSetting.DefaultValEnc)
                        {
                            error = 2;
                            errDesc = Message.Circle.CircleIdRequired;
                            return error;
                        }
                        circleId = Convert.ToInt32(GblFunctions.Base64Decode(crcl.CircleId));
                    }
                    if (mode != Mode.DELETE)
                    {
                        #region VALIDATIONS

                        circleName = Sanitizer.GetSafeHtmlFragment((crcl.CircleName ?? "").Trim());
                        if ((string.IsNullOrEmpty(crcl.BlockId)) || Sanitizer.GetSafeHtmlFragment(crcl.BlockId) == DefaultSetting.DefaultValEnc)
                        {
                            error = 2;
                            errDesc = Message.Block.BlockNameRequired;
                            return error;
                        }
                        if (string.IsNullOrEmpty(circleName))
                        {
                            error = 2;
                            errDesc = Message.Circle.CircleNameRequired;
                            return error;
                        }
                        if (!string.IsNullOrEmpty(circleName))
                        {
                            Match match = Regex.Match(circleName, @"^([A-Za-z0-9 \/\-()_,.]+)$", RegexOptions.IgnoreCase);
                            if (!match.Success)
                            {
                                error = 2;
                                errDesc = Message.Circle.CircleNameInvalid;
                                return error;
                            }
                        }

                        #endregion

                        //circleName = (string.IsNullOrEmpty(crcl.CircleName) == true) ? "" : crcl.CircleName.Trim();
                        blockId = Convert.ToInt32(GblFunctions.Base64Decode(crcl.BlockId));
                    }
                    ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                    Circle oCrcl = new Circle();
                    error = oCrcl.saveMstCircle(mode, circleId, circleName, blockId, ipAddress, createdBy, ref errDesc);
                }
            }
            catch (Exception ex)
            {
                error = 1;
                errDesc = ex.Message;
            }
            if (error == 1)
            {
                MCommon.saveExceptionLog(errDesc, "Circles/saveMstCircle(MMstCircle)", "AdminCircleController");
            }

            return error;
        }
    }
}