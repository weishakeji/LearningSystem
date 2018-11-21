<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="SMSSetup_Msg.aspx.cs" Inherits="Song.Site.Manage.SMS.SMSSetup_Msg"
    Title="短信接口编辑" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
   
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="80" class="right">
                短信模板：
            </td>
            <td>
                <asp:TextBox ID="tbSmsTemplate" runat="server" Height="60px" TextMode="MultiLine" MaxLength="200"
                    Width="99%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right" valign="top" style="padding-top:5px;">
              说明：
            </td>
            <td>
             <p>  1、结尾必须是【您的平台名称】，注意一定是中文的中括号结尾。
            <br />
            2、替代符：{vcode}验证码，{platform}平台名称，{org}机构简称,{date}时间。<br />
3、案例：您正在登录{plate},验证码：{vcode}。转发可能导致账号被盗。{date},【{org}】
</p>
</td>
        </tr>
        <tr>
            <td class="right" valign="top" style="padding-top:5px;">
              实际效果：
            </td>
            <td>
                <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label>

            </td>
            </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
