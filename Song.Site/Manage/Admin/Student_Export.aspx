<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Student_Export.aspx.cs" Inherits="Song.Site.Manage.Admin.Student_Export" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <br />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80">
                班级：
            </td>
            <td>
            <div class="selectBtn"><a class="all">全选</a><a class="invert">反选</a><a class="cancel">取消</a></div>
                <asp:CheckBoxList ID="cblSort" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                </asp:CheckBoxList>
            </td>
        </tr>
         <tr>
            <td class="right">
                学员总数：
            </td>
            <td>

             <asp:Label ID="lbSumcount" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                &nbsp;
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="right">
                &nbsp;
            </td>
            <td>
                <cc1:Button ID="btnExport" CssClass="Button" runat="server" Text="导出到Excel" OnClick="btnExport_Click" /> &nbsp; 
               <%-- <a href="Students_Details.aspx" id="linkExportDetails" class="btnButton" target="_blank" link="Students_Details.aspx">导出详细信息</a>&nbsp; --%>
                <cc1:Button ID="btnExportDetails" runat="server" Text="打印详细信息" /> &nbsp; 
            </td>
        </tr>
        <tr>
            <td class="right">
                &nbsp;
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="right">
               
            </td>
            <td>
              （注："打印详细信息"需要在360浏览器、QQ浏览器的极速模式下执行，极速模式采用的是Chrome内核。）
            </td>
        </tr>
    </table>
    <iframe src="" id="iframeExportDetails" link="../student/Students_Details.aspx" scrolling="auto" style="display:none;" height="30" width="100%"></iframe>
</asp:Content>
