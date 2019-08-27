<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Special_Article.aspx.cs" Inherits="Song.Site.Manage.Content.Special_Article"
    Title="无标题页" %>

<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <div style="float: left; width: auto">
            当前专题：<asp:Label ID="lbName" runat="server" Text="Label"></asp:Label></div>
        <div class="searchBox">
            标题：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:Button
                ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40px" />
                <itemtemplate>
<%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="审核">
                <itemstyle cssclass="center" width="50px" />
                <itemtemplate>
               
<span title="已经通过审核"><%# Eval("Art_IsVerify","{0}")=="True" ? "※" : ""%></span>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="标题">
                <itemstyle cssclass="left" />
                <itemtemplate>
                <span title="图片资讯" style="color:Blue"><%# Eval("Art_IsImg","{0}")=="True" ? "[图]" : ""%></span>
                <span title="热点资讯" style="color:Red"><%# Eval("Art_IsHot", "{0}") == "True" ? "[热]" : ""%></span>
                <span title="推荐资讯" style="color:Green"><%# Eval("Art_IsRec", "{0}") == "True" ? "[荐]" : ""%></span>
                <span title="置顶资讯" style="color:Red"><%# Eval("Art_IsTop", "{0}") == "True" ? "[顶]" : ""%></span>
<a href="News_Preview.aspx?id=<%# Eval("Art_Id")%>" target="_blank"><%# Eval("Art_Title", "{0}")%> 
</a>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="作者">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>
             <%# Eval("Art_Author", "{0}") %>  

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="创建时间">
                <itemstyle cssclass="center" width="140px" />
                <itemtemplate>
               
<%# Eval("Art_CrtTime", "{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<asp:LinkButton id="ltRemove" runat="server" __designer:wfdid="w2" ToolTip="从当前专题移除该文章" OnClick="ltRemove_Click">移除</asp:LinkButton> 
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
