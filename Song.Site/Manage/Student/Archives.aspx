<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Archives.aspx.cs" MasterPageFile="~/Manage/Student/Parents.Master"
    Inherits="Song.Site.Manage.Student.Archives" %>

<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <div class="searchBox">
            考试：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有任何满足条件的考试！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40px" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   +   1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="考试主题">
                <ItemStyle CssClass="left" />
                <ItemTemplate>
                    <div class="Exam_Title" title="<%# Eval("Exam_Title")%>">
                        <%# Eval("Exam_Title")%></div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="专业">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Sbj_Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="考试时间">
                <ItemStyle CssClass="center" Width="120px" />
                <ItemTemplate>
                    <span title="参加此次考试的交卷时间">
                        <%# Eval("Exr_CrtTime", "{0:yyyy-MM-dd HH:mm}")%></span></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="回顾">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                   <%-- <a href="#" onclick="OpenWin('/ExamReview.ashx?id=<%# Eval("Exr_ID")%>&stid=<%# Eval("Ac_Id")%>','<%# Eval("Exam_Title","考试回顾：《{0}》")%>',980,95);return false;">
                        回顾</a>--%>
                         <a href='/ExamReview.ashx?id=<%# Eval("Exr_ID")%>&stid=<%# Eval("Ac_Id")%>' target='_blank'>回顾</a>
                </ItemTemplate>
            </asp:TemplateField>
           <%-- <asp:TemplateField HeaderText="模拟测试">
                <ItemStyle CssClass="center" Width="100px" />
                <ItemTemplate>
                    <a href="/testscene.ashx?id=<%# Eval("Tp_ID")%>" target="_blank">进入测试</a>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="成绩">
                <ItemStyle CssClass="center" Width="60" />
                <ItemTemplate>
                    <b>
                        <%# Eval("Exr_ScoreFinal", "{0:0.0}")%></b>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
