<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Learningcard_Code.aspx.cs" Inherits="Song.Site.Manage.Card.Learningcard_Code"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="topbar">
        <a href="output_excel.aspx?id=<%= id %>" target="_blank">导出Excel</a> <a href="output_qrcode.aspx?id=<%= id %>"
            target="_blank">导出二维码</a>
        <div class="top-right">
            <asp:TextBox ID="tbCode" runat="server"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
        </div>
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
            <asp:TemplateField HeaderText="学习卡-密码">
                <ItemTemplate>
                    <span class="code">
                        <%# Eval("Lc_Code", "{0}")%></span> -
                    <%# Eval("Lc_Pw", "{0}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="面额">
                <ItemTemplate>
                    <%# Eval("Lc_Price", "{0}元")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="50px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="有效期">
                <ItemTemplate>
                    <%# Eval("Lc_LimitStart","{0:yyyy-MM-dd}")%>至<%# Eval("Lc_LimitEnd", "{0:yyyy-MM-dd}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="160px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <%# Convert.ToBoolean(Eval("Lc_IsUsed")) ? (Eval("Lc_State", "{0}") == "-1" ? "被回滚" : (Eval("Lc_State", "{0}") == "0" ? "暂存" : "已使用")) : ""%>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="70px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用人">
                <ItemTemplate>
                    <%# !Convert.ToBoolean(Eval("Lc_IsUsed")) ? "" : Eval("Ac_AccName","{0}")%>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="回滚">
                <ItemTemplate>
                    <asp:LinkButton ID="btnGoBack" tag="btnGoBack" runat="server" code='<%# Eval("Lc_Code", "{0}")%>' pw='<%# Eval("Lc_Pw", "{0}")%>'
                        href="goBack.aspx" CommandArgument='<%# Eval("Lc_ID")%>' Visible='<%# Convert.ToBoolean(Eval("Lc_IsUsed")) && (Eval("Lc_State","{0}")!="-1")%>'>回滚</asp:LinkButton>
                </ItemTemplate>
                <ItemStyle CssClass="center" />
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
