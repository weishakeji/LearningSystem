<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Comments.aspx.cs" Inherits="Song.Site.Manage.Teacher.Comments" Title="教师评分" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Comments_Edit.aspx" AddButtonVisible="false" GvName="GridView1"
            WinWidth="640" WinHeight="480" OnDelete="DeleteEvent" />
           <div class="searchBox"> 教师：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:Button
                ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" /></div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的分类！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="46px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="教师">
                <ItemStyle CssClass="center"  Width="60px"  />
                <ItemTemplate>
                    <%# Eval("Th_Name")%>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="评分">
                <ItemStyle CssClass="center"  Width="40px"  />
                <ItemTemplate>
                    <%# Eval("Thc_Score","{0:0}")%>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="评价">
                <ItemStyle CssClass="left" />
                <ItemTemplate>
                    <%# Eval("Thc_Comment")%>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="时间">
                <ItemStyle CssClass="center"   Width="180px"  />
                <ItemTemplate>
                    <%# Eval("Thc_CrtTime")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="设置">
                <ItemStyle CssClass="center" Width="150px" />
                <ItemTemplate>
                    <cc1:StateButton ID="sbUse" OnClick="sbUseClick" runat="server" TrueText="使用" FalseText="禁用"
                        State='<%# Eval("Thc_IsUse","{0}")=="True"%>'></cc1:StateButton>
                        /
                        <cc1:StateButton ID="sbShow" OnClick="sbShowClick" runat="server" TrueText="显示" FalseText="隐藏"
                        State='<%# Eval("Thc_IsShow","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
      <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
