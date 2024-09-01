using System;
using System.Web;
using Common;
using System.Data;

namespace WBBSE.Models
{
    public class MUserPermission
    {
        public void chkPagePermission(int roleId, ref string ViewYN, ref string AddYN, ref string EditYN, ref string DeleteYN, ref string ReportYN, ref string SystemYN)
        {
            DataTable dt = new DataTable();

            try
            {
                // || (HttpContext.Current.Session[SessionNames.EmailVerifiedYN].ToString() == "N" && HttpContext.Current.Session[SessionNames.ContactNoVerifiedYN].ToString() == "N")
                if (HttpContext.Current.Session[SessionNames.NewPasswordChangedYN].ToString() == "N")
                {
                    ViewYN = "N";
                    AddYN = "N";
                    EditYN = "N";
                    DeleteYN = "N";
                    ReportYN = "N";
                    SystemYN = "N";
                }
                else
                {
                    dt = (DataTable)HttpContext.Current.Session[SessionNames.RoleDetails];

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataTable dtRole = dt.Select("RoleId=" + roleId).CopyToDataTable();

                        if (dtRole != null && dtRole.Rows.Count == 1)
                        {
                            ViewYN = dtRole.Rows[0]["ViewYN"].ToString();
                            AddYN = dtRole.Rows[0]["AddYN"].ToString();
                            EditYN = dtRole.Rows[0]["EditYN"].ToString();
                            DeleteYN = dtRole.Rows[0]["DeleteYN"].ToString();
                            ReportYN = dtRole.Rows[0]["ReportYN"].ToString();
                            SystemYN = dtRole.Rows[0]["SystemYN"].ToString();
                        }
                        else
                        {
                            ViewYN = "N";
                            AddYN = "N";
                            EditYN = "N";
                            DeleteYN = "N";
                            ReportYN = "N";
                            SystemYN = "N";
                        }
                    }
                    else
                    {
                        ViewYN = "N";
                        AddYN = "N";
                        EditYN = "N";
                        DeleteYN = "N";
                        ReportYN = "N";
                        SystemYN = "N";
                    }
                }
            }
            catch
            {
                ViewYN = "N";
                AddYN = "N";
                EditYN = "N";
                DeleteYN = "N";
                ReportYN = "N";
                SystemYN = "N";
            }
        }
    }
}
