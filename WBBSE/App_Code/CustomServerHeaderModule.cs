using System;
using System.Text;
using System.Web;

namespace WBBSE.App_Code
{
    public class CustomServerHeaderModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        public void Dispose()
        { }

        void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            // modify the "Server" Http Header
            HttpContext.Current.Response.Headers.Set("Server", "NA");
        }
    }
}