using System;
using TrabalhoFinal3.Models;

namespace TrabalhoFinal3.Account
{
    public partial class Verify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            bool ok = false;
            if (int.TryParse(Request.QueryString["u"], out int uid))
            {
                string tok = Request.QueryString["t"];
                ok = new UserService().ValidarToken(uid, tok);
            }

            if (ok)
            {
                litMsg.Text = "<span class='text-success fs-5'>Conta verificada com sucesso.</span>";
                lnkLogin.Visible = true;
            }
            else
            {
                litMsg.Text = "<span class='text-danger fs-5'>Link inválido ou expirado.</span>";
            }
        }
    }
}
