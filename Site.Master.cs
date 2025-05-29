using System;
using System.Web.Security;
using System.Web.UI;

namespace TrabalhoFinal3
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        protected void ButtonLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Account/Login.aspx");
        }
    }
}