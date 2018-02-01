<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPaper.aspx.cs" MasterPageFile="~/Manage/ManagePage.Master"
    Inherits="Song.Site.Manage.Exam.TestPaper" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="TestPaper_Edit.aspx" DelShowMsg="注：如果当前试卷已经参与过考试，则无法删除！"
            AddButtonOpen="true" GvName="GridView1" WinWidth="980" WinHeight="90" OnDelete="DeleteEvent" />
        <div class="searchBox">
            <cc1:DropDownTree ID="ddlTree" runat="server" IdKeyName="Sbj_ID" ParentIdKeyName="Sbj_PID"
                TaxKeyName="Sbj_Tax" Width="120" AutoPostBack="True" 
                onselectedindexchanged="ddlTree_SelectedIndexChanged">
            </cc1:DropDownTree>
            &nbsp;
            <cc1:DropDownTree ID="ddlCourse" runat="server" IdKeyName="Cou_ID" ParentIdKeyName="Cou_PID"
                TaxKeyName="Cou_Tax" Width="120" AutoPostBack="True" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
            </cc1:DropDownTree>
            &nbsp; 名称：
            <asp:TextBox ID="tbSear" runat="server" Width="100" MaxLength="10"></asp:TextBox>
            <asp:Button ID="btnSear" runat="server" Width="60" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="true">
        <EmptyDataTemplate>
            没有任何满足条件的试卷！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40px" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   +   1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="试卷">
                <ItemStyle CssClass="left" />
                <ItemTemplate>
                    <div class="TpName" title="<%# Eval("Tp_Name")%>">
                        <%# Eval("Tp_Name")%></div>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="类型">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    
                        <%# Eval("Tp_Type","{0}")=="1" ? "统一试题" : "随机试题"%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="分数">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <%# Eval("Tp_PassScore", "<span title='及格分'>{0}</span>")%>/<%# Eval("Tp_Total", "{0}")%></ItemTemplate>
            </asp:TemplateField>
           
            <asp:TemplateField HeaderText="时长">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <%# Eval("Tp_Span", "{0} 分钟")%></ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="难度">
                <ItemStyle CssClass="center" Width="40px" />
                <ItemTemplate>
                    <%# Eval("Tp_Diff")%>-<%# Eval("Tp_Diff2")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="成绩">
                <ItemStyle CssClass="center" Width="50px" />
                <ItemTemplate>
                     <a href="#" onclick="OpenWin('TestPaper_Statistics.aspx?id=<%# Eval("Tp_id") %>','查看考试成绩',980,80);return false;">查看</a>
                </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:TemplateField HeaderText="类型">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <%# Eval("Tp_Type", "{0}")=="1" ? "固定试题" : "随机试卷"%></ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="使用">
                <ItemStyle CssClass="center" Width="50px" />
                <ItemTemplate>
                    <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="使用" FalseText="禁用"
                        State='<%# Eval("Tp_IsUse","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
