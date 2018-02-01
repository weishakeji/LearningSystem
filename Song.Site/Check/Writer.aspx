<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Writer.aspx.cs" Inherits="Song.Site.Check.Writer" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnWriter" runat="server" Text="测试写入文件" OnClick="btnWriter_Click" />
    <asp:Label ID="lbScuess" runat="server" Text="权限设置正确！" Visible="False" 
            ForeColor="#006600"></asp:Label>
        <br />
    </div>
    <hr />
    测试系统是否拥有写入权，如果没有写入权限，则无法正常上传文档！
    <br />
    点击上述按钮，将在系统设定的上传文件夹中（默认是Upload）创建一个文本文件，如果正常创建则说明有写入权限。
    <hr />
    <asp:Panel ID="plError" runat="server" Visible="false">
        <asp:Label ID="lbError" runat="server" Text="没有写入权限！" Font-Bold="True" ForeColor="Red"></asp:Label><br />
        <asp:Literal ID="ltError" runat="server"></asp:Literal>
    </asp:Panel>
    </form>
</body>
</html>
