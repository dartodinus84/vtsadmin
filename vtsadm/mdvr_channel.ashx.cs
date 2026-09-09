using System;
using System.Web;
using System.Web.SessionState;

namespace vtsadm
{
    /// <summary>
    /// Handler AJAX untuk Setting Channel MDVR (load master + existing, save setting).
    /// Dipakai oleh device_qc_new.aspx dan device_qc_returned.aspx.
    /// </summary>
    public class MdvrChannelHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string action = MdvrChannelService.GetAction(context);
                object result = MdvrChannelService.HandleAction(context, action);
                MdvrChannelService.WriteJson(context, result);
            }
            catch (Exception ex)
            {
                MdvrChannelService.WriteJson(context, new { success = false, message = ex.Message });
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
