<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Course/CourseEdit.Master"
    AutoEventWireup="true" CodeBehind="Courses_Outlines.aspx.cs" Inherits="Song.Site.Manage.Course.Courses_Outlines" %>

<%@ MasterType VirtualPath="~/Manage/Course/CourseEdit.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<%@ Register Src="../Utility/Uploader.ascx" TagName="Uploader" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div loyout="row" height="30">
        <asp:Button ID="btnAdd" runat="server" Text="新增" CssClass="btnAdd toolsBtn outlineName" /></div>
    <div loyout="row" overflow="auto">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
            <EmptyDataTemplate>
                <div style="text-align: center">
                    当前课程还没有添加章节</div>
            </EmptyDataTemplate>
            <EmptyDataRowStyle CssClass="center" />
            <Columns>
                <asp:TemplateField HeaderText="移动">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbUp" OnClick="lbUp_Click" runat="server" Enabled='<%# Container.DataItemIndex!=0 %>'>&#8593; </asp:LinkButton>
                        <asp:LinkButton ID="lbDown" OnClick="lbDown_Click" runat="server" Enabled='<%# Container.DataItemIndex+1< ((System.Data.DataTable)GridView1.DataSource).Rows.Count %>'>&#8595;</asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="课程章节">
                    <ItemTemplate>
                        <span class="treeIco">
                            <%# Eval("Tree")%></span>
                        <asp:LinkButton ID="btnName" runat="server" OnClick="btnToEditor_Click"><%# Eval("Ol_Name", "{0}")%></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <span class="treeIco">
                            <%# Eval("Tree")%></span>
                        <asp:TextBox ID="tbName" runat="server" Width="95%" Text='<%# Eval("Ol_Name", "{0}")%>' nullable="false" group="edit"></asp:TextBox>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        课程章节
                    </HeaderTemplate>
                    <ItemStyle CssClass="left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="试用">
                    <ItemTemplate>
                        试用
                    </ItemTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <ItemStyle CssClass="center" Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="编辑">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnToEditor_Click">编辑</asp:LinkButton>
                        新增下级
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button ID="btnEditEnter" runat="server" CssClass="editBtn" group="edit" verify="true"
                            OnClick="btnEditEnter_Click" Text="确定" />
                        <asp:Button ID="btnEditBack" runat="server" CssClass="backBtn" OnClick="btnEditBack_Click"
                            Text="返回" />
                    </EditItemTemplate>
                    <ItemStyle CssClass="center" Width="120px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="内容">
                    <ItemTemplate>
                        简述 视频 附件
                    </ItemTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <ItemStyle CssClass="center" Width="120px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="删除">
                    <ItemTemplate>
                        <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                        <%-- <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>--%>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="30px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
