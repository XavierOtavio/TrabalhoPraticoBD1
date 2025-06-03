using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using TrabalhoFinal3.Models;

namespace TrabalhoFinal3
{
    public partial class CourseList : System.Web.UI.Page
    {
        private const int PageSize = 20;
        private int _currentPage = 1;
        private readonly string _cs =
            ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString;

        /* ----------------------------- PAGE LOAD ----------------------------- */
        protected void Page_Load(object sender, EventArgs e)
        {
            btnCreateCourse.Visible = Session["Perfil"]?.ToString().ToLower() == "administrador";
            if (!IsPostBack)
            {
                CarregarCategorias();
                CarregarAreas(null);
                CarregarTopicos(null);
                CarregarCursos();
            }
        }
        /* ------------------------- DROPDOWNS CAT/AREA/TOPIC ------------------ */
        protected void FiltroChanged(object sender, EventArgs e)
        {
            _currentPage = 1;
            CarregarCursos();
        }
        private void CarregarCategorias()
        {
            const string sql = "SELECT CATEGORY_ID, CATEGORY_NAME " +
                               "FROM sc24_197.COURSE_CATEGORY ORDER BY CATEGORY_NAME";

            ddlCategory.DataSource = ExecutarLista(sql);
            ddlCategory.DataTextField = "Text";
            ddlCategory.DataValueField = "Value";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("Todas", ""));
        }
        private void CarregarAreas(string categoryId)
        {
            string sql = "SELECT AREA_ID, AREA_NAME FROM sc24_197.COURSE_AREA ";
            if (!string.IsNullOrEmpty(categoryId))
                sql += "WHERE CATEGORY_ID = @c ";
            sql += "ORDER BY AREA_NAME";

            ddlArea.DataSource = ExecutarLista(sql,
                                      new SqlParameter("@c", categoryId ?? (object)DBNull.Value));
            ddlArea.DataTextField = "Text";
            ddlArea.DataValueField = "Value";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("Todas", ""));
        }
        private void CarregarTopicos(string areaId)
        {
            string sql = "SELECT TOPIC_ID, TOPIC_NAME FROM sc24_197.COURSE_TOPIC ";
            if (!string.IsNullOrEmpty(areaId))
                sql += "WHERE AREA_ID = @a ";
            sql += "ORDER BY TOPIC_NAME";

            ddlTopic.DataSource = ExecutarLista(sql,
                                      new SqlParameter("@a", areaId ?? (object)DBNull.Value));
            ddlTopic.DataTextField = "Text";
            ddlTopic.DataValueField = "Value";
            ddlTopic.DataBind();
            ddlTopic.Items.Insert(0, new ListItem("Todos", ""));
        }
        /*------------------ EVENTOS CASCATA --------------------*/

        // Categoria mudou  ➜  carrega Áreas + Tópicos
        protected void DdlCategory_Changed(object sender, EventArgs e)
        {
            string cat = ddlCategory.SelectedValue;
            btnSearch.Focus();
            CarregarAreas(cat);
            CarregarTopicos(null);
            CarregarCursos();
        }

        // Área mudou ➜ garante categoria correcta + carrega tópicos
        protected void DdlArea_Changed(object sender, EventArgs e)
        {
            string area = ddlArea.SelectedValue;

            // 1) obter CATEGORY_ID desta área
            if (!string.IsNullOrEmpty(area))
            {
                using (SqlConnection cn = new SqlConnection(_cs))
                using (SqlCommand cmd = new SqlCommand(
                         "SELECT C.CATEGORY_ID FROM sc24_197.COURSE_AREA A INNER JOIN COURSE_CATEGORY C ON C.CATEGORY_ID = A.CATEGORY_ID WHERE AREA_ID=@a", cn))
                {
                    cmd.Parameters.AddWithValue("@a", area);
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string catId = (dr.GetInt32(0)).ToString();

                            ddlCategory.SelectedValue = catId;
                            CarregarAreas(catId);

                            ddlArea.SelectedValue = area;
                        }
                    }
                }
                CarregarCursos();
            }

            // 2) actualizar tópicos
            CarregarTopicos(area);
        }
        // Tópico mudou ➜ define Área e Categoria correspondentes
        protected void DdlTopic_Changed(object sender, EventArgs e)
        {
            string topic = ddlTopic.SelectedValue;
            if (string.IsNullOrEmpty(topic)) return;

            using (SqlConnection cn = new SqlConnection(_cs))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT T.AREA_ID, A.CATEGORY_ID " +
                "FROM sc24_197.COURSE_TOPIC T " +
                "JOIN sc24_197.COURSE_AREA A ON A.AREA_ID = T.AREA_ID " +
                "WHERE T.TOPIC_ID = @t", cn))
            {
                cmd.Parameters.AddWithValue("@t", topic);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        string areaId = (dr.GetInt32(0)).ToString();
                        string catId = (dr.GetInt32(1)).ToString();

                        ddlCategory.SelectedValue = catId;
                        CarregarAreas(catId);

                        ddlArea.SelectedValue = areaId;
                        CarregarTopicos(areaId);

                        ddlTopic.SelectedValue = topic;
                    }
                }
                CarregarCursos();
            }
        }

        /* ----------------------------- LISTA ------------------------------ */
        private void CarregarCursos()
        {
            // 1) construir filtro
            var where = new List<string> { "1=1" };
            var pars = new List<SqlParameter>();

            void AddFilter(string clause, string name, string value)
            {
                where.Add(clause);
                pars.Add(new SqlParameter(name, value));
            }

            if (!string.IsNullOrEmpty(ddlCategory.SelectedValue))
                AddFilter("A.CATEGORY_ID = @cat", "@cat", ddlCategory.SelectedValue);

            if (!string.IsNullOrEmpty(ddlArea.SelectedValue))
                AddFilter("T.AREA_ID     = @area", "@area", ddlArea.SelectedValue);

            if (!string.IsNullOrEmpty(ddlTopic.SelectedValue))
                AddFilter("C.TOPIC_ID    = @topic", "@topic", ddlTopic.SelectedValue);

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                AddFilter("C.COURSE_NAME LIKE @s", "@s", "%" + txtSearch.Text.Trim() + "%");

            string filtro = string.Join(" AND ", where);

            // 2) query paginada
            string sql =
              "SELECT C.COURSE_ID, C.COURSE_NAME, " +
              "       (SELECT USER_FIRST_NAME + ' ' + USER_LAST_NAME " +
              "          FROM sc24_197.[USER] WHERE USER_ID = C.TRAINER_USER_ID) AS Trainer, " +
              "       C.COURSE_START_DATE, C.COURSE_END_DATE, C.COURSE_SLOTS, " +
              "       (SELECT COUNT(*) FROM sc24_197.ENROLLMENT E WHERE E.COURSE_ID=C.COURSE_ID) AS Inscritos, " +
              "       T.TOPIC_NAME, " +
              "       A.AREA_NAME, " +
              "       CT.CATEGORY_NAME " +
              "FROM   sc24_197.COURSE C " +
              "LEFT  JOIN sc24_197.COURSE_TOPIC T ON C.TOPIC_ID = T.TOPIC_ID " +
              "LEFT  JOIN sc24_197.COURSE_AREA A ON A.AREA_ID  = T.AREA_ID " +
              "LEFT  JOIN sc24_197.COURSE_CATEGORY CT ON CT.CATEGORY_ID  = A.CATEGORY_ID " +
              $"WHERE  {filtro} " +
              "ORDER BY C.COURSE_START_DATE DESC " +
              "OFFSET (@skip) ROWS FETCH NEXT (@take) ROWS ONLY";

            int skip = (_currentPage - 1) * PageSize;
            var lista = new List<dynamic>();

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddRange(pars.ToArray());
                cmd.Parameters.AddWithValue("@skip", skip);
                cmd.Parameters.AddWithValue("@take", PageSize);

                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DateTime ini = dr.GetDateTime(3), fim = dr.GetDateTime(4);
                        string estado = DateTime.Now < ini ? "Criado" :
                                          DateTime.Now <= fim ? "Em curso" : "Terminado";

                        string badgeClass = estado == "Criado" ? "badge bg-secondary rounded-pill" :
                                            estado == "Em curso" ? "badge bg-success rounded-pill" :
                                                                   "badge bg-dark rounded-pill";

                        bool vagasOk = dr.IsDBNull(5) || dr.GetInt32(5) == 0;
                        string cat = dr.GetString(9);
                        string area = dr.GetString(8);
                        string topic = dr.GetString(7);

                        lista.Add(new
                        {
                            CourseId = dr.GetInt32(0),
                            CourseName = dr.GetString(1),
                            Trainer = dr.IsDBNull(2) ? "—" : dr.GetString(2),
                            StartDate = ini,
                            EndDate = fim,

                            Category = cat,
                            Area = area,
                            Topic = topic,

                            BadgeText = estado,
                            BadgeClass = badgeClass,
                            PlacesInfo = vagasOk ? "∞" : $"{dr.GetInt32(6)}/{dr.GetInt32(5)}",
                            LinkDetalhe = $"/CourseDetail.aspx?id={dr.GetInt32(0)}"
                        });
                    }
                }
            }

            rptCursos.DataSource = lista;
            rptCursos.DataBind();

            // 3) paginação
            int total = ContarCursos(filtro, pars);
            GerarPaginacao(total);
        }

        /* ---------------------- TOTAL REGISTOS ---------------------- */
        private int ContarCursos(string filtro, List<SqlParameter> pars)
        {
            string sql = $"SELECT COUNT(*) FROM sc24_197.COURSE C " +
                         "LEFT  JOIN sc24_197.COURSE_TOPIC T ON C.TOPIC_ID = T.TOPIC_ID " +
                         "LEFT  JOIN sc24_197.COURSE_AREA  A ON A.AREA_ID  = T.AREA_ID " +
                         $"WHERE {filtro}";

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, cn))
            {
                // criar cópias dos parâmetros para não reutilizar as mesmas instâncias
                foreach (var p in pars)
                    cmd.Parameters.AddWithValue(p.ParameterName, p.Value);

                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        /* ---------------------- PAGINAÇÃO --------------------------- */
        private void GerarPaginacao(int total)
        {
            int pages = (int)Math.Ceiling(total / (double)PageSize);
            var ds = Enumerable.Range(1, pages).Select(p => new
            {
                PageNumber = p,
                Label = p.ToString(),
                Active = p == _currentPage ? "active" : ""
            });

            rptPages.DataSource = ds;
            rptPages.DataBind();
        }

        /* ---------------------- EVENTOS REPEATER -------------------- */
        protected void rptPages_ItemCommand(object src, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                _currentPage = int.Parse(e.CommandArgument.ToString());
                CarregarCursos();
            }
        }

        protected void btnCreateCourse_Click(object sender, EventArgs e)
        {
            Response.Redirect("CourseCreate.aspx");
        }
        /*==================  UTILITÁRIO ==================*/
        /// <summary>Lê pares Value/Text e devolve IEnumerable&lt;ListItem&gt;.</summary>
        private IEnumerable<ListItem> ExecutarLista(string sql, params SqlParameter[] prms)
        {
            using (SqlConnection cn = new SqlConnection(_cs))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (prms != null && prms.Length > 0)
                    cmd.Parameters.AddRange(prms);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        yield return new ListItem(dr[1].ToString(), dr[0].ToString());
                }
            }
        }
    }
}
