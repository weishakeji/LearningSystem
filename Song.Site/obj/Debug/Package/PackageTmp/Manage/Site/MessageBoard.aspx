<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true" CodeBehind="MessageBoard.aspx.cs" Inherits="Song.Site.Manage.Site.MessageBoard" Title="无标题页" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <div id="header">
      <div class="toolsBar">
      <uc1:toolsbar id="ToolsBar1" runat="server" winpath="MessageBoard_Ans.aspx" gvname="GridView1"
        winwidth="800" winheight="600" iswinopen="true" ModifyButtonVisible=false AddButtonVisible=false DelButtonVisible=false />
    
      
        院系：<cc1:DropDownTree ID="ddlDepart" runat="server" Width="100" IdKeyName="dep_id" ParentIdKeyName="dep_PatId"
            TaxKeyName="dep_Tax" OnSelectedIndexChanged="ddlDepart_SelectedIndexChanged"
            AutoPostBack="True">
        </cc1:DropDownTree>&nbsp;
        专业：<asp:DropDownList ID="ddlSubject" runat="server" Width="100" 
            AutoPostBack="True" onselectedindexchanged="ddlSubject_SelectedIndexChanged">
        </asp:DropDownList>&nbsp;
       课程： <asp:DropDownList ID="ddlCourse" runat="server" Width="150" AutoPostBack="True" 
            onselectedindexchanged="ddlCourse_SelectedIndexChanged">
        </asp:DropDownList>
        </div>
        <div class="searchBox" runat="server">
            标题：<asp:TextBox ID="tbSear" runat="server" Width="80px" MaxLength="10"></asp:TextBox>
            
            <asp:Button ID="btnSear" runat="server" Width="60" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:gridview id="GridView1" runat="server" autogeneratecolumns="False" 
        selectboxkeyname="SelectBox" showselectbox="false">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
</itemtemplate>
            </asp:TemplateField>
         <asp:TemplateField HeaderText="操作">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>  
            <asp:TemplateField HeaderText="主题">
                <itemstyle cssclass="left" />
                <itemtemplate>
<%# Eval("Mb_Content")%>...
</itemtemplate>
            </asp:TemplateField>  
            <%--<asp:TemplateField HeaderText="回复/查看">
                <itemstyle cssclass="center"  width="100px" />
                <itemtemplate>
<%# Eval("Mb_ReplyNumber")%>/<%# Eval("Mb_FluxNumber")%>
</itemtemplate>
            </asp:TemplateField>  --%>    
               <asp:TemplateField HeaderText="发贴时间">
                <itemstyle cssclass="center"  width="150px" />
                <itemtemplate>
<%# Eval("Mb_CrtTime")%>
</itemtemplate>
            </asp:TemplateField>  
            <asp:TemplateField HeaderText="最后回复">
                <itemstyle cssclass="center"  width="150px" />
                <itemtemplate>
<%# Eval("Mb_IsAns","{0}")=="True" ? Eval("Mb_AnsTime") : ""%>
</itemtemplate>
            </asp:TemplateField>       
                      <asp:TemplateField HeaderText="显示">
                    <ItemTemplate>
                        <cc1:StateButton ID="sbUse" OnClick="sbShow_Click" runat="server" TrueText="显示" FalseText="隐藏"
                            State='<%# Eval("Mb_IsShow","{0}")=="True"%>'></cc1:StateButton>
                    </ItemTemplate>
                    <HeaderStyle CssClass="center noprint" />
                    <ItemStyle CssClass="center noprint" Width="60px" />
                </asp:TemplateField>
           
        </Columns>
    </cc1:gridview>

    <uc2:pager id="Pager1" runat="server" size="15" onpagechanged="BindData" />
</asp:Content>
