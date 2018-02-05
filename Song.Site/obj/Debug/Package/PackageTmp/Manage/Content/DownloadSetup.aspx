<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="DownloadSetup.aspx.cs" Inherits="Song.Site.Manage.Content.DownloadSetup"
    Title="无标题页" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <fieldset>
        <legend>下载信息二维码</legend>图片宽高：
        <asp:TextBox ID="tbQrWH" runat="server" MaxLength="3" Width="50px"></asp:TextBox>
        px
        <br />
        信息模板：<br />
        <asp:TextBox ID="tbQrTextTmp" runat="server" Height="133px" TextMode="MultiLine"
            Width="290px"></asp:TextBox>
        <br />
    </fieldset>
    <asp:Button ID="btnEnter" runat="server" Text="保 存" OnClick="btnEnter_Click" />
    <asp:Button ID="btnBuild" runat="server" Text="保存并批量生成" OnClick="btnBuild_Click" />
</asp:Content>
