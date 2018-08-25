<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="RechargeSet.aspx.cs" Inherits="Song.Site.Manage.Money.RechargeSet" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="RechargeSet_Edit.aspx" DelShowMsg="注：如果当前设置下的充值码已经有消费记录，则无法删除，只能禁用。"
            AddButtonOpen="true" GvName="GridView1" WinWidth="600" WinHeight="450" OnDelete="DeleteEvent" />
        <asp:Panel ID="searchBox" CssClass="searchBox" runat="server">
            主题：<asp:TextBox ID="tbSear" runat="server" Width="80" MaxLength="10"></asp:TextBox>
            <asp:Button ID="btnSear" runat="server" Width="60" Text="查询" OnClick="btnsear_Click" />
        </asp:Panel>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="true">
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
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center noprint" Width="44px" />
                <HeaderStyle CssClass="center noprint" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="主题">
                <ItemTemplate>
                    <%# Eval("Rs_Theme", "{0}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="数量">
                <ItemTemplate>
                    <span title="已经消费">
                        <%# Eval("Rs_UsedCount")%></span>/<%# Eval("Rs_Count")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="面额">
                <ItemTemplate>
                    <%# Eval("Rs_Price", "{0}元")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="50px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="有效期">
                <ItemTemplate>
                    <%# Eval("Rs_LimitStart","{0:yyyy/MM/dd}")%>至<%# Eval("Rs_LimitEnd", "{0:yyyy/MM/dd}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="160px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用">
                <ItemTemplate>
                    <cc1:StateButton ID="sbEnable" OnClick="sbEnable_Click" runat="server" State='<%# Eval("Rs_IsEnable","{0}")=="True"%>'
                        FalseText="禁用" TrueText="使用"></cc1:StateButton>
                </ItemTemplate>
                <ItemStyle CssClass="center noprint" />
                <HeaderStyle CssClass="center noprint" Width="40px" />
            </asp:TemplateField>
             <asp:TemplateField HeaderText="详情">
                <ItemTemplate>
                    <a href="#" onclick="OpenWin('RechargeSet_Code.aspx?id=<%# Eval("Rs_ID")%>','<%# Eval("Rs_Theme", "{0}")%>',980,80,null,window.name);return false;">
                        查看</a>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="50px" />
            </asp:TemplateField>            
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
