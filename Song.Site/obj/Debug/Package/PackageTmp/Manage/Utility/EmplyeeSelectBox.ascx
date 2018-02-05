<%@ Control Language="C#" AutoEventWireup="true" Codebehind="EmplyeeSelectBox.ascx.cs"
    Inherits="Song.Site.Manage.Utility.EmplyeeSelectBox" EnableViewState="false" %>
<script type="text/javascript">
//初始化一些参数
//
//宽度
var emplyeeselBoxWidth="<%= Width %>";
//高度
var emplyeeselBoxHeight=<%= Height %>;
//目标
var emplyeeselBoxTarget="<%= Target %>";
//选中的对象id，记录下的TextBox
var emplyeeselTextBoxTarget="<%= TargetTextBox %>";
</script>
<div id="EmplyeeSelectBox" style="width:<%= Width %>; height:<%= Height %>">
    <dl id="EmplyeeSelectBoxTitle">
        <dd type="depart">
            院系</dd>
        <dd type="posi">
            岗位</dd>
        <dd type="group">
            工作组</dd>
        <dd type="online">
            在线</dd>
    </dl>
     <%-- 院系列表--%>
    <div paneltype="depart" type="zone" class="zone"  style="width:<%= Width %>; height:<%= Height-20 %>px">
        <asp:Literal ID="ltDepart" runat="server"></asp:Literal></div>
    <%-- 岗位列表--%>
    <div paneltype="posi" type="zone" class="zone"   style="width:<%= Width %>; height:<%= Height-20 %>px">
        <asp:Literal ID="ltPosi" runat="server"></asp:Literal>
    </div>
    <%-- 组列表--%>
    <div paneltype="group" type="zone" class="zone"  style="width:<%= Width %>; height:<%= Height-20 %>px">
        <asp:Literal ID="lgGroup" runat="server"></asp:Literal>
    </div>
   <%-- 在线人员--%>
   <div paneltype="online" type="zone" class="zone"  style="width:<%= Width %>; height:<%= Height-20 %>px">
   <asp:Literal ID="ltOnline" runat="server"></asp:Literal>
   </div>
</div>
