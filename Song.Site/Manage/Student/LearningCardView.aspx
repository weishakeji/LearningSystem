<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LearningCardView.aspx.cs" Inherits="Song.Site.Manage.Student.LearningCardView"
    MasterPageFile="~/Manage/Student/Parents.Master" %>
<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
 
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80px">
                卡号：
            </td>
            <td>
                <asp:Label ID="Lc_Code" runat="server" Text=""></asp:Label> -
                  <asp:Label ID="Lc_Pw" runat="server" Text=""></asp:Label> 
            </td>
           
        </tr>
        <tr>
            <td class="right">
                名称：
            </td>
            <td>
                <asp:Label ID="Lcs_Theme" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                面额：
            </td>
            <td>
                <asp:Label ID="Lc_Price" runat="server" Text=""></asp:Label>  元
            </td>
        </tr>
        <tr>
            <td class="right">
                学习时间：
            </td>
            <td>
                <asp:Label ID="Lcs_Span" runat="server" Text=""></asp:Label>
                <asp:Label ID="Lcs_Unit" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                有效期：
            </td>
            <td>
                <asp:Label ID="Lcs_LimitStart" runat="server" Format="yyyy年M月d日" Text=""></asp:Label> 
                - <asp:Label ID="Lcs_LimitEnd" runat="server" Format="yyyy年M月d日"></asp:Label>
            </td>
        </tr>
        
    </table>
    <asp:Repeater ID="rptCourses" runat="server">
        <HeaderTemplate>
            <fieldset>
                <legend>关联课程</legend>
                <dl class="courses">
        </HeaderTemplate>
        <ItemTemplate>
            <dd>
                <%# Container.ItemIndex + 1%>、
                    <%# Eval("Cou_Name")%>
                    </dd>
        </ItemTemplate>
        <FooterTemplate>
            </dl> </fieldset>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
