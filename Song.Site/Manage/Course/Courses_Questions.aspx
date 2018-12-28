<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Course/CourseEdit.Master"
    AutoEventWireup="true" CodeBehind="Courses_Questions.aspx.cs" Inherits="Song.Site.Manage.Course.Courses_Questions" %>

<%@ MasterType VirtualPath="~/Manage/Course/CourseEdit.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header" loyout="row" height="40">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="../Questions/List_Edit.aspx"
            DelShowMsg="注：如果当前试题已经被试卷采用，则无法删除！" AddButtonOpen="true" GvName="GridView1" WinWidth="1000"
            WinHeight="600" OnDelete="DeleteEvent" InputButtonVisible="true" OutputButtonVisible="false"
            OnOutput="Output_Click" />
        <asp:Panel ID="searchBox" CssClass="searchBox" runat="server">
            <cc1:DropDownTree ID="ddlOutline" runat="server" IdKeyName="Ol_ID" ParentIdKeyName="Ol_PID"
                TaxKeyName="Ol_Tax">
            </cc1:DropDownTree>
            <asp:DropDownList ID="ddlType" runat="server" Width="60">
                <asp:ListItem Value="-1">-题型-</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlDiff" runat="server" Width="60">
                <asp:ListItem Value="-1">难度</asp:ListItem>
                <asp:ListItem Value="1">1</asp:ListItem>
                <asp:ListItem Value="2">2</asp:ListItem>
                <asp:ListItem Value="3">3</asp:ListItem>
                <asp:ListItem Value="4">4</asp:ListItem>
                <asp:ListItem Value="5">5</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="tbSear" runat="server" Width="80" MaxLength="10"></asp:TextBox>
            <asp:Button ID="btnSear" runat="server" Width="60" Text="查询" OnClick="btnsear_Click" />
        </asp:Panel>
    </div>
    <div loyout="row" overflow="auto">
        <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
            ShowSelectBox="true">
            <EmptyDataTemplate>
                没有任何满足条件的试题！
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
                <asp:TemplateField HeaderText="题型">
                    <ItemTemplate>
                        <%# typeStr[Convert.ToInt32(Eval("Qus_Type","{0}")) - 1]%>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="60px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="题干">
                    <ItemTemplate>
                        <div class="QusTitle" title="<%# Eval("Qus_Title")%>">
                            <%# Eval("Qus_Title")%></div>
                    </ItemTemplate>
                    <ItemStyle CssClass="left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="章节">
                    <ItemTemplate>
                        <%# getOutlineName(Convert.ToInt32(Eval("Ol_ID","{0}")))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="难度">
                    <ItemTemplate>
                        <%# Eval("Qus_Diff")%>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="状态">
                    <ItemTemplate>
                        <span title='<%# Eval("Qus_ErrorInfo")%>'>
                            <%# Eval("Qus_IsError", "{0}") == "True" ? "错误" : ""%></span>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="使用">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lbUse" runat="server" OnClick="lbUse_Click">用</asp:LinkButton>
                        /<asp:LinkButton ID="lbNoUse" runat="server" OnClick="lbNoUse_Click">禁</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" State='<%# Eval("Qus_IsUse","{0}")=="True"%>'
                            FalseText="禁用" TrueText="使用"></cc1:StateButton>
                    </ItemTemplate>
                    <ItemStyle CssClass="center noprint" />
                    <HeaderStyle CssClass="center noprint" Width="50px" />
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
    </div>
    <div class="pagerBar" loyout="row" height="35">
        <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
    </div>
</asp:Content>
