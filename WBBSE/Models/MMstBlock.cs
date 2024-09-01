using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using Common;
using DAL;

namespace WBBSE.Models
{
    public class MMstBlock
    {
        public List<VMMstBlock> getMstBlockListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstBlock> result = new List<VMMstBlock>();

            try
            {
                dt = new Block().getMstBlockListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstBlock
                               {
                                   BlockId = data.Field<string>("BlockId"),
                                   BlockName = data.Field<string>("BlockName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetBlockList/getMstBlockListDropDown(MMstBlock)", "AdminCircleController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }
        
        public List<VMMstBlock> getMstBlockList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total)
        {
            DataTable dt = new DataTable();
            Block oBlck = new Block();
            List<VMMstBlock> result = new List<VMMstBlock>();

            try
            {
                dt = oBlck.getMstBlockList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstBlock
                               {
                                   SlNo = data.Field<Int64>("SlNo"),
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
                oBlck = null;
                dt = null;
            }
            return result;
        }

        public DataTable downloadMstBlockList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, int reportFormat)
        {
            DataTable dt = new DataTable();
            
            try
            {
                dt = new Block().downloadMstBlockList(((string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString), page, limit, ref total, reportFormat);
            }
            catch (Exception ex)
            {
                dt = null;
                MCommon.saveExceptionLog(ex.Message, "DownloadReport/downloadMstBlockList(MMstBlock)", "AdminBlockController");
            }

            return dt;
        }

        public int saveMstBlock(VMMstBlock blck, ref string errDesc)
        {
            int error = 0;
            try
            {
                int blockId = 0, createdBy = 0;
                string blockName = string.Empty, ipAddress = string.Empty;
                string mode = ((blck.EntType == EntType.ADD) ? Mode.ADD : ((blck.EntType == EntType.EDIT) ? Mode.EDIT : ((blck.EntType == EntType.DELETE) ? Mode.DELETE : Mode.ERROR)));
                if (mode == Mode.ERROR)
                {
                    error = 1;
                }
                else
                {
                    if (mode != Mode.ADD)
                    {
                        if (string.IsNullOrEmpty(blck.BlockId))
                        {
                            error = 2;
                            errDesc = Message.Block.BlockIdRequired;
                            return error;
                        }
                        blockId = Convert.ToInt32(GblFunctions.Base64Decode(blck.BlockId));
                    }
                    if (mode != Mode.DELETE)
                    {

                        #region VALIDATIONS FOR REQUIRED FIELD AND FORMAT
                        blockName = Sanitizer.GetSafeHtmlFragment((blck.BlockName ?? "").Trim());
                        if (string.IsNullOrEmpty(blockName))
                        {
                            error = 2;
                            errDesc = Message.Block.BlockNameRequired;
                            return error;
                        }
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, blockName))
                        {
                            error = 2;
                            errDesc = Message.Block.BlockNameInvalid;
                            return error;
                        }
                        #endregion
                    }

                    ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                    error = new Block().saveMstBlock(mode, blockId, blockName, ipAddress, createdBy, ref errDesc);
                }
            }
            catch
            {
                error = 1;
            }
            if (error == 1)
            {
                MCommon.saveExceptionLog(errDesc, "Blocks/saveMstBlock(MMstBlock)", "AdminBlockController");
            }

            return error;
        }
    }
}
