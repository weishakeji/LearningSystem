<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="EmpGroup_Member.aspx.cs" Inherits="Song.Site.Manage.Sys.EmpGroup_Member"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Repeater ID="rtEmp" runat="server">
        <HeaderTemplate>
            <dl id="emplyee" style="display:none">
        </HeaderTemplate>
        <ItemTemplate>
            <dd empid="<%# Eval("Acc_Id")%>" depid="<%# Eval("Dep_Id")%>"><%# Eval("Acc_Name")%></dd>
        </ItemTemplate>
        <FooterTemplate>
            </dl></FooterTemplate>
    </asp:Repeater>
    <%--属于当前角色的员工--%>
<asp:Repeater ID="rbEmp4Posi" runat="server">
        <HeaderTemplate>
            <dl id="Emp4Posi" style="display:none">
        </HeaderTemplate>
        <ItemTemplate>
            <dd nodeEmpId="<%# Eval("Acc_Id")%>" depid="<%# Eval("Dep_Id")%>"><%# Eval("Acc_Name")%></dd>
        </ItemTemplate>
        <FooterTemplate>
            </dl></FooterTemplate>
    </asp:Repeater>
    <script type="text/javascript" src="Scripts/Depart/DepartNode.js"></script>

    <script type="text/javascript" src="Scripts/Depart/DepartEmployeeTree.js"></script>

    <!--顶部条 -->
    <div id="contTop">
        <!--预载进度条 -->
        <div id="loadingBar">
            <img src="../Images/loading/load_016.gif" /></div>
    </div>
    <!-- 左侧树形的区域 -->
    <div id="MenuTreePanel" style="-moz-user-select: none;" onselectstart="return false;">
    </div>
    <!-- 右侧员工所属角色，管理区域 -->
    <div id="EmplyeePanel" style="-moz-user-select: none;" onselectstart="return false;">
    <div id="EmplyeeListTitle">属于
        <asp:Label ID="lbGroup" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label> 用户组的员工有：</div>
    <div id="EmplyeeList">无</div>
    </div>
    <div id="show">
        <font color="red">*</font>将员工或院系拖至右侧区域。
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify=true  ID="btnEnter" runat="server" Text="确定" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
