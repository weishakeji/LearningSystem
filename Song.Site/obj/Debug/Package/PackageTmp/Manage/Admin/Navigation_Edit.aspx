<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Navigation_Edit.aspx.cs" Inherits="Song.Site.Manage.Admin.Navigation_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="Kind" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

    <script type="text/javascript" src="../CoreScripts/iColorPicker.js"></script>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="80" class="right">
                名称：</td>
            <td width="200">
                <asp:TextBox ID="tbName" Width="180" MaxLength="100" runat="server" nullable="false"></asp:TextBox></td>
            <td width="80" class="right">
                </td>
            <td>
                 <asp:CheckBox ID="cbIsShow" runat="server" Text="是否显示" Checked="True" /></td>
        </tr>
        <tr>
            <td class="right">
                上级导航：</td>
            <td>
                <cc1:DropDownTree ID="ddlTree" runat="server" IdKeyName="Nav_ID" ParentIdKeyName="Nav_PID"
                    TaxKeyName="Nav_Tax" Width="180">
                </cc1:DropDownTree></td>
            <td width="80" class="right">
            </td>
            <td>
                </td>
        </tr>
        <tr>
            <td class="right">
                链接地址：</td>
            <td colspan="3">
                <asp:TextBox ID="tbUrl" runat="server" Width="98%" MaxLength="255"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="right">
                提示信息：</td>
            <td colspan="3">
                <asp:TextBox ID="tbTitle" runat="server" Width="98%" MaxLength="255"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="right">
                打开方式：</td>
            <td colspan="3">
                <asp:DropDownList ID="ddlTarget" runat="server">
                    <asp:ListItem Selected="True"> </asp:ListItem>
                    <asp:ListItem>_blank</asp:ListItem>
                    <asp:ListItem>_self</asp:ListItem>
                    <asp:ListItem>_parent</asp:ListItem>
                    <asp:ListItem>_top</asp:ListItem>
                    <asp:ListItem>_open</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="right">
                字体：</td>
            <td colspan="3">
                <asp:TextBox ID="tbFont" runat="server"></asp:TextBox>
                &nbsp;颜色：
                <asp:TextBox ID="tbColor" runat="server" Width="60" CssClass="iColorPicker" SkinID="d"></asp:TextBox>
                &nbsp;
                <asp:CheckBox ID="cbIsBold" runat="server" Text="是否粗体" /></td>
        </tr>
        <tr>
            <td class="right" valign="top">
                导航说明：</td>
            <td colspan="3">
                <asp:TextBox ID="tbIntro" runat="server" Width="98%" TextMode="MultiLine" Height="60px"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="right">
                排序号：</td>
            <td colspan="3">
                <asp:TextBox ID="tbTax" runat="server" Width="64" MaxLength="3" datatype="uint"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="right">
            图标：
            </td>
            <td colspan="3">
            <img src="../Images/nophoto.jpg" name="imgShow" width="128" height="128" id="imgShow"
                    runat="server" /><br />
                <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png" Width="128" />
            </td>
        </tr>
         <tr>
            <td class="right">
            </td>
            <td colspan="3">
           
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
