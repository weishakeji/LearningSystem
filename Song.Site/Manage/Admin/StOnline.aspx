<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="StOnline.aspx.cs" Inherits="Song.Site.Manage.Admin.StOnline"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
   
    <div id="header"><uc1:toolsBar ID="ToolsBar1" runat="server" GvName="GridView1" WinWidth="980" WinHeight="90" AddButtonVisible=false
            OnDelete="DeleteEvent" DelShowMsg="删除后无法恢复，是否确定继续删除？" /> 
        <div class="searchBox">
            时间：<asp:TextBox ID="tbStartTime" runat="server" onfocus="WdatePicker()" Width="100" CssClass="Wdate" EnableTheming="false"></asp:TextBox>
            -
            <asp:TextBox ID="tbEndTime" runat="server" onfocus="WdatePicker()" Width="100" CssClass="Wdate" EnableTheming="false"></asp:TextBox> 
            学员：<asp:TextBox ID="tbStName" runat="server" Width="80"></asp:TextBox>
            手机：<asp:TextBox ID="tbStMobi" runat="server" Width="100"></asp:TextBox>
            &nbsp;<asp:Button ID="btnSear" runat="server" Width="60" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
   
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="true">
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
            <asp:TemplateField HeaderText="学员">
                <ItemTemplate>
                    <%# Eval("Ac_Name")%>/<%# Eval("Ac_AccName")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
           <asp:TemplateField HeaderText="日期">
                <ItemStyle CssClass="center" Width="100px" />
                <ItemTemplate>
                    <%# Eval("Lso_LoginDate", "{0:yyyy-MM-dd}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="在线时间">
                <ItemStyle CssClass="center" Width="100px" />
                <ItemTemplate>
                    <span title="真正观看网页的时间"> <%# Convert.ToInt32(Eval("Lso_BrowseTime"))/60%></span>/<span title="从登录到退出的时间总计"><%# Eval("Lso_OnlineTime", "{0}分钟")%></span>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="来源">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Lso_Platform")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="IP/浏览器/操作系统">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Lso_IP")%>/<%# Eval("Lso_Browser")%>/<%# Eval("Lso_OS")%>
                </ItemTemplate>
            </asp:TemplateField>
          
        </Columns>
    </cc1:GridView>
     <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
