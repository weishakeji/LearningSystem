<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="List_View.aspx.cs" Inherits="Song.Site.Manage.Template.List_View"
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
                《<asp:Literal ID="ltName" runat="server"></asp:Literal>》
            </td>
            <td width="200" rowspan="6" class="right" valign="top">
                <img src="../Images/nophoto.jpg" name="imgShow" width="200" height="200" id="imgShow"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td class="right">
                作者：
            </td>
            <td>
             <asp:Literal ID="ltAuthor" runat="server"></asp:Literal>
               
            </td>
        </tr>
         <tr>
            <td class="right">
                电话：
            </td>
            <td>
            <asp:Literal ID="ltPhone" runat="server"></asp:Literal>
               
            </td>
        </tr>
        <tr>
            <td class="right">
                QQ：
            </td>
            <td>
            <asp:Literal ID="ltQQ" runat="server"></asp:Literal>
               
            </td>
        </tr>
        <tr>
            <td class="right">
                创建时间：
            </td>
            <td>
             <asp:Literal ID="ltCrtTime" runat="server"></asp:Literal>
               
            </td>
        </tr>
        <tr>
            <td class="right">
                简介：
            </td>
            <td>
            <asp:Literal ID="ltIntro" runat="server"></asp:Literal>
                
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
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
