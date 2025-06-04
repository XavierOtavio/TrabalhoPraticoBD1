using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace TrabalhoFinal3
{
    public partial class Backoffice : System.Web.UI.Page
    {
        private readonly string cs = ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Perfil"]?.ToString().ToLower() != "administrador")
            {
                Response.Redirect("~/Home.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCategories();
                LoadAreas();
                LoadTopics();
                LoadCourses();
                LoadStudents();
            }
        }

        private void LoadCategories()
        {
            const string sql = "SELECT CATEGORY_ID, CATEGORY_NAME FROM sc24_197.COURSE_CATEGORY ORDER BY CATEGORY_NAME";
            using (SqlConnection cn = new SqlConnection(cs))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                gvCategories.DataSource = dt;
                gvCategories.DataBind();

                ddlAreaCategory.DataSource = dt;
                ddlAreaCategory.DataTextField = "CATEGORY_NAME";
                ddlAreaCategory.DataValueField = "CATEGORY_ID";
                ddlAreaCategory.DataBind();
            }
        }

        private void LoadAreas()
        {
            const string sql = "SELECT A.AREA_ID, A.AREA_NAME, C.CATEGORY_NAME, A.CATEGORY_ID " +
                               "FROM sc24_197.COURSE_AREA A JOIN sc24_197.COURSE_CATEGORY C ON A.CATEGORY_ID=C.CATEGORY_ID " +
                               "ORDER BY A.AREA_NAME";
            using (SqlConnection cn = new SqlConnection(cs))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                gvAreas.DataSource = dt;
                gvAreas.DataBind();

                ddlTopicArea.DataSource = dt;
                ddlTopicArea.DataTextField = "AREA_NAME";
                ddlTopicArea.DataValueField = "AREA_ID";
                ddlTopicArea.DataBind();
            }
        }

        private void LoadTopics()
        {
            const string sql = "SELECT T.TOPIC_ID, T.TOPIC_NAME, A.AREA_NAME, T.AREA_ID " +
                               "FROM sc24_197.COURSE_TOPIC T JOIN sc24_197.COURSE_AREA A ON T.AREA_ID = A.AREA_ID " +
                               "ORDER BY T.TOPIC_NAME";
            using (SqlConnection cn = new SqlConnection(cs))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                gvTopics.DataSource = dt;
                gvTopics.DataBind();
            }
        }

        private void LoadCourses()
        {
            const string sql = "SELECT COURSE_ID, COURSE_NAME FROM sc24_197.COURSE ORDER BY COURSE_NAME";
            using (SqlConnection cn = new SqlConnection(cs))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                ddlEnrollCourse.DataSource = dt;
                ddlEnrollCourse.DataTextField = "COURSE_NAME";
                ddlEnrollCourse.DataValueField = "COURSE_ID";
                ddlEnrollCourse.DataBind();
            }
        }

        private void LoadStudents()
        {
            const string sql = "SELECT U.USER_ID, U.USER_FIRST_NAME + ' ' + U.USER_LAST_NAME AS NAME " +
                               "FROM sc24_197.[USER] U JOIN sc24_197.USERROLE R ON U.ROLE_ID = R.ROLE_ID " +
                               "WHERE R.ROLE_NAME = 'Formando' ORDER BY NAME";
            using (SqlConnection cn = new SqlConnection(cs))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                ddlEnrollStudent.DataSource = dt;
                ddlEnrollStudent.DataTextField = "NAME";
                ddlEnrollStudent.DataValueField = "USER_ID";
                ddlEnrollStudent.DataBind();
            }
        }

        private bool AreaExists(int? id, string name)
        {
            using (SqlConnection cn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("SELECT dbo.fn_CheckAreaExixts(@id, @n)", cn))
            {
                cmd.Parameters.AddWithValue("@id", (object?)id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@n", (object?)name ?? DBNull.Value);
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("INSERT INTO sc24_197.COURSE_CATEGORY (CATEGORY_NAME) VALUES (@n)", cn))
            {
                cmd.Parameters.AddWithValue("@n", txtNewCategory.Text.Trim());
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            txtNewCategory.Text = string.Empty;
            LoadCategories();
        }

        protected void gvCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCategories.EditIndex = e.NewEditIndex;
            LoadCategories();
        }

        protected void gvCategories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCategories.EditIndex = -1;
            LoadCategories();
        }

        protected void gvCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = (int)gvCategories.DataKeys[e.RowIndex].Value;
            string name = ((TextBox)gvCategories.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            using (SqlConnection cn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("UPDATE sc24_197.COURSE_CATEGORY SET CATEGORY_NAME=@n WHERE CATEGORY_ID=@id", cn))
            {
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            gvCategories.EditIndex = -1;
            LoadCategories();
        }

        protected void btnAddArea_Click(object sender, EventArgs e)
        {
            lblAreaMessage.Text = string.Empty;
            string name = txtNewArea.Text.Trim();
            if (AreaExists(null, name))
            {
                lblAreaMessage.Text = "Área já existe";
                return;
            }

            using (SqlConnection cn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("INSERT INTO sc24_197.COURSE_AREA (AREA_NAME, CATEGORY_ID) VALUES (@n, @c)", cn))
            {
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@c", ddlAreaCategory.SelectedValue);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            txtNewArea.Text = string.Empty;
            LoadAreas();
            lblAreaMessage.CssClass = "text-success";
            lblAreaMessage.Text = "Área adicionada com sucesso";
        }

        protected void gvAreas_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvAreas.EditIndex = e.NewEditIndex;
            LoadAreas();
        }

        protected void gvAreas_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAreas.EditIndex = -1;
            LoadAreas();
        }

        protected void gvAreas_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = (int)gvAreas.DataKeys[e.RowIndex].Value;
            string name = ((TextBox)gvAreas.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            lblAreaMessage.Text = string.Empty;
            if (AreaExists(id, name))
            {
                lblAreaMessage.Text = "Área já existe";
                return;
            }

            using (SqlConnection cn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("UPDATE sc24_197.COURSE_AREA SET AREA_NAME=@n WHERE AREA_ID=@id", cn))
            {
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            gvAreas.EditIndex = -1;
            LoadAreas();
            lblAreaMessage.CssClass = "text-success";
            lblAreaMessage.Text = "Área atualizada com sucesso";
        }

        protected void btnAddTopic_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("INSERT INTO sc24_197.COURSE_TOPIC (TOPIC_NAME, AREA_ID) VALUES (@n, @a)", cn))
            {
                cmd.Parameters.AddWithValue("@n", txtNewTopic.Text.Trim());
                cmd.Parameters.AddWithValue("@a", ddlTopicArea.SelectedValue);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            txtNewTopic.Text = string.Empty;
            LoadTopics();
        }

        protected void gvTopics_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTopics.EditIndex = e.NewEditIndex;
            LoadTopics();
        }

        protected void gvTopics_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTopics.EditIndex = -1;
            LoadTopics();
        }

        protected void gvTopics_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = (int)gvTopics.DataKeys[e.RowIndex].Value;
            string name = ((TextBox)gvTopics.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            using (SqlConnection cn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("UPDATE sc24_197.COURSE_TOPIC SET TOPIC_NAME=@n WHERE TOPIC_ID=@id", cn))
            {
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            gvTopics.EditIndex = -1;
            LoadTopics();
        }

        protected void btnEnroll_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(cs))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("INSERT INTO sc24_197.ENROLLMENT (COURSE_ID, TRAINEE_USER_ID) VALUES (@c, @u)", cn))
                {
                    cmd.Parameters.AddWithValue("@c", ddlEnrollCourse.SelectedValue);
                    cmd.Parameters.AddWithValue("@u", ddlEnrollStudent.SelectedValue);
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand("sp_AtualizarEstadoCursos", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }

            lblEnrollMessage.Text = "Aluno inscrito com sucesso!";
        }
    }
}
