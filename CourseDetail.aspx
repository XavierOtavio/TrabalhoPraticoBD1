<%@ Page Title="Detalhes do Curso" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CourseDetail.aspx.cs" Inherits="TrabalhoFinal3.CourseDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Detalhes do Curso
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .menu-container {
            display: flex;
            gap: 10px;
        }

        .menu-column {
            width: 200px;
            background-color: #f9f9f9;
            border: 1px solid #ccc;
            padding: 10px;
            position: relative;
        }

        .submenu {
            display: none;
            position: absolute;
            top: 0;
            left: 200px;
            background-color: #fff;
            border: 1px solid #ccc;
            z-index: 1000;
        }

        .menu-column:hover > .submenu {
            display: block;
        }

        .menu-item {
            padding: 5px;
            cursor: pointer;
        }

        .menu-item:hover {
            background-color: #eaeaea;
        }
    </style>

    <div class="menu-container">
        <div class="menu-column">
            <h4>Categorias</h4>
            <asp:Repeater ID="rptCategorias" runat="server">
                <ItemTemplate>
                    <div class="menu-item" 
                         onmouseover="showSubMenu('<%# Eval("Id") %>')">
                        <%# Eval("Nome") %>
                        <div class="submenu" id="submenu_<%# Eval("Id") %>">
                            <asp:Repeater ID="rptAreas" runat="server" DataSource='<%# Eval("Areas") %>'>
                                <ItemTemplate>
                                    <div class="menu-item" onmouseover="showTopicMenu('<%# Eval("Id") %>')">
                                        <%# Eval("Nome") %>
                                        <div class="submenu" id="topicmenu_<%# Eval("Id") %>">
                                            <asp:Repeater ID="rptTopicos" runat="server" DataSource='<%# Eval("Topics") %>'>
                                                <ItemTemplate>
                                                    <div class="menu-item">
                                                        <asp:LinkButton ID="btnTopico" runat="server" CommandName="Selecionar" CommandArgument='<%# Eval("Id") %>'>
                                                            <%# Eval("Nome") %>
                                                        </asp:LinkButton>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <hr />

    <h2><asp:Literal ID="litTitulo" runat="server" /></h2>
    <p><strong>Descrição:</strong> <asp:Literal ID="litDescricao" runat="server" /></p>
    <p><strong>Formador:</strong> <asp:Literal ID="litFormador" runat="server" /></p>
    <p><strong>Datas:</strong> <asp:Literal ID="litDatas" runat="server" /></p>

    <script>
        function showSubMenu(id) {
            document.querySelectorAll('.submenu').forEach(el => el.style.display = 'none');
            var sub = document.getElementById('submenu_' + id);
            if (sub) sub.style.display = 'block';
        }

        function showTopicMenu(id) {
            var topic = document.getElementById('topicmenu_' + id);
            if (topic) topic.style.display = 'block';
        }
    </script>
</asp:Content>
