<%@ Page Title="Perfil" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="Profile.aspx.cs"
    Inherits="TrabalhoFinal3.Account.Profile" %>

<asp:Content ID="ContentProfile" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <h1 class="h3 mb-4">Perfil do Utilizador</h1>

            <!-- IDENTIFICAÇÃO -->
            <div class="card mb-4 shadow-sm">
                <div class="card-header fw-bold">Identificação</div>
                <div class="card-body row g-3">

                    <div class="col-md-3 text-center">
                        <asp:Image ID="ImgAvatar" runat="server" CssClass="rounded-circle img-thumbnail"
                            Width="140" Height="140" />
                        <asp:FileUpload ID="FupAvatar" runat="server" CssClass="form-control mt-2" />
                        <asp:Button ID="BtnUploadAvatar" runat="server"
                            Text="Actualizar foto" CssClass="btn btn-outline-primary btn-sm mt-2"
                            OnClick="BtnUploadAvatar_Click" />
                    </div>

                    <div class="col-md-9">
                        <div class="form-group row">
                            <div class="mb-3 col-6">
                                <label class="form-label">Primeiro Nome</label>
                                <asp:TextBox ID="TxtFirstName" runat="server" CssClass="form-control" />
                            </div>
                            <div class="mb-3 col-6">
                                <label class="form-label">Sobrenome</label>
                                <asp:TextBox ID="TxtLastName" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Cargo / Função</label>
                            <asp:TextBox ID="TxtTitle" runat="server" CssClass="form-control" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Biografia (máx. 250 car.)</label>
                            <asp:TextBox ID="TxtBio" runat="server" CssClass="form-control" TextMode="MultiLine"
                                MaxLength="250" Rows="4" />
                        </div>
                    </div>

                </div>
            </div>

            <!-- CREDENCIAIS DE CONTA -->
            <div class="card mb-4 shadow-sm">
                <div class="card-header fw-bold">Credenciais de conta</div>
                <div class="card-body row g-3 align-items-center">
                    <div class="col-auto">
                        <label class="col-form-label">
                            E-mail
                            <asp:Label ID="LblStatusIcon" runat="server" CssClass="fs-5" />
                        </label>
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="LblEmail" runat="server" CssClass="form-control-plaintext" />
                    </div>
                    <div class="btn-group col-sm-3 float-right" role="group">
                        <asp:Button ID="LnkResend" runat="server"
                            CssClass="btn btn-outline-secondary"
                            Text="Reenviar o e-mail de verificação"
                            OnClick="LnkResend_Click"
                            Visible="false" />
                        <asp:Button ID="BtnChangePassword" runat="server" Text="Alterar palavra-passe"
                            CssClass="btn btn-outline-secondary" OnClick="BtnChangePassword_Click" Visible="false" />
                    </div>
                </div>
            </div>

            <!-- DADOS DE CONTACTO -->
            <div class="card mb-4 shadow-sm">
                <div class="card-header fw-bold">Dados de contacto</div>
                <div class="card-body row g-3">
                    <div class="col-md-4">
                        <label class="form-label">Telemóvel</label>
                        <asp:TextBox ID="TxtPhone" runat="server" CssClass="form-control" />
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="TxtPhone"
                            ValidationExpression="^[0-9+\-\s]{9,15}$" ErrorMessage="Número inválido" CssClass="text-danger" />
                    </div>
                    <div class="col-md-4">
                        <label class="form-label">Localidade</label>
                        <asp:TextBox ID="TxtCity" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-4">
                        <label class="form-label">País</label>
                        <asp:DropDownList ID="DdlCountry" runat="server" data-live-search="true" CssClass="form-select" />
                    </div>
                    <div class="col-md-12">
                        <label class="form-label">Morada</label>
                        <asp:TextBox ID="TxtAddress" runat="server" CssClass="form-control" />
                    </div>
                </div>
            </div>

            <!-- PREFERÊNCIAS -->
            <div class="card mb-4 shadow-sm">
                <div class="card-header fw-bold">Preferências</div>
                <div class="card-body row g-3">
                    <div class="col-md-4">
                        <label class="form-label">Idioma</label>
                        <asp:DropDownList ID="DdlLanguage" runat="server" CssClass="form-select">
                            <asp:ListItem Value="pt-PT">Português</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <label class="form-label">Fuso horário</label>
                        <asp:DropDownList ID="DdlTimeZone" runat="server" CssClass="form-select" />
                    </div>
                    <div class="col-md-4">
                        <label class="form-label">Notificações</label>
                        <div class="form-check">
                            <input type="checkbox" id="CkbEmail" runat="server" class="form-check-input" />
                            <asp:Label AssociatedControlID="CkbEmail" CssClass="form-check-label" runat="server">Email</asp:Label>
                        </div>
                        <div class="form-check">
                            <input type="checkbox" id="CkbPush" runat="server" class="form-check-input" />
                            <asp:Label AssociatedControlID="CkbPush" CssClass="form-check-label" runat="server">Push</asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <!-- ACÇÕES -->
            <asp:ValidationSummary runat="server" CssClass="text-danger" />

            <div class="d-flex gap-2">
                <asp:Button ID="BtnSave" runat="server" Text="Guardar" CssClass="btn btn-primary"
                    OnClick="BtnSave_Click" />
                <asp:Button ID="BtnReset" runat="server" Text="Repor" CssClass="btn btn-secondary"
                    CausesValidation="False" OnClick="BtnReset_Click" />
                <asp:Button ID="BtnCancel" runat="server" Text="Cancelar" CssClass="btn btn-outline-secondary"
                    CausesValidation="False" PostBackUrl="~/Home.aspx" />
                <asp:Button ID="BtnDelete" runat="server" Text="Apagar conta"
                    CssClass="btn btn-outline-danger ms-auto"
                    OnClientClick="return confirm('Tem a certeza que pretende eliminar a conta?');"
                    OnClick="BtnDelete_Click" />
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
