<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Output_Qrcode.aspx.cs"
    Inherits="Song.Site.Manage.Card.Output_Qrcode" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>学习卡：</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="theme">
        <asp:Literal ID="Lcs_Theme" runat="server"></asp:Literal>
    </div>
    <div class="price row">
        <span class="box">面额：<asp:Literal ID="Lcs_Price" runat="server"></asp:Literal>元</span> <span class="box">数量：<asp:Literal
            ID="Lcs_Count" runat="server"></asp:Literal>张</span> <span class="box">使用：<asp:Literal ID="Lsc_UsedCount"
                runat="server"></asp:Literal>张</span>
    </div>
    <div class="row">
        <span class="box">有效期：<asp:Label ID="Lcs_LimitStart" runat="server" Format="yyyy-MM-dd"></asp:Label>
            至
            <asp:Label ID="Lcs_LimitEnd" runat="server" Format="yyyy-MM-dd"></asp:Label></span>
        <span class="box">学习时间：<asp:Literal ID="Lcs_Span" runat="server" Text=""></asp:Literal><asp:Literal
            ID="Lcs_Unit" runat="server" Text=""></asp:Literal></span>
    </div>
    <div class="row">
        当前学习卡包含的课程：<input id="btnPrint" type="button" value="打印" onclick="window.print();" />
    </div>
    <asp:DataList ID="dlCourses" runat="server" RepeatColumns="2" CssClass="courses"
        RepeatDirection="Horizontal" Width="100%" CellSpacing="1">
        <ItemTemplate>
            <%# Container.ItemIndex + 1%>、<%# Eval("Cou_Name") %>
        </ItemTemplate>
    </asp:DataList>
    <div class="row">
        简述：<asp:Literal ID="Lcs_Intro" runat="server"></asp:Literal>
    </div>
    <span style="display: none">
        <asp:Label ID="lbUrl" runat="server" Text="{0}/mobile/Learningcard.ashx?code={1}&pw={2}"></asp:Label>
        <asp:Label ID="lbUsedImg" runat="server" Text="../images/used.png"></asp:Label></span>
    <div class="qrcode-area">
        <dl class="qrcode">
            <asp:Repeater ID="rtpCode" runat="server">
                <ItemTemplate>
                    <dd>
                        <div class="code">
                            <%# Eval("Lc_Code")%>-<%# Eval("Lc_Pw")%></div>
                        <img src='<%# Eval("Lc_QrcodeBase64","{0}") %>' />
                        <div class="info">
                            学习卡-<%= org.Org_Name %></div>
                    </dd>
                </ItemTemplate>
            </asp:Repeater>
        </dl>
    </div>
    </form>
</body>
</html>
