<%@ Page Language="C#"  MasterPageFile="~/Manage/Student/Parents.Master" AutoEventWireup="true"
 CodeBehind="MoneyDetails.aspx.cs" Inherits="Song.Site.Manage.Student.MoneyDetails" %>

<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <div id="header">
        <asp:Panel ID="searchBox" CssClass="searchBox" runat="server">
            时间：<asp:TextBox ID="tbStartTime" runat="server" Width="90" onfocus="WdatePicker()"
                CssClass="Wdate" EnableTheming="false"></asp:TextBox>
            -
            <asp:TextBox ID="tbEndTime" runat="server" Width="90" onfocus="WdatePicker()" CssClass="Wdate"
                EnableTheming="false"></asp:TextBox>
            &nbsp;<asp:DropDownList ID="ddlForm" runat="server" Width="90">
                <asp:ListItem Value="-1">--所有来源--</asp:ListItem>
                <asp:ListItem Value="2">充值码充值</asp:ListItem>
                <asp:ListItem Value="3">在线支付</asp:ListItem>
                <asp:ListItem Value="1">管理员操作</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlType" runat="server" Width="90">
                <asp:ListItem Value="-1">--所有操作--</asp:ListItem>
                <asp:ListItem Value="1">支出</asp:ListItem>
                <asp:ListItem Value="2">充值</asp:ListItem>
            </asp:DropDownList>
           
            <asp:Button ID="btnSear" runat="server" Width="60" Text="查询" OnClick="btnsear_Click" />
        </asp:Panel>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有任何满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="40px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="金额">
                <ItemTemplate>
                    <%# Eval("Ma_Type", "{0}") == "1" ? "<span class='type1'>支出</span>" : "<span class='type2'>充值</span>"%> <span> >> </span><%# Eval("Ma_Monery", "{0:0.00}元")%>
                </ItemTemplate>
                <ItemStyle CssClass="left" Width="120px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <span class="<%# Convert.ToBoolean(Eval("Ma_IsSuccess", "{0}")) ? "succ" : "faild"%>"><%# Convert.ToBoolean(Eval("Ma_IsSuccess", "{0}")) ? "成功" : "失败"%></span>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="余额">
                <ItemTemplate>
                    <%# Eval("Ma_Total", "{0:0.00}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="学员">
                <ItemTemplate>
                    <%# GetStudent(Eval("Ac_ID", "{0}"))%>
                </ItemTemplate>
                <ItemStyle CssClass="center"/>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="说明">
                <ItemTemplate>
                    <%# Eval("Ma_Source", "{0}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="时间">
                <ItemTemplate>
                    <%# Eval("Ma_CrtTime", "{0}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="160px" />
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="详情">
                <ItemTemplate>
                    <a href="#" onclick="OpenWin('../money/Details_View.aspx?id=<%# Eval("Ma_ID")%>','<%# Eval("Ma_Source", "{0}")%>',600,400);return false;">
                        查看</a>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="50px" />
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>

