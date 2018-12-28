<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Subject.aspx.cs" Inherits="Song.Site.Manage.Sys.Subject" Title="专业管理" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Subject_Edit.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="80" OnDelete="DeleteEvent" DelShowMsg="" />
        <div class="searchBox">
            名称：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:Button
                ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True" ShowFooter="false">
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
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <HeaderStyle CssClass="center noprint" />
                <ItemStyle CssClass="center noprint" Width="44px" />
            </asp:TemplateField>
         <asp:TemplateField HeaderText="移动">
                <itemstyle cssclass="center" />
                <headerstyle width="40px" />
                <itemtemplate>
<asp:LinkButton id="lbUp" onclick="lbUp_Click" runat="server">&#8593; </asp:LinkButton> 
<asp:LinkButton id="lbDown" onclick="lbDown_Click" runat="server">&#8595;</asp:LinkButton>

</itemtemplate>
            </asp:TemplateField>
           <asp:TemplateField HeaderText="名称">
                <ItemTemplate><span class="treeIco"><%# Eval("Tree")%></span> 
                    <span title="<%# Eval("Sbj_Intro", "{0}")%>">
                        <%# Eval("Sbj_Name")%></span>
                </ItemTemplate>
                <ItemStyle CssClass="left"  />
            </asp:TemplateField>
            <%-- <asp:TemplateField HeaderText="院系">
                <ItemTemplate>
                    
                        <%# Eval("Dep_CnName")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="150px" />
            </asp:TemplateField>--%>
            
           
            <asp:TemplateField HeaderText="清空">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lkbClear" OnClientClick="return confirm('是否确定清空?\n1、清空后无法恢复\n2、如果该试题已经被考试采用，将会出错。')"
                        CommandArgument='<%# Eval("Sbj_ID", "{0}")%>' OnClick="lkbClear_Click">清空</asp:LinkButton>
                </ItemTemplate>
                <HeaderStyle CssClass="center noprint" />
                <ItemStyle CssClass="center noprint" Width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="使用" FalseText="禁用"
                        State='<%# Eval("Sbj_IsUse","{0}")=="True"%>'></cc1:StateButton> /
                        <cc1:StateButton ID="sbRec" OnClick="sbRec_Click" runat="server" TrueText="推荐" FalseText="已荐"
                        State='<%# Eval("Sbj_IsRec","{0}")=="False"%>'></cc1:StateButton>
                </ItemTemplate>
                <HeaderStyle CssClass="center noprint" />
                <ItemStyle CssClass="center noprint" Width="100px" />
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
   <%-- <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />--%>
</asp:Content>
