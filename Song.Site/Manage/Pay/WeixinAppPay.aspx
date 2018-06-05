<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="WeixinAppPay.aspx.cs" Inherits="Song.Site.Manage.Pay.WeixinAppPay" Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Src="PayInterfacerType.ascx" TagName="PayInterfacerType" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="120" class="right">
                支付方式：
            </td>
            <td>
                <uc1:PayInterfacerType ID="Pai_Pattern" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="right">
                支付方式名称：
            </td>
            <td>
                <asp:TextBox ID="Pai_Name" runat="server" Width="200" nullable="false"></asp:TextBox> <asp:CheckBox ID="Pai_IsEnable" Checked="true" runat="server" Text="是否启用" />
            </td>
        </tr>
         <tr>
            <td class="right">
               
            </td>
            <td>
               以下信息来自微信小程序
            </td>
        </tr>
        <tr>
            <td class="right">
                AppID(小程序ID)：
            </td>
            <td>
                <asp:TextBox ID="Pai_ParterID" runat="server" Width="60%" nullable="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                AppSecret：
            </td>
            <td>
               <asp:TextBox ID="Pai_Key" runat="server" Width="60%" nullable="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                回调域：
            </td>
            <td>
             <asp:TextBox ID="Pai_Returl" runat="server" Width="60%" nullable="false" begin="http://|https://"></asp:TextBox>/Pay/Weixin/
            </td>
        </tr>
         <tr>
            <td class="right">
               
            </td>
            <td>
               以下信息来自微信商户号
            </td>
        </tr>
        <tr>
            <td class="right">
                商户ID：
            </td>
            <td>
                <asp:TextBox ID="tbMCHID" runat="server" Width="300" nullable="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                支付密钥：
            </td>
            <td>
                <asp:TextBox ID="tbPaykey" runat="server" Width="300" nullable="false"></asp:TextBox>
            </td>
        </tr>
          <tr>
            <td class="right">
            </td>
            <td>
               &nbsp;
            </td>
        </tr>
        <tr>
            <td class="right">
                费率：
            </td>
            <td>
                <asp:TextBox ID="Pai_Feerate" runat="server" Width="80" MaxLength="5" datatype="number"></asp:TextBox>%
            </td>
        </tr>
        <tr>
            <td class="right">
                简介：
            </td>
            <td>
                <asp:TextBox ID="Pai_Intro" runat="server" Width="98%" Height="50"></asp:TextBox>
            </td>
        </tr>
      
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
