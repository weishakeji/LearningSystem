<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Employee_Edit.aspx.cs" Inherits="Song.Site.Manage.Sys.Employee_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="../CoreScripts/hanzi2pinyin.js"></script>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="120" class="right">
                登录账号：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="tbAccName" runat="server" MaxLength="100"></asp:TextBox>
            </td>
            <td width="200" rowspan="7" valign="top">
                <img src="../Images/nophoto.jpg" name="imgShow" width="150" height="200" id="imgShow"
                    runat="server" /><br />
                <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png" Width="150" />
            </td>
        </tr>
        <tr id="trPw1" runat="server">
            <td class="right">
                初始密码：
            </td>
            <td>
                <asp:TextBox ID="tbPw1" runat="server" MaxLength="100" Width="100" nullable="false"></asp:TextBox>
                再次输入：
                <asp:TextBox ID="tbPw2" runat="server" MaxLength="100" Width="100" sametarget="tbPw1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                姓名：
            </td>
            <td>
                <asp:TextBox ID="tbName" runat="server" MaxLength="4" pinyin="tbNamePinjin" nullable="false"></asp:TextBox>
                <asp:CheckBox ID="cbIsUse" runat="server" Checked="True" Text="在职" AutoPostBack="True"
                    OnCheckedChanged="cbIsUse_CheckedChanged" />
            </td>
        </tr>
        <tr>
            <td class="right">
                拼音缩写：
            </td>
            <td>
                <asp:TextBox ID="tbNamePinjin" runat="server" MaxLength="10" Width="50px"></asp:TextBox>
               
            </td>
        </tr>
        <tr>
            <td class="right">
                员工工号：
            </td>
            <td>
                <asp:TextBox ID="tbEmpCode" runat="server" MaxLength="100"></asp:TextBox>
                &nbsp;
                <asp:CheckBox ID="cbIsPartTime" runat="server" Checked="True" Text="全职" AutoPostBack="True" />
            </td>
        </tr>
        <tr>
            <td class="right">
                性别：
            </td>
            <td>
                <asp:RadioButtonList ID="rbSex" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True" Value="1">男</asp:ListItem>
                    <asp:ListItem Value="2">女</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="right">
                身份证：
            </td>
            <td>
                <asp:TextBox ID="tbIdCard" runat="server" MaxLength="160"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                年龄：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbAge" runat="server" datatype="uint" MaxLength="3" Width="50px"></asp:TextBox>
            </td>
        </tr>
        <tr style="display:none">
            <td class="right">
                所在院系：
            </td>
            <td colspan="2">
                <cc1:DropDownTree ID="ddlDepart" runat="server" Width="150" IdKeyName="dep_id" ParentIdKeyName="dep_PatId"
                    TaxKeyName="dep_Tax" OnSelectedIndexChanged="ddlDepart_SelectedIndexChanged"
                    AutoPostBack="True">
                </cc1:DropDownTree>
                &nbsp;<span id="spanTeam" runat="server" visible="false"> 班组：<asp:DropDownList ID="ddlTeam"
                    runat="server">
                </asp:DropDownList>
                </span>
            </td>
        </tr>
        <tr>
            <td class="right" style="display:none">
                所在工作组：
            </td>
            <td colspan="2">
                <asp:CheckBoxList ID="cblGroup" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="right">
                职务（头衔）：
            </td>
            <td colspan="2">
                <asp:DropDownList ID="ddlTitle" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="right">
                所在岗位：
            </td>
            <td colspan="2">
                <asp:DropDownList ID="ddlPosi" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="right">
                电子信箱：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbEmail" MaxLength="80" Width="50%" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                固定电话：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbTel" MaxLength="50" Width="50%" runat="server"></asp:TextBox>
                <asp:CheckBox ID="cbIsOpenTel" runat="server" Text="在单位内公开" ToolTip="公开后，在单位通讯录中可见" />
            </td>
        </tr>
        <tr>
            <td class="right">
                移动电话：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbMobleTel" MaxLength="50" Width="50%" runat="server"></asp:TextBox>
                <asp:CheckBox ID="cbIsOpenMobile" runat="server" Text="在单位内公开" ToolTip="公开后，在单位通讯录中可见" />
            </td>
        </tr>
        <tr>
            <td class="right">
                Q Q：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbQQ" MaxLength="50" Width="50%" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                微信：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbWeixin" MaxLength="100" Width="50%" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                入职时间：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbRegTime" MaxLength="80" onfocus="WdatePicker()" runat="server"></asp:TextBox>
                &nbsp;
                <asp:CheckBox ID="cbIsAutoOut" runat="server" Text="是否预设自动离职" AutoPostBack="True"
                    OnCheckedChanged="cbIsAutoOut_CheckedChanged" />
                <div id="spanOutTime" runat="server">
                    离职时间：
                    <asp:TextBox ID="tbOutTime" MaxLength="80" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                        runat="server"></asp:TextBox>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
