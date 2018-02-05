<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="CourseHot.aspx.cs" Inherits="Song.Site.Manage.Admin.CourseHot" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <div class="searchBox">
            <cc1:DropDownTree ID="ddlSubject" runat="server" IdKeyName="Sbj_ID" ParentIdKeyName="Sbj_PID"
                TaxKeyName="Sbj_Tax" Width="120">
            </cc1:DropDownTree>
            &nbsp;<asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True" ShowFooter="false" OnRowDataBound="GridView1_RowDataBound">
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
            <asp:TemplateField HeaderText="课程名称">
                <ItemTemplate>
                    
                        <%# Eval("Cou_Name")%>
                </ItemTemplate>
                <ItemStyle CssClass="left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="学员数量">
                <ItemTemplate>
                    <%# Eval("count")%>
                </ItemTemplate>
                <ItemStyle CssClass="center"  Width="80px" />
            </asp:TemplateField>
            <%--  <asp:TemplateField HeaderText="费用">
                <ItemTemplate>
                    <%# Eval("Cou_Price")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>--%>
        </Columns>
    </cc1:GridView>
</asp:Content>
