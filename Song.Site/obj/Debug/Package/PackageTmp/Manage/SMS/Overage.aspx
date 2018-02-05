<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Overage.aspx.cs" Inherits="Song.Site.Manage.SMS.Overage" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <fieldset>
        <legend>余额查询</legend>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
                <td width="120" class="right">
                    当前短信平台：</td>
                <td>
                    <asp:Label ID="lbPlate" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="right">
                    短信帐号名称：</td>
                <td>
                    <asp:Label ID="lbName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="right">
                    密码：</td>
                <td>
                    <asp:Label ID="lbPw" runat="server" Text=""></asp:Label>
                </td>
            </tr>
              <tr>
                <td class="right">
                </td>
                <td>
                   
                </td>
            </tr>
              <tr>
                <td class="right">
                余额：
                </td>
                <td>
                    <asp:Label ID="lbOverNumber" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="right">
                </td>
                <td>
                    <asp:Button ID="btnEnter" runat="server" Text="查 看" verify="true" OnClick="btnEnter_Click"
                        ValidationGroup="enter" />
                </td>
            </tr>
        </table>
    </fieldset>
   
</asp:Content>
