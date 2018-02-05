<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="PwChanage.aspx.cs" Inherits="Song.Site.Manage.SMS.PwChanage" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <fieldset>
        <legend>密码修改</legend>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="120" class="right">
                    短信帐号名称：</td>
                <td>
                    <asp:Label ID="lbName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="right">
                    旧密码：</td>
                <td>
                    <asp:TextBox ID="tbPwOld" runat="server" Width="200px" MaxLength="100" nullable="false" ></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td>&nbsp;
                   </td>
                <td>
                    
                </td>
            </tr>
            <tr>
                <td class="right">
                    新密码：</td>
                <td>
                    <asp:TextBox ID="tbPw1" runat="server" Width="200px" MaxLength="100"   sametarget="tbPw2" nullable="false" ></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td class="right">
                    再录入：</td>
                <td>
                    <asp:TextBox ID="tbPw2" runat="server" Width="200px" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
              <tr>
                <td class="right">
                    </td>
                <td>
                    <asp:Label ID="lbSend" runat="server" Text=""></asp:Label>
                </td>
            </tr>
              <tr>
                <td class="right">
                   </td>
                <td>
                     <asp:Button ID="btnEnter" runat="server" Text="确定" verify="true" OnClick="btnEnter_Click"
            ValidationGroup="enter" />
                </td>
            </tr>
        </table>
       </fieldset>
    <br />
</asp:Content>
