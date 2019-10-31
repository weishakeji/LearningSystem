<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Outline_Live.aspx.cs" Inherits="Song.Site.Manage.Course.Outline_Live"
    Title="章节附件" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <table width="98%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="120">
               
            </td>
            <td>
             <asp:CheckBox ID="cbIsLive" runat="server" Text="当前课程章节作为直播课" Checked="false" />
            </td>
        </tr>
        <tr>
            <td class="right">
                开始时间：
            </td>
            <td>
            <asp:TextBox ID="tbLiveTime" runat="server" Width="150" EnableTheming="false" CssClass="TextBox Wdate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"></asp:TextBox>
            </td>
        </tr>
          <tr>
            <td class="right">
                直播时长：
            </td>
            <td>
           计划直播 <asp:TextBox ID="tbLiveSpan" runat="server" Width="60" MaxLength="5" Text="30"></asp:TextBox>分钟
            </td>
        </tr>
    </table>
    <hr />
     <table width="98%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="120">
               推流地址：
            </td>
            <td>
                <asp:Literal ID="ltPublish" runat="server"></asp:Literal>
           
            </td>
        </tr>
        <tr>
            <td class="right">
                HLS播放地址：
            </td>
            <td>
            <asp:Literal ID="ltPlayHls" runat="server"></asp:Literal>
            </td>
        </tr>
          <tr>
            <td class="right">
                RTMP播放地址：
            </td>
            <td>
           <asp:Literal ID="ltPlayRtmp" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    <asp:Panel ID="panelError" runat="server" Visible=false>
       <asp:Label ID="lbError" runat="server" Text="错误" ForeColor="Red"></asp:Label>
       <br />

       <span style="color:#ff0000">可能原因：<br />
       1、对接七牛云平台的密钥信息填写错误；<br />
       2、七牛云账户欠费；<br />
       请联络系统管理员，登录七牛云账户查看具体原因</span>
    </asp:Panel>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
