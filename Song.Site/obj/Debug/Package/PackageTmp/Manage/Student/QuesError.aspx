<%@ Page Language="C#" MasterPageFile="~/Manage/Student/Parents.Master" AutoEventWireup="true"
    CodeBehind="QuesError.aspx.cs" Inherits="Song.Site.Manage.Student.QuesError"
    Title="错题回顾" %>
<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Ques_View.aspx" GvName="GridView1"
            WinWidth="850" WinHeight="600" OnDelete="DeleteEvent" AddButtonVisible="false" />
        <div class="searchBox">
            专业：<asp:DropDownList ID="ddlSubject" runat="server" Width="160px">
            </asp:DropDownList>
            题型：<asp:DropDownList ID="ddlType" runat="server">
            </asp:DropDownList>
            难度：
            <asp:DropDownList ID="ddlDiff" runat="server">
                <asp:ListItem Value="-1">--全部--</asp:ListItem>
                <asp:ListItem Value="1">1</asp:ListItem>
                <asp:ListItem Value="2">2</asp:ListItem>
                <asp:ListItem Value="3">3</asp:ListItem>
                <asp:ListItem Value="4">4</asp:ListItem>
                <asp:ListItem Value="5">5</asp:ListItem>
            </asp:DropDownList>
            &nbsp;<asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="32px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="题型">
                <ItemTemplate>
                    <%# typeStr[Convert.ToInt32(Eval("Qus_Type","{0}")) - 1]%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="题干">
                <ItemTemplate>
                    <div class="QusTitle" title="<%# Eval("Qus_Title")%>">
                        <%# Eval("Qus_Title")%></div>
                </ItemTemplate>
                <ItemStyle CssClass="left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="学科">
                <ItemTemplate>
                    <%# Eval("Sbj_Name","{0}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="100px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="难度">
                <ItemTemplate>
                    <%# Eval("Qus_Diff")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="50px" />
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
