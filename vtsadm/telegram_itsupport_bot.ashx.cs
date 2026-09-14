using System;
using System.Web;

namespace vtsadm
{
    public class TelegramItsupportBot : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                ItsAssignTelegramService.ProcessWebhook(context);
            }
            catch (Exception ex)
            {
                // Telegram retries on non-2xx; return 200 with error text for troubleshooting.
                context.Response.Clear();
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.Write("IT Support Telegram webhook error: " + ex.GetType().Name + ": " + ex.Message);
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
