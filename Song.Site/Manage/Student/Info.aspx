<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Info.aspx.cs" Inherits="Song.Site.Manage.Student.Info"
    MasterPageFile="~/Manage/Student/Parents.Master" %>
<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="../CoreScripts/hanzi2pinyin.js"></script>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80px">
                姓名：
            </td>
            <td>
                <asp:TextBox ID="Ac_Name" nullable="false" pinyin="Ac_Pinyin" Width="60" MaxLength="6"
                    runat="server"></asp:TextBox>
            </td>
            <td width="200" rowspan="8" class=right valign="top">
                <img src="../Images/nophoto.jpg" name="imgShow" width="150" height="200" id="imgShow"
                    runat="server" /><br />
                <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png" Width="150" />
            </td>
        </tr>
        <tr>
            <td class="right">
                账号：
            </td>
            <td>
                <asp:TextBox ID="Ac_AccName" runat="server" Width="120" MaxLength="5" Enabled=false></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                拼音：
            </td>
            <td>
                <asp:TextBox ID="Ac_Pinyin" runat="server" Width="60" MaxLength="5"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                签名：
            </td>
            <td>
                <asp:TextBox ID="Ac_Signature" runat="server" lenlimit="200" TextMode="MultiLine"
                    Width="80%" Height="60"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                性别：
            </td>
            <td>
                <asp:RadioButtonList ID="Ac_Sex" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0">未知</asp:ListItem>
                    <asp:ListItem Value="1">男</asp:ListItem>
                    <asp:ListItem Value="2">女</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
  
        <tr>
            <td class="right">
                出生年月：
            </td>
            <td>
                <asp:TextBox ID="Ac_Birthday" runat="server" onfocus="WdatePicker()" Width="100" CssClass="Wdate" EnableTheming="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                身份证：
            </td>
            <td>
                <asp:TextBox ID="Ac_IDCardNumber" runat="server" datatype="idcard"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                最高学历：
            </td>
            <td>
                <asp:DropDownList ID="Ac_Education" runat="server">
                    <asp:ListItem Value="81">小学</asp:ListItem>
                    <asp:ListItem Value="71">初中</asp:ListItem>
                    <asp:ListItem Value="61">高中</asp:ListItem>
                    <asp:ListItem Value="41">中等职业教育</asp:ListItem>
                    <asp:ListItem Value="31">大学（专科）</asp:ListItem>
                    <asp:ListItem Value="21">大学（本科）</asp:ListItem>
                    <asp:ListItem Value="14">硕士</asp:ListItem>
                    <asp:ListItem Value="11">博士</asp:ListItem>
                    <asp:ListItem Value="90">其它</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="right">
                专业：
            </td>
            <td colspan="2">
                <asp:TextBox ID="Ac_Major" Width="300" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                籍贯：
            </td>
            <td colspan="2">
                <asp:TextBox ID="Ac_Native" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                民族：
            </td>
            <td colspan="2">
                <asp:TextBox ID="Ac_Nation" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right" width="80px" valign="top">
                <br />
                个人介绍：
            </td>
            <td colspan="2">
                <WebEditor:Editor ID="Ac_Intro"  runat="server" Height="250px" Width="99%" 
                    ThemeType="mini"></WebEditor:Editor>
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                &nbsp;
            </td>
        </tr
        <tr>
            <td class="right">
            </td>
            <td>
                <cc1:Button ID="btnEnter" runat="server" Text="保存" verify="true" OnClick="btnEnter_Click" />
            </td>
        </tr>
    </table>

</asp:Content>
