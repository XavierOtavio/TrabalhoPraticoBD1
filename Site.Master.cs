using System;
using System.Web.Security;
using System.Web.UI;
using TrabalhoFinal3.Models;

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

            if (!IsPostBack)
            {
                if (Session["Perfil"]?.ToString().ToLower() == "administrador" &&
                    Session["Nome"] != null)
                {
                    litNomeAdmin.Text = $"<a class='nav-link text-white'>👑 {Session["Nome"]}</a>";
                    litNomeAdmin.Visible = true;
                }
            }

            AtualizarNotificacoes();
        }

        private void AtualizarNotificacoes()
        {
            var user = new UserService().GetUser(Page.User.Identity.Name);
            if (user == null) return;
            var svc = new NotificationService();
            int count = svc.CountUnread(user.UserId);
            if (count > 0)
                litNotifBadge.Text = $"<span class='position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger'>{count}</span>";
            else
                litNotifBadge.Text = string.Empty;
        }


        protected void ButtonLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Account/Login.aspx");
        }
    }
}