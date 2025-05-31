using System;
using System.Web.Security;

namespace SeuProjeto.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Pode adicionar lógica se necessário, por exemplo,
            // redirecionar se o utilizador já estiver autenticado
            if (User.Identity.IsAuthenticated)
            {

                Response.Redirect("~/Home.aspx");
            }
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string email = TextBoxEmail.Text.Trim();
                string password = TextBoxPassword.Text;

                UserService userService = new UserService();
                bool isValid = userService.ValidateUser(email, password);

                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(email, CheckBoxRememberMe.Checked);
                    Response.Redirect("~/Home.aspx");
                }
                else
                {
                    LoginError.InnerText = "Email ou password inválidos.";
                    LoginError.Visible = true;
                }
            }
        }
    }
}
