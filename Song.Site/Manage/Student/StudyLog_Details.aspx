<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="StudyLog_Details.aspx.cs" Inherits="Song.Site.Manage.Student.StudyLog_Details"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   +  1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="章节">
                <ItemStyle CssClass="left" />
                <ItemTemplate>
                    <%# Eval("Tree")%></span>
                    <a href='/CourseStudy.ashx?couid=<%=couid%>&olid=<%# Eval("Ol_ID", "{0}")%>' target="_blank"> <%# Eval("Ol_IsVideo", "{0}") == "True" ? Eval("Ol_Name","<b>{0}</b>") : Eval("Ol_Name")%></a>
                    
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="视频">
                <ItemStyle CssClass="center" Width="40px" />
                <ItemTemplate>
                    <%# Eval("Ol_IsVideo", "{0}")=="True" ? "" : "无"%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="学习时间">
                <ItemStyle CssClass="center" Width="150px" />
                <ItemTemplate>
                    <%# Eval("playTime","{0}")=="0" ? "" : Eval("LastTime", "{0:yyyy-MM-dd HH:mm:ss}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="进度/视频时长">
                <ItemStyle CssClass="center" Width="120px" />
                <ItemTemplate>
                    <%# CaleTotalTime(Eval("playTime", "{0}"),"{0} /")%>
                     <%# CaleTotalTime(Eval("totalTime", "{0}"), "{0}")%>                 
                </ItemTemplate>
            </asp:TemplateField>
           <%-- <asp:TemplateField HeaderText="视频时长">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# CaleTotalTime(Eval("totalTime", "{0}"))%>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="累计时长">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# CaleStudyTime(Eval("studyTime", "{0}"))%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="完成度">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Convert.ToDouble(Eval("complete", "{0}") == "" ? "0" : Eval("complete", "{0}")) >= 100 ? "100%" : Eval("complete", "{0:0.00}%")%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
