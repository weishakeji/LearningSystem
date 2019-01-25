<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Link.aspx.cs" Inherits="Song.Site.Manage.Student.Link"
    MasterPageFile="~/Manage/Student/Parents.Master" %>
<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="../CoreScripts/hanzi2pinyin.js"></script>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
       
       
        <tr>
            <td class="right" width="80">
                电话：
            </td>
            <td>
                <asp:TextBox ID="Ac_Tel" runat="server" datatype="tel"></asp:TextBox>&nbsp;<asp:CheckBox
                    ID="Ac_IsOpenPhone" runat="server" Text="公开" />
            </td>
        </tr>
        <tr>
            <td class="right">
                移动电话：
            </td>
            <td>
                <asp:TextBox ID="Ac_MobiTel1" runat="server" datatype="mobile"></asp:TextBox>&nbsp;<asp:CheckBox
                    ID="Ac_IsOpenMobi" runat="server" Text="公开" />
            </td>
        </tr>
        <tr>
            <td class="right">
                电子邮箱：
            </td>
            <td>
                <asp:TextBox ID="Ac_Email" runat="server" datatype="email"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                QQ：
            </td>
            <td>
                <asp:TextBox ID="Ac_Qq" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                微信：
            </td>
            <td>
                <asp:TextBox ID="Ac_Weixin" runat="server"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td class="right">
                住址：
            </td>
            <td>
                <asp:TextBox ID="Ac_Address" runat="server" Width="300"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                通讯地址：
            </td>
            <td>
                <asp:TextBox ID="Ac_AddrContact" runat="server" Width="300"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                邮编：
            </td>
            <td>
                <asp:TextBox ID="Ac_Zip" runat="server" datatype="zip" Width="80"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td class="right">
                紧急联系人：
            </td>
            <td>
                <asp:TextBox ID="Ac_LinkMan" runat="server" Width="160"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td class="right">
                紧急电话：
            </td>
            <td>
                <asp:TextBox ID="Ac_LinkManPhone" runat="server"  Width="160"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                <asp:Button ID="btnEnter" runat="server" Text="保存" verify=true  OnClick="btnEnter_Click" />
            </td>
        </tr>
    </table>
   
</asp:Content>
