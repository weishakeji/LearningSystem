<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Output_Qrcode.aspx.cs" Inherits="Song.Site.Manage.Card.Output_Qrcode" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Lcs_Theme" runat="server" Text=""></asp:Label>
    </div>
    <div>
        面额：<asp:Label ID="Lcs_Price" runat="server" Text=""></asp:Label>
    </div>
    <div>
        有效期：<asp:Label ID="Lcs_LimitStart" runat="server" Format="yyyy-MM-dd"></asp:Label> 至 <asp:Label ID="Lcs_LimitEnd" runat="server" Format="yyyy-MM-dd"></asp:Label>
    </div>
    <div>
        学习时间：<asp:Label ID="Lcs_Span" runat="server" Text=""></asp:Label><asp:Label ID="Lcs_Unit" runat="server" Text=""></asp:Label>
    </div>
    <asp:DataList ID="dlCourses" runat="server" RepeatColumns="2" 
        RepeatDirection="Horizontal">
        <ItemTemplate>
           <%# Container.ItemIndex + 1%>、<%# Eval("Cou_Name") %>
        </ItemTemplate>
    </asp:DataList>
    <asp:Label ID="lbUrl" runat="server" Text="{0}/mobile/Learningcard.ashx?code={1}&pw={2}"></asp:Label>
     <dl>
         <asp:Repeater ID="rtpCode" runat="server">
         <ItemTemplate>
         <dd><%# Eval("Lc_Code")%>- <%# Eval("Lc_Pw")%>
         <br />
         <img src='data:image/JPG;base64,<%# build_Qrcode(Eval("Lc_Code","{0}"),Eval("Lc_Pw","{0}")) %>' />
         </dd>
         </ItemTemplate>
         </asp:Repeater>
     </dl>
    </form>
</body>
</html>
