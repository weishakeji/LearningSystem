<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Organization_Admin.aspx.cs" Inherits="Song.Site.Manage.Sys.Organization_Admin"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有满足条件的员工信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   + 1 %>
</itemtemplate>
            </asp:TemplateField>
        
            <asp:TemplateField HeaderText="姓名/电话">
                <itemstyle cssclass="left" width="160px" />
                <itemtemplate>
                <%# Eval("acc_Name")%><%# Eval("Posi_Id","{0}") == superid ? "<span style=\"color:Red\" title=\"管理员\">*</span>" : ""%>
                 <span class="mobileTel" title="员工移动电话"><%# Eval("acc_MobileTel")%></span>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="工号">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>
               
<%# Eval("acc_EmpCode", "{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="岗位">
                <itemstyle cssclass="center" />
                <itemtemplate>
               
<%# Eval("Posi_Name", "{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="分厂管理员">
                <itemstyle cssclass="center" width="100px" />
                <itemtemplate>
               
<cc1:StateButton id="sbAdmin" onclick="sbAdmin_Click" runat="server" TrueText="管理员" FalseText="设置" State='<%# GetAminState(Eval("Acc_ID","{0}"))%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
         
          
        </Columns>
    </cc1:GridView>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
