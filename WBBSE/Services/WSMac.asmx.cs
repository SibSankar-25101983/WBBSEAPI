using System;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using Common;
using DAL;

namespace WBBSE.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    //To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSMac : System.Web.Services.WebService
    {
        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string validateMACRequestSource(string m)
        {
            string message = string.Empty, decMac = string.Empty, ipAddress = string.Empty;
            DateTime timeStamp = DateTime.Now;

            try
            {
                decMac = GblFunctions.decryptToken(m);
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                int status = new MAC().ValidateMACRequestSource(decMac, ref message, ipAddress, ref timeStamp);

                message = status.ToString() + "$" + message + "^" + timeStamp.ToFileTime();
            }
            catch (Exception ex)
            {
                message = "0$" + ex.Message.Replace("'", "");
            }

            return GblFunctions.encryptToken(message);
        }
    }
}
