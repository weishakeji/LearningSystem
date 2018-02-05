<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="MessageBoard_Ans.aspx.cs" Inherits="Song.Site.Manage.Site.MessageBoard_Ans"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <%--贴子列表--%>
    <div class="topBox">
        <div class="title">
            主题：<asp:Label ID="lbTitle" runat="server" Text=""></asp:Label></div>
        <div class="ansBtn">
            <a href="#ans">回复</a></div>
        <div class="uid" style="display: none">
            <asp:Label ID="lbUid" runat="server" Text=""></asp:Label></div>
    </div>
    <div class="context">
        <div class="item">
            <div class="mLeft">
                <img src='/Upload/Student/<%= mb.Ac_Photo %>' />
                <div class="accname">
                    <%= mb.Ac_Name %>
                </div>
            </div>
            <div class="mRight">
                <div class="mHeader">
                    <span class="mTime">时间：<%= mb.Mb_CrtTime%>
                    </span><span class="mIP">IP：<%= mb.Mb_IP%>
                    </span>
                </div>
                <div class="mContext">
                    <%= mb.Mb_Content%>
                </div>
            </div>
        </div>
    </div>
      <div class="topBox">
        <div class="title">
            回答咨询</div>
        
        
    </div>
    <%--回贴--%>
    <a name="ans" id="ans"></a>
    <table cellspacing="0" cellpadding="0" width="760px" border="0" id="table1" runat="server">
        <tr>
            <td width="85" class="center" valign="top">
                <asp:ImageButton ID="imbCurrPhoto" runat="server" Width="80px" />
                <div class="accname">
                    <asp:Label ID="lbCurrName" runat="server" Text=""></asp:Label></div>
            </td>
            <td style="text-align:right">
               <asp:TextBox ID="tbAns" runat="server" Height="100px" TextMode="MultiLine" 
                    Width="99%" CssClass="tbAns"></asp:TextBox>
            </td>
            
        </tr>
         <tr>
         <td></td>
             
         <td><asp:CheckBox ID="cbIshow" runat="server" Text="是否在前端显示" /></td>
         </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton ID="btnEnter" runat="server" Text="确 定" OnClick="btnEnter_Click"
        verify="true" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
