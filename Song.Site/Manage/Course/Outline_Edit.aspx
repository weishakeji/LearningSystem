<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Outline_Edit.aspx.cs" Inherits="Song.Site.Manage.Course.Outline_Edit"
    Title="章节编辑" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
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
                            &nbsp; 排序号：
                             <asp:TextBox ID="Ol_Tax"  runat="server" Width="80" group="ent" datatype='uint'
                                MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                     <tr >
                        <td class="right">
                            
                        </td>
                        <td>
                            <asp:CheckBox ID="cbIsLive" runat="server" Text="直播课" Checked="false" />
                            &nbsp;
                             开始时间：<asp:TextBox ID="tbLiveTime" runat="server" Width="150" EnableTheming="false" CssClass="TextBox Wdate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
                              &nbsp;
                              计划直播 <asp:TextBox ID="tbLiveSpan" runat="server" Width="60" MaxLength="5" Text="30"></asp:TextBox>分钟
                        </td>
                    </tr>
                    <tr>
                       <td class="right" width="80">
                            学习内容：
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
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click" group="ent" 
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
