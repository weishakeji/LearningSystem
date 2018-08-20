<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Pager.ascx.cs" Inherits="Song.Site.Manage.Utility.Pager" %>
<div class="pager">
    共<span><%= RecordAmount %></span>条 &nbsp; 第<%= Index %>页/共<%= PageAmount %>页 &nbsp;
    每页<asp:TextBox ID="tbSize" runat="server" MaxLength="2" Width="20px"></asp:TextBox>条
    &nbsp;<span class="noprint">
    <asp:LinkButton ID="lbnFirst" runat="server" Text="首页" OnClick="lbnFirst_Click"></asp:LinkButton>
    <asp:LinkButton ID="lbnPrevious" runat="server" Text="上页" OnClick="lbnPrevious_Click"></asp:LinkButton>
    <asp:LinkButton ID="lbnNext" runat="server" Text="下页" OnClick="lbnNext_Click"></asp:LinkButton>
    <asp:LinkButton ID="lbnLast" runat="server" Text="末页" OnClick="lbnLast_Click"></asp:LinkButton>
    <asp:DropDownList ID="ddlGoPage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlGoPage_SelectedIndexChanged">
    </asp:DropDownList></span></div>
