<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Courses.aspx.cs" Inherits="Song.Site.Manage.Admin.Courses" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<% this.Title = "课程管理"; %>
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" GvName="GridView1" WinWidth="980" WinHeight="90"
            OnDelete="DeleteEvent" DelShowMsg="" />       
        <div class="searchBox">
            <cc1:DropDownTree ID="ddlSubject" runat="server" IdKeyName="Sbj_ID" ParentIdKeyName="Sbj_PID"
                TaxKeyName="Sbj_Tax" Width="120">
            </cc1:DropDownTree>
            &nbsp;名称：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:Button
                ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True" ShowFooter="false" OnRowDataBound="GridView1_RowDataBound">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                      <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                     <%--<%# Eval("Cou_Tax")%>--%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="40px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作" Visible="false">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server" IsJsEvent="false"></cc1:RowEdit>
                </ItemTemplate>
                <HeaderStyle CssClass="center noprint" />
                <ItemStyle CssClass="center noprint" Width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="移动">
                <ItemStyle CssClass="center" />
                <HeaderStyle Width="40px" />
                <ItemTemplate>
                    <asp:LinkButton ID="lbUp" OnClick="lbUp_Click" runat="server">&#8593; </asp:LinkButton>
                    <asp:LinkButton ID="lbDown" OnClick="lbDown_Click" runat="server">&#8595;</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="名称">
                <ItemTemplate>
                    <%--<span class="treeIco">
                        <%# Eval("Tree")%></span><span title="点击进行编辑"><a href="../Course/Courses_Edit.aspx?couid=<%# Eval("Cou_ID", "{0}")%>"
                            target="_blank" class="courname">
                            <%# Eval("Cou_Name")%></a> </span>--%> 
                             <a href="#" onclick="OpenWin('../Course/Courses_Edit.aspx?couid=<%# Eval("Cou_ID", "{0}")%>','《<%# Eval("Cou_Name")%>》',100,100);return false;">
                              <%# Eval("Cou_Name")%></a>
                </ItemTemplate>
                <ItemStyle CssClass="left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所属专业">
                <ItemTemplate>
                    <%# Eval("Sbj_Name")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
            <%--  <asp:TemplateField HeaderText="费用">
                <ItemTemplate>
                    <%# Eval("Cou_Price")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="教师">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlTeacher" Width="80" runat="server" onchange="if(!confirm('您是否确定要更改当前课程的授课教师？')){$('form')[0].reset();return false;};"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlTeacher_OnSelectedIndexChanged">
                    </asp:DropDownList>
                    <span style="display: none">
                        <asp:Label ID="lbThID" runat="server" Text='<%# Eval("Th_ID", "{0}")%>'></asp:Label></span>
                </ItemTemplate>
                <EditItemTemplate>
                    编辑
                </EditItemTemplate>
                <ItemStyle CssClass="center" Width="90px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="数据/状态">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lkbClear" OnClientClick="return confirm('是否确定清空当前课程?\n1、清空后无法恢复。\n2、当前课程下试题、考试等所有内容将清空。')"
                        CommandArgument='<%# Eval("Cou_ID", "{0}")%>' OnClick="lkbClear_Click">清空</asp:LinkButton>/<cc1:StateButton
                            ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="使用" FalseText="禁用"
                            State='<%# Eval("Cou_IsUse","{0}")=="True"%>'></cc1:StateButton>/<cc1:StateButton ID="sbRec"
                            OnClick="sbRec_Click" runat="server" TrueText="推荐" FalseText="推荐"
                        State='<%# Eval("Cou_IsRec","{0}")=="False"%>'></cc1:StateButton>
                </ItemTemplate>
                <HeaderStyle CssClass="center noprint" />
                <ItemStyle CssClass="center noprint" Width="120px" />
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
      <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
