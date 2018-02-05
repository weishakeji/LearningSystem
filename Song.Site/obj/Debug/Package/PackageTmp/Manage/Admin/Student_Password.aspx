<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Student_Password.aspx.cs" Inherits="Song.Site.Manage.Admin.Student_Password"
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
        </tr><tr>
            <td class="right" >
                账号：
            </td>
            <td>
                <asp:TextBox ID="tbStudentAcc" runat="server" Width="80%" nullable="false" group="acc" ></asp:TextBox>
                <asp:CustomValidator
                    ID="cusv" runat="server" ControlToValidate="tbStudentAcc" Display="Dynamic"
                    ErrorMessage="账号已经存在" SetFocusOnError="True" ValidationGroup="enter" 
                    onservervalidate="cusv_ServerValidate" ForeColor="Red"></asp:CustomValidator>
            </td>
        </tr>
         <tr>
            <td class="right">
                &nbsp;
            </td>
            <td>
                <cc1:Button ID="btnAcc"  verify="true" group="acc" runat="server" Text="修改账号" 
                    onclick="btnAcc_Click" />
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
                重置密码：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="tbPw1" runat="server" MaxLength="100"  
                    Width="80%" group="pw" TextMode="Password" ></asp:TextBox>
               
            </td>
        </tr>
        <tr>
            <td class="right">
                再次输入：
            </td>
            <td>
                <asp:TextBox ID="tbPw2" runat="server" MaxLength="100" TextMode="Password" sametarget="tbPw1" Width="80%" group="pw" ></asp:TextBox>
                
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

