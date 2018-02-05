<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="ShowPicture.aspx.cs" Inherits="Song.Site.Manage.Admin.ShowPicture"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="ShowPicture_Edit.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="600" IsWinOpen="true" OnDelete="DeleteEvent" />
        <div class="searchBox">
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            当前机构没有创建轮换图片
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate><div style="margin-left:5px">
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit> </div>
                    <br />
                    <asp:LinkButton ID="lbUp" OnClick="lbUp_Click" runat="server" Enabled='<%# Container.DataItemIndex!=0 %>'>上移</asp:LinkButton>
                    <asp:LinkButton ID="lbDown" OnClick="lbDown_Click" runat="server" Enabled='<%# Container.DataItemIndex+1< ((Song.Entities.ShowPicture[])GridView1.DataSource).Length %>'>下移</asp:LinkButton>
                    <br />
                    <cc1:StateButton ID="sbShow" OnClick="sbShow_Click" runat="server" TrueText="显示"
                        FalseText="隐藏" State='<%# Eval("Shp_IsShow","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="图片">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <img src="<%# Eval("Shp_File")%>" style="max-height: 100px;" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
