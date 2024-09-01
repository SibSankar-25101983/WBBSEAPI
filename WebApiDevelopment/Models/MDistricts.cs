using System.Data;
using WebApiDevelopment.ViewModel;
using WebApiDevelopment.DAL;


namespace WebApiDevelopment.Models
{
    public class MDistricts
    {
        readonly IHttpContextAccessor _contextAccessor;
        public MDistricts()
        {

        }
        public MDistricts(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        //public List<VMMstDistrict> getMstDistrictList(int? page, int? limit, string sortBy, string direction, string searchString, ref int total)
        public List<VMMstDistrict> getMstDistrictList()
        {
            DataTable dt = new DataTable();
            District oDis = new District();
            List<VMMstDistrict> result = new List<VMMstDistrict>();
            int total = 0;
            try
            {
                dt = oDis.getMstDistrictList("", 1, 1000, ref total);

                var records = (from data in dt.AsEnumerable()
                               select new VMMstDistrict
                               {
                                   SlNo = data.Field<Int64>("SlNo"),
                                   DistrictId = data.Field<string>("DistrictId"),
                                   DistrictName = data.Field<string>("DistrictName"),
                                   ZoneId = data.Field<string>("ZoneId"),
                                   ZoneName = data.Field<string>("ZoneName"),
                                   IndexInitial = data.Field<string>("IndexInitial"),
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
                oDis = null;
                dt = null;
            }
            return result;
        }

        public ResponseDetails saveMstDistrict(VMMstDistrict data)
        {
            int error = 0, createdBy = 0;
            string ipAddress = string.Empty, mode = string.Empty,errDesc=string.Empty;
            ResponseDetails r=new ResponseDetails(); 
            try
            {
                mode = data.EntType=="I"?"ADD":(data.EntType=="E"?"EDIT":"DELETE");

                //ipAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
                ipAddress="::1";
                createdBy =1;

                District oDis = new District();
                error = oDis.saveMstDistrict(data, mode, ipAddress, createdBy, ref errDesc);
            }
            catch (Exception ex)
            {
                error = 1;
                errDesc = ex.Message;
            }
            r.Err = error;
            r.ErrDesc = errDesc;
            return r;
        }

        public ResponseDetails UpdateDistrict(VMMstDistrict data)
        {
            int error = 0, createdBy = 0;
            string ipAddress = string.Empty, mode = string.Empty, errDesc = string.Empty;
            ResponseDetails r = new ResponseDetails();
            try
            {
               // mode = data.EntType == "I" ? "ADD" : (data.EntType == "E" ? "EDIT" : "DELETE");

                //ipAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
                ipAddress = "::1";
                createdBy = 1;

                District oDis = new District();
                error = oDis.saveMstDistrict(data, "EDIT", ipAddress, createdBy, ref errDesc);
            }
            catch (Exception ex)
            {
                error = 1;
                errDesc = ex.Message;
            }
            r.Err = error;
            r.ErrDesc = errDesc;
            return r;
        }

        public ResponseDetails DeleteDistrict(VMMstDistrict data)
        {
            int error = 0, createdBy = 0;
            string ipAddress = string.Empty, mode = string.Empty, errDesc = string.Empty;
            ResponseDetails r = new ResponseDetails();
            try
            {
                //mode = data.EntType == "I" ? "ADD" : (data.EntType == "E" ? "EDIT" : "DELETE");

                //ipAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
                ipAddress = "::1";
                createdBy = 1;

                District oDis = new District();
                error = oDis.saveMstDistrict(data, "DELETE", ipAddress, createdBy, ref errDesc);
            }
            catch (Exception ex)
            {
                error = 1;
                errDesc = ex.Message;
            }
            r.Err = error;
            r.ErrDesc = errDesc;
            return r;
        }
    }
}
