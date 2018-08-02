<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Setup_Stamp.aspx.cs" Inherits="Song.Site.Manage.Admin.Setup_Stamp"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript" src="../CoreScripts/iColorPicker.js"></script>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td class="right" width="100" valign="top">
                说明：
            </td>
            <td>
                <p>
                    1、公章的用途主要在学员学习情况导出时使用；
                </p>
                <p>
                    2、考虑到公章需要加盖到学习信息之上，公章图片需要为gif或png格式的背景透明图片。</p>
            </td>
        </tr>
        <tr>
            <td class="right" valign="top">
                公章：
            </td>
            <td>
                <img src="../Images/nophoto.jpg" name="imgShow" id="imgShow" runat="server" style="max-height: 60px;" /><br />
                <cc1:FileUpload ID="fuLoad" runat="server" FileAllow="gif|png" group="img" Width="150" />
                
            </td>
        </tr>
        <tr>
            <td class="right" valign="top">
                显示位置：
            </td>
            <td><p style="display:none">
                <asp:TextBox ID="tbPosition" runat="server"></asp:TextBox>
                </p>
                <table width="100%" border="1" cellspacing="0" cellpadding="1" class="position">
                    <tr>
                        <td tag="left-top">
                        左上
                        </td>
                        <td tag="right-top">
                           右上
                        </td>
                    </tr>
                    <tr>
                        <td tag="left-bottom">
                           左下
                        </td>
                        <td tag="right-bottom">
                            右下
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                <cc1:Button ID="btnLogo" runat="server" Text="确定" group="img" verify="true" OnClick="btn_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
