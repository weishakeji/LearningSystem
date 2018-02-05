<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="EmplyeeBook.aspx.cs" Inherits="Song.Site.Manage.Common.EmplyeeBook"
    Title="无标题页" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
     <div id="header"> 
        院系：
        <cc1:DropDownTree ID="ddlDepart" runat="server" Width="150" IdKeyName="dep_id" ParentIdKeyName="dep_PatId"
            TaxKeyName="dep_Tax">
        </cc1:DropDownTree>
        姓名：
        <asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>
        &nbsp;
        <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="BindData" />
    </div>
    <div>
        <asp:Repeater runat="server" ID="rpList">
            <ItemTemplate>
                <div class="emplyeeListBox">
                    姓名：<span class="empName"><%# Eval("Acc_Name", "{0}")%></span>
                    <%# Eval("Posi_Name", "{0}")%>
                    <br />
                    院系：<%# Eval("Dep_CnName", "{0}")%><br />
                    电话：<%# Eval("Acc_Tel", "{0}")%><br />
                    手机：<%# Eval("Acc_MobileTel", "{0}")%><br />
                    邮箱：<%# Eval("Acc_Email", "{0}")%><br />
                    Q Q：<%# Eval("Acc_QQ", "{0}")%><br />
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
