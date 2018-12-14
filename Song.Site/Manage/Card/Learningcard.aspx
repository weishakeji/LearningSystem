<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Learningcard.aspx.cs" Inherits="Song.Site.Manage.Card.Learningcard"
    Title="学习卡" %>

<%@ MasterType VirtualPath="~/Manage/ManagePage.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Learningcard_Edit.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="600" OnDelete="DeleteEvent" />
        <asp:Panel ID="searchBox" CssClass="searchBox" runat="server">
            <asp:DropDownList ID="ddlOrg" runat="server" Width="200" DataTextField="Org_Name"
                DataValueField="Org_ID">
            </asp:DropDownList>
            <asp:TextBox ID="tbSearch" runat="server" Width="100" MaxLength="20"></asp:TextBox>
            <asp:Button ID="btnSear" runat="server" Width="40" Text="查询" OnClick="btnsear_Click" />
        </asp:Panel>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="46px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="主题">
                <ItemStyle CssClass="left" />
                <ItemTemplate>
                    <%# Eval("Lcs_Theme")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="课程">
                <ItemTemplate>
                   <%# Eval("Lcs_CoursesCount")%>个
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="数量">
                <ItemTemplate>
                    <span title="已经消费">
                        <%# Eval("Lsc_UsedCount")%></span>/<%# Eval("Lcs_Count")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="面额">
                <ItemTemplate>
                    <%# Eval("Lcs_Price", "{0}元")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="50px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="有效期">
                <ItemTemplate>
                    <%# Eval("Lcs_LimitStart","{0:yyyy/MM/dd}")%>至<%# Eval("Lcs_LimitEnd", "{0:yyyy/MM/dd}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="160px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用">
                <ItemTemplate>
                    <cc1:StateButton ID="sbEnable" OnClick="sbEnable_Click" runat="server" State='<%# Eval("Lcs_IsEnable","{0}")=="True"%>'
                        FalseText="禁用" TrueText="使用"></cc1:StateButton>
                </ItemTemplate>
                <ItemStyle CssClass="center noprint" />
                <HeaderStyle CssClass="center noprint" Width="40px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="详情">
                <ItemTemplate>
                    <a href="#" onclick="OpenWin('Learningcard_Code.aspx?id=<%# Eval("Lcs_ID")%>','<%# Eval("Lcs_Theme", "{0}")%>',980,80,null,window.name);return false;">
                        查看</a>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="50px" />
            </asp:TemplateField>           
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
