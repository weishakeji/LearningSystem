<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="PictureRecycle.aspx.cs" Inherits="Song.Site.Manage.Content.Picture.Recycle"
    Title="图片回收站" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
    <div>
        
        标题：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:Button ID="btnSear" runat="server" Width="100"
                Text="查询" OnClick="btnsear_Click" />
    </div></div>
    <div class="imgList">
    <asp:Repeater ID="rptPict" runat="server">
        <ItemTemplate>
            <div class="imgBox" style="width: 100px; height: 120px;">
                <div class="picShow" >
                    <div style="width: 100px; height: 120px; background-image: url(<%# Eval("Pi_FilePathSmall", "{0}") %>)"
                        iscover="<%# Eval("Pi_IsCover") %>">
                    </div>
                </div>
                <div class="piName" title="<%# Eval("Pi_Name") %>">
                    <span>
                        <%# Eval("Pi_Name") %>
                    </span>
                </div>
                <div>
                    <asp:LinkButton ID="lbCover" runat="server" CommandArgument='<%# Eval("Pi_Id") %>'
                        OnClick="lbRestore_Click">还原</asp:LinkButton>
                    <asp:LinkButton ID="lbDel" runat="server" OnClientClick="return confirm('您是否确定永久删除？\n注：删除后将无法恢复。')"
                        CommandArgument='<%# Eval("Pi_Id")%>' OnClick="lbDel_Click">删</asp:LinkButton>
                   
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    </div>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
