<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DB.aspx.cs" Inherits="Song.Site.Check.DB" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnDblink" runat="server" Text="测试数据库链接" OnClick="btnDblink_Click" />
        &nbsp;
        <asp:Button ID="btnDbTable" runat="server" Text="测试数据结构是否完整" 
            onclick="btnDbTable_Click" />
    </div>
    <hr />
    <div>
        <asp:Label ID="lbShowText" runat="server" Text=""></asp:Label>
        <asp:Panel ID="plCorrectShow" runat="server" Visible="false">
            数据库链接不正确！<br />
            请查看db.config中数据库配置项。
            <br />
            <hr />
        </asp:Panel>
        <asp:Panel ID="lbComplate" runat="server" Visible="false">
            数据库链接不正确！<br />
            请查看db.config中数据库配置项。
            <br />
            <hr />
        </asp:Panel>
    </div>
    </form>
</body>
</html>
