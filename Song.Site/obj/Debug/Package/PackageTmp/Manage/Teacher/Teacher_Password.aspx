<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Teacher_Password.aspx.cs" Inherits="Song.Site.Manage.Teacher.Teacher_Password"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
         <tr>
            <td class="right" width="80px">
                姓名：
            </td>
            <td>
                <asp:Label ID="lbName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                重置密码：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="tbPw1" runat="server" MaxLength="100"  group="pw" ></asp:TextBox>
               
            </td>
        </tr>
        <tr>
            <td class="right">
                再次输入：
            </td>
            <td>
                <asp:TextBox ID="tbPw2" runat="server" MaxLength="100" sametarget="tbPw1" group="pw" ></asp:TextBox>
                
            </td>
        </tr>
      
           <tr>
            <td class="right">
                
            </td>
            <td>
               （由于学员与教师共有一个账号与密码，此处的密码修改后，对应的学员账号密码也会修改）
                
            </td>
        </tr>
           <tr>
            <td class="right">
                
            </td>
            <td>
               &nbsp;
                
            </td>
        </tr>
          <tr>
            <td class="right">
                &nbsp;
            </td>
            <td>
                <cc1:Button ID="btnPw" verify="true" group="pw" runat="server" Text="重置密码" 
                    onclick="btnPw_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
