<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="AddressBook_Edit.aspx.cs" Inherits="Song.Site.Manage.Common.AddressBook_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
 <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    姓名：<asp:TextBox ID="tbName" runat="server" nullable="false" datatype="user" Width="180px" MaxLength="20"></asp:TextBox>
    分类：<asp:DropDownList ID="ddlTpye" runat="server">
       
    </asp:DropDownList>
    <br />
    性别：<asp:RadioButtonList ID="rblSex" runat="server" RepeatDirection="Horizontal"
        RepeatLayout="Flow">
        <asp:ListItem Selected="True" Value="1">男</asp:ListItem>
        <asp:ListItem Value="2">女</asp:ListItem>
        <asp:ListItem Value="0">未知</asp:ListItem>
    </asp:RadioButtonList><br />
    住宅电话：<asp:TextBox ID="tbTel" runat="server" datatype="tel" MaxLength="100" Width="180px"></asp:TextBox>
    &nbsp;&nbsp;
    办公电话：<asp:TextBox ID="tbCoTel" runat="server"  MaxLength="100" datatype="tel" Width="180px"></asp:TextBox><br />
    移动电话：<asp:TextBox ID="tbMobileTel" runat="server" datatype="mobile" MaxLength="100" Width="180px"></asp:TextBox>&nbsp;<br />
    就职公司：<asp:TextBox ID="tbCompany" runat="server" MaxLength="100" Width="450px"></asp:TextBox><br />
    家庭住址：<asp:TextBox ID="tbAddress" runat="server" MaxLength="100" Width="450px"></asp:TextBox><br />
    邮政编码：<asp:TextBox ID="tbZip" runat="server" datatype="zip" MaxLength="6" Width="60px"></asp:TextBox><br />
    电子信箱：<asp:TextBox ID="tbEmail" runat="server" datatype="email" MaxLength="100" Width="180px"></asp:TextBox>
    &nbsp;&nbsp;
    QQ号：<asp:TextBox ID="tbQQ" runat="server" datatype="number" MaxLength="100" Width="180px"></asp:TextBox><br />
    MSN帐号：<asp:TextBox ID="tbMsn" runat="server" MaxLength="100" Width="180px"></asp:TextBox><br />
    博客地址：<asp:TextBox ID="tbBlog" runat="server" MaxLength="100" Width="450px"></asp:TextBox><br />
    <br />
    生日：<asp:TextBox ID="tbBirthday" runat="server" MaxLength="100" Width="80px" onfocus="WdatePicker()"></asp:TextBox><br />
    爱好：<asp:CheckBoxList ID="cbLike" runat="server" RepeatDirection="Horizontal"
        RepeatLayout="Flow">
        <asp:ListItem Value="上网">上网</asp:ListItem>
        <asp:ListItem Value="阅读">阅读</asp:ListItem>
        <asp:ListItem Value="唱歌">唱歌</asp:ListItem>
        <asp:ListItem>音乐</asp:ListItem>
        <asp:ListItem>体育</asp:ListItem>
        <asp:ListItem>旅游</asp:ListItem>
        <asp:ListItem>电影</asp:ListItem>
    </asp:CheckBoxList><br />
    个人简介：<br />
    <asp:TextBox ID="tbIntro" runat="server" MaxLength="255" Width="531px" Height="60px" TextMode="MultiLine"></asp:TextBox><br />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton ID="btnEnter" runat="server" Text="确定" verify="true" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
