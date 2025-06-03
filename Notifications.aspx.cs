using System;
using TrabalhoFinal3.Models;
using System.Web.UI.WebControls;

namespace TrabalhoFinal3
{
    public partial class Notifications : System.Web.UI.Page
    {
        private readonly NotificationService _svc = new NotificationService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            var user = new UserService().GetUser(Page.User.Identity.Name);
            if (user == null) return;
            rptNotifications.DataSource = _svc.GetNotifications(user.UserId);
            rptNotifications.DataBind();
        }

        protected void rptNotifications_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "read")
            {
                if (int.TryParse(e.CommandArgument.ToString(), out int id))
                {
                    _svc.MarkAsRead(id);
                    LoadData();
                }
            }
        }
    }
}
