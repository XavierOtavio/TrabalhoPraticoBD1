<%@ Page Title="Lista de Cursos" Language="C#"
    MasterPageFile="~/Site.master"
    AutoEventWireup="true"
    CodeBehind="CourseList.aspx.cs"
    Inherits="TrabalhoFinal3.CourseList" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="h3 mb-4">Oferta formativa</h1>

    <!-- BARRA DE FILTRO -->
    <div class="row g-3 mb-4">
        <div class="col-md-3">
            <label class="form-label">Categoria</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select"
                AutoPostBack="true"
                OnSelectedIndexChanged="DdlCategory_Changed" />
        </div>
        <div class="col-md-3">
            <label class="form-label">Área</label>
            <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-select"
                AutoPostBack="true"
                OnSelectedIndexChanged="DdlArea_Changed" />
        </div>
        <div class="col-md-3">
            <label class="form-label">Tópico</label>
            <asp:DropDownList ID="ddlTopic" runat="server" CssClass="form-select"
                AutoPostBack="true"
                OnSelectedIndexChanged="DdlTopic_Changed" />
        </div>
        <div class="col-md-3">
            <label class="form-label">Pesquisar</label>
            <div class="input-group">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" />
                <button class="btn btn-outline-secondary" type="button"
                    onclick="__doPostBack('<%= btnSearch.UniqueID %>','')">
                    🔍</button>
                <asp:LinkButton ID="btnSearch" runat="server"
                    CssClass="d-none" OnClick="FiltroChanged" />
            </div>
        </div>
    </div>

    <!-- LISTA DE CURSOS -->
    <asp:UpdatePanel ID="upList" runat="server">
        <ContentTemplate>

            <asp:Repeater ID="rptCursos" runat="server">
                <HeaderTemplate>
                    <div class="row row-cols-1 row-cols-md-2 g-4">
                </HeaderTemplate>

                <ItemTemplate>
                    <div class="col">
                        <div class="card h-100 shadow-sm">
                            <div class="card-body d-flex flex-column">

                                <h5 class="card-title"><%# Eval("CourseName") %></h5>
                                <p class="card-text small text-muted mb-1">
                                    <%# Eval("Category") %> › <%# Eval("Area") %> › <%# Eval("Topic") %>
                                </p>
                                <ul class="list-group list-group-flush small">
                                    <li class="list-group-item">
                                        <strong>Formador:</strong> <%# Eval("Trainer") %>
                                    </li>
                                    <li class="list-group-item">
                                        <strong>Datas:</strong>
                                        <%# Eval("StartDate","{0:dd/MM/yyyy}") %>
                                        —
                                        <%# Eval("EndDate","{0:dd/MM/yyyy}") %>
                                    </li>
                                    <li class="list-group-item">
                                        <strong>Vagas:</strong> <%# Eval("PlacesInfo") %>
                                    </li>
                                </ul>

                                <div class="mt-auto pt-3 d-flex justify-content-between align-items-center">
                                    <!-- Badge de estado -->
                                    <asp:Label runat="server" CssClass='<%# Eval("BadgeClass") %>'
                                        Text='<%# Eval("BadgeText") %>' />

                                    <!-- Link para detalhe -->
                                    <a class="btn btn-outline-primary btn-sm"
                                        href='<%# $"/CourseDetail.aspx?id={Eval("CourseId")}" %>'>Ver detalhes
                                    </a>

                                </div>

                            </div>
                        </div>
                    </div>
                </ItemTemplate>

                <FooterTemplate></div></FooterTemplate>
            </asp:Repeater>

            <!-- PAGINAÇÃO -->
            <nav class="mt-4" aria-label="pagination">
                <ul class="pagination justify-content-center">
                    <asp:Repeater ID="rptPages" runat="server">
                        <ItemTemplate>
                            <li class="page-item <%# Eval("Active") %>">
                                <asp:LinkButton runat="server" CssClass="page-link"
                                    CommandName="Page"
                                    CommandArgument='<%# Eval("PageNumber") %>'
                                    Text='<%# Eval("Label") %>' />
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </nav>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
