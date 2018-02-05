<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="RechargeSet_Edit.aspx.cs" Inherits="Song.Site.Manage.Money.RechargeSet_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="Kind" %>
<%@ Register Src="../Utility/SortSelect.ascx" TagName="SortSelect" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80" >
                主题：
            </td>
            <td>
                <asp:TextBox ID="tbTheme" nullable="false" runat="server" MaxLength="20" Width="95%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                面额：
            </td>
            <td>
                <asp:TextBox ID="tbPrice" datatype="uint" nullable="false" runat="server" MaxLength="20"
                    Width="100"></asp:TextBox>
                元
            </td>
        </tr>
        <tr>
            <td class="right">
                数量：
            </td>
            <td>
                <asp:TextBox ID="tbCount" datatype="uint" nullable="false" runat="server" MaxLength="20"
                    Width="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                有限期：
            </td>
            <td>
                <asp:TextBox ID="tbStart" runat="server" onfocus="WdatePicker()" CssClass="Wdate"
                    EnableTheming="false" MaxLength="20" Width="100"></asp:TextBox>
                至
                <asp:TextBox ID="tbEnd" runat="server" onfocus="WdatePicker()" CssClass="Wdate" EnableTheming="false"
                    MaxLength="20" Width="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                简介：
            </td>
            <td>
                <asp:TextBox ID="tbIntro" runat="server" MaxLength="20" Width="95%" TextMode="MultiLine"
                    Height="60"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                <asp:CheckBox ID="cbIsEnable" runat="server" Checked="true" Text="是否启用" />
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
                密钥：
            </td>
            <td>
                <asp:TextBox ID="tbPw" runat="server" nullable="false" lenlimit="10-32" MaxLength="100" Width="95%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
               
            </td>
            <td>
                （密钥只是作为增强充值码安全性，实际操作中用不到该值。）
            </td>
        </tr>
          <tr>
            <td class="right">
               充值码长度：
            </td>
            <td>
                <asp:TextBox ID="tbCodeLength" datatype="uint" nullable="false" numlimit="6-32"  runat="server" MaxLength="2"
                    Width="30" Text="12"></asp:TextBox>
            </td>
        </tr>
          <tr>
            <td class="right">
               密码长度：
            </td>
            <td>
                <asp:TextBox ID="tbPwLength" datatype="uint" nullable="false" numlimit="3-8"  runat="server" MaxLength="2"
                    Width="30" Text="3"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
