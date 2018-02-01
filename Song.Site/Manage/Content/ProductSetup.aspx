<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="ProductSetup.aspx.cs" Inherits="Song.Site.Manage.Content.ProductSetup"
    Title="产品管理的参数设置" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <%--图片尺寸管理的设置区域--%>
    <fieldset>
        <legend>图片尺寸</legend>
        <asp:CheckBox ID="cbIsWH" runat="server" Text="是否强制约束宽高" ToolTip="无论上传文件尺寸大小，都强制为指定宽高" />（<asp:TextBox
            ID="tbCompelWd" datatype="uint" runat="server" Width="40"></asp:TextBox>
        ×
        <asp:TextBox ID="tbCompelHg" datatype="uint" runat="server" Width="40"></asp:TextBox>）
        <br />
        默认缩略图大小：
        <asp:TextBox nullable="false" ID="tbWidth" datatype="uint" runat="server" Width="40"></asp:TextBox>
        ×
        <asp:TextBox nullable="false" ID="tbHeight" datatype="uint" runat="server" Width="40"></asp:TextBox>
        单位：px(像素)<br />
    </fieldset>
    <fieldset>
        <legend>二维码</legend>图片宽高：<asp:TextBox ID="tbQrWH" runat="server" MaxLength="3" Width="50px"></asp:TextBox>
        px
        <br />
        信息模板：<br />
        <asp:TextBox ID="tbQrTextTmp" runat="server" Height="133px" TextMode="MultiLine"
            Width="290px"></asp:TextBox>
    </fieldset>
    <asp:Button ID="btnEnter" runat="server" Text="保 存" OnClick="btnEnter_Click" />
    <asp:Button ID="btnBuild" runat="server" Text="保存并批量生成" OnClick="btnBuild_Click" />
</asp:Content>
