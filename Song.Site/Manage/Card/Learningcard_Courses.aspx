<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Learningcard_Courses.aspx.cs" Inherits="Song.Site.Manage.Card.Learningcard_Courses"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="Kind" %>
<%@ Register Src="../Utility/SortSelect.ascx" TagName="SortSelect" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<script type="text/javascript">
    //此代码为UpdatePanel刷新完成后要执行的代码
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            _Event_Select();
        }
        else {
            var msg = args.get_error().message;
            if (msg.indexOf(":")) msg = msg.substring(msg.indexOf(":") + 1);
            alert(msg);
        }
    }
    function load() {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }
    $(load);
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%"  height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="60%" valign="top">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-bottom:10px;">
                            <tr>
                                <td class="right" width="60">
                                    机构：
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOrg" runat="server" Width="200" DataTextField="Org_Name"
                                        DataValueField="Org_ID" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">
                                    专业：
                                </td>
                                <td>
                                    <cc1:DropDownTree ID="ddlSubject" runat="server" IdKeyName="Sbj_ID" ParentIdKeyName="Sbj_PID"
                                        TaxKeyName="Sbj_Tax" Width="200" AutoPostBack="True" 
                                        onselectedindexchanged="ddlSubject_SelectedIndexChanged">
                                    </cc1:DropDownTree>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">
                                    课程：
                                </td>
                                <td>
                                    <asp:TextBox ID="tbCour" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">
                                </td>
                                <td>
                                    <cc1:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
                                    
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvCourses" runat="server" EnableTheming="true" Width="90%" AutoGenerateColumns="False"
                            ShowFooter="false">
                            <EditRowStyle CssClass="editRow" />
                            <EmptyDataTemplate>
                                没有满足条件的信息！
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="序号">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                                        <%--<%# Eval("Cou_Tax")%>--%>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="center" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="名称">
                                    <ItemTemplate>
                                        <%# Eval("Cou_Name")%>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="选择">
                                    <HeaderTemplate>
                                        <a href="select" couid="all">全选</a>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <a href="select" couid="<%# Eval("Cou_ID")%>" title="<%# Eval("Cou_Name")%>">选择</a>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <uc2:Pager ID="Pager1" runat="server" Size="10" OnPageChanged="BindData" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlOrg" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td valign="top">
                <dl class="courses">
                <dt>选中的课程<span class="clear">清空</span></dt>
                </dl>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
