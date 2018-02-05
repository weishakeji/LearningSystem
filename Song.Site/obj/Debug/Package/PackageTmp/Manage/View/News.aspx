<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="News.aspx.cs" Inherits="Song.Site.Manage.View.News" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <div id="fieldSear" class="searchBox" runat="server">
            标题：<asp:TextBox ID="tbSear" runat="server" Width="100px" MaxLength="10"></asp:TextBox>&nbsp;
             <asp:CheckBox ID="cbIsHot" runat="server" Text="热点" />
            <asp:CheckBox ID="cbIsTop" runat="server" Text="置顶" />
            <asp:CheckBox ID="cbIsRec" runat="server" Text="推荐" />
            <asp:CheckBox ID="cbIsImg" runat="server" Text="图片" />
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <dl>
        <asp:Repeater ID="rtpList" runat="server">
            <ItemTemplate>
                <dd>
                    <a href="#"  onclick="OpenWin('News_Preview.aspx?id=<%# Eval("Art_Id")%>','预览',80,80);return false;">
                        <%# Container.ItemIndex * Pager1.Index + 1%>
                        、<%# Eval("Art_Title", "{0}")%>
                    </a>
                </dd>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Panel ID="plNoData" runat="server" Visible="false">
            没有检索到相关数据！
        </asp:Panel>
    </dl>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
