<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Departs_Edit.aspx.cs" Inherits="Song.Site.Manage.Admin.Departs_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80px">
                中文名称：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="Dep_CnName" lenlimit="2-25" runat="server" Width="95%"
                    MaxLength="100" group="ent"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                中文简称：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="Dep_CnAbbr" lenlimit="2-25" runat="server" Width="160"
                    MaxLength="25" group="ent"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                英文名称：
            </td>
            <td>
                <asp:TextBox ID="Dep_EnName" runat="server" Width="95%" MaxLength="100"
                    group="ent"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                英文简称：
            </td>
            <td>
                <asp:TextBox ID="Dep_EnAbbr" runat="server" Width="160" MaxLength="25"
                    group="ent"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                院系代码：
            </td>
            <td>
                <asp:TextBox ID="Dep_Code"  runat="server" Width="160" MaxLength="50"
                    group="ent"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                电话：
            </td>
            <td>
                <asp:TextBox ID="Dep_Phone" datatype="tel" runat="server" Width="160" MaxLength="100"
                    group="ent"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                传真：
            </td>
            <td>
                <asp:TextBox ID="Dep_Fax" datatype="tel" runat="server" Width="160" MaxLength="100"
                    group="ent"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                邮箱：
            </td>
            <td>
                <asp:TextBox ID="Dep_Email" datatype="email" runat="server" Width="160" MaxLength="100"
                    group="ent"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                地址：
            </td>
            <td>
                <asp:TextBox ID="Dep_WorkAddr" runat="server" Width="95%" MaxLength="100" group="ent"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                简介：
            </td>
            <td>
                <asp:TextBox ID="Dep_Func" runat="server" Width="99%" Height="100" group="ent" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                <asp:CheckBox ID="Dep_IsUse" runat="server" Text="是否启用" Checked="true" />
                <asp:CheckBox ID="Dep_IsShow" runat="server" Text="是否显示"  Checked="true" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" group="ent" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
