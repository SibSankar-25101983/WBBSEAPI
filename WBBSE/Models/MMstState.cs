using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel;
using Common;
using DAL;

namespace WBBSE.Models
{
    public class MMstState
    {
        public List<VMMstState> getMstStateListDropDown()
        {
            DataTable dt = new DataTable();
            List<VMMstState> result = new List<VMMstState>();

            try
            {
                dt = new State().getMstStateListDropDown();

                var records = (from data in dt.AsEnumerable()
                               select new VMMstState
                               {
                                   StateId = data.Field<string>("StateId"),
                                   StateName = data.Field<string>("StateName")
                               }).AsQueryable();

                result = records.ToList();
            }
            catch (Exception ex)
            {
                MCommon.saveExceptionLog(ex.Message, "GetStateList/getMstStateListDropDown(Model)", "AdminStateController");
            }
            finally
            {
                dt = null;
            }

            return result;
        }
    }
}