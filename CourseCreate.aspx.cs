using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace TrabalhoFinal3
{
    public partial class CourseCreate : System.Web.UI.Page
    {
        private readonly string _cs = ConfigurationManager.ConnectionStrings["SoftSkillsConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            CarregarCategorias();
            CarregarAreas(null);
            CarregarTopicos(null);
            CarregarTrainers();
        }

        private void CarregarCategorias()
        {
            const string sql = "SELECT CATEGORY_ID, CATEGORY_NAME FROM sc24_197.COURSE_CATEGORY ORDER BY CATEGORY_NAME";
            ddlCategory.DataSource = ExecutarLista(sql);
            ddlCategory.DataTextField = "Text";
            ddlCategory.DataValueField = "Value";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("—", ""));
        }

        private void CarregarAreas(string categoryId)
        {
            ddlArea.Items.Clear();

            if (string.IsNullOrEmpty(categoryId))
            {
                ddlArea.Items.Insert(0, new ListItem("—", ""));
                ddlArea.Enabled = false;
                return;
            }

            const string sql = "SELECT AREA_ID, AREA_NAME FROM sc24_197.COURSE_AREA WHERE CATEGORY_ID = @c ORDER BY AREA_NAME";
            ddlArea.DataSource = ExecutarLista(sql, new SqlParameter("@c", categoryId));
            ddlArea.DataTextField = "Text";
            ddlArea.DataValueField = "Value";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("—", ""));
            ddlArea.Enabled = true;
        }

        private void CarregarTopicos(string areaId)
        {
            ddlTopic.Items.Clear();

            if (string.IsNullOrEmpty(areaId))
            {
                ddlTopic.Items.Insert(0, new ListItem("—", ""));
                ddlTopic.Enabled = false;
                return;
            }

            const string sql = "SELECT TOPIC_ID, TOPIC_NAME FROM sc24_197.COURSE_TOPIC WHERE AREA_ID = @a ORDER BY TOPIC_NAME";
            ddlTopic.DataSource = ExecutarLista(sql, new SqlParameter("@a", areaId));

            ddlTopic.DataTextField = "Text";
            ddlTopic.DataValueField = "Value";
            ddlTopic.DataBind();
            ddlTopic.Items.Insert(0, new ListItem("—", ""));
            ddlTopic.Enabled = true;
        }

        private void CarregarTrainers()
        {
            const string sql = "SELECT USER_ID, USER_FIRST_NAME + ' ' + USER_LAST_NAME " +
                               "FROM sc24_197.[USER] U JOIN sc24_197.USERROLE R ON U.ROLE_ID = R.ROLE_ID " +
                               "WHERE R.ROLE_NAME = 'Gestor' ORDER BY USER_FIRST_NAME";
            ddlTrainer.DataSource = ExecutarLista(sql);
            ddlTrainer.DataTextField = "Text";
            ddlTrainer.DataValueField = "Value";
            ddlTrainer.DataBind();
            ddlTrainer.Items.Insert(0, new ListItem("—", ""));
        }

        protected void DdlCategory_Changed(object sender, EventArgs e)
        {
            CarregarAreas(ddlCategory.SelectedValue);
            ddlArea.SelectedIndex = 0;
            CarregarTopicos(null);
        }

        protected void DdlArea_Changed(object sender, EventArgs e)
        {
            CarregarTopicos(ddlArea.SelectedValue);
            ddlTopic.SelectedIndex = 0;
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            const string sql = @"INSERT INTO sc24_197.COURSE (TOPIC_ID, COURSE_NAME, TRAINER_USER_ID, COURSE_START_DATE, COURSE_END_DATE, COURSE_SLOTS) 
                                 VALUES (@topic, @name, @trainer, @start, @end, @slots)";
            using (SqlConnection cn = new SqlConnection(_cs))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@topic", string.IsNullOrEmpty(ddlTopic.SelectedValue) ? (object)DBNull.Value : ddlTopic.SelectedValue);
                cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@trainer", string.IsNullOrEmpty(ddlTrainer.SelectedValue) ? (object)DBNull.Value : ddlTrainer.SelectedValue);
                cmd.Parameters.AddWithValue("@start", DateTime.Parse(txtStart.Text));
                cmd.Parameters.AddWithValue("@end", DateTime.Parse(txtEnd.Text));
                if (string.IsNullOrEmpty(txtSlots.Text))
                    cmd.Parameters.AddWithValue("@slots", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@slots", int.Parse(txtSlots.Text));

                cn.Open();
                cmd.ExecuteNonQuery();
            }
            lblMessage.Text = "Curso criado!";
        }

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
