using System;
using System.Web;
using System.Web.Mvc;
using Common;
using WBBSE.Models;
using System.Web.Security;

namespace WBBSE.Controllers
{
    public class CommonController : Controller
    {
        [HttpGet]
        [Authorize, NoCache]
        public JsonResult CheckYN()
        {
            string status = string.Empty;
            int groupId = (Session[SessionNames.GroupId] == null) ? 0 : Convert.ToInt32(Session[SessionNames.GroupId]);
            if (groupId == 1 || groupId == 2)
            {
                status = "Y";
            }
            else if (groupId == 0)
            {
                status = "E";
            }
            else
            {
                status = "N";
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IO()
        {
            string Status = "Y";
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return Json(new { Status }, JsonRequestBehavior.AllowGet);
        }

        //DropDown List for Zone Name
        [HttpGet]
        public JsonResult GetZoneList()
        {
            var records = new MMstZone().getMstZoneListDropDown();
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        //DropDown List for District Name by ZoneId
        [HttpGet]
        public JsonResult GetDistrictList(string zi)
        {
            int zoneId = 0;
            try
            {
                zoneId = Convert.ToInt32(GblFunctions.Base64Decode(zi));
            }
            catch
            {
            }
            var records = new MMstDistrict().getMstDistrictListDropDown(zoneId);
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        //DropDown List for Sub-Division Name by DistrictId
        [HttpGet]
        public JsonResult GetSubDivisionList(string di)
        {
            int districtId = 0;
            try
            {
                districtId = Convert.ToInt32(GblFunctions.Base64Decode(di));
            }
            catch
            {
            }
            var records = new MMstSubDivision().getMstSubDivisionListDropDown(districtId);
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        //DropDown List for Notification Type
        [HttpGet]
        public JsonResult GetNotificationTypeList()
        {
            var records = new MCommon().getNotificationTypeListDropDown();

            return Json(records, JsonRequestBehavior.AllowGet);
        }
    }
}
