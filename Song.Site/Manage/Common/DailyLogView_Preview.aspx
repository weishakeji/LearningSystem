<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="DailyLogView_Preview.aspx.cs" Inherits="Song.Site.Manage.Common.DailyLogView_Preview"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
 <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
时间：<asp:Literal ID="ltWrtTime" runat="server"></asp:Literal>
员工：<asp:Literal ID="ltName" runat="server"></asp:Literal>
<br />
    类别：<asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal"
        RepeatLayout="Flow" Enabled="False" >
        <asp:ListItem Selected="True" Value="1">日志</asp:ListItem>
        <asp:ListItem Value="2">周志</asp:ListItem>
        <asp:ListItem Value="3">月志</asp:ListItem>
        <asp:ListItem Value="4">季度总结</asp:ListItem>
        <asp:ListItem Value="5">年度总结</asp:ListItem>
    </asp:RadioButtonList>
    <div id="divPlan" runat="server">
        原定计划：<br />
        <div class="TextBox" style="padding:5px; width:97%">
            <asp:Literal ID="ltPlan" runat="server"></asp:Literal></div>
    </div><br />
    工作记录：<br />
    <asp:TextBox ID="tbNote" runat="server" Height="120px" MaxLength="600" TextMode="MultiLine"
        Width="98%"></asp:TextBox><br /><br />
    工作计划：<br />
    <asp:TextBox ID="tbPlan" runat="server" Height="60px" MaxLength="300" TextMode="MultiLine"
        Width="98%"></asp:TextBox>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">

    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
