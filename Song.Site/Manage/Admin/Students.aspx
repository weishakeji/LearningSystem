<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Students.aspx.cs" Inherits="Song.Site.Manage.Admin.Students" Title="学员" %>

<%@ MasterType VirtualPath="~/Manage/ManagePage.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Students_Edit.aspx" GvName="GridView1"
            WinWidth="850" WinHeight="600" OnDelete="DeleteEvent" InputButtonVisible="true"
            OutputButtonVisible="true" />
        <asp:Panel ID="searchBox" CssClass="searchBox" runat="server">
            <asp:DropDownList ID="ddlSort" runat="server" Width="80" DataTextField="Sts_Name"
                DataValueField="Sts_ID">
            </asp:DropDownList>
            账号：<asp:TextBox ID="tbAccName" runat="server" Width="60" MaxLength="10"></asp:TextBox>
            姓名：<asp:TextBox ID="tbName" runat="server" Width="50" MaxLength="10"></asp:TextBox>
            手机号：<asp:TextBox ID="tbPhone" runat="server" Width="80" MaxLength="11"></asp:TextBox><asp:Button
                ID="btnSear" runat="server" Width="40" Text="查询" OnClick="btnsear_Click" />
        </asp:Panel>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的学生信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="46px" />
            </asp:TemplateField>
            <%-- <asp:TemplateField HeaderText="ID">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <%# Eval("Ac_id","{0}")%>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="姓名/手机号">
                <ItemStyle CssClass="left" />
                <ItemTemplate>
                    <%# Eval("Ac_Name")%><span class="accname"><%# string.IsNullOrWhiteSpace(Eval("Ac_MobiTel1", "{0}")) ? Eval("Ac_MobiTel2", "{0}") : Eval("Ac_MobiTel1", "{0}")%></span>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="账号">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Ac_accname", "{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="&yen; 资金">
                <ItemStyle CssClass="right" />
                <HeaderStyle CssClass="right" />
                <ItemTemplate>
                    <%# Eval("Ac_money", "{0:C}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="&spades; 卡券">
                <ItemStyle CssClass="right" />
                <HeaderStyle CssClass="right" />
                <ItemTemplate>
                    <a href="#" onclick="OpenWin('Students_Coupon.aspx?id=<%# Eval("Ac_id") %>','卡券充扣',400,300);return false;">
                        &spades;
                        <%# Eval("Ac_Coupon", "{0:0}")%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="&clubs; 积分">
                <ItemStyle CssClass="right" />
                <HeaderStyle CssClass="right" />
                <ItemTemplate>
                    <a href="#" onclick="OpenWin('Students_Point.aspx?id=<%# Eval("Ac_id") %>','积分充扣',400,300);return false;">
                        &clubs;
                        <%# Eval("Ac_Point", "{0:0}")%></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="组">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("sts_name", "{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <%-- <asp:TemplateField HeaderText="最后登录">
                <ItemStyle CssClass="center" Width="160" />
                <ItemTemplate>
                    <%# Convert.ToDateTime(Eval("Ac_LastTime"))>DateTime.Now.AddYears(-100) ? Eval("Ac_LastTime") : "" %>
                    
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="启用/审核/密码/课程">
                <ItemStyle CssClass="center" Width="150px" />
                <ItemTemplate>
                    <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="启用" FalseText="禁用"
                        State='<%# Eval("Ac_IsUse","{0}")=="True"%>'></cc1:StateButton>/<cc1:StateButton
                            ID="sbPass" OnClick="sbPass_Click" runat="server" TrueText="通过" FalseText="未审"
                            State='<%# Eval("Ac_IsPass","{0}")=="True"%>'></cc1:StateButton>/
                            <a href="#" onclick="OpenWin('Student_Password.aspx?id=<%# Eval("Ac_id") %>','重置密码',400,300);return false;">密码</a>
                            / <a href="#" onclick="OpenWin('Students_Courses.aspx?id=<%# Eval("Ac_id") %>','<%# Eval("Ac_Name","“{0}”的课程")%>',1000,80);return false;">课程</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
 
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
