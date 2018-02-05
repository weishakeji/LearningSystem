<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="History_Edit.aspx.cs" Inherits="Song.Site.Manage.Teacher.History_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="80" class="right">
                主题：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="Thh_Theme" runat="server" Width="98%" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                履历类型：
            </td>
            <td>
                <asp:DropDownList ID="Thh_Type" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="Thh_Type_SelectedIndexChanged">
                    <asp:ListItem>学习</asp:ListItem>
                    <asp:ListItem>实习</asp:ListItem>
                    <asp:ListItem>工作</asp:ListItem>
                    <asp:ListItem>其它</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="right">
                时间：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="Thh_StartTime" Formate="yyyy-MM-dd" runat="server" Width="80" onfocus="WdatePicker()"
                    MaxLength="100"></asp:TextBox>
                -至-
                <asp:TextBox ID="Thh_EndTime" runat="server"  formate="yyyy-MM-dd" onfocus="WdatePicker()" Width="80" MaxLength="100"></asp:TextBox>
                （结束时间为“至今”，则不填）
            </td>
        </tr>
    </table>
    <%--如果履历类型为“学习”--%>
    <asp:Panel ID="plStudy" runat="server">
        <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
            <tr>
                <td width="80" class="right">
                    所在学校：
                </td>
                <td>
                    <asp:TextBox ID="Thh_School" runat="server" Width="98%" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    主修专业：
                </td>
                <td>
                    <asp:TextBox ID="Thh_Major" runat="server" Width="98%" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    学历：
                </td>
                <td>
                   <asp:DropDownList ID="Thh_Education" runat="server">
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
        </table>
    </asp:Panel>
     <%--如果履历类型为“工作”--%>
    <asp:Panel ID="plWork" runat="server">
        <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
            <tr>
                <td width="80" class="right">
                    所在公司：
                </td>
                <td>
                    <asp:TextBox ID="Thh_Compay" runat="server" Width="98%" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    主要工作：
                </td>
                <td>
                    <asp:TextBox ID="Thh_Job" runat="server" Width="98%" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    职务：
                </td>
                <td>
                    <asp:TextBox ID="Thh_Post" runat="server" Width="98%" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="80" class="right">
                详情：
            </td>
            <td>
                <asp:TextBox ID="Thh_Intro" runat="server" Width="98%" MaxLength="2000" TextMode="MultiLine"
                    Height="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                成绩：
            </td>
            <td>
                <asp:TextBox ID="Thh_Success" runat="server" Width="98%" MaxLength="1000" TextMode="MultiLine"
                    Height="60"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
