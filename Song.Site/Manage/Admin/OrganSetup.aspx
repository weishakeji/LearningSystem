<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="OrganSetup.aspx.cs" Inherits="Song.Site.Manage.Admin.OrganSetup"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="100" class="right">
                平台名称：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="Org_PlatformName" group="para" runat="server" MaxLength="200"
                    Width="90%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                域名：
            </td>
            <td class="domain">
                <asp:Label ID="Org_TwoDomain" runat="server" Text=""></asp:Label>.<asp:Label ID="lbDomain"
                    runat="server" Text=""></asp:Label>:<%= WeiSha.Common.Server.Port%>
            </td>
        </tr>
        <tr>
            <td width="100" class="right">
                标志：
            </td>
            <td>
                <img src="../Images/nophoto.jpg" name="imgShow" id="imgShow" runat="server" style="max-height: 60px;" /><br />
                <cc1:FileUpload ID="fuLoad" runat="server" FileAllow="jpg|bmp|gif|png" group="img"
                    Width="150" />
            </td>
        </tr>
        <tr>
            <td class="right">
                ICP备案号：
            </td>
            <td>
                <asp:TextBox ID="Org_ICP" runat="server" MaxLength="200" Width="50%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                公案备案号：
            </td>
            <td>
                <asp:TextBox ID="Org_GonganBeian" runat="server" MaxLength="200" Width="50%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right" valign="top">
                移动端：
            </td>
            <td>
                <p>
                    <asp:CheckBox ID="cbDisenableWeixin" runat="server" Text="禁止在微信中使用" /></p>
                <p>
                    <asp:CheckBox ID="cbDisenableMini" runat="server" Text="禁止在微信小程序中使用" /></p>
                <p>
                    <asp:CheckBox ID="cbDisenableMweb" runat="server" Text="禁止在手机网页中使用" /></p>
                <p>
                    <asp:CheckBox ID="cbDisenableAPP" runat="server" Text="禁止在手机APP中使用" /></p>
                <p>
                    <asp:CheckBox ID="cbIsMobileRemoveMoney" runat="server" Text="手机端隐藏关于“充值收费”等资费相关信息" /></p>
            </td>
        </tr>
        <tr>
            <td class="right" valign="top">
                电脑端：
            </td>
            <td>
             <p>
                    <asp:CheckBox ID="cbIsWebRemoveMoney" runat="server" Text="电脑端隐藏资金相关信息" /></p>
             <p>
                    <asp:CheckBox ID="cbWebForDeskapp" runat="server" Text="当前系统必须运行于桌面应用之中（如果本地未安装桌面应用，请勿轻易勾选）"  /></p>
                <p>
                    <asp:CheckBox ID="cbStudyForDeskapp" runat="server" Text="课程学习需要在桌面应用打开" />
                    &nbsp;<asp:CheckBox
                        ID="cbFreeForDeskapp" runat="server" Text="免费课程和试用章节除外" /><br />

                    （请将DesktopApp.exe文件手工上传至/Download/DesktopApp/文件夹下）
                </p>
            </td>
        </tr>
        <tr>
             <td class="right" valign="top">
                课程学习：
            </td>
            <td>
                <p>
                    <asp:CheckBox ID="cbIsSwitchPlay" runat="server" Text="禁用“视频课程学习时的切换窗体暂停播放”功能" /></p>
                <p>
                   视频学习完成度的容差： <asp:TextBox ID="tbTolerance" runat="server" MaxLength="200" Width="60px"></asp:TextBox>% 
                    （说明：假如为5%，则学习完成度大于95%时，显示100%）
                    
                </p>
            </td>
        </tr>
        <tr>
            <td width="80" class="right">
            </td>
            <td>
                <cc1:Button ID="btnBase" runat="server" Text="确定" verify="true" group="para" OnClick="btnBase_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
