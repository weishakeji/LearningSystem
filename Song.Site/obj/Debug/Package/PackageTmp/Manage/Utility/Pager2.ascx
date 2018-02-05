<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager2.ascx.cs" Inherits="Song.Site.Manage.Utility.Pager2" %>
<div class="pager2 noprint">

    <%= First %>
    <%= Prev %>
        <%= NumberNav(this.Index,"span")%>
    <%= Next %>
    <%= Last %>
        <span class="info">
        当前<%= Size >= RecordAmount ? RecordAmount : Size%>条/共<%= RecordAmount %>条</span>
         <span class="info">
          共<%= PageAmount%>页
         </span>
         <span class="info">
         跳至<asp:TextBox ID="tbGoPagenum" runat="server" Width="40" nullable="false" star="false" datatype="uint" place="top" group="GoPagenum"></asp:TextBox>页 
<asp:Button ID="btnGoPagenum" runat="server" Text="跳转" verify="true" 
        group="GoPagenum" onclick="btnGoPagenum_Click"/>
         </span>
    
    

</div>
