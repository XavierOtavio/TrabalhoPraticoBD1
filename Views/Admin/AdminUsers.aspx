<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminUsers.aspx.cs" Inherits="TrabalhoFinal3.AdminUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Gestão de Utilizadores</h2>

    <asp:Button ID="btnAdicionarUtilizador" runat="server" Text="Adicionar Utilizador" OnClick="btnAdicionarUtilizador_Click" CssClass="btn btn-primary mb-3" />

    <asp:GridView ID="gvUtilizadores" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvUtilizadores_RowCommand">
        <Columns>
            <asp:BoundField DataField="UserId" HeaderText="ID" />
            <asp:BoundField DataField="FirstName" HeaderText="Nome" />
            <asp:BoundField DataField="LastName" HeaderText="Apelido" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="RoleName" HeaderText="Perfil" />
            <asp:TemplateField HeaderText="Ações">
                <ItemTemplate>
                    <asp:Button ID="btnVerPerfil" runat="server" Text="Ver Perfil" CommandName="VerPerfil" CommandArgument='<%# Eval("UserId") %>' CssClass="btn btn-outline-primary btn-sm" />
                    <asp:Button ID="btnApagar" runat="server" Text="Apagar" CommandName="Apagar" CommandArgument='<%# Eval("UserId") %>' CssClass="btn btn-outline-danger btn-sm" OnClientClick="return confirm('Tem a certeza que deseja apagar este utilizador?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
