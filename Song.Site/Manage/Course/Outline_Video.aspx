<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Outline_Video.aspx.cs" Inherits="Song.Site.Manage.Course.Outline_Video"
    Title="章节附件" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<%@ Register Src="../Utility/Uploader.ascx" TagName="Uploader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <fieldset>
        <legend>视频</legend>
        <%--视频文件上传--%>
        <uc1:uploader id="Uploader1" runat="server" uploadpath="CourseVideo" limitcount="1" />
        <%--(如果需要在手机上播放，必须是H.264编码的MP4)--%><br />
        <asp:DataList ID="dlVideo" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
            <ItemTemplate>
                <%# Container.ItemIndex   + 1 %>
                、
                <asp:HyperLink ID="hl" runat="server" NavigateUrl='<%#Eval("As_FileName") %>' Target="_blank"
                    CssClass="video"><%#Eval("As_Name") %> <%#Eval("As_IsOuter","{0}")=="True" ? " <span style='color:red'>(外部链接)</span>" : ""%>
                     <%#Eval("As_IsOther","{0}")=="True" ? " <span style='color:red'>(视频平台)</span>" : ""%>
                </asp:HyperLink>
                &nbsp; <span class="sizeSpan">(<span class="size"><%#Eval("As_Size") %></span>)</span>&nbsp;
                &nbsp;
                <asp:LinkButton ID="lb" runat="server" CommandArgument='<%#Eval("As_Id") %>' ForeColor="Red"
                    OnClick="lb_VideoDelClick" OnClientClick="return confirm('是否确定要删除？')">删</asp:LinkButton>
                &nbsp;
                <br />
            </ItemTemplate>
        </asp:DataList>
    </fieldset>
    <fieldset>
        <legend>视频事件-<a href="OutlineEvent_Edit.aspx?couid=<%=couid %>&uid=<%= uid %>&olid=<%= olid %>"
            btntype="openwin" title="新增视频事件">添加</a></legend>
        <cc1:GridView ID="gvEventList" runat="server" AutoGenerateColumns="False">
            <EmptyDataTemplate>
                没有满足条件的信息！
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="序号">
                    <ItemTemplate>
                        <%# Container.DataItemIndex   + 1 %>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="40px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <cc1:RowDelete ID="btnDel" OnClick="btnEventDel_Click" runat="server" CommandArgument='<%# Eval("Oe_ID")%>'>
                        </cc1:RowDelete>
                        <cc1:RowEdit ID="btnEdit" runat="server" IsJsEvent="false" btnType="openwin" ToolTip="编辑章节事件">
                        </cc1:RowEdit>
                    </ItemTemplate>
                    <HeaderStyle CssClass="center noprint" />
                    <ItemStyle CssClass="center noprint" Width="44px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="类型">
                    <ItemTemplate>
                        <%# Eval("Oe_EventType", "{0}") == "1" ? "提醒" : ""%>
                        <%# Eval("Oe_EventType", "{0}") == "2" ? "知识展示" : ""%>
                        <%# Eval("Oe_EventType", "{0}") == "3" ? "课堂提问" : ""%>
                        <%# Eval("Oe_EventType", "{0}") == "4" ? "实时反馈" : ""%>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="80px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="事件主题">
                    <ItemTemplate>
                        <a href="OutlineEvent_Edit.aspx?id=<%# Eval("Oe_ID","{0}")%>&couid=<%=couid %>&uid=<%= uid %>&olid=<%= olid %>"
                            btntype="openwin" title="编辑章节事件">
                            <%# Eval("Oe_Title","{0}")%></a>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="触发时间">
                    <ItemTemplate>
                        <%# Eval("Oe_TriggerPoint", "{0}秒")%>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="80px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="使用">
                    <ItemTemplate>
                        <cc1:StateButton ID="sbUse" OnClick="sbEventUse_Click" runat="server" TrueText="使用"
                            FalseText="禁用" State='<%# Eval("Oe_IsUse","{0}")=="True"%>'></cc1:StateButton>
                    </ItemTemplate>
                    <HeaderStyle CssClass="center noprint" />
                    <ItemStyle CssClass="center noprint" Width="60px" />
                </asp:TemplateField>
                <%-- <asp:TemplateField HeaderText="编辑">
                            <ItemTemplate>
                            <a href="OutlineEvent_Edit.aspx?id=<%# Eval("Oe_ID","{0}")%>&couid=<%=couid %>&uid=<%= UID %>&olid=<%= olid %>" btnType="openwin" title="编辑章节事件">编辑</a>
                            </ItemTemplate>
                            <ItemStyle CssClass="center"  Width="60px" />
                        </asp:TemplateField>--%>
            </Columns>
        </cc1:GridView>
    </fieldset>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
