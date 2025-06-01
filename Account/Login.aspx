<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TrabalhoFinal3.Account.Login" %>

<!DOCTYPE html>
<html lang="pt-PT">
<head runat="server">
    <meta charset="utf-8" />
    <title>Iniciar Sessão - SoftSkills</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server" class="container d-flex justify-content-center align-items-center vh-100">
        <div class="card p-4 shadow-sm" style="width: 350px;">
            <h3 class="card-title text-center mb-4">Iniciar Sessão</h3>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" HeaderText="Por favor, corrija os erros abaixo:" />

            <div class="mb-3">
                <asp:Label ID="LabelEmail" runat="server" Text="Email" AssociatedControlID="TextBoxEmail" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="TextBoxEmail" runat="server" CssClass="form-control" placeholder="exemplo@dominio.com"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredEmail" runat="server" ControlToValidate="TextBoxEmail" ErrorMessage="O email é obrigatório." CssClass="text-danger" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="RegexEmail" runat="server" ControlToValidate="TextBoxEmail"
                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Email inválido." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
                <asp:Label ID="LabelPassword" runat="server" Text="Palavra-passe" AssociatedControlID="TextBoxPassword" CssClass="form-label"></asp:Label>
                <asp:TextBox ID="TextBoxPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="••••••••"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredPassword" runat="server" ControlToValidate="TextBoxPassword" ErrorMessage="A palavra-passe é obrigatória." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="form-check mb-3">
                <asp:CheckBox ID="CheckBoxRememberMe" runat="server" CssClass="form-check-input" />
                <asp:Label ID="LabelRememberMe" runat="server" Text="Lembrar-me" AssociatedControlID="CheckBoxRememberMe" CssClass="form-check-label"></asp:Label>
            </div>
            <asp:Button ID="ButtonLogin" runat="server" Text="Entrar" CssClass="btn btn-primary w-100 mb-3" OnClick="ButtonLogin_Click" />
            <asp:Button ID="ButtonRegister" runat="server" UseSubmitBehavior="false" CausesValidation="False" Text="Ainda não tens conta? Regista-te aqui" CssClass="btn btn-warning w-100" OnClick="ButtonRegister_Click" />
            <div class="mt-3 text-center text-danger" runat="server" id="LoginError" visible="false"></div>
        </div>
    </form>
</body>
</html>
