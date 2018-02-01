<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Task_Edit.aspx.cs" Inherits="Song.Site.Manage.Common.Task_Edit" Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>

    任务名称：
    <asp:TextBox ID="tbName" runat="server" Width="90%" MaxLength="255" nullable="false"></asp:TextBox>
    <br />
    优先级别：
    <asp:DropDownList ID="ddlLevel" runat="server" Width="65">
        <asp:ListItem>5</asp:ListItem>
        <asp:ListItem>4</asp:ListItem>
        <asp:ListItem>3</asp:ListItem>
        <asp:ListItem>2</asp:ListItem>
        <asp:ListItem>1</asp:ListItem>
    </asp:DropDownList>
    　　指派给：<cc1:DropDownTree ID="ddlDepart" runat="server" Width="150" IdKeyName="dep_id"
        ParentIdKeyName="dep_PatId" TaxKeyName="dep_Tax" AutoPostBack="True" OnSelectedIndexChanged="ddlDepart_SelectedIndexChanged">
    </cc1:DropDownTree><asp:DropDownList ID="ddlEmployee" runat="server" Width="80">
    </asp:DropDownList>
    <table width="98%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                任务描述：<br />
                <asp:TextBox ID="tbContext" runat="server" Height="200px" MaxLength="255" TextMode="MultiLine"
                    Width="98%"></asp:TextBox></td>
     
        </tr>
    </table>
   
    <br />
    开始时间：
    <asp:TextBox ID="tbStart" runat="server" onfocus="WdatePicker()" Width="80"></asp:TextBox>
    &nbsp;计划完成时间：
    <asp:TextBox ID="tbEnd" runat="server" onfocus="WdatePicker()" Width="80"></asp:TextBox>
    &nbsp;
    <br />
    <%--如果是新增任务，则不显示下面的内容--%>
    <asp:Panel ID="plPara" runat="server">
       完成工作量：<asp:TextBox ID="tbCompletePer" runat="server" datatype="uint" MaxLength="3" Width="40px" ></asp:TextBox>
    %<br />
        <asp:CheckBox ID="cbIsUse" Text="是否关闭任务" runat="server" />
        <br />
        <asp:CheckBox ID="cbIsComplete" Text="是否完成任务" runat="server" AutoPostBack="True"
            OnCheckedChanged="cbIsComplete_CheckedChanged" />
        <div id="divCompleteBox" runat="server">
            实际完成时间：
            <asp:TextBox ID="tbComplete" runat="server" onfocus="WdatePicker()" Width="80"></asp:TextBox>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
