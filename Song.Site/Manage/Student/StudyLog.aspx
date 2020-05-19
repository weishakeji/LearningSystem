<%@ Page Language="C#" MasterPageFile="~/Manage/Student/Parents.Master" AutoEventWireup="true"
    CodeBehind="StudyLog.aspx.cs" Inherits="Song.Site.Manage.Student.StudyLog" Title="我的学习记录" %>

<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" GvName="GridView1" AddButtonVisible="false"
            DelButtonVisible="false" OutputButtonVisible="true" />
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="true">
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
            <asp:TemplateField HeaderText="课程">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Cou_Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="学习时间">
                <ItemStyle CssClass="center" Width="150px" />
                <ItemTemplate>
                    <%# Eval("LastTime", "{0:yyyy-MM-dd}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="累计学习">
                <ItemStyle CssClass="center" Width="100px" />
                <ItemTemplate>
                    <%# CaleStudyTime(Eval("studyTime", "{0}"))%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="完成度">
                <ItemStyle CssClass="center" Width="100px" />
                <ItemTemplate>
                    <%# Convert.ToDouble(Eval("complete", "{0}") == "" ? "0" : Eval("complete", "{0}")) >= 95 ? "100%" : Eval("complete", "{0:0.00}%")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="详情">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <a href="#" onclick="OpenWin('StudyLog_Details.aspx?couid=<%# Eval("Cou_ID")%>',' 《<%# Eval("Cou_Name")%>》',980,80);return false;">
                        详情</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <iframe src="" id="iframeExportDetails" link="Students_Details.aspx?sts=-1" scrolling="auto" style="display:none;" height="30" width="100%"></iframe>
</asp:Content>
