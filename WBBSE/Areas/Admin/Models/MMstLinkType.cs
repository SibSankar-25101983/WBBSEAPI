using System;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections.Generic;
using Microsoft.Security.Application;
using WBBSE.Models;
using ViewModel;
using Common;
using DAL;

namespace WBBSE.Areas.Admin.Models
{
    public class MMstLinkType
    {
        public List<VMMstLinkType> getMstLinkTypeListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstLinkType> result = new List<VMMstLinkType>();

            try
            {
                dt = new LinkType().GetMstLinkTypeListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstLinkType
                               {
                                   LinkTypeId = data.Field<string>("LinkTypeId"),
                                   LinkType = data.Field<string>("LinkType")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetLinkTypeList/getMstLinkTypeListDropDown(MMstLinkType)", "Common");
            }
            finally
            {
                dt = null;
            }

            return result;
        }
    }
}
