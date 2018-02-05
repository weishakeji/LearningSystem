<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileExplorer.aspx.cs" Inherits="Song.Site.Manage.Utility.UploadPath.FileExplorer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>文件资源管理</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="currpath">
        
    当前文件夹：<span id="ltCuurentPath"><asp:Literal ID="ltCuurentPath" runat="server"></asp:Literal></span>
    </div>
    <div>
        <asp:Repeater ID="rptDirs" runat="server">
            <HeaderTemplate>
              <div class="itemArea" style='display:<%= bool.Parse((rptDirs.Items.Count<1).ToString()) ? "none" : "block" %>' >
                <div class="bar">
                    <div class="tit">文件夹</div></div>
                    <div class="items">
            </HeaderTemplate>            
            <ItemTemplate>
                <div class="item">
                 <%= rptDirs.Items.Count %> 、 <%# Eval("Name") %></div>
            </ItemTemplate>
             <FooterTemplate>
            </div></div>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div>
        <asp:Repeater ID="rptFiles" runat="server">
            <HeaderTemplate>
            <div class="itemArea" style='display:<%= bool.Parse((rptFiles.Items.Count<1).ToString()) ? "none" : "block" %>' >
                <div class="bar">
                    <div class="tit">文件列表</div></div>
                    <div class="items" id="files">
            </HeaderTemplate>
            <ItemTemplate>
                 <div class="item" title="点击选择该文件">
                    <%# Eval("Name") %></div>
            </ItemTemplate>
            <FooterTemplate>
            </div></div>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
