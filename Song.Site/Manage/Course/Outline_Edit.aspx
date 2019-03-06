<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Outline_Edit.aspx.cs" Inherits="Song.Site.Manage.Course.Outline_Edit"
    Title="章节编辑" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="98%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="right" width="80">
                            章节名称：
                        </td>
                        <td>
                            <asp:TextBox ID="Ol_Name" group="ent" nullable="false" runat="server" Width="350"
                                MaxLength="200"></asp:TextBox>
                            <asp:CheckBox ID="cbIsUse" runat="server" Text="启用" state="true" Checked="true" />
                            <asp:CheckBox ID="cbIsFree" runat="server" Text="免费" state="true" Checked="false" />
                             <asp:CheckBox ID="cbIsFinish" runat="server" Text="完结" state="true" Checked="true" />
                        </td>
                    </tr>
                    <tr id="trOutline" runat="server">
                        <td class="right">
                            所属章节：
                        </td>
                        <td>
                            <cc1:DropDownTree ID="ddlOutline" runat="server" IdKeyName="Ol_ID" ParentIdKeyName="Ol_PID"
                                TaxKeyName="Ol_Tax" Width="350">
                            </cc1:DropDownTree>
                        </td>
                    </tr>
                    <tr>
                       <td class="right" width="80">
                            内容：
                        </td>
                        <td></td>
                        </tr>
                    <tr>
                        <td colspan="2">
                            <WebEditor:Editor ID="Ol_Intro" runat="server" ThemeType="default" 
                                Height="400px" FilterMode="false"></WebEditor:Editor>
                        </td>
                    </tr>
                </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
