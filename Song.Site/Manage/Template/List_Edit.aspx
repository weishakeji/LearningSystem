<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="List_Edit.aspx.cs" Inherits="Song.Site.Manage.Template.List_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80px">
                模板名称：
            </td>
            <td>
                <asp:TextBox ID="tbName" nullable="false" Width="95%" MaxLength="200" runat="server"></asp:TextBox>
            </td>
            <td width="200" rowspan="6" class="right" valign="top">
                <img src="../Images/nophoto.jpg" name="imgShow" width="200" height="200" id="imgShow"
                    runat="server" /><br />
                <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png" Width="200" />
            </td>
        </tr>
        <tr>
            <td class="right">
                作者：
            </td>
            <td>
                <asp:TextBox ID="tbAuthor" Width="160" MaxLength="200" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                电话：
            </td>
            <td>
                <asp:TextBox ID="tbPhone" Width="160" MaxLength="200" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                QQ：
            </td>
            <td>
                <asp:TextBox ID="tbQQ" Width="160" MaxLength="200" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                创建时间：
            </td>
            <td>
                <asp:TextBox ID="tbCrtTime" MaxLength="80" Width="160" EnableTheming=false CssClass="Wdate" onfocus="WdatePicker()" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                简介：
            </td>
            <td>
                <asp:TextBox ID="tbIntro" Width="98%" TextMode="MultiLine" Height="100" MaxLength="200"
                    runat="server"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td class="right">
                文件夹：
            </td>
            <td colspan="2">
                <asp:Label ID="lbFileName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
         <tr>
            <td class="right">
             
            </td>
            <td colspan="2">
              &nbsp;
            </td>
        </tr>
          <tr>
            <td class="right prompt" valign=top>
             注：
            </td>
            <td colspan="2" class="prompt">
             此处仅是编辑模板库的信息，非选择模板。<br />
             模板库文件夹中_self.xml需要有写入权限。
            </td>
        </tr>
    </table>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
