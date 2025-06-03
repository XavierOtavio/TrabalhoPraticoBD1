<%@ Page Title="Criar Curso" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CourseCreate.aspx.cs" Inherits="TrabalhoFinal3.CourseCreate" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="mb-4">Criar Curso</h2>
    <div class="row g-3">
        <div class="col-md-6">
            <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" CssClass="form-label" Text="Nome" />
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
        </div>
        <div class="col-md-6">
            <asp:Label ID="lblTrainer" runat="server" AssociatedControlID="ddlTrainer" CssClass="form-label" Text="Formador" />
            <asp:DropDownList ID="ddlTrainer" runat="server" CssClass="form-select" />
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblCategory" runat="server" AssociatedControlID="ddlCategory" CssClass="form-label" Text="Categoria" />
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="DdlCategory_Changed" />
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblArea" runat="server" AssociatedControlID="ddlArea" CssClass="form-label" Text="Área" />
            <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="DdlArea_Changed" Enabled="false" />
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblTopic" runat="server" AssociatedControlID="ddlTopic" CssClass="form-label" Text="Tópico" />
            <asp:DropDownList ID="ddlTopic" runat="server" CssClass="form-select" Enabled="false" />
        </div>
        <div class="col-md-3">
            <asp:Label ID="lblStart" runat="server" AssociatedControlID="txtStart" CssClass="form-label" Text="Início" />
            <asp:TextBox ID="txtStart" runat="server" CssClass="form-control" TextMode="Date" />
        </div>
        <div class="col-md-3">
            <asp:Label ID="lblEnd" runat="server" AssociatedControlID="txtEnd" CssClass="form-label" Text="Fim" />
            <asp:TextBox ID="txtEnd" runat="server" CssClass="form-control" TextMode="Date" />
        </div>
        <div class="col-md-3">
            <asp:Label ID="lblSlots" runat="server" AssociatedControlID="txtSlots" CssClass="form-label" Text="Vagas" />
            <asp:TextBox ID="txtSlots" runat="server" CssClass="form-control" TextMode="Number" />
        </div>
        <div class="col-12 mt-3">
            <asp:Button ID="btnCreate" runat="server" CssClass="btn btn-primary" Text="Criar" OnClick="BtnCreate_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="ms-3 text-success" />
        </div>
    </div>
</asp:Content>
