<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="Song.Site.Manage.Course.List" Title="课程列表" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="subjectBox">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="itemBox">
        <asp:Repeater ID="rptCourse" runat="server">
            <ItemTemplate>
                <div class="item" couid="<%# Eval("Cou_ID") %>" wd="800" hg="600">
                    <img src="<%# Eval("Cou_LogoSmall") %>" />
                    <div class="itemName">
                        <%# Eval("Cou_Name") %>&nbsp;
                        <cc1:StateButton ID="sbShow" OnClick="sbShow_Click" CommandArgument='<%# Eval("Cou_ID")%>'
                            runat="server" TrueText="显示" FalseText="隐藏" State='<%# Eval("Cou_IsUse","{0}")=="True"%>'></cc1:StateButton>
                        &nbsp;<asp:LinkButton ID="lbDelete" runat="server" CommandArgument='<%# Eval("Cou_ID") %>'
                            OnClick="lbDelete_Click">删除</asp:LinkButton></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
