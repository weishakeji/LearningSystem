<%@ Page Language="C#" MasterPageFile="~/Manage/Student/Parents.Master" AutoEventWireup="true"
    CodeBehind="Online.aspx.cs" Inherits="Song.Site.Manage.Student.Online" Title="我的学员" %>
<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
 <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <div id="header">
        <div class="searchBox">
            时间：<asp:TextBox ID="tbStartTime" runat="server" onfocus="WdatePicker()" Width="100" CssClass="Wdate" EnableTheming="false"></asp:TextBox>
            -
            <asp:TextBox ID="tbEndTime" runat="server" onfocus="WdatePicker()" Width="100" CssClass="Wdate" EnableTheming="false"></asp:TextBox>
            &nbsp;<asp:Button ID="btnSear" runat="server" Width="60" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="46px" />
            </asp:TemplateField>--%>
           <asp:TemplateField HeaderText="日期">
                <ItemStyle CssClass="center" Width="100px" />
                <ItemTemplate>
                    <%# Eval("Lso_LoginDate", "{0:yyyy-MM-dd}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="在线时间">
                <ItemStyle CssClass="center" Width="100px" />
                <ItemTemplate>
                    <span title="从登录到退出的时间总计"><%# Eval("Lso_OnlineTime", "{0} 分钟")%></span>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="浏览时间">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                   <span title="真正观看网页的时间"> <%# Convert.ToInt32(Eval("Lso_BrowseTime"))/60%> 分钟</span>
                   
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="来源">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Lso_Platform")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="IP">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Lso_IP")%>
                </ItemTemplate>
            </asp:TemplateField>
           <asp:TemplateField HeaderText="浏览器">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Lso_Browser")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作系统">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Lso_OS")%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
