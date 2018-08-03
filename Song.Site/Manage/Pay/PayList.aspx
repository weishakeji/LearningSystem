<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="PayList.aspx.cs" Inherits="Song.Site.Manage.Pay.PayList" Title="支付接口" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
   <div id="header"> <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="PayList_Edit.aspx" GvName="GridView1"
        WinWidth="640" WinHeight="480" OnDelete="DeleteEvent" />
   </div>
    
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        SelectBoxKeyName="SelectBox" ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   + 1 %>
</itemtemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="操作">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="移动">
                <itemstyle cssclass="center"/>
                <headerstyle width="60px" />
                <itemtemplate>
<asp:LinkButton id="lbUp" onclick="lbUp_Click" runat="server"  Enabled='<%# Container.DataItemIndex!=0 %>'>上移</asp:LinkButton> 
<asp:LinkButton id="lbDown" onclick="lbDown_Click" runat="server"  Enabled='<%# Container.DataItemIndex+1< ((Song.Entities.PayInterface[])GridView1.DataSource).Length %>'>下移</asp:LinkButton>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="接口名称">
                <itemstyle  cssclass="center"/>
                <itemtemplate>
                <%# Eval("Pai_Name", "{0}")%>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="类型">
                <itemstyle  cssclass="center" width="120px"/>
                <itemtemplate>
                <%# Eval("Pai_Pattern", "{0}")%>

</itemtemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="设备">
                <itemstyle  cssclass="center"/>
                <itemtemplate>
                <%# Eval("Pai_Platform", "{0}")=="web" ? "电脑" : ""%>
                <%# Eval("Pai_Platform", "{0}")=="mobi" ? "手机" : ""%>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="合作ID">
                <itemstyle  cssclass="center"/>
                <itemtemplate>
            <%# Eval("Pai_ParterID")%>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="费率">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
            <%# Eval("Pai_Feerate", "{0:0.00} %")%>

</itemtemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="状态">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbEnable" onclick="sbEnable_Click" runat="server" TrueText="使用" FalseText="禁用" State='<%# Eval("Pai_IsEnable","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>

</asp:Content>
