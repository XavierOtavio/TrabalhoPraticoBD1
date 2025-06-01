using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using TrabalhoFinal3.Models;
using TrabalhoFinal3.Utils;

namespace TrabalhoFinal3.Account
{
    public partial class Register : System.Web.UI.Page
    {
        private readonly UserService userService = new UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlRoles.DataSource = userService.GetAllRoles();
                ddlRoles.DataBind();
            }
        }

        protected void BtnRegistar_Click(object sender, EventArgs e)
        {
            var novoUser = new User
            {
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                UserRoleId = int.Parse(ddlRoles.SelectedValue),
                UserStatusId = 2,
            };

            try
            {
                ValidateUser(novoUser); 
                InserirUtilizador(novoUser);
                lblMensagem.Text = "Conta criada com sucesso!";
            }
            catch (Exception ex)
            {
                lblMensagem.ForeColor = System.Drawing.Color.Red;
                lblMensagem.Text = "Erro: " + ex.Message;
            }
        }
        private bool ValidateUser(User user)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString))
            {
                var sql = "SELECT COUNT(*) FROM sc24_197.[USER] " +
                            "WHERE USER_EMAIL=@Email";
                using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private void InserirUtilizador(User user)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString))
            {
                var sql = @"INSERT INTO sc24_197.[USER]
                            (USER_EMAIL, USER_PASSWORD, ROLE_ID, STATUS_ID, USER_FIRST_NAME, USER_LAST_NAME, USER_CREATED_AT)
                            VALUES (@Email, @Password, @RoleId, @StatusId, @FirstName, @LastName, @CreatedDate)";

                using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@RoleId", user.UserRoleId);
                    cmd.Parameters.AddWithValue("@StatusId", user.UserStatusId);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            using (var conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString)) { 
                var sql2 = "SELECT USER_ID FROM sc24_197.[USER] " +
                            "WHERE USER_EMAIL=@email";
                using (var cmd2 = new System.Data.SqlClient.SqlCommand(sql2, conn))
                {
                    cmd2.Parameters.AddWithValue("@Email", user.Email);
                    conn.Open();
                    using (SqlDataReader dr = cmd2.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            user.UserId = dr.GetInt32(0);
                        }
                    }
                }
            }
            string token = Guid.NewGuid().ToString("N");
            string verifyUrl = $"{Request.Url.GetLeftPart(UriPartial.Authority)}/Account/Verify.aspx?u={user.UserId}&t={token}";


            new UserService().GuardarTokenVerificacao(user.UserId, token);

            EmailHelper.Send(
                to: user.Email,
                subject: "Confirme o seu endereço de e-mail",
                body: $"Olá {user.FirstName},<br/>Clique no link para confirmar: <a href='{verifyUrl}'>verificar e-mail</a>");

            ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                "alert('E-mail de verificação enviado. Verifique a sua caixa de correio.');", true);
        }
        protected void BackToLogin(object sender, EventArgs e) => Response.Redirect("/Account/Login");
    }
}