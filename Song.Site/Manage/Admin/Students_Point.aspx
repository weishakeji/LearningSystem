<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Students_Point.aspx.cs" Inherits="Song.Site.Manage.Admin.Students_Point"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80px">
                学员：
            </td>
            <td>
                <asp:Label ID="lbName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                当前积分：
            </td>
            <td>
                <asp:Label ID="lbPoint" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                分（ 历史最高
                <asp:Label ID="lbPointmax" runat="server"></asp:Label>
                分）
            </td>
        </tr>
        <%--  <tr>
            <td class="right">
            </td>
            <td>
                &nbsp;
            </td>
        </tr>--%>
        <tr>
            <td class="right">
            </td>
            <td>
                <asp:RadioButtonList ID="rblOpera" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                    AutoPostBack="True" onselectedindexchanged="rblOpera_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="2">充值</asp:ListItem>
                    <asp:ListItem Value="1">扣除</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="right">
                数量：
            </td>
            <td>
                <asp:Label ID="lbOperator" runat="server" Text="+"></asp:Label>
                <asp:TextBox ID="tbPoint" runat="server" Width="40%" nullable="false" datatype="uint"
                    group="acc"></asp:TextBox>分
            </td>
        </tr>
        <tr>
            <td class="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="tbRemark" runat="server" MaxLength="100" Height="60" Width="80%"
                    TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnAddMoney" runat="server" Text="确定" group="acc"
        OnClick="btnAddMoney_Click" ValidationGroup="enter" OnClientClick="return confirm('确定操作吗？')" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
