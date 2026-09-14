using System.Web;

namespace vtsadm
{
    public class TelegramItsupportBot : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            ItsAssignTelegramService.ProcessWebhook(context);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
