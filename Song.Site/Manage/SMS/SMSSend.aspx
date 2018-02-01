<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="SMSSend.aspx.cs" Inherits="Song.Site.Manage.SMS.SMSSend" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <br />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="120" class="right">
                    通讯录分类：</td>
                <td>
                    <asp:DropDownList ID="ddlTpye" runat="server">
                    </asp:DropDownList>
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
                    <asp:Button ID="btnEnter" runat="server" Text="确定发送" verify="true" OnClick="btnEnter_Click"/>
                        <asp:Button ID="btnSave" runat="server" Text="保存到草稿箱" verify="true" OnClick="btnSave_Click"/>
                </td>
            </tr>
            <tr>
            <td>&nbsp;</td><td><asp:Label ID="lblSend" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
        </table>
    
</asp:Content>
