<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="EmplyeeGroup.aspx.cs" Inherits="Song.Site.Manage.Common.EmplyeeGroup"
    Title="无标题页" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    
    <div>
        <asp:Repeater runat="server" ID="rpList">
            <ItemTemplate>
                <div class="emplyeeListBox">
                <img src='<%# Eval(" Acc_Photo", "{0}")%>' />
                    <div class="empName"><%# Eval("Acc_Name", "{0}")%></div>
                    
                   
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
