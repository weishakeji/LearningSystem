<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Order.aspx.cs" Inherits="Song.Site.Manage.Teacher.Order" Title="教师评分排行" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
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
                    <%# Container.DataItemIndex   + 1 %>
                </ItemTemplate>
            </asp:TemplateField>  
            <asp:TemplateField HeaderText="教师">
                <ItemStyle CssClass="center" Width="120px"  />
                <ItemTemplate>
                    <%# Eval("Th_Name")%>
                </ItemTemplate>
            </asp:TemplateField>   
             <asp:TemplateField HeaderText="评分">
                <ItemStyle CssClass="right" />
                <ItemTemplate>
                    <span class="star" score="<%# Eval("Thc_Score","{0:0.00}")%>" size="10"></span>
                </ItemTemplate>
            </asp:TemplateField>          
            <asp:TemplateField HeaderText="评分">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                   <%# Eval("Thc_Score","{0:0.00}")%> 分
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
