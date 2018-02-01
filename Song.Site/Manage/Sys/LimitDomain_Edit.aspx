<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="LimitDomain_Edit.aspx.cs" Inherits="Song.Site.Manage.Sys.LimitDomain_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="../CoreScripts/hanzi2pinyin.js"></script>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="80" class="right">
                名称：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="LD_Name" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                简介：
            </td>
            <td>
                <asp:TextBox ID="LD_Intro" runat="server" lenlimit="500" TextMode="MultiLine" Width="95%"
                    Height="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                状态：
            </td>
            <td>
                <asp:CheckBox ID="LD_IsUse" runat="server" Text="是否启用" Checked="true" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
