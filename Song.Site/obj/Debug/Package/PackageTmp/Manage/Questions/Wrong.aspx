<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wrong.aspx.cs" MasterPageFile="~/Manage/ManagePage.Master"
    Inherits="Song.Site.Manage.Questions.Wrong" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="List_Edit.aspx" DelShowMsg="注：如果当前试题已经被试卷采用，则无法删除！"
            AddButtonOpen="true" GvName="GridView1" WinWidth="1000" WinHeight="600" OnDelete="DeleteEvent"
             OutputButtonVisible="true" />
        <asp:Panel ID="searchBox" CssClass="searchBox" runat="server">
            <cc1:DropDownTree ID="ddlSubject" runat="server" IdKeyName="Sbj_ID" ParentIdKeyName="Sbj_PID"
                TaxKeyName="Sbj_Tax" Width="80" AutoPostBack="True" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
            </cc1:DropDownTree>
            <cc1:DropDownTree ID="ddlCourse" runat="server" IdKeyName="Cou_ID" ParentIdKeyName="Cou_PID"
                TaxKeyName="Cou_Tax" Width="80" AutoPostBack="True" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
            </cc1:DropDownTree>
            <cc1:DropDownTree ID="ddlOutline" runat="server" IdKeyName="Ol_ID" ParentIdKeyName="Ol_PID"
                TaxKeyName="Ol_Tax" Width="80">
            </cc1:DropDownTree>
            <asp:DropDownList ID="ddlDiff" runat="server" Width="40">
                <asp:ListItem Value="-1">难度</asp:ListItem>
                <asp:ListItem Value="1">1</asp:ListItem>
                <asp:ListItem Value="2">2</asp:ListItem>
                <asp:ListItem Value="3">3</asp:ListItem>
                <asp:ListItem Value="4">4</asp:ListItem>
                <asp:ListItem Value="5">5</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="tbSear" runat="server" Width="80" MaxLength="10"></asp:TextBox>
            <asp:Button ID="btnSear" runat="server" Width="60" Text="查询" OnClick="btnsear_Click" />
        </asp:Panel>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="true">
        <EmptyDataTemplate>
            没有任何满足条件的试题！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="40px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center noprint" Width="44px" />
                <HeaderStyle CssClass="center noprint" />
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
                <ItemStyle CssClass="center" Width="150px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="难度">
                <ItemTemplate>
                    <%# Eval("Qus_Diff")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="50px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="错误">
                <ItemTemplate>
                    <span title='<%# Eval("Qus_WrongInfo")%>'>
                       详情</span>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="50px" />
            </asp:TemplateField>
           
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
