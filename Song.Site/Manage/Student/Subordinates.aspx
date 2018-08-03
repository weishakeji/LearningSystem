<%@ Page Language="C#" MasterPageFile="~/Manage/Student/Parents.Master" AutoEventWireup="true"
    CodeBehind="Subordinates.aspx.cs" Inherits="Song.Site.Manage.Student.Subordinates" %>

<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <div id="header">
        <div class="toolsBar">
            累计<asp:Literal ID="ltSubSum" runat="server"></asp:Literal>个朋友,
            好朋友<asp:Literal ID="ltSubCount" runat="server"></asp:Literal>个
        </div>
        <asp:Panel ID="searchBox" CssClass="searchBox" runat="server">
            账号：<asp:TextBox ID="tbAccName" runat="server" Width="80" MaxLength="10"></asp:TextBox>&nbsp;
            姓名：<asp:TextBox ID="tbName" runat="server" Width="80" MaxLength="10"></asp:TextBox>&nbsp;
            手机号：<asp:TextBox ID="tbPhone" runat="server" Width="100" MaxLength="11"></asp:TextBox>&nbsp;<asp:Button
                ID="btnSear" runat="server" Width="60" Text="查询" OnClick="btnsear_Click" />
        </asp:Panel>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有任何满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="账号">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Ac_accname", "{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="姓名/手机号">
                <ItemStyle CssClass="left" />
                <ItemTemplate>
                    <%# Eval("Ac_Name")%><span class="accname"><%# string.IsNullOrWhiteSpace(Eval("Ac_MobiTel1", "{0}")) ? Eval("Ac_accname", "{0}") : Eval("Ac_MobiTel1", "{0}")%></span>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="&yen; 资金">
                <ItemStyle CssClass="right" />
                <HeaderStyle CssClass="right" Width="100px" />
                <ItemTemplate>
                    <%# Eval("Ac_money", "{0:C}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="&spades; 点券">
                <ItemStyle CssClass="right" />
                <HeaderStyle CssClass="right" Width="100px" />
                <ItemTemplate>
                    <%# Eval("Ac_Coupon", "{0:0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="&clubs; 积分">
                <ItemStyle CssClass="right" />
                <HeaderStyle CssClass="right" Width="100px" />
                <ItemTemplate>
                    &clubs;
                    <%# Eval("Ac_Point", "{0:0}")%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
