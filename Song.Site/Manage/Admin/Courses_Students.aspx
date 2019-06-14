<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Courses_Students.aspx.cs" Inherits="Song.Site.Manage.Admin.Courses_Students"
    Title="当前课程的学员" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header"> 姓名： <asp:TextBox ID="tbName" Width="100px" runat="server"></asp:TextBox> 手机号: <asp:TextBox ID="tbMobi" Width="100px" runat="server"></asp:TextBox> 
   <asp:Button ID="btnSearch" runat="server" Text="查询" onclick="btnSearch_Click" />
   </div>
 <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"  PrimaryKey="couid"
        ShowSelectBox="True" ShowFooter="false"  IsEncrypt="False">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                      <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                     <%--<%# Eval("Cou_Tax")%>--%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="40px" />
            </asp:TemplateField>          
          
            <asp:TemplateField HeaderText="名称">
                <ItemTemplate>
                  <%# Eval("Ac_Name")%>
                </ItemTemplate>
                <ItemStyle CssClass="left" Width="100px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="手机">
                <ItemTemplate>
                  <%# Eval("Ac_MobiTel1")%>
                </ItemTemplate>
                <ItemStyle CssClass="left" Width="120px" />
            </asp:TemplateField>
           <asp:TemplateField HeaderText="完成度">
                <ItemTemplate>
                 <%# GetComplete(Eval("Ac_ID", "{0}"))%>% 
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
      <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
