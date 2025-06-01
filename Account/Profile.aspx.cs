using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using TrabalhoFinal3.Data;
using TrabalhoFinal3.Models;
using TrabalhoFinal3.Utils;

namespace TrabalhoFinal3.Account
{
    public partial class Profile : System.Web.UI.Page
    {
        private readonly UserService _svc = new UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                PopularFusoHorarios();
                PopulateCountries();
                LoadData();
            }
            ScriptManager.RegisterStartupScript( this, GetType(), "initTooltip","$(function () { var t=$('[data-bs-toggle=\"tooltip\"]'); if (t.length) new bootstrap.Tooltip(t); });",true);
        }

        private void LoadData()
        {
            var user = _svc.GetUser(Page.User.Identity.Name);
            if (user == null) return;

            if (string.Equals(user.RoleName, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                Session["Nome"] = $"{user.FirstName} {user.LastName}".Trim();
                Session["Perfil"] = "Administrador";
            }

            ImgAvatar.ImageUrl = user.PhotoPath ?? "~/Content/placeholder.png";
            TxtFirstName.Text = $"{user.FirstName}";
            TxtLastName.Text = $"{user.LastName}";
            TxtTitle.Text = user.Title;
            TxtBio.Text = user.Bio ?? "";

            LblEmail.Text = user.Email;
            bool verificada = string.Equals(user.StatusName, "Ativo", StringComparison.OrdinalIgnoreCase)
                  || user.UserStatusId == 1;

            string iconHtml;
            if (verificada)
            {
                iconHtml = "<span class='text-success' " +
                           "data-bs-toggle='tooltip' title='Conta verificada'>&#10003;</span>"; // ✓S
                BtnChangePassword.Visible = true;
            }
            else
            {
                iconHtml = "<span class='text-danger' " +
                           "data-bs-toggle='tooltip' title='Conta não verificada'>&#10007;</span>"; // ✗
                LnkResend.Visible = true;
            }

            LblStatusIcon.Text = iconHtml;

            TxtPhone.Text = user.Phone;
            TxtAddress.Text = user.Address;
            TxtCity.Text = user.City;
            DdlCountry.Text = user.Country;

            DdlLanguage.SelectedValue = user.Language ?? "pt-PT";
            DdlTimeZone.SelectedValue = user.TimeZone ?? "UTC";

            var notify = JsonConvert.DeserializeObject<NotifyOptions>(user.NotifyOptions ?? "{}");
            CkbEmail.Checked = notify.Email;
            CkbPush.Checked = notify.Push;
        }
        private void PopularFusoHorarios()
        {
            if (DdlTimeZone.Items.Count > 0) return;
            foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
            {
                DdlTimeZone.Items.Add(new ListItem($"{tz.DisplayName}", tz.Id));
            }
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var user = _svc.GetUser(Page.User.Identity.Name);
            if (user == null) return;

            user.FirstName = TxtFirstName.Text;
            user.LastName = TxtLastName.Text;
            user.Title = TxtTitle.Text;
            user.Bio = TxtBio.Text;
            user.Phone = TxtPhone.Text;
            user.Address = TxtAddress.Text;
            user.City = TxtCity.Text;
            user.Country = DdlCountry.SelectedValue;
            user.Language = DdlLanguage.SelectedValue;
            user.TimeZone = DdlTimeZone.SelectedValue;
            user.NotifyOptions = JsonConvert.SerializeObject(new {Email = CkbEmail.Checked, SMS = false, Push = CkbPush.Checked});

            _svc.UpdateUser(user);

            ScriptManager.RegisterStartupScript(this, GetType(), "ok",
               "alert('Dados gravados com sucesso!');", true);
        }
        protected void BtnReset_Click(object sender, EventArgs e) => LoadData();
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            _svc.DeleteAccount(Page.User.Identity.Name);
            FormsAuthentication.SignOut();
            Response.Redirect("~/Account/Login.aspx");
        }

        protected void BtnUploadAvatar_Click(object sender, EventArgs e)
        {
            if (!FupAvatar.HasFile) return;
            string filename = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(FupAvatar.FileName)}";
            string path = "~/Uploads/Avatars/" + filename;
            FupAvatar.SaveAs(Server.MapPath(path));

            _svc.UpdateUserPhoto(Page.User.Identity.Name, path);
            ImgAvatar.ImageUrl = path;
        }

        protected void BtnChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/ChangePassword.aspx");
        }
        private void PopulateCountries()
        {
            if (DdlCountry.Items.Count > 0) return;

            string path = Server.MapPath("~/Data/CountryList.json");
            var lista = CountryList.All(path)
                                     .OrderBy(c => c.Name)
                                     .ToList();

            DdlCountry.DataTextField = "name";
            DdlCountry.DataValueField = "code";
            DdlCountry.DataSource = lista;
            DdlCountry.DataBind();

            DdlCountry.Items.Insert(0, new ListItem("Escolher país", ""));

            // Pré-seleccionar valor guardado
            var user = new UserService().GetUser(Page.User.Identity.Name);
            if (user != null && !string.IsNullOrEmpty(user.Country))
                DdlCountry.SelectedValue = user.Country;
        }

        protected void LnkResend_Click(object sender, EventArgs e)
        {
            var user = new UserService().GetUser(Page.User.Identity.Name);
            if (user == null) return;

            // gerar token / link de verificação
            string token = Guid.NewGuid().ToString("N");
            string verifyUrl = $"{Request.Url.GetLeftPart(UriPartial.Authority)}/Account/Verify.aspx?u={user.UserId}&t={token}";

            // gravar token na BD (ex.: tabela USER_VERIFY_TOKEN)
            new UserService().GuardarTokenVerificacao(user.UserId, token);

            // enviar e-mail 

            EmailHelper.Send(
                to: user.Email,
                subject: "Confirme o seu endereço de e-mail",
                body: $"Olá {user.FirstName},<br/>Clique no link para confirmar: <a href='{verifyUrl}'>verificar e-mail</a>");

            ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                "alert('E-mail de verificação enviado. Verifique a sua caixa de correio.');", true);
        }
    }

}