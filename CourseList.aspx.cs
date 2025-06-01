using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using TrabalhoFinal3.Models;


namespace TrabalhoFinal3
{
    public partial class CourseList : System.Web.UI.Page
    {
        private static List<Category> CategoriasBD;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarCategoriasComAreasETopicos();
                rptCategorias.DataSource = CategoriasBD;
                rptCategorias.DataBind();
            }
        }

        protected void CategoriaSelecionada(object sender, RepeaterCommandEventArgs e)
        {
            int idCategoria = int.Parse(e.CommandArgument.ToString());
            var categoria = CategoriasBD.FirstOrDefault(c => c.Id == idCategoria);

            rptAreas.DataSource = categoria?.Areas;
            rptAreas.DataBind();

            rptTopicos.DataSource = null;
            rptTopicos.DataBind();
            rptCursos.DataSource = null;
            rptCursos.DataBind();
        }

        protected void AreaSelecionada(object sender, RepeaterCommandEventArgs e)
        {
            int idArea = int.Parse(e.CommandArgument.ToString());
            var area = CategoriasBD.SelectMany(c => c.Areas).FirstOrDefault(a => a.Id == idArea);

            rptTopicos.DataSource = area?.Topics;
            rptTopicos.DataBind();

            rptCursos.DataSource = null;
            rptCursos.DataBind();
        }

        protected void TopicoSelecionado(object sender, RepeaterCommandEventArgs e)
        {
            int idTopico = int.Parse(e.CommandArgument.ToString());
            var cursos = ObterCursosPorTopico(idTopico);

            rptCursos.DataSource = cursos;
            rptCursos.DataBind();
        }

        private void CarregarCategoriasComAreasETopicos()
        {
            CategoriasBD = new List<Category>();
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Categorias
                using (SqlCommand cmd = new SqlCommand("SELECT CATEGORY_ID, CATEGORY_NAME FROM COURSE_CATEGORY", conn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        CategoriasBD.Add(new Category
                        {
                            Id = Convert.ToInt32(dr["CATEGORY_ID"]),
                            Nome = dr["CATEGORY_NAME"].ToString(),
                            Areas = new List<Area>()
                        });
                    }
                }

                // Áreas
                using (SqlCommand cmd = new SqlCommand("SELECT AREA_ID, AREA_NAME, CATEGORY_ID FROM COURSE_AREA", conn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var area = new Area
                        {
                            Id = Convert.ToInt32(dr["AREA_ID"]),
                            Nome = dr["AREA_NAME"].ToString(),
                            CategoryId = Convert.ToInt32(dr["CATEGORY_ID"]),
                            Topics = new List<Topic>()
                        };

                        var categoria = CategoriasBD.FirstOrDefault(c => c.Id == area.CategoryId);
                        categoria?.Areas.Add(area);
                    }
                }

                // Tópicos
                using (SqlCommand cmd = new SqlCommand("SELECT TOPIC_ID, TOPIC_NAME, AREA_ID FROM COURSE_TOPIC", conn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var topico = new Topic
                        {
                            Id = Convert.ToInt32(dr["TOPIC_ID"]),
                            Nome = dr["TOPIC_NAME"].ToString(),
                            AreaId = Convert.ToInt32(dr["AREA_ID"])
                        };

                        var area = CategoriasBD.SelectMany(c => c.Areas).FirstOrDefault(a => a.Id == topico.AreaId);
                        area?.Topics.Add(topico);
                    }
                }
            }
        }

        private List<Course> ObterCursosPorTopico(int idTopico)
        {
            List<Course> cursos = new List<Course>();
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"SELECT COURSE_ID, COURSE_NAME
                                 FROM COURSE
                                 WHERE TOPIC_ID = @TOPIC_ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TOPIC_ID", idTopico);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cursos.Add(new Course
                            {
                                Id = Convert.ToInt32(dr["COURSE_ID"]),
                                Titulo = dr["COURSE_NAME"].ToString()
                            });
                        }
                    }
                }
            }

            return cursos;
        }
    }
}