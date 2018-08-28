<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Output_Qrcode.aspx.cs"
    Inherits="Song.Site.Manage.Money.Output_Qrcode" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>充值卡：</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="theme">
        <asp:Literal ID="Rs_Theme" runat="server"></asp:Literal>
    </div>
    <div class="price row">
        <span>面额：<asp:Literal ID="Rs_Price" runat="server"></asp:Literal>元</span> 
        <span>数量：<asp:Literal ID="Rs_Count" runat="server"></asp:Literal>张</span> 
        <span>使用：<asp:Literal ID="Rs_UsedCount" runat="server"></asp:Literal>张</span> 
    </div>
    <div class="row">
        <span>有效期：<asp:Label ID="Rs_LimitStart" runat="server" Format="yyyy-MM-dd"></asp:Label>
            至
            <asp:Label ID="Rs_LimitEnd" runat="server" Format="yyyy-MM-dd"></asp:Label></span>
            <input id="btnPrint" type="button" value="打印" onclick="window.print();" />
    </div>
 <div class="row">
 简述：<asp:Literal ID="Rs_Intro" runat="server"></asp:Literal>
 </div>
    <span style="display: none">
        <asp:Label ID="lbUrl" runat="server" Text="{0}/mobile/recharge.ashx?code={1}&pw={2}"></asp:Label>
        <asp:Label ID="lbUsedImg" runat="server" Text="../images/used.png"></asp:Label></span>
    <div class="qrcode-area">
    <dl class="qrcode">
        <asp:Repeater ID="rtpCode" runat="server">
            <ItemTemplate>
                <dd>
                    <div class="code">
                        <%# Eval("Rc_Code")%>-<%# Eval("Rc_Pw")%></div>
                    <img src='<%# Eval("Rc_QrcodeBase64","{0}") %>' />
                    <div class="info">充值卡-<%= org.Org_Name %></div>
                </dd>
            </ItemTemplate>
        </asp:Repeater>
    </dl></div>
    </form>
</body>
</html>
