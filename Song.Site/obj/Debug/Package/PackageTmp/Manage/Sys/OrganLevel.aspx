<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="OrganLevel.aspx.cs" Inherits="Song.Site.Manage.Sys.OrganLevel" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
   <div id="header"> <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="OrganLevel_Edit.aspx" AddButtonOpen="true"
         WinWidth="600" WinHeight="400" GvName="GridView1" OnDelete="DeleteEvent"/>
         </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        SelectBoxKeyName="SelectBox" ShowSelectBox="True">
        <EmptyDataTemplate>
            还没有创建等级！
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
  
            <asp:TemplateField HeaderText="名称">
                <itemstyle cssclass="center" width="150px"  />
                <itemtemplate>
<%# Eval("Olv_Name")%>
</itemtemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="等级">
                <itemstyle cssclass="center"  width="44px" />
                <itemtemplate>
<%# Eval("Olv_Level")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="标识">
                <itemstyle cssclass="center"  width="60px" />
                <itemtemplate>
<%# Eval("Olv_Tag")%>
</itemtemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="简介">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("Olv_Intro")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="默认">
                <itemstyle cssclass="center" width="100px" />
                <itemtemplate>
<cc1:StateButton id="sbDef" onclick="sbDef_Click" runat="server" Enabled='<%# Eval("Olv_IsDefault","{0}")=="False"%>' TrueText="默认" FalseText="设置默认" State='<%# Eval("Olv_IsDefault","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbUse" onclick="sbShow_Click" runat="server" TrueText="使用" FalseText="禁用" State='<%# Eval("Olv_IsUse","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="管理">
                <itemstyle cssclass="center"  width="60px" />
                <itemtemplate>
<a href="#" onclick="OpenWin('purview.aspx?id=<%# Eval("Olv_ID")%>&type=orglevel','设置权限',800,600);return false;">权限</a>
</itemtemplate>
            </asp:TemplateField>
              
        </Columns>
    </cc1:GridView>
</asp:Content>
