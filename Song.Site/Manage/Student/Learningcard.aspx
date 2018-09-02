<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Learningcard.aspx.cs" Inherits="Song.Site.Manage.Student.Learningcard"
    Title="学习卡" %>

<%@ MasterType VirtualPath="~/Manage/ManagePage.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80px">
                学习卡：
            </td>
            <td>
                <asp:TextBox ID="tbCode" runat="server" Width="300px" nullable="false" regex="\d+(\s|)-(\s|)\d+"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                （注：格式为“学习卡-密码”，破折号不可缺少）
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                <cc1:Button ID="btnGet" runat="server" Text="暂存" verify="true" />
                <cc1:Button ID="btnUse" runat="server" Text="立即使用" verify="true" />
            </td>
        </tr>
    </table>
    <asp:Repeater ID="rptCards" runat="server">
        <HeaderTemplate>
            <fieldset>
                <legend>我的学习卡</legend>
                <dl class="cards">
        </HeaderTemplate>
        <ItemTemplate>
            <dd>
                <%# Container.ItemIndex + 1%>、 <a href="LearningCardView.aspx?code=<%# Eval("Lc_Code")%>&pw=<%# Eval("Lc_pw")%>"
                    class="viewinfo">
                    <%# Eval("Lc_Code")%>-<%# Eval("Lc_pw")%></a><%# Eval("Lc_State", "{0}") != "0" ? "<span class='badge'>已经使用</span>" : "<span class='badge use'>使用</span>"%></dd>
        </ItemTemplate>
        <FooterTemplate>
            </dl> </fieldset>
        </FooterTemplate>
    </asp:Repeater>
   
</asp:Content>
