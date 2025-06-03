<%@ Page Title="Notificações" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="TrabalhoFinal3.Notifications" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="mb-4">Notificações</h2>
    <asp:UpdatePanel ID="upList" runat="server">
        <ContentTemplate>
            <asp:Repeater ID="rptNotifications" runat="server" OnItemCommand="rptNotifications_ItemCommand">
                <HeaderTemplate>
                    <ul class="list-group">
                </HeaderTemplate>
                <ItemTemplate>
                    <li class="list-group-item d-flex justify-content-between align-items-start">
                        <div class="ms-2 me-auto">
                            <div class="fw-bold"><%# Eval("Message") %></div>
                            <small class="text-muted"><%# Eval("SentDate", "{0:dd/MM/yyyy HH:mm}") %></small>
                        </div>
                        <asp:Button runat="server" CssClass="btn btn-sm btn-link" CommandName="read" CommandArgument='<%# Eval("Id") %>' Text="Marcar como lida" Visible='<%# !(bool)Eval("Read") %>' />
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
