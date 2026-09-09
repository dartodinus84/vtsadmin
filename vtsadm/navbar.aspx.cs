using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace vtsadm
{
    public partial class navbar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {

        }

        protected void CmdHome_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("dashboard.aspx");
            }
            catch(Exception ex)
            {
                Response.Redirect("login.aspx");
            }
        }
    }
}