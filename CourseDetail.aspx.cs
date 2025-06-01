using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TrabalhoFinal3.Models;
using System.Data.SqlClient;


namespace TrabalhoFinal3
{
    public partial class CourseDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (!int.TryParse(Request.QueryString["id"], out int idCurso))
            {
                Response.Redirect("~/CourseList.aspx");
                return;
            }

            var curso = ObterCursoPorId(idCurso);
            if (curso == null)
            {
                Response.Redirect("~/CourseList.aspx");
                return;
            }

            litTitulo.Text = curso.Titulo;
            litDescricao.Text = curso.Descricao;
            litFormador.Text = curso.Formador;
            litDatas.Text = $"{curso.DataInicio:dd/MM/yyyy} a {curso.DataFim:dd/MM/yyyy}";
        }

        private Course ObterCursoPorId(int id)
        {
            Course curso = null;
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT COURSE_NAME, TRAINER_USER_ID, COURSE_START_DATE, COURSE_END_DATE FROM COURSE WHERE COURSE_ID = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            curso = new Course
                            {
                                Titulo = dr["COURSE_NAME"].ToString(),
                                Formador = dr["TRAINER_USER_ID"].ToString(),
                                DataInicio = Convert.ToDateTime(dr["COURSE_START_DATE"]),
                                DataFim = Convert.ToDateTime(dr["COURSE_END_DATE"])
                            };
                        }
                    }
                }
            }

            return curso;
        }
    }
}