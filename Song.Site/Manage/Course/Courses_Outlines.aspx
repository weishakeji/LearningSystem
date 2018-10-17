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
                            <%# Eval("Tree")%></span> <a href="Courses_Outline.aspx?couid=<%= couid %>&olid=<%# Eval("Ol_ID", "{0}")%>"
                                class="outlineName">
                                <%# Eval("Ol_Name", "{0}")%></a>
                    </ItemTemplate>
                    <HeaderTemplate>
                        课程章节
                    </HeaderTemplate>
                    <ItemStyle CssClass="left" />
                    <HeaderStyle CssClass="left" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="内容">
                    <ItemTemplate>
                        简述 视频 附件
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="120px" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="编辑">
                    <ItemTemplate>
                        编辑 新增下级
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="100px" />
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
