<%@ Page Language="C#" MasterPageFile="~/Manage/Course/CourseEdit.Master" Title="课程章节"
    AutoEventWireup="true" CodeBehind="Courses_Outlines.aspx.cs" Inherits="Song.Site.Manage.Course.Courses_Outlines" %>

<%@ MasterType VirtualPath="~/Manage/Course/CourseEdit.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<%@ Register Src="../Utility/Uploader.ascx" TagName="Uploader" TagPrefix="uc1" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div loyout="row" height="30">
       <uc2:toolsBar ID="ToolsBar1" runat="server" WinPath="Outline_Edit.aspx"
           AddButtonOpen="true" GvName="GridView1" WinWidth="80"  DelButtonVisible="false"
            WinHeight="80" InputButtonVisible="false" OutputButtonVisible="false" />
        <span class="btnarea"><asp:Button ID="btnUse" CssClass="btnModfiy" runat="server" OnClick="btnModfiy_Click" Text="启用" />
        <asp:Button ID="btnFinish"  OnClick="btnFinish_Click"  CssClass="btnGeneral" runat="server" Text="完结" />
        <asp:Button ID="btnFree"  OnClick="btnFree_Click" CssClass="btnModfiy" runat="server" Text="免费" />
        <asp:Button ID="btnNofree" OnClick="btnNofree_Click"  CssClass="btnGeneral" runat="server" Text="收费" />
        </span>
       <%-- <asp:Button ID="btnAdd" runat="server" Text="新增" CssClass="btnAdd toolsBtn outlineName" />--%></div>
    <div loyout="row" overflow="auto">
        <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
            ShowSelectBox="true" IsEncrypt="False">
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
                <asp:TemplateField HeaderText="序号">
                    <ItemTemplate>
                       <%# Eval("Ol_Tax")%></span>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="课程章节">
                    <ItemTemplate>
                        <span class="treeIco">
                            <%# Eval("Tree")%></span>
                            <%# Eval("Ol_IsLive", "{0}") == "True" ? "<l title='直播'></l>" : ""%>
                        <asp:LinkButton ID="btnName" runat="server" OnClick="btnToEditor_Click"><%# Eval("Ol_Name", "{0}")%></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <span class="treeIco">
                            <%# Eval("Tree")%></span>                            
                        <asp:TextBox ID="tbName" runat="server" Width="95%" Text='<%# Eval("Ol_Name", "{0}")%>'
                            nullable="false" group="edit"></asp:TextBox>
                        <div style="display: none">
                            <asp:Label ID="lbID" runat="server" Text='<%# Eval("Ol_ID", "{0}")%>'></asp:Label>
                            <asp:Label ID="lbPID" runat="server" Text='<%# Eval("Ol_PID", "{0}")%>'></asp:Label>
                            <asp:Label ID="lbTax" runat="server" Text='<%# Eval("Ol_Tax", "{0}")%>'></asp:Label></div>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        课程章节
                    </HeaderTemplate>
                    <ItemStyle CssClass="left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="试题数">
                    <ItemTemplate>
                      <%# Eval("Ol_QuesCount", "{0}")%>
                    </ItemTemplate>                   
                    <ItemStyle CssClass="center" Width="60px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="编辑">
                    <ItemTemplate>
                        <a href="Outline_Edit.aspx?id=<%# Eval("Ol_ID", "{0}")%>&couid=<%=couid %>" type="open">编辑</a>
                        <asp:LinkButton ID="btnAddSub" runat="server" OnClick="btnAddSub_Click">新增下级</asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button ID="btnEditEnter" runat="server" CssClass="editBtn" group="edit" verify="true"
                            OnClick="btnEditEnter_Click" Text="确定" />
                        <asp:Button ID="btnEditBack" runat="server" CssClass="backBtn" OnClick="btnEditBack_Click"
                            Text="返回" />
                    </EditItemTemplate>
                    <ItemStyle CssClass="center" Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="内容">
                    <ItemTemplate>
                         <a href="Outline_Video.aspx?id=<%# Eval("Ol_ID", "{0}")%>&uid=<%# Eval("Ol_UID", "{0}")%>&couid=<%=couid %>" type="open">
                         <%# Eval("Ol_IsVideo", "{0}") == "True" ? "<b>视频</b>" : "视频"%>
                         </a>
                          <a href="Outline_Live.aspx?id=<%# Eval("Ol_ID", "{0}")%>" type="open">
                          <%# Eval("Ol_IsLive", "{0}") == "True" ? "<b>直播</b>" : "直播"%>
                          </a>
                        <a href="Outline_Accessory.aspx?id=<%# Eval("Ol_ID", "{0}")%>&uid=<%# Eval("Ol_UID", "{0}")%>" type="open">附件</a>
                    </ItemTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <ItemStyle CssClass="center" Width="120px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="免费">
                    <ItemTemplate>
                        <cc1:StateButton ID="sbFree" OnClick="sbFree_Click" runat="server" TrueText="[免费]"
                            FalseText="[收费]" State='<%# Eval("Ol_IsFree","{0}")=="True"%>'></cc1:StateButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <ItemStyle CssClass="center" Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="启用">
                    <ItemTemplate>
                        <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="[启用]"
                            FalseText="[禁用]" State='<%# Eval("Ol_IsUse","{0}")=="True"%>'></cc1:StateButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <ItemStyle CssClass="center" Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="完结">
                    <ItemTemplate>
                        <cc1:StateButton ID="sbFinish" OnClick="sbFinish_Click" runat="server" TrueText="[完结]"
                            FalseText="[未完]" State='<%# Eval("Ol_IsFinish","{0}")=="True"%>'></cc1:StateButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <ItemStyle CssClass="center" Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="删除">
                    <ItemTemplate>
                        <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                        <%-- <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>--%>
                    </ItemTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <ItemStyle CssClass="center" Width="30px" />
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
    </div>
</asp:Content>
