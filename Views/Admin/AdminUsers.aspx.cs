using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TrabalhoFinal3.Models;

namespace TrabalhoFinal3
{
    public partial class AdminUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Perfil"]?.ToString().ToLower() != "administrador")
            {
                Response.Redirect("~/Home.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CarregarUtilizadores();
            }
        }

        private void CarregarUtilizadores()
        {
            List<User> utilizadores = ObterTodosUsers();
            gvUtilizadores.DataSource = utilizadores;
            gvUtilizadores.DataBind();
        }

        protected void gvUtilizadores_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Apagar")
            {
                ApagarUser(id);
                CarregarUtilizadores();
            }
        }

        protected void btnAdicionarUtilizador_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Account/Register.aspx");
        }

        private List<User> ObterTodosUsers()
        {
            var lista = new List<User>();
            string connStr = ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sc24_197.sp_ListUsersCursor", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new User
                    {
                        UserId = Convert.ToInt32(reader["USER_ID"]),
                        FirstName = reader["USER_FIRST_NAME"].ToString(),
                        LastName = reader["USER_LAST_NAME"].ToString(),
                        Email = reader["USER_EMAIL"].ToString(),
                        UserRoleId = Convert.ToInt32(reader["ROLE_ID"]),
                        RoleName = reader["ROLE_NAME"].ToString()
                    });
                }
            }

            return lista;
        }

        private void ApagarUser(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string email;
                using (SqlCommand cmdEmail = new SqlCommand("SELECT USER_EMAIL FROM sc24_197.[USER] WHERE USER_ID = @id", conn))
                {
                    cmdEmail.Parameters.AddWithValue("@id", id);
                    email = cmdEmail.ExecuteScalar()?.ToString();
                }

                if (string.IsNullOrEmpty(email)) return;

                using (SqlCommand cmd = new SqlCommand("sc24_197.sp_DeleteUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
