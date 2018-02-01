<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Export.aspx.cs" Inherits="Song.Site.Manage.Questions.Export" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80">
                题型：
            </td>
            <td>
                <asp:CheckBoxList ID="cblType" runat="server" RepeatDirection="Horizontal" 
                    RepeatLayout="Flow">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="right">
                范围：
            </td>
            <td>
                <cc1:DropDownTree ID="ddlSubject" runat="server" IdKeyName="Sbj_ID" ParentIdKeyName="Sbj_PID"
                    TaxKeyName="Sbj_Tax" Width="160" AutoPostBack="True" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                </cc1:DropDownTree>
                <cc1:DropDownTree ID="ddlCourse" runat="server" IdKeyName="Cou_ID" ParentIdKeyName="Cou_PID"
                    TaxKeyName="Cou_Tax" Width="200" AutoPostBack="True" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                </cc1:DropDownTree>
                <cc1:DropDownTree ID="ddlOutline" runat="server" IdKeyName="Ol_ID" ParentIdKeyName="Ol_PID"
                    TaxKeyName="Ol_Tax" Width="200">
                </cc1:DropDownTree>
            </td>
        </tr>
        <tr>
            <td class="right">
                难度：
            </td>
            <td>
                <asp:CheckBoxList ID="cblDiff" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Value="1" Selected="True"> 1 </asp:ListItem>
                    <asp:ListItem Value="2" Selected="True"> 2 </asp:ListItem>
                    <asp:ListItem Value="3" Selected="True"> 3 </asp:ListItem>
                    <asp:ListItem Value="4" Selected="True"> 4 </asp:ListItem>
                    <asp:ListItem Value="5" Selected="True"> 5 </asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="right">
                其它：
            </td>
            <td>
                <asp:RadioButtonList ID="rblOther" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Value="1" Selected="True">所有 </asp:ListItem>
                    <asp:ListItem Value="2">仅正常的试题 </asp:ListItem>
                    <asp:ListItem Value="3">仅存在错误的试题 </asp:ListItem>
                    <asp:ListItem Value="4">仅用户反馈的试题 </asp:ListItem>
                </asp:RadioButtonList>
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
                &nbsp;
            </td>
            <td>
                <cc1:Button ID="btnExport" CssClass="Button" runat="server" Text="导出试题到Excel" 
                    onclick="btnExport_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
