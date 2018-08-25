<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="RechargeSet_Code.aspx.cs" Inherits="Song.Site.Manage.Money.RechargeSet_Code"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<div class="topbar">
        <a href="RechargeCode_Output.aspx?id=<%= id %>"  target="_blank">导出Excel</a> <a href="output_qrcode.aspx?id=<%= id %>" target="_blank">导出二维码</a>
        <div class="top-right">
             <asp:TextBox ID="tbCode" runat="server"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="查询" onclick="btnSearch_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有任何满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="40px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="充值码-密码">
                <ItemTemplate>
                    <span class="code"><%# Eval("Rc_Code", "{0}")%></span> - <%# Eval("Rc_Pw", "{0}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="面额">
                <ItemTemplate>
                    <%# Eval("Rc_Price", "{0}元")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="50px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="有效期">
                <ItemTemplate>
                    <%# Eval("Rc_LimitStart","{0:yyyy-MM-dd}")%>至<%# Eval("Rc_LimitEnd", "{0:yyyy-MM-dd}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="160px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="是否使用">
                <ItemTemplate>
                    <%# Convert.ToBoolean(Eval("Rc_IsUsed")) ? "已使用" : ""%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="70px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用人">
                <ItemTemplate>
                    <%# !Convert.ToBoolean(Eval("Rc_IsUsed")) ? "" : Eval("Ac_AccName","{0}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
