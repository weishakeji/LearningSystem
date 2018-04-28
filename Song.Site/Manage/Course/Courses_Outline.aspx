<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Course/CourseEdit.Master"
    AutoEventWireup="true" CodeBehind="Courses_Outline.aspx.cs" Inherits="Song.Site.Manage.Course.Courses_Outline" %>

<%@ MasterType VirtualPath="~/Manage/Course/CourseEdit.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<%@ Register Src="../Utility/Uploader.ascx" TagName="Uploader" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div id="leftBox" loyout="column" width="400">
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
                    <asp:TemplateField HeaderText="课程章节">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbUp" OnClick="lbUp_Click" runat="server" Enabled='<%# Container.DataItemIndex!=0 %>'>&#8593; </asp:LinkButton>
                            <asp:LinkButton ID="lbDown" OnClick="lbDown_Click" runat="server" Enabled='<%# Container.DataItemIndex+1< ((System.Data.DataTable)GridView1.DataSource).Rows.Count %>'>&#8595;</asp:LinkButton>
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
                    <asp:TemplateField HeaderText=" ">
                        <ItemTemplate>
                            <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                            <%-- <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>--%>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" Width="30px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div loyout="column" width="10">
    </div>
    <%--右侧编辑区--%>
    <div id="rightBox" loyout="column" class="rightBox" runat="server">
        <div loyout="row" height="30">
            <div loyout="column" class="olTitle" width="100">
                章节内容编辑 <span style="display: none">
                    <asp:Label ID="lbOutlineID" runat="server" Text="0"></asp:Label></span>
            </div>
            <div loyout="column">
            </div>
            <div loyout="column" width="160">
                <cc1:Button ID="btnEnter" runat="server" Text="保存章节编辑" OnClick="btnEnter_Click" ValidationGroup="enter"
                    group="ent" verify="true" /></div>
        </div>
        <div loyout="row" overflow="auto">
            <fieldset>
                <legend>基本信息</legend>
                <table width="98%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="right" width="80">
                            章节名称：
                        </td>
                        <td>
                            <asp:TextBox ID="Ol_Name" group="ent" nullable="false" runat="server" Width="350"
                                MaxLength="200"></asp:TextBox>
                            <asp:CheckBox ID="cbIsUse" runat="server" Text="启用" state="true" Checked="true" />
                        </td>
                    </tr>
                    <tr id="trOutline" runat="server">
                        <td class="right">
                            所属章节：
                        </td>
                        <td>
                            <cc1:DropDownTree ID="ddlOutline" runat="server" IdKeyName="Ol_ID" ParentIdKeyName="Ol_PID"
                                TaxKeyName="Ol_Tax" Width="350">
                            </cc1:DropDownTree>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <WebEditor:Editor ID="Ol_Intro" runat="server" ThemeType="default" 
                                Height="150px" FilterMode="false"></WebEditor:Editor>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <asp:Panel ID="plOtherEdit" runat="server">
                <fieldset>
                    <legend>视频</legend>
                    <%--视频文件上传--%>
                    <uc1:Uploader ID="Uploader1" runat="server" UploadPath="CourseVideo" LimitCount="1" />
       
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
               
                <asp:LinkButton ID="lb" runat="server" CommandArgument='<%#Eval("As_Id") %>'
                    ForeColor="Red" OnClick="lb_VideoDelClick" OnClientClick="return confirm('是否确定要删除？')">删</asp:LinkButton>
                &nbsp;
                <br />
            </ItemTemplate>
        </asp:DataList>
        </fieldset>
        <fieldset>
            <legend>视频事件-<a href="/manage/Course/OutlineEvent_Edit.aspx?couid=<%=couid %>&uid=<%= UID %>&olid=<%= olid %>"
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
                            <a href="/manage/Course/OutlineEvent_Edit.aspx?id=<%# Eval("Oe_ID","{0}")%>&couid=<%=couid %>&uid=<%= UID %>&olid=<%= olid %>"
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
        <fieldset>
            <legend>附件</legend>
            <cc1:FileUpload ID="fuLoad" runat="server" FileAllow="zip|rar|pdf|doc|docx|xls|xlsx|ppt|pptx"
                group="up" nullable="false" Width="500px" />
            <asp:Button verify="true" group="up" ID="btn" runat="server" Text="上传" OnClick="btn_Click" />
            <br />
            <asp:DataList ID="dlAcc" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <ItemTemplate>
                    <%# Container.ItemIndex   + 1 %>
                    、
                    <asp:HyperLink ID="hl" runat="server" NavigateUrl='<%#Eval("As_FileName") %>' Target="_blank"><%#Eval("As_Name") %></asp:HyperLink>
                    &nbsp; <span class="sizeSpan">(<span class="size"><%#Eval("As_Size") %></span>)</span>&nbsp;
                    &nbsp;<asp:LinkButton ID="lb" runat="server" CommandArgument='<%#Eval("As_Id") %>'
                        ForeColor="Red" OnClick="lb_Click">删</asp:LinkButton>
                    &nbsp;
                    <br />
                </ItemTemplate>
            </asp:DataList>
        </fieldset>
        </asp:Panel>
        <asp:Panel ID="plAddShow" runat="server" Visible="false">
            <span style="color: Red">*视频、视频事件、附件需要编辑完章节基础信息后才可以编辑。</span>
        </asp:Panel>
    </div>
    </div>
    <%--<script language="javascript" src="scripts/tab.js" type="text/javascript"></script>--%>
</asp:Content>
