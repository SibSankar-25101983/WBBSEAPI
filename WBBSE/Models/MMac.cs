using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel;
using Common;
using DAL;
using Microsoft.Security.Application;
using System.Xml.Linq;

namespace WBBSE.Models
{
    public class MMac
    {
        public string chkMACAuthorizationRequestPermision(string machineId)
        {
            string data = string.Empty;

            try
            {
                data = new MAC().ChkMACAuthorizationRequestPermision(machineId);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "ChkRequestPermision/chkMACAuthorizationRequestPermision(MMac)", "WebController");
            }

            return data;
        }

        public int validateMAC(string machineId, string tokenId, string successYN)
        {
            int err = 0;

            try
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                int createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = new MAC().ValidateMAC(machineId, tokenId, successYN, createdBy, ipAddress);
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "Login/validateMAC(MMac)", "AdminLoginController");
            }

            return err;
        }

        public string chkMACSoftwareDownloadPermission(int userId)
        {
            string data = string.Empty;

            try
            {
                data = new MAC().ChkMACSoftwareDownloadPermission(userId);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "UnAuthorized/chkMACSoftwareDownloadPermission(MMac)", "WebController");
            }

            return data;
        }

        public int saveMachineDetails(VMMac data, int createdBy, ref string errDesc)
        {
            int err = 0, tblId = 0;
            string machineName = string.Empty, machineId = string.Empty, authorizedYN = string.Empty;

            try
            {
                string mode = ((data.EntType == "I") ? Mode.ADD : ((data.EntType == "E") ? Mode.EDIT : ((data.EntType == "D") ? Mode.DELETE : Mode.ERROR)));
                #region VALIDATIONS
                if (mode == Mode.ERROR)
                {
                    err = 1;
                    errDesc = Message.OperationError;
                    return err;
                }

                if (mode != Mode.ADD)
                {
                    tblId = Convert.ToInt32(Sanitizer.GetSafeHtmlFragment(data.TblId).Trim());
                }

                if (mode != Mode.DELETE)
                {
                    if (mode == Mode.ADD)
                    {
                        machineName = Sanitizer.GetSafeHtmlFragment(data.ComputerName ?? "").Trim();
                        if (string.IsNullOrEmpty(machineName))
                        {
                            err = 2;
                            errDesc = Message.MAC.MachineNameRequired;
                            return err;
                        }
                    }
                    machineId = Sanitizer.GetSafeHtmlFragment(data.MachineId ?? "").Trim();
                    if (string.IsNullOrEmpty(machineId))
                    {
                        err = 2;
                        errDesc = Message.MAC.MachineIdRequired;
                        return err;
                    }
                    authorizedYN = Sanitizer.GetSafeHtmlFragment(data.AuthorizedYN ?? "").Trim();
                    if (string.IsNullOrEmpty(authorizedYN) || (authorizedYN != "Y" && authorizedYN != "N"))
                    {
                        err = 2;
                        errDesc = Message.MAC.AuthorizationStatusRequired;
                        return err;
                    }

                    //check whether request is alerady made or not
                    errDesc = chkMACAuthorizationRequestPermision(machineId);

                    if (errDesc == "N")
                    {
                        err = 2;
                        errDesc = Message.MAC.DuplicateAuthorizationRequest;
                        return err;
                    }
                }
                #endregion

                string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                err = new MAC().SaveMachineDetails(machineName, machineId, authorizedYN, tblId, mode, ipAddress, createdBy, ref errDesc);
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "Token/saveMachineDetails(MMac)", "WebController");
            }

            return err;
        }

        public int SaveMACSoftwareDownloadDetails(int userId, string downloadType, ref string errDesc)
        {

            int err = 0;

            try
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                err = new MAC().SaveMACSoftwareDownloadDetails(userId, downloadType, ipAddress, ref errDesc);

            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "UnAuthorized/SaveMACSoftwareDownloadDetails(MMac)", "WebController");
            }

            return err;
        }

        public int deleteMACToken()
        {
            int err = 0;
            string errDesc = string.Empty;

            try
            {
                err = new MAC().DeleteMACToken(ref errDesc);

                if (err == 1)
                {
                    MCommon.saveExceptionLog(errDesc, "Login/deleteMACToken(MMac)", "AdminLoginController");
                }
            }
            catch (Exception ex)
            {
                err = 1;
                MCommon.saveExceptionLog(ex.Message, "Login/deleteMACToken(MMac)", "AdminLoginController");
            }

            return err;
        }

        public List<VMMac> loadMACList(DateTime? fromDate, DateTime? toDate, string searchType, string searchValue)
        {
            DataTable dt = new DataTable();
            List<VMMac> result = new List<VMMac>();

            try
            {
                dt = new MAC().loadMACList(fromDate, toDate, searchType, searchValue);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["TblId"] = GblFunctions.encryptPassword(dt.Rows[i]["TblId"].ToString());
                    }
                }

                var records = (from data in dt.AsEnumerable()
                               select new VMMac
                               {
                                   SlNo = data.Field<string>("SlNo"),
                                   TblId = data.Field<string>("TblId"),
                                   ComputerName = data.Field<string>("ComputerName"),
                                   MachineId = data.Field<string>("MachineId"),
                                   AuthorizedYN = data.Field<string>("AuthorizedYN"),
                                   CreationDateTime = data.Field<string>("CreationDateTime"),
                                   IpAddress = data.Field<string>("IpAddress")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "MacRequestList/loadMACList(MMac)", "AdminMacAuthorizationController");
            }

            return result;
        }

        public int saveMacAuthorizationData(List<VMMac> MacAuthorizationData, ref string errDesc)
        {
            int err = 0;

            try
            {
                var xmlAuthorizationData = new XElement("Permission",
                from data in MacAuthorizationData
                select new XElement("Device",
                             //new XAttribute("TblId", GblFunctions.decryptPassword(data.TblId)),
                             new XElement("TblId", GblFunctions.decryptPassword(data.TblId)),
                             new XElement("AuthorizedYN", (string.IsNullOrEmpty(Sanitizer.GetSafeHtmlFragment(data.AuthorizedYN)) ? "N" : "Y"))
                           ));

                string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                int createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                err = new MAC().SaveMacAuthorizationData(xmlAuthorizationData.ToString(), ipAddress, createdBy, ref errDesc);
            }
            catch (Exception ex)
            {
                err = 1;
                errDesc = ex.Message;
            }
            if (err == 1)
            {
                MCommon.saveExceptionLog(errDesc, "MacRequestList/saveMacAuthorizationData(MMac)", "AdminMacAuthorizationController");
            }

            return err;
        }
    }
}
