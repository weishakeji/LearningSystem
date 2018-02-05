<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="CustomService_Edit.aspx.cs" Inherits="Song.Site.Manage.Site.CustomService_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
 <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    标题：<asp:TextBox nullable="false" ID="tbTtl" runat="server" MaxLength="100" Width="60%"></asp:TextBox><asp:CheckBox
        ID="cbIsShow" runat="server" Checked="True" Text="显示" /><br />
    内容：<br />
     <WebEditor:Editor ID="tbContent" runat="server" ThemeType="mini">
    </WebEditor:Editor>
    发布时间：<asp:TextBox nullable="false" ID="tbStarTime" runat="server" MaxLength="100" onfocus="WdatePicker({dateFmt:'yyyy-MM-d HH:mm:ss'})" Width="160"></asp:TextBox><br />
    发布　人：<asp:TextBox ID="tbName" runat="server" MaxLength="100" Width="100"></asp:TextBox><br />
    发布单位：<asp:TextBox ID="tbOrg" runat="server" MaxLength="100" Width="100"></asp:TextBox>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
