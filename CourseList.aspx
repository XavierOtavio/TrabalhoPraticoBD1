<%@ Page Title="Lista Cursos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CourseList.aspx.cs" Inherits="TrabalhoFinal3.CourseList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Lista de Cursos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- MENU DE 3 COLUNAS -->
    <div class="menu-container" style="display: flex; gap: 30px; padding-bottom: 30px;">
        <!-- Categorias -->
        <div>
            <h3>Categorias</h3>
            <asp:Repeater ID="rptCategorias" runat="server" OnItemCommand="CategoriaSelecionada">
                <ItemTemplate>
                    <div>
                        <asp:LinkButton ID="btnCategoria" runat="server" CommandName="Selecionar" CommandArgument='<%# Eval("Id") %>'>
                            <%# Eval("Nome") %>
                        </asp:LinkButton>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- Áreas -->
        <div>
            <h3>Áreas</h3>
            <asp:Repeater ID="rptAreas" runat="server" OnItemCommand="AreaSelecionada">
                <ItemTemplate>
                    <div>
                        <asp:LinkButton ID="btnArea" runat="server" CommandName="Selecionar" CommandArgument='<%# Eval("Id") %>'>
                            <%# Eval("Nome") %>
                        </asp:LinkButton>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- Tópicos -->
        <div>
            <h3>Tópicos</h3>
            <asp:Repeater ID="rptTopicos" runat="server" OnItemCommand="TopicoSelecionado">
                <ItemTemplate>
                    <div>
                        <asp:LinkButton ID="btnTopico" runat="server" CommandName="Selecionar" CommandArgument='<%# Eval("Id") %>'>
                            <%# Eval("Nome") %>
                        </asp:LinkButton>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <hr />

    <!-- LISTA DE CURSOS FILTRADOS -->
    <h3>Cursos disponíveis:</h3>
    <asp:Repeater ID="rptCursos" runat="server">
        <ItemTemplate>
            <div style="margin-bottom: 20px;">
                <h4><%# Eval("Titulo") %></h4>
                <p><%# Eval("Descricao") %></p>
                <a href='<%# "CourseDetail.aspx?id=" + Eval("Id") %>'>Ver Detalhes</a>
            </div>
        </ItemTemplate>
    </asp:Repeater>

</asp:Content>
