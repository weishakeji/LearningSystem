<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="OrganSetup.aspx.cs" Inherits="Song.Site.Manage.Admin.OrganSetup" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<script type="text/javascript" src="../CoreScripts/iColorPicker.js"></script>
    <fieldset>
        <legend>基础参数</legend>
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
                    <asp:Label ID="Org_TwoDomain" runat="server" Text=""></asp:Label>.<asp:Label ID="lbDomain" runat="server" Text=""></asp:Label>:<%= WeiSha.Common.Server.Port%>
                </td>
            </tr>  
               <tr>
                <td class="right">
                   ICP网站备案：
                </td>
                <td>
                    <asp:TextBox  ID="Org_ICP" runat="server" MaxLength="200" Width="50%"></asp:TextBox>
                </td>
            </tr>    
               <tr>
                <td class="right">
                   二维码：
                </td>
                <td>
                    <asp:TextBox ID="tbQrColor" runat="server" MaxLength="10" Width="60px" CssClass="iColorPicker" SkinID="不存在皮肤主题"></asp:TextBox>（点击选择二维码前景色）
                </td>
            </tr> 
               <tr>
                <td class="right">
                   
                </td>
                <td>
                    <asp:CheckBox ID="cbQrCodeImg" runat="server" Text="在二维码中心显示Logo" />
                    <br />
                    <img src="../Images/nophoto.gif" name="imgShow" width="60" id="imgQrCenter" runat="server" /><br />
        <cc1:FileUpload ID="fuQrCenter" runat="server" fileallow="png|jpg|gif" />
                </td>
            </tr>  
            <tr>
                <td width="80" class="right" valign="top">
                二维码信息：</td>
                
                <td>
                <asp:TextBox  ID="Org_QrCodeUrl" group="para" runat="server" MaxLength="200"
                        Width="90%"></asp:TextBox><br />（默认信息是：http://<span class="domain"></span>/mobile/default.ashx）
                </td>
            <tr>
                <td width="80" class="right">
                </td>
                <td>
                    <cc1:Button ID="btnBase" runat="server" Text="保存" verify="true" group="para" OnClick="btnBase_Click" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset><legend>系统图标</legend>
     <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
            <tr>
                <td width="100" class="right">
                    Logo标志：
               </td>
               <td>
                    <img src="../Images/nophoto.jpg" name="imgShow" id="imgShow" runat="server" style="max-height:60px;" /><br />
        <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png" group="img"
            Width="150" />
                </td>
            </tr>
             <tr>
                <td class="right">
                </td>
                <td>
                    <cc1:Button ID="btnLogo" runat="server" Text="确定" group="img" verify="true" OnClick="btnLogo_Click" />
                </td>
            </tr>
            </table>    
    </fieldset>
    <fieldset><legend>学习相关</legend>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
    
             <tr>
                <td class="right" width="100"  >
                    登录注册:
                </td>
                <td>
                    <p><asp:CheckBox ID="cbIsRegTeacher" runat="server" Text="禁止教师注册" />
                    <asp:CheckBox ID="cbIsRegStudent" runat="server" Text="禁止学生注册" />
                    <asp:CheckBox ID="cbIsRegSms" runat="server" Text="注册时采用短信验证" />
                    </p>
                    <p><asp:CheckBox ID="cbIsVerifyTeahcer" runat="server" Text="启用教师注册审核" />
                    <asp:CheckBox ID="cbIsVerifyStudent" runat="server" Text="启用学生注册审核" />
                    
                    </p>
                    <p><asp:CheckBox ID="cbIsLoginForPw" runat="server" Text="启用账号密码登录" />
                    <asp:CheckBox ID="cbIsLoginForSms" runat="server" Text="启用手机短信验证登录" />
                    </p>
                    <p><asp:CheckBox ID="cbIsTraningLogin" runat="server" Text="试题练习需要学员登录后才能操作" /></p>
                </td>
            </tr>
            <tr>
                <td class="right">
                    视频质量:
                </td>
                <td>
                    <asp:DropDownList ID="ddlQscale" runat="server">
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                    </asp:DropDownList> （视频上传后转码清晰度，数值越小视频越清晰，当然视频文件也越大）
                </td></tr>
                 <tr>
                <td class="right">
                    
                </td><td>
                <asp:CheckBox ID="cbIsVideoNoload" runat="server" Text="开启视频防下载" />
                </td>
                </tr>
            <tr>
                <td width="80" class="right">
                </td>
                <td>
                    <cc1:Button ID="btnLogin" runat="server" Text="保存" verify="true" group="para" OnClick="btnLogin_Click" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>SEO设置</legend>
        <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
            <tr>
                <td width="100" class="right">
                    Keywords：
                </td>
                <td>
                    <asp:TextBox ID="Org_Keywords" runat="server" MaxLength="1000" Width="98%" TextMode="MultiLine"
                        Height="40"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    Description：
                </td>
                <td>
                    <asp:TextBox ID="Org_Description" runat="server" MaxLength="1000" Width="98%" TextMode="MultiLine"
                        Height="40"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    附属代码：<br />
                    （例如流量统计代码）
                </td>
                <td>
                    <asp:TextBox ID="Org_Extracode" runat="server" MaxLength="1000" Width="98%" TextMode="MultiLine"
                        Height="100"></asp:TextBox>
                </td>
            </tr>
            <td width="80" class="right">
            </td>
            <td>
                <cc1:Button ID="btnSeo" runat="server" Text="保存" OnClick="btnSeo_Click" />
            </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
