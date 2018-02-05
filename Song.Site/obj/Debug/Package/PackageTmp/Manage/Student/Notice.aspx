<%@ Page Language="C#" MasterPageFile="~/Manage/Student/Parents.Master" AutoEventWireup="true"
    CodeBehind="Notice.aspx.cs" Inherits="Song.Site.Manage.Student.Notice" Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" GvName="GridView1" WinWidth="640" WinHeight="480"
            IsWinOpen="true" OnDelete="DeleteEvent" />
        <div class="searchBox">
            标题：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;&nbsp;<a
                href="#" type="ClearBtn">清空</a><asp:Button ID="btnSear" runat="server" Width="100"
                    Text="查询" OnClick="btnsear_Click" />
        </div>
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
                <ItemStyle CssClass="center" Width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <ItemStyle CssClass="center" Width="50px" />
                <ItemTemplate>
                    <%# Eval("No_Id","{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="主题">
                <ItemStyle CssClass="left" />
                <ItemTemplate>
                    <a href="/notice.ashx?id=<%# Eval("No_ID")%>" target="_blank">
                        <%# Eval("No_Ttl")%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="发布时间">
                <ItemStyle CssClass="center" Width="140px" />
                <ItemTemplate>
                    <%# Eval("No_StartTime")%>
                </ItemTemplate>
            </asp:TemplateField>
           <%-- <asp:TemplateField HeaderText="显示">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <cc1:StateButton ID="sbShow" OnClick="sbShow_Click" runat="server" TrueText="显示"
                        FalseText="隐藏" State='<%# Eval("No_IsShow","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
            </asp:TemplateField>--%>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
