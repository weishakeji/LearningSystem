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
        <span>面额：<asp:Literal ID="Lcs_Price" runat="server"></asp:Literal>元</span> <span>学习时间：<asp:Literal
            ID="Lcs_Span" runat="server" Text=""></asp:Literal><asp:Literal ID="Lcs_Unit" runat="server"
                Text=""></asp:Literal></span>
    </div>
    <div class="row">
        <span>有效期：<asp:Label ID="Lcs_LimitStart" runat="server" Format="yyyy-MM-dd"></asp:Label>
            至
            <asp:Label ID="Lcs_LimitEnd" runat="server" Format="yyyy-MM-dd"></asp:Label></span>
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
    <span style="display: none">
        <asp:Label ID="lbUrl" runat="server" Text="{0}/mobile/Learningcard.ashx?code={1}&pw={2}"></asp:Label>
        <asp:Label ID="lbUsedImg" runat="server" Text="../images/used.png"></asp:Label></span>
    <dl class="qrcode">
        <asp:Repeater ID="rtpCode" runat="server">
            <ItemTemplate>
                <dd>
                    <div class="code">
                        <%# Eval("Lc_Code")%>-<%# Eval("Lc_Pw")%></div>
                    <img src='<%# build_Qrcode(Eval("Lc_Code","{0}"),Eval("Lc_Pw","{0}"),Eval("Lc_IsUsed")) %>' />
                </dd>
            </ItemTemplate>
        </asp:Repeater>
    </dl>
    </form>
</body>
</html>
