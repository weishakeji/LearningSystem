<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="News.aspx.cs" Inherits="Song.Site.Manage.Content.News" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Article.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="600" IsWinOpen="true" OnDelete="DeleteEvent" />
        <div id="fieldSear" class="searchBox" runat="server">
            标题：<asp:TextBox ID="tbSear" runat="server" Width="100px" MaxLength="10"></asp:TextBox>&nbsp;
            <asp:CheckBox ID="cbIsHot" runat="server" Text="热点" />
            <asp:CheckBox ID="cbIsTop" runat="server" Text="置顶" />
            <asp:CheckBox ID="cbIsRec" runat="server" Text="推荐" />
            <asp:CheckBox ID="cbIsImg" runat="server" Text="图片" />
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"  SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="操作">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
          <%--  <asp:TemplateField HeaderText="审核">
                <itemtemplate>
               
<span title="已经通过审核"><%# Eval("Art_IsVerify","{0}")=="True" ? "※" : ""%></span>
</itemtemplate>
                <itemstyle cssclass="center" width="50px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <itemtemplate>
<%# Eval("Art_Id","{0}")%>
</itemtemplate>
                <itemstyle cssclass="center" width="50px" />
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="文章标题">
                <itemtemplate>
                <span title="图片资讯" style="color:Blue"><%# Eval("Art_IsImg","{0}")=="True" ? "[图]" : ""%></span>
                <span title="热点资讯" style="color:Red"><%# Eval("Art_IsHot", "{0}") == "True" ? "[热]" : ""%></span>
                <span title="推荐资讯" style="color:Green"><%# Eval("Art_IsRec", "{0}") == "True" ? "[荐]" : ""%></span>
                <span title="置顶资讯" style="color:Red"><%# Eval("Art_IsTop", "{0}") == "True" ? "[顶]" : ""%></span>
<%--<a href="#" onclick="OpenWin('News_Preview.aspx?id=<%# Eval("Art_Id")%>','预览',80,80);return false;">--%><%# Eval("Art_Title", "{0}")%> 
<%--</a>--%>
</itemtemplate>
                <itemstyle cssclass="left" />
            </asp:TemplateField>
         <asp:TemplateField HeaderText="预览">
                <itemtemplate>
<a href='/article.ashx?id=<%# Eval("Art_Id")%>' target="_blank">预览</a>
</itemtemplate>
                <itemstyle cssclass="center" width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="显示">
                <itemtemplate>
<cc1:StateButton id="sbShow" onclick="sbShow_Click" runat="server" TrueText="显示" FalseText="隐藏" State='<%# Eval("Art_IsShow","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
                <itemstyle cssclass="center" width="60px" />
            </asp:TemplateField>
         <%--   <asp:TemplateField HeaderText="创建时间">
                <itemtemplate>
               <asp:LinkButton id="lbCrtTime" runat="server" onclick="lbCrtTime_Click"><%# Eval("Art_CrtTime", "{0}")%></asp:LinkButton>

</itemtemplate>
                <itemstyle cssclass="center" width="140px" />
            </asp:TemplateField>--%>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
