<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Safe.aspx.cs" Inherits="Song.Site.Manage.Student.Safe"
    MasterPageFile="~/Manage/Student/Parents.Master" %>
<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="../CoreScripts/hanzi2pinyin.js"></script>
    <fieldset>
        <legend>密码修改</legend>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80px">
                原密码：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="tbOldPw" group="pw" runat="server" MaxLength="100" TextMode="Password"></asp:TextBox>
  <asp:Label ID="lbShow" runat=server Text="原密码不正确" Visible=False ForeColor="Red"></asp:Label>
            </td>
            
        </tr>
        <tr>
            <td class="right">
                新密码：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="tbPw1"  group="pw" runat="server" MaxLength="100"  sametarget="tbPw2" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td class="right">
                再次输入：
            </td>
            <td>
              <asp:TextBox ID="tbPw2" runat="server"  group="pw" MaxLength="100"  TextMode="Password"></asp:TextBox>   
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                <asp:Button ID="btnEnter" runat="server" Text="保存" verify=true  group="pw"  OnClick="btnEnter_Click" />
            </td>
        </tr>
    </table>
    </fieldset>
    <fieldset>
        <legend>安全问题</legend>
        <asp:Panel ID="plSafeQues" runat="server">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80px">
                安全问题：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="Ac_Qus"  group="ques" runat="server" Width="200" MaxLength="100"></asp:TextBox>

            </td>
            
        </tr>
        <tr>
            <td class="right">
                答案：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="Ac_Anwser" group="ques" runat="server"  Width="200" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
         
        <tr>
            <td class="right">
            </td>
            <td>
                <asp:Button ID="btnSafeQues" runat="server" verify=true  group="ques" Text="保存" OnClick="btnSafeQues_Click" />
            </td>
        </tr>
    </table>
        </asp:Panel>
        </fieldset>
</asp:Content>
