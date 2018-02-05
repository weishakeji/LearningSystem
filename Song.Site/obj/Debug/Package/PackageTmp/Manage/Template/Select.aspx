<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Select.aspx.cs" Inherits="Song.Site.Manage.Template.Select" Title="模板管理" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Panel ID="plWeb" runat="server" Visible=false>
        <div class="typeTitle">
            电脑Web模板</div>
        <div class="ListBox">
            <div class="ListBoxInner">
                <asp:Repeater ID="rtpTemplate" runat="server">
                    <ItemTemplate>
                        <div class="<%# Convert.ToBoolean(Eval("isCurrent")) ? "item itemCurrent" : "item"%>"
                         tag="<%# Eval("tag") %>" type="Web" wd="600" hg="400">
                            <img src="<%# Eval("logo")+"?s="+DateTime.Now.Ticks %>" />
                            <div class="itemName">
                                <%# Eval("name") %><%# Eval("size","（{0}）") %>
                            </div>
                            <div class="select">
                                <asp:LinkButton ID="btnSelect" OnClick="btnSelectWeb_Click" CommandArgument='<%# Eval("tag") %>'
                                    Visible='<%# !Convert.ToBoolean(Eval("isCurrent")) %>' ToolTip="点击选择该模板" runat="server">点击选择该模板</asp:LinkButton>
                                <%# Convert.ToBoolean(Eval("isCurrent")) ? "当前使用模板" : ""%></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="plMobi" runat="server" Visible=false>
        <div class="typeTitle">
            手机Wap模板</div>
        <div class="ListBox">
            <div class="ListBoxInner">
                <asp:Repeater ID="rtpTempMobi" runat="server">
                    <ItemTemplate>
                        <div class="<%# Convert.ToBoolean(Eval("isCurrent")) ? "item itemCurrent" : "item"%>"
                            tag="<%# Eval("tag") %>" type="Mobile" wd="600" hg="400">
                            <img src="<%# Eval("logo")+"?s="+DateTime.Now.Ticks %>" />
                            <div class="itemName">
                                <%# Eval("name") %><%# Eval("size","（{0}）") %>
                            </div>
                            <div class="select">
                                <asp:LinkButton ID="btnSelect" OnClick="btnSelectMobi_Click" CommandArgument='<%# Eval("tag") %>'
                                    Visible='<%# !Convert.ToBoolean(Eval("isCurrent")) %>'  ToolTip="点击选择该模板" runat="server">点击选择该模板</asp:LinkButton>
                                <%# Convert.ToBoolean(Eval("isCurrent")) ? "当前使用模板" : ""%></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
