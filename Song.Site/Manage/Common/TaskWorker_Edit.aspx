<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="TaskWorker_Edit.aspx.cs" Inherits="Song.Site.Manage.Common.TaskWorker_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>

    任务名称：
    <asp:Label ID="lbName" runat="server" Font-Bold="True"></asp:Label>
    <br />
    优先级别：
    <asp:Label ID="lbLevel" runat="server"></asp:Label>
    指派给：
    <asp:Label ID="lbWorkerName" runat="server"></asp:Label>
    [<asp:Label ID="lbDepart" runat="server"></asp:Label>]
    <table width="98%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td width="50%" valign="top">
                任务描述：<br />
                <asp:Label ID="lbContext" runat="server"></asp:Label>
                <br />
                <br />
                开始时间：
                <asp:Label ID="lbStart" runat="server"></asp:Label>
                <br />
                计划完成时间：
                <asp:Label ID="lbEnd" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Panel ID="plLog" runat="server">
                    工作记录(完成情况)：<br />
                    <asp:TextBox ID="tbWorkLog" runat="server" Height="200px" MaxLength="255" TextMode="MultiLine"
                        Width="98%"></asp:TextBox>
                    <br />
                    完成工作量：
                    <asp:TextBox ID="tbCompletePer" runat="server" datatype="uint" MaxLength="3" Width="40px"></asp:TextBox>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:Panel ID="plGoback" runat="server" Visible="False">
        任务退回原因：<br />
        <asp:TextBox ID="tbGoBackText" runat="server" Height="60px" nullable="false" MaxLength="255"
            TextMode="MultiLine" Width="98%"></asp:TextBox>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <asp:Button ID="btnGoBack" runat="server" Text="退回任务" OnClick="btnGoBack_Click" />
    <asp:Button ID="btnGoBackEnter" runat="server" Text="确定退回任务" verify="true" ValidationGroup="goback"
        Visible="False" OnClick="btnGoBackEnter_Click" />
    <asp:Button ID="btnGoBackEnterCancel" runat="server" Text="取消操作" Visible="False"
        OnClick="btnGoBackEnterCancel_Click" />
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
