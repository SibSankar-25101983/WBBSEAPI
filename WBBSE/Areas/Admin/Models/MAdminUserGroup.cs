using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using ViewModel;
using DAL;
using Common;
using WBBSE.Models;

namespace WBBSE.Areas.Admin.Models
{
    public class MAdminUserGroup
    {
        public List<VMMstUserGroup> getUserGroupList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total, int groupId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserGroup ug = new UserGroup();
            List<VMMstUserGroup> result = new List<VMMstUserGroup>();

            try
            {
                int userTypeId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserTypeId]);

                ds = ug.GetUserGroupList(groupId, userTypeId, (string.IsNullOrWhiteSpace(searchString) == true) ? "" : searchString, page, limit, ref total);

                dt = ds.Tables[0];

                var records = (from data in dt.AsEnumerable()
                               select new VMMstUserGroup
                               {
                                   GroupId = data.Field<string>("GroupId"),
                                   GroupName = data.Field<string>("GroupName"),
                                   AccessType = data.Field<string>("AccessType"),
                                   SystemYN = data.Field<string>("SystemYN"),
                                   ActiveYN = data.Field<string>("ActiveYN"),
                                   UserTypeId = data.Field<string>("UserTypeId"),
                                   UserType = data.Field<string>("UserType")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch
            {
                result = null;
            }
            finally
            {
                ug = null;
                ds = null;
                dt = null;
            }
            return result;
        }

        public List<VMRoleDetails> getParentRoleList()
        {
            DataTable dt = new DataTable();
            UserGroup ug = new UserGroup();
            List<VMRoleDetails> result = new List<VMRoleDetails>();

            try
            {
                dt = ug.GetParentRoleList();

                //no id encryption needed as it is used for filter only with custom value
                var records = (from data in dt.AsEnumerable()
                               select new VMRoleDetails
                               {
                                   RoleId = data.Field<string>("ParentId"),
                                   RoleName = data.Field<string>("ParentName")
                               });

                result = records.ToList();
                VMRoleDetails vrd = new VMRoleDetails();
                vrd.RoleId = "c0";
                vrd.RoleName = "ALL";
                result.Insert(0, vrd);
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public string getRoleList(string parentId, string systemYN, string mode, string groupId)
        {
            DataTable dt = new DataTable();
            UserGroup ug = new UserGroup();
            string roleMenu = string.Empty;

            try
            {
                parentId = GblFunctions.Base64Decode(parentId);
                groupId = GblFunctions.Base64Decode(groupId);

                //head construction
                roleMenu += "<thead><tr class='chead' style='background-color:#f5f5f5;'>";
                roleMenu += "<th style='width:43%;'>Role Name</th>";
                roleMenu += "<th>View<br><span class='text-center'><input type='checkbox' class='checkbox-custom' id='chkViewAll' title='Select All'></span></th>";
                roleMenu += "<th>Add<br><span class='text-center'><input type='checkbox' class='checkbox-custom' id='chkAddAll' title='Select All'></span></th>";
                roleMenu += "<th>Edit<br><span class='text-center'><input type='checkbox' class='checkbox-custom' id='chkEditAll' title='Select All'></span></th>";
                roleMenu += "<th>Delete<br><span class='text-center'><input type='checkbox' class='checkbox-custom' id='chkDeleteAll' title='Select All'></span></th>";
                roleMenu += "<th>Report<br><span class='text-center'><input type='checkbox' class='checkbox-custom' id='chkReportAll' title='Select All'></span></th>";
                if (systemYN == "Y")
                {
                    roleMenu += "<th class='gj-grid-bootstrap-thead-cell'>System<br><span class='text-center'><input type='checkbox' class='checkbox-custom' id='chkSystemAll' title='Select All'></span></th>";
                }
                roleMenu += "</tr></thead>";

                //body construction
                roleMenu += "<tbody>";

                dt = ug.GetRoleList(Convert.ToInt32(parentId), mode, Convert.ToInt32(groupId));

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        roleMenu += "<tr class='" + dt.Rows[i]["ParentId"].ToString() + "'>";
                        roleMenu += "<td style='width:43%;'><input type='hidden' name='VMRoleDetails[" + i + "].RoleId' value='" + GblFunctions.Base64Encode(dt.Rows[i]["RoleId"].ToString()) + "'>" + dt.Rows[i]["RoleName"].ToString() + "</td>";
                        roleMenu += "<td><input type='checkbox' class='checkbox-custom' name='VMRoleDetails[" + i + "].ViewYN' value=" + ((dt.Rows[i]["ViewYN"].ToString() == "Y") ? "'Y' checked" : "'N'") + "></td>";
                        roleMenu += "<td><input type='checkbox' class='checkbox-custom' name='VMRoleDetails[" + i + "].AddYN' value=" + ((dt.Rows[i]["AddYN"].ToString() == "Y") ? "'Y' checked" : "'N'") + "></td>";
                        roleMenu += "<td><input type='checkbox' class='checkbox-custom' name='VMRoleDetails[" + i + "].EditYN' value=" + ((dt.Rows[i]["EditYN"].ToString() == "Y") ? "'Y' checked" : "'N'") + "></td>";
                        roleMenu += "<td><input type='checkbox' class='checkbox-custom' name='VMRoleDetails[" + i + "].DeleteYN' value=" + ((dt.Rows[i]["DeleteYN"].ToString() == "Y") ? "'Y' checked" : "'N'") + "></td>";
                        roleMenu += "<td><input type='checkbox' class='checkbox-custom' name='VMRoleDetails[" + i + "].ReportYN' value=" + ((dt.Rows[i]["ReportYN"].ToString() == "Y") ? "'Y' checked" : "'N'") + "></td>";
                        if (systemYN == "Y")
                        {
                            roleMenu += "<td><input type='checkbox' class='checkbox-custom' name='VMRoleDetails[" + i + "].SystemYN' value=" + ((dt.Rows[i]["SystemYN"].ToString() == "Y") ? "'Y' checked" : "'N'") + "></td>";
                        }
                        roleMenu += "</tr>";
                    }
                    roleMenu += "</tbody>";
                }
            }
            catch
            {
                roleMenu = "";
            }
            return roleMenu;
        }

        public int saveMstUserGroup(VMAdminUserGroup aug, ref string errDesc)
        {
            int error = 0;

            try
            {
                VMMstUserGroup ug = new VMMstUserGroup();
                List<VMRoleDetails> rd = new List<VMRoleDetails>();
                ug = aug.VMMstUserGroup;
                rd = aug.VMRoleDetails;

                int curGroupId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.GroupId]);
                int groupId = 0, userTypeId = 0, createdBy = 0;
                string groupName = string.Empty, accessType = string.Empty, systemYN = string.Empty;
                string activeYN = string.Empty, ipAddress = string.Empty;
                string xmlRoleDetails = string.Empty;
                string mode = ((ug.EntType == EntType.ADD) ? Mode.ADD : ((ug.EntType == EntType.EDIT) ? Mode.EDIT : ((ug.EntType == EntType.DELETE) ? Mode.DELETE : Mode.ERROR)));
                if (mode == Mode.ERROR)
                {
                    error = 1;
                }
                else
                {
                    if (mode != Mode.ADD)
                    {
                        groupId = Convert.ToInt32(GblFunctions.Base64Decode(ug.GroupId));
                    }
                    groupName = ug.GroupName.Trim();
                    accessType = "W";
                    systemYN = "N";
                    activeYN = (string.IsNullOrEmpty(ug.ActiveYN) == true) ? "N" : ug.ActiveYN;

                    if (mode != Mode.DELETE)
                    {
                        if (activeYN != DefaultSetting.DefaultValY && activeYN != DefaultSetting.DefaultValN)
                        {
                            error = 1;
                            return error;
                        }
                        groupName = Sanitizer.GetSafeHtmlFragment((ug.GroupName ?? "").Trim());
                        if (string.IsNullOrEmpty(groupName))
                        {
                            error = 2;
                            errDesc = Message.UserGroup.UserGroupNameRequired;
                            return error;
                        }
                        if (!GblFunctions.chkDataFormat(RegexType.Alpha, groupName))
                        {
                            error = 2;
                            errDesc = Message.UserGroup.InvalidUserGroupName;
                            return error;
                        }
                    }

                    ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    if (curGroupId == 1 || curGroupId == 2)
                    {
                        userTypeId = Convert.ToInt32(GblFunctions.Base64Decode(ug.UserTypeId));
                    }
                    else
                    {
                        userTypeId = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserTypeId]);
                    }
                    createdBy = Convert.ToInt32(HttpContext.Current.Session[SessionNames.UserId]);

                    //make role details data
                    xmlRoleDetails += "<DocumentElement>";
                    if (mode != Mode.DELETE)
                    {
                        for (int i = 0; i < rd.Count; i++)
                        {
                            xmlRoleDetails += "<Roles>";
                            if (!string.IsNullOrEmpty(rd[i].RoleId))
                            {
                                xmlRoleDetails += "<RoleId>" + GblFunctions.Base64Decode(rd[i].RoleId) + "</RoleId>";
                                xmlRoleDetails += "<ViewYN>" + ((string.IsNullOrEmpty(rd[i].ViewYN) == true) ? "N" : rd[i].ViewYN) + "</ViewYN>";
                                xmlRoleDetails += "<AddYN>" + ((string.IsNullOrEmpty(rd[i].AddYN) == true) ? "N" : rd[i].AddYN) + "</AddYN>";
                                xmlRoleDetails += "<EditYN>" + ((string.IsNullOrEmpty(rd[i].EditYN) == true) ? "N" : rd[i].EditYN) + "</EditYN>";
                                xmlRoleDetails += "<DeleteYN>" + ((string.IsNullOrEmpty(rd[i].DeleteYN) == true) ? "N" : rd[i].DeleteYN) + "</DeleteYN>";
                                xmlRoleDetails += "<ReportYN>" + ((string.IsNullOrEmpty(rd[i].ReportYN) == true) ? "N" : rd[i].ReportYN) + "</ReportYN>";
                                xmlRoleDetails += "<SystemYN>" + ((string.IsNullOrEmpty(rd[i].SystemYN) == true) ? "N" : rd[i].SystemYN) + "</SystemYN>";
                            }
                            xmlRoleDetails += "</Roles>";
                        }
                    }
                    xmlRoleDetails += "</DocumentElement>";

                    UserGroup ugDAL = new UserGroup();

                    error = ugDAL.SaveMstUserGroup(groupId, groupName, accessType, systemYN, activeYN, userTypeId, xmlRoleDetails, ipAddress, mode, createdBy, ref errDesc);

                    if(error == 1)
                    {
                        MCommon.saveExceptionLog(errDesc, "AdminUserGroup/saveMstUserGroup(MAdminUserGroup)", "AdminUserGroupController");
                    }
                }
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "AdminUserGroup/saveMstUserGroup(MAdminUserGroup)", "AdminUserGroupController");

                error = 1;
            }

            return error;
        }
    }
}
