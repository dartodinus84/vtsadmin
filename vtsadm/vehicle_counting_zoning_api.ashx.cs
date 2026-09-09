using System;
using System.Web;
using System.Web.SessionState;

namespace vtsadm
{
    public class VehicleCountingZoningApi : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string action = VehicleCountingZoningService.GetAction(context);
                object result = VehicleCountingZoningService.HandleAction(context, action);
                VehicleCountingZoningService.WriteJson(context, result);
            }
            catch (Exception ex)
            {
                VehicleCountingZoningService.WriteJson(context, new { success = false, message = ex.Message });
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
