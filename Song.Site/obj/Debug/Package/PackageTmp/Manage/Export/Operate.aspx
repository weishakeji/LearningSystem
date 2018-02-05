<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Operate.aspx.cs" Inherits="Song.Site.Manage.Export.Operate" Title="课程列表" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <fieldset>
        <legend>学员导出</legend>
        <asp:Button ID="btnExpStudent" runat="server" Text="学员导出" 
            onclick="btnExpStudent_Click" /></fieldset>
</asp:Content>
