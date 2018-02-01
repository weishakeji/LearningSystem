<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="PointSetup.aspx.cs" Inherits="Song.Site.Manage.Sys.PointSetup" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
    <tr>
            <td class="right" colspan="2">
                <hr />
            </td>
        </tr>
         <tr>
            <td class="right">
                注册积分：
            </td>
            <td>
                初次注册，送<asp:TextBox ID="tbRegFirst" runat="server" Width="40"  datatype="uint" MaxLength="6"></asp:TextBox>积分；（注册即得分）
            </td>
        </tr>
        <tr>
            <td width="80" class="right">
                登录积分：
            </td>
            <td>
                每次登录增加<asp:TextBox ID="tbLoginPoint" runat="server" Width="40" datatype="uint" MaxLength="6"></asp:TextBox>积分；每天最多<asp:TextBox
                    ID="tbLoginPointMax" runat="server" Width="40" datatype="uint" MaxLength="6"></asp:TextBox>积分；
            </td>
        </tr>
        <tr>
            <td class="right">
                分享积分：
            </td>
            <td>
                每次访问增加<asp:TextBox ID="tbSharePoint" runat="server" Width="40" datatype="uint" MaxLength="6"></asp:TextBox>积分；每天最多<asp:TextBox
                    ID="tbSharePointMax" runat="server" Width="40"  datatype="uint" MaxLength="6"></asp:TextBox>积分；（分享链接被访问，即得分）
            </td>
        </tr>
        <tr>
            <td class="right">
                分享注册：
            </td>
            <td>
                每注册一位加<asp:TextBox ID="tbRegPoint" runat="server" Width="40"  datatype="uint" MaxLength="6"></asp:TextBox>积分；每天最多<asp:TextBox
                    ID="tbRegPointMax" runat="server" Width="40"  datatype="uint" MaxLength="6"></asp:TextBox>积分；（从分享链接注册，或是推荐注册，即得分）
            </td>
        </tr>
        <tr>
            <td class="right" colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="right">
                积分兑换：
            </td>
            <td>
                <asp:TextBox ID="tbPointConvert" runat="server" Width="60"  datatype="uint" MaxLength="10"></asp:TextBox>积分兑换1个卡券；（卡券可以购买课程，但不可以提现）
            </td>
        </tr>
        <tr>
            <td class="right" colspan="2">
                <hr />
            </td>
        </tr>
    </table>
    <cc1:Button verify="true" ID="btnEnter" runat="server" Text="确定" 
        onclick="btnEnter_Click" style="width: 40px" />
</asp:Content>
