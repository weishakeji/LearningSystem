<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Authorization.aspx.cs" Inherits="Song.Site.Manage.Panel.Authorization"
    Title="无标题页" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

    <script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>

    <asp:Panel ID="plNoLic" runat="server">
    <div class="tit">当前系统副本为<span style="color:Red">免费版</span>，联系电话 400-6015 615 <a href="http://shop35387540.taobao.com/"
                target="_blank">在线客服</a></div>

        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="80" class="right">
                    激活码类型：</td>
                <td>
                    <asp:RadioButtonList ID="rblActivType" runat="server" RepeatDirection="Horizontal"
                        RepeatLayout="Flow" AutoPostBack="True" OnSelectedIndexChanged="rblActivType_SelectedIndexChanged">
                       <%-- <asp:ListItem Value="1">CPU串号</asp:ListItem>
                        <asp:ListItem Value="2">硬盘串号</asp:ListItem>--%>
                        <asp:ListItem Value="3">IP</asp:ListItem>
                        <asp:ListItem Selected="True" Value="4">域名</asp:ListItem>
                         <asp:ListItem Value="5">主域名</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
            <td class="right">
                激活码：
            </td>
            <td>
                <asp:Label ID="lbActivCode" runat="server" Text=""></asp:Label>
            </td>
            </tr>
        </table>
    </asp:Panel>
    <%--已经授权--%>
    <asp:Panel ID="plYesLic" runat="server">
        <div class="tit">当前系统副本已经获得<asp:Label ID="lbVersion" runat="server" ForeColor="red" Text=""></asp:Label>授权，授权信息如下：</div>
       
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="80" class="right">
                    授权类型：</td>
                <td>
                    <asp:Label ID="lbLicType" runat="server" Text=""></asp:Label>
                </td>
                <td width="80" class="right">
                    授权主体：</td>
                <td>
                    <asp:Label ID="lbLicInfo" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="80" class="right">
                    起始时间：</td>
                <td>
                    <asp:Label ID="lbStartTime" runat="server" Text=""></asp:Label>
                </td>
                <td width="80" class="right">
                    失效时间：</td>
                <td>
                    <asp:Label ID="lbEndTime" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <div class="limitBox">
        <div class="tit">
            以下为当前版本所能承载的最大数据量</div>
        <div class="context">
            <asp:Repeater ID="rptLimit" runat="server">
                <ItemTemplate>
                    <div class="limitItem">
                        <%# Eval("Key") %>
                        :
                        <%# Convert.ToInt32(Eval("Value")) == 0 ? "不限" : Eval("Value")%>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="footer">上传授权文件：<asp:FileUpload ID="uploadfile" runat="server" />&nbsp;<a href="/license.aspx" target="_blank"> 查看授权信息详情</a> 
        </div>
        <div class="buttom">
            <asp:Label ID="lbShow" runat="server" ForeColor="Red"></asp:Label></div>
        <script type="text/javascript">
            $("input[name$=uploadfile]").change(function () {
                $("form").submit();
            });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
