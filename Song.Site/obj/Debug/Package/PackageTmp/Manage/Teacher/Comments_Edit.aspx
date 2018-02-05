<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Comments_Edit.aspx.cs" Inherits="Song.Site.Manage.Teacher.Comments_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="../CoreScripts/hanzi2pinyin.js"></script>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80px">
                教师：
            </td>
            <td>
                <asp:Label ID="Th_Name" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                评分：
            </td>
            <td>
                <span id="score" style="display: none">
                    <asp:Literal ID="Thc_Score" runat="server"></asp:Literal></span> <span class="star"
                        score="" size="10"></span>
            </td>
        </tr>
        <tr>
            <td class="right">
                评价：
            </td>
            <td>
                <asp:Label ID="Thc_Comment" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                时间：
            </td>
            <td>
                <asp:Label ID="Thc_CrtTime" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                来源IP：
            </td>
            <td>
                <asp:Label ID="Thc_IP" runat="server" Text=""></asp:Label>
                （<asp:Label ID="Thc_Device" runat="server" Text=""></asp:Label>）
            </td>
        </tr>
        <tr>
            <td class="right">
                评价人：
            </td>
            <td>
                <asp:Panel ID="plAccount" runat="server">
                    <asp:Label ID="Ac_Name" runat="server" Text=""></asp:Label>
                    <asp:Label ID="Ac_MobiTel1" runat="server" Text=""></asp:Label>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <hr /><br />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80px">
                回复：
            </td>
            <td>
                <asp:TextBox ID="Thc_Reply" runat="server" Height="100px" TextMode="MultiLine" Width="98%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                <asp:CheckBox ID="Thc_IsUse" runat="server" Text="是否启用" />
                <asp:CheckBox ID="Thc_IsShow" runat="server" Text="是否显示" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
