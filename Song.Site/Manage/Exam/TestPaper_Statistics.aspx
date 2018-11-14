<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="TestPaper_Statistics.aspx.cs" Inherits="Song.Site.Manage.Exam.TestPaper_Statistics"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="topheader">
        试卷名称：《<asp:Label ID="lbPagerName" runat="server" Text=""></asp:Label>》
    </div>
    <div class="infoBox">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td width="10%" class="right">
                    分值：
                </td>
                <td>
                    <asp:Label ID="lbPassnum" runat="server" Text=""></asp:Label>分及格 / 满分：<asp:Label
                        ID="lbScore" runat="server" Text=""></asp:Label>分
                </td>
                <td width="10%" class="right">
                    及格率：
                </td>
                <td>
                    <asp:Label ID="lbPassPer" runat="server" Text="0"></asp:Label>%
                </td>
                <td width="10%" class="right">
                    平均分：
                </td>
                <td>
                    <asp:Label ID="lbAvg" runat="server" Text="0"></asp:Label>分
                </td>
            </tr>
            <tr>
                <td class="right">
                    参考人次：
                </td>
                <td>
                    <asp:Label ID="lbPersontime" runat="server" Text=""></asp:Label>
                    人次
                </td>
                <td class="right">
                    最高分：
                </td>
                <td>
                    <asp:Label ID="lbHeight" runat="server" Text="0"></asp:Label>分 &nbsp;<asp:Label ID="lbHeightStudent"
                        runat="server" Text=""></asp:Label>
                </td>
                <td class="right">
                    最低分：
                </td>
                <td>
                    <asp:Label ID="lbLower" runat="server" Text="0"></asp:Label>分 &nbsp;<asp:Label ID="lbLowerStudent"
                        runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有任何满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="删除">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <asp:LinkButton ID="btnDel" runat="server" CommandArgument=' <%# Eval("Tr_ID", "{0}")%>'
                        OnClientClick="return confirm('是否确定删除该学员测试成绩？');" OnClick="btnDel_Click">删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40px" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   +   1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="学员">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Ac_Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <%-- <asp:TemplateField HeaderText="手机">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Ac_PhoneMobi")%>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="测试时间">
                <ItemStyle CssClass="center" Width="160px" />
                <ItemTemplate>
                    <%# Eval("Tr_CrtTime", "{0}")%></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="分数">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Eval("Tr_Score","{0}")%></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="详情">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <a href="/testview.ashx?trid=<%# Eval("Tr_ID","{0}")%>&tpid=<%# Eval("Tp_Id","{0}")%>" title=" 学员“<%# Eval("Ac_Name")%>”在<%# Eval("Tp_Name","《{0}》")%>的测试成绩" type="review">查看</a></ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="btnClose" runat="server" />
</asp:Content>
