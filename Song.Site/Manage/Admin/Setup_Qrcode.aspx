<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Setup_Qrcode.aspx.cs" Inherits="Song.Site.Manage.Admin.Setup_Qrcode"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript" src="../CoreScripts/iColorPicker.js"></script>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
                <td width="80"  class="right">
                   颜色：
                </td>
                <td>
                    <asp:TextBox ID="tbQrColor" runat="server" MaxLength="10" Width="60px" CssClass="iColorPicker" SkinID="不存在皮肤主题"></asp:TextBox>（点击选择二维码前景色）
                </td>
            </tr> 
             <tr>
                <td class="right" valign="top">
                中心图片：
                </td>
                 <td>
                   <asp:CheckBox ID="cbQrCodeImg" runat="server" Text="在二维码中心显示" />
                    </td>
                </tr>
               <tr>
                <td class="right">
                   
                </td>
                <td>
                    
                    <img src="../Images/nophoto.gif" name="imgShow" width="60" id="imgQrCenter" runat="server" /><br />
        <cc1:FileUpload ID="fuQrCenter" runat="server" fileallow="png|jpg|gif" />
                </td>
            </tr>  
            <tr>
                <td width="80" class="right" valign="top">
                二维码信息：</td>
                
                <td>
                <asp:TextBox  ID="Org_QrCodeUrl" group="para" runat="server" MaxLength="200"
                        Width="90%"></asp:TextBox><p>（默认信息是：http://<span class="domain"></span>/mobile/default.ashx）</p>
                </td>
              
            <tr>
                <td width="80" class="right">
                </td>
                <td>
                    <cc1:Button ID="btnBase" runat="server" Text="确定" verify="true" group="para" OnClick="btn_Click" />
                </td>
            </tr>
        </table>
</asp:Content>
