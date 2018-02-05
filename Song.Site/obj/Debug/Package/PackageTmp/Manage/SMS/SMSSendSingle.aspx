<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="SMSSendSingle.aspx.cs" Inherits="Song.Site.Manage.SMS.SMSSendSingle" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <br />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="120" class="right">
                    姓名：</td>
                <td>
                    <asp:Label ID="lbName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="120" class="right">
                    电话：</td>
                <td>
                    <asp:Label ID="lbMobile" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="120" class="right">
                    单位：</td>
                <td>
                    <asp:Label ID="lbCompany" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="right" valign="top">
                    短信内容：
                </td>
                <td>
                    <asp:TextBox ID="tbContext" runat="server" Height="150px" MaxLength="255" TextMode="MultiLine"  nullable="false" 
                        Width="80%"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="right">
                    发送时间：
                </td>
                <td><asp:TextBox ID="tbSendTime" runat="server" onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd HH:mm:ss'})" ></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td></td><td>&nbsp;</td>
            </tr>
            <tr>
                <td class="right">
                </td>
                <td>
                    <asp:Button ID="btnEnter" runat="server" Text="发送" verify="true" OnClick="btnEnter_Click"
                        ValidationGroup="enter" />
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="保存到草稿箱" verify="true" /></td>
            </tr>
            <tr>
            <td>&nbsp;</td><td><asp:Label ID="lblSend" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
        </table>
    
</asp:Content>
