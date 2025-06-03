<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Backoffice.aspx.cs" Inherits="TrabalhoFinal3.Backoffice" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="mb-4">Backoffice</h2>

    <h3>Categorias</h3>
    <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True" DataKeyNames="CATEGORY_ID" CssClass="table table-bordered mb-2" OnRowEditing="gvCategories_RowEditing" OnRowUpdating="gvCategories_RowUpdating" OnRowCancelingEdit="gvCategories_RowCancelingEdit">
        <Columns>
            <asp:BoundField DataField="CATEGORY_ID" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Nome" />
        </Columns>
    </asp:GridView>
    <div class="input-group mb-4" style="max-width:400px;">
        <asp:TextBox ID="txtNewCategory" runat="server" CssClass="form-control" Placeholder="Nova categoria" />
        <asp:Button ID="btnAddCategory" runat="server" Text="Adicionar" CssClass="btn btn-primary" OnClick="btnAddCategory_Click" />
    </div>

    <h3>Áreas</h3>
    <div class="mb-2" style="max-width:400px;">
        <asp:DropDownList ID="ddlAreaCategory" runat="server" CssClass="form-select mb-1" />
        <asp:TextBox ID="txtNewArea" runat="server" CssClass="form-control mb-1" Placeholder="Nova área" />
        <asp:Button ID="btnAddArea" runat="server" Text="Adicionar" CssClass="btn btn-primary" OnClick="btnAddArea_Click" />
    </div>
    <asp:GridView ID="gvAreas" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True" DataKeyNames="AREA_ID" CssClass="table table-bordered mb-4" OnRowEditing="gvAreas_RowEditing" OnRowUpdating="gvAreas_RowUpdating" OnRowCancelingEdit="gvAreas_RowCancelingEdit">
        <Columns>
            <asp:BoundField DataField="AREA_ID" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="AREA_NAME" HeaderText="Nome" />
            <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Categoria" ReadOnly="True" />
        </Columns>
    </asp:GridView>

    <h3>Tópicos</h3>
    <div class="mb-2" style="max-width:400px;">
        <asp:DropDownList ID="ddlTopicArea" runat="server" CssClass="form-select mb-1" />
        <asp:TextBox ID="txtNewTopic" runat="server" CssClass="form-control mb-1" Placeholder="Novo tópico" />
        <asp:Button ID="btnAddTopic" runat="server" Text="Adicionar" CssClass="btn btn-primary" OnClick="btnAddTopic_Click" />
    </div>
    <asp:GridView ID="gvTopics" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True" DataKeyNames="TOPIC_ID" CssClass="table table-bordered mb-4" OnRowEditing="gvTopics_RowEditing" OnRowUpdating="gvTopics_RowUpdating" OnRowCancelingEdit="gvTopics_RowCancelingEdit">
        <Columns>
            <asp:BoundField DataField="TOPIC_ID" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="TOPIC_NAME" HeaderText="Nome" />
            <asp:BoundField DataField="AREA_NAME" HeaderText="Área" ReadOnly="True" />
        </Columns>
    </asp:GridView>

    <h3>Inscrever Aluno em Curso</h3>
    <div class="row g-2 mb-3" style="max-width:600px;">
        <div class="col-md-5">
            <asp:DropDownList ID="ddlEnrollCourse" runat="server" CssClass="form-select" />
        </div>
        <div class="col-md-5">
            <asp:DropDownList ID="ddlEnrollStudent" runat="server" CssClass="form-select" />
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnEnroll" runat="server" Text="Inscrever" CssClass="btn btn-success w-100" OnClick="btnEnroll_Click" />
        </div>
    </div>
    <asp:Label ID="lblEnrollMessage" runat="server" CssClass="text-success" />
</asp:Content>
