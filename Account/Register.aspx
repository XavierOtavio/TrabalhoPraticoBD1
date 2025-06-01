<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs"
    Inherits="TrabalhoFinal3.Account.Register" %>

<!DOCTYPE html>
<html lang="pt-PT">
<head runat="server">
    <meta charset="utf-8" />
    <title>Registo de Utilizador</title>

    <!-- Bootstrap 5 CDN -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</head>

<body class="bg-light">
    <form id="form1" runat="server" class="container d-flex justify-content-center align-items-center min-vh-100">

        <!-- Card central -->
        <div class="card shadow-sm" style="max-width: 500px; width: 100%;">
            <div class="card-header text-center">
                <h2 class="h4 mb-0">Registo</h2>
            </div>

            <div class="card-body">

                <!-- PRIMEIRO NOME -->
                <div class="mb-3">
                    <asp:Label ID="lblFirstName" AssociatedControlID="txtFirstName"
                        CssClass="form-label" runat="server" Text="Primeiro Nome" />
                    <asp:TextBox ID="txtFirstName" runat="server"
                        CssClass="form-control" />
                </div>

                <!-- ÚLTIMO NOME -->
                <div class="mb-3">
                    <asp:Label ID="lblLastName" AssociatedControlID="txtLastName"
                        CssClass="form-label" runat="server" Text="Último Nome" />
                    <asp:TextBox ID="txtLastName" runat="server"
                        CssClass="form-control" />
                </div>

                <!-- E-MAIL -->
                <div class="mb-3">
                    <asp:Label ID="lblEmail" AssociatedControlID="txtEmail"
                        CssClass="form-label" runat="server" Text="E-mail" />
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                    <asp:RegularExpressionValidator runat="server"
                        ControlToValidate="txtEmail"
                        ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
                        CssClass="invalid-feedback d-block"
                        ErrorMessage="E-mail inválido." />
                </div>

                <!-- PASSWORD -->
                <div class="mb-3">
                    <asp:Label ID="lblPassword" AssociatedControlID="txtPassword"
                        CssClass="form-label" runat="server" Text="Password" />
                    <asp:TextBox ID="txtPassword" runat="server"
                        TextMode="Password" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="A password é obrigatória."
                        CssClass="invalid-feedback d-block" />
                </div>

                <!-- ROLE -->
                <div class="mb-4">
                    <asp:Label ID="lblRole" AssociatedControlID="ddlRoles"
                        CssClass="form-label" runat="server" Text="Perfil / Role" />
                    <asp:DropDownList ID="ddlRoles" runat="server"
                        CssClass="form-select"
                        DataTextField="RoleName"
                        DataValueField="UserRoleId">
                        <asp:ListItem Value="">— escolher —</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server"
                        ControlToValidate="ddlRoles"
                        InitialValue=""
                        ErrorMessage="Selecione um perfil."
                        CssClass="invalid-feedback d-block" />
                </div>

                <!-- BOTÃO -->
                <div class="d-grid mb-3">
                    <asp:Button ID="btnRegistar" runat="server"
                        Text="Criar Conta"
                        CssClass="btn btn-primary"
                        OnClick="BtnRegistar_Click" />
                </div>
                <div class="d-grid">
                    <asp:Button ID="btnLogin" runat="server" UseSubmitBehavior="false" CausesValidation="False"
                        Text="Voltar ao Login"
                        CssClass="btn btn-warning"
                        OnClick="BackToLogin" />
                </div>

                <!-- MENSAGEM -->
                <asp:Label ID="lblMensagem" runat="server"
                    CssClass="d-block mt-3 fw-bold text-success" />
            </div>
        </div>
    </form>
</body>
</html>
