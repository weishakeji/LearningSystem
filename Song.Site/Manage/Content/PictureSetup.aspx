<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="PictureSetup.aspx.cs" Inherits="Song.Site.Manage.Content.Picture.PictureSetup"
    Title="无标题页" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <fieldset runat=server id="tm" visible=false>
        <legend>基本参数</legend>最多置顶数：
        <asp:TextBox ID="tbMaxTop" runat="server" Text="10" MaxLength="3" Width="60px" nullable="false"
            datatype="uint"></asp:TextBox>
        <br />
        最多推荐数：
        <asp:TextBox ID="tbMaxRec" runat="server" Text="10" MaxLength="3" Width="60px" nullable="false"
            datatype="uint"></asp:TextBox></fieldset>
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
    <%--图片水印管理设置区域--%>
    <fieldset>
        <legend>图片水印</legend>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="220" valign="top">
                    <asp:CheckBox ID="cbIsAddWater" runat="server" Text="是否添加水印" /><br />
                    <div id="picShowBox">
                        <img src="../../Images/nophoto.gif" name="imgShow" height="200" id="imgShow" runat="server" /></div>
                    选择：
                    <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png" />
                </td>
                <td valign="top">
                    水印位置：<span id="spanLocal"></span><asp:TextBox ID="tbLocal" runat="server" Width="100px"></asp:TextBox><br />
                    <table width="150" border="1" cellspacing="0" cellpadding="0" class="place">
                        <tr>
                            <td width="50" height="50">
                                左上</td>
                            <td width="50">
                                上</td>
                            <td width="50">
                                右上</td>
                        </tr>
                        <tr>
                            <td height="50">
                                左</td>
                            <td>
                                中</td>
                            <td>
                                右</td>
                        </tr>
                        <tr>
                            <td height="50">
                                左下</td>
                            <td>
                                下</td>
                            <td>
                                右下</td>
                        </tr>
                    </table>
                    水印透明度：<asp:TextBox ID="tbOpacity" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox></td>
            </tr>
        </table>
    </fieldset>
    <br />
    <asp:Button verify="true" ID="btnEnter" runat="server" Text="确定" ValidationGroup="enter"
        OnClick="btnEnter_Click" />
</asp:Content>
