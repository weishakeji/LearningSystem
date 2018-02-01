<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Picture.aspx.cs" Inherits="Song.Site.Manage.Content.Picture.PictureInfo"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
      <div class="toolsBar">
                <cc1:FileUpload ID="fuLoad" runat="server" nullable="false"  fileallow="jpg|bmp|gif|png" group="upfile"
                    Width="400" />
                <asp:Button ID="btnUpFile" runat="server" Text="上传" group="upfile" verify="true"
                    OnClick="btnUpFile_Click" />
            </div>
        <div class="searchBox">
            名称：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:CheckBox ID="cbIsHot" runat="server" Text="热点" />
            <asp:CheckBox ID="cbIsTop" runat="server" Text="置顶" />
            <asp:CheckBox ID="cbIsRec" runat="server" Text="推荐" /><asp:Button ID="btnSear" runat="server" Width="100" Text="查询"
                OnClick="btnsear_Click" />
        </div>
    </div>
    
            <div class="imgList">
                <asp:Repeater ID="rptPict" runat="server">
                    <HeaderTemplate>
                        <dl>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <dd class="imgBox" style="width: 146px">
                            <div class="picShow" style="width: 140px; height: 140px;background-image:url(<%# Eval("Pic_FilePathSmall", "{0}") %>)"
                                imgid="<%# Eval("Pic_Id") %>">
                                <div style="display:<%# Eval("Pic_IsCover","{0}")=="True" ? "block" : "none" %>" class="cover" >封面</div></div>
                            <div class="control">
                                <asp:LinkButton ID="lbCover" runat="server" CommandArgument='<%# Eval("Pic_Id") %>'
                                    OnClick="lbCover_Click">设为封面</asp:LinkButton>
                                <asp:LinkButton ID="lbDel" runat="server" OnClientClick="return confirm('您确定删除吗？')"
                                    CommandArgument='<%# Eval("Pic_Id")%>' OnClick="lbDel_Click">删</asp:LinkButton>
                                <cc1:StateButton ID="sbUse" OnClick="sbShow_Click" runat="server" TrueText="显" FalseText="隐"
                                    State='<%# Eval("Pic_IsShow","{0}")=="True"%>' CommandArgument='<%# Eval("Pic_Id")%>'></cc1:StateButton>
                            </div>
                        </dd>
                    </ItemTemplate>
                    <FooterTemplate>
                        </dl>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
