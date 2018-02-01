<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="History.aspx.cs" Inherits="Song.Site.Manage.Teacher.History" Title="教师列表" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="History_Edit.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="600" OnDelete="DeleteEvent" />
     
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的教师履历！
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
            <asp:TemplateField HeaderText="主题">
                <ItemStyle CssClass="left"/>
                <ItemTemplate>
                    <%# Eval("Thh_Theme")%>
                  
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="类型">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Eval("Thh_Type", "{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
          <asp:TemplateField HeaderText="开始时间">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Eval("Thh_StartTime", "{0:yyyy-MM-dd}")%>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="结束时间">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Convert.ToDateTime(Eval("Thh_EndTime")) > DateTime.Now.AddYears(100) ? "至今" : Eval("Thh_EndTime", "{0:yyyy-MM-dd}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="单位">
                <ItemStyle CssClass="center"/>
                <ItemTemplate>
                    <%# Eval("Thh_Type", "{0}") == "学习" ? Eval("Thh_School") : Eval("Thh_Compay")%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
