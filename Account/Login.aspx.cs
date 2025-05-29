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

                Response.Redirect("~/Default.aspx");
            }
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string email = TextBoxEmail.Text.Trim();
                string password = TextBoxPassword.Text;

                // Aqui deve implementar a validação real, por exemplo consultar a base de dados
                // Exemplo provisório:
                if (email == "admin@exemplo.com" && password == "1234")
                {
                    FormsAuthentication.SetAuthCookie(email, CheckBoxRememberMe.Checked);
                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    LoginError.InnerText = "Login inválido. Verifique o email e a palavra-passe.";
                    LoginError.Visible = true;
                }
            }
        }
    }
}
