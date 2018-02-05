<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="Song.Site.Manage.Template.List" Title="模板管理" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="typeTitle">
        电脑Web模板</div>
    <div class="ListBox">
        <div class="ListBoxInner">
            <asp:Repeater ID="rtpTemplate" runat="server">
                <ItemTemplate>
                    <div class="item" tag="<%# Eval("tag") %>" type="Web" wd="640" hg="480">
                        <img src="<%# Eval("logo")+"?s="+DateTime.Now.Ticks %>" />
                        <div class="itemName">
                            <%# Eval("name") %><%# Eval("size","（{0}）") %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <br />
    <hr />
    <div class="typeTitle">
        手机Wap模板</div>
    <div class="ListBox">
        <div class="ListBoxInner">
            <asp:Repeater ID="rtpTempMobi" runat="server">
                <ItemTemplate>
                    <div class="item" tag="<%# Eval("tag") %>" type="Mobile" wd="640" hg="480">
                        <img src="<%# Eval("logo")+"?s="+DateTime.Now.Ticks %>" />
                        <div class="itemName">
                            <%# Eval("name") %><%# Eval("size","（{0}）") %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
