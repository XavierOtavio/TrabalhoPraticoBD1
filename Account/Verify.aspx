<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Verify.aspx.cs"
    Inherits="TrabalhoFinal3.Account.Verify" %>

<!DOCTYPE html>
<html lang="pt-PT">
<head runat="server">
    <meta charset="utf-8" />
    <title>Iniciar Sessão - SoftSkills</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <div class="row justify-content-center">
        <div class="col-lg-6">
            <div class="card shadow-sm mt-5">
                <div class="card-body text-center">
                    <h1 class="h4 mb-4">Confirmação de e-mail</h1>
                    <asp:Literal ID="litMsg" runat="server" />
                    <asp:HyperLink ID="lnkLogin" runat="server" CssClass="btn btn-primary mt-4 w-100"
                        NavigateUrl="~/Account/Login.aspx"
                        Visible="false">Ir para Login</asp:HyperLink>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
