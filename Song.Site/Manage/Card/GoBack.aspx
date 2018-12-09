<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoBack.aspx.cs" Inherits="Song.Site.Manage.Card.GoBack" %>

<!DOCTYPE html">
<html style="overflow: hidden;">
<head runat="server">
    <title></title>
</head>
<body>
    <script type="text/javascript" src="/Manage/CoreScripts/AutoLoyout.js"></script>
    <form id="form1" runat="server" loyout="row">
    <div loyout="row" overflow="auto">
        <div class="context">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="right" width="80px">
                        学习卡：
                    </td>
                    <td>
                        <%= code %>
                        -
                        <%= pw %>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        时效：
                    </td>
                    <td>
                        <asp:Label ID="lbLimit" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        使用人：
                    </td>
                    <td>
                        <asp:Label ID="lbAccname" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        状态：
                    </td>
                    <td>
                        <asp:Label ID="lbState" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        使用时间：
                    </td>
                    <td>
                        <asp:Label ID="lbUsetime" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="right" valign="top">
                        关联课程：
                    </td>
                    <td>
                        <asp:Repeater ID="rptCourse" runat="server">
                        <ItemTemplate>
                        <div style="height:20px;"><%#Container.ItemIndex + 1%> 、 <%# Eval("Cou_Name") %></div>
                        </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="footer" loyout="row" height="60px">
        <div loyout="column" width="20px">
        </div>
        <div loyout="column">
            <asp:Button ID="btnGoback1" CssClass="btn green"  OnClientClick="return confirm('是否确定要回滚该学习卡？')" runat="server" Text="回滚，但保留学习记录"
                Width="98%" onclick="btnGoback1_Click" /></div>
        <div loyout="column">
            <asp:Button ID="btnGoback2" CssClass="btn blue"  
                OnClientClick="return confirm('是否确定要回滚该学习卡？')" runat="server" Text="回滚，且清除学习记录"
                Width="98%" onclick="btnGoback2_Click" /></div>
        <div loyout="column" width="100">
            <input type="submit" name="btnClose" class="btn yellow" value="关闭" style="width: 98%" /></div>
        <div loyout="column" width="10px">
        </div>
    </div>
    </form>
</body>
</html>
