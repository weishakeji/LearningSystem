<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestArchives.aspx.cs" MasterPageFile="~/Manage/Student/Parents.Master"
    Inherits="Song.Site.Manage.Student.TestArchives" %>
<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:toolsBar ID="ToolsBar1" runat="server" ModifyButtonVisible=false AddButtonVisible=false
        AddButtonOpen="true" GvName="GridView1" OnDelete="DeleteEvent" />
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="true">
        <EmptyDataTemplate>
            没有任何满足条件的成绩信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40px" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   +   1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="试卷名称">
                <ItemStyle CssClass="left" />
                <ItemTemplate>
                    <div class="Exam_Title" title="<%# Eval("Tp_Name")%>">
                        <%# Eval("Tp_Name")%></div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="专业">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Sbj_Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="测试时间">
                <ItemStyle CssClass="center" Width="120px" />
                <ItemTemplate>
                    <span title="参加此次考试的交卷时间">
                        <%# Eval("Tr_CrtTime", "{0:yyyy-MM-dd HH:mm}")%></span></ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="回顾">
                <itemstyle cssclass="center"  Width="60px" />
                <itemtemplate>
               <%-- <a href="#" onclick="OpenWin('/testview.ashx?trid=<%# Eval("Tr_ID")%>&tpid=<%# Eval("tp_Id")%>','<%# Eval("Tp_Name","测试回顾：《{0}》")%>',980,100);return false;">回顾</a>--%>
                 <a href='/testview.ashx?trid=<%# Eval("Tr_ID")%>&tpid=<%# Eval("tp_Id")%>' target='_blank'>回顾</a>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="成绩">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Tr_Score")%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
