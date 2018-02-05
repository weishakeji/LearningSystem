<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Product_Message.aspx.cs" Inherits="Song.Site.Manage.Content.Product_Message"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td valign="top" style="height: 310px">
                标题：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:Button
                    ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
                <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
                    ShowSelectBox="False">
                    <EmptyDataTemplate>
                        当前产品没有咨询信息！
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="序号">
                            <itemtemplate>
<%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
</itemtemplate>
                            <itemstyle cssclass="center" width="40px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID">
                            <itemtemplate>
<%# Eval("Pm_Id","{0}")%>
</itemtemplate>
                            <itemstyle cssclass="center" width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="标题">
                            <itemtemplate>
              <asp:LinkButton runat="server" Text='<%# Eval("Pm_Title", "{0}")%> ' OnClick="showMsg_Click" ></asp:LinkButton>

</itemtemplate>
                            <itemstyle cssclass="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="回复">
                            <itemtemplate>
                <%# Eval("Pm_IsAns", "{0}") == "True" ? "" : "未回复"%>
</itemtemplate>
                            <itemstyle cssclass="center" width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="显示">
                            <itemtemplate>
<cc1:StateButton id="sbUse" onclick="sbUse_Click" runat="server" TrueText="显示" FalseText="隐藏" State='<%# Eval("Pm_IsShow","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
                            <itemstyle cssclass="center" width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="留言时间">
                            <itemtemplate>
               <asp:LinkButton  runat="server" Text='<%# Eval("Pm_CrtTime", "{0}")%> ' OnClick="showMsg_Click" ></asp:LinkButton>

</itemtemplate>
                            <itemstyle cssclass="center" width="120px" />
                        </asp:TemplateField>
                    </Columns>
                </cc1:GridView>
                <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
            </td>
            <!--右侧为留言回复 -->
            <td width="400" valign="top">
                <b>产品咨询留言：</b><asp:Label ID="lbId" runat="server" Text="" Visible="false"></asp:Label><br />
                主题：<asp:TextBox ID="tbText" runat="server" MaxLength="200" Width="276px"></asp:TextBox><br />
                内容：
                <br />
                <asp:TextBox ID="tbContext" runat="server" Rows="5" TextMode="MultiLine" Width="99%"></asp:TextBox><br />
                留言时间：<asp:Label ID="lbCrtTime" runat="server" Text=""></asp:Label>
                IP：<asp:Label ID="lbIP" runat="server" Text=""></asp:Label><br />
                电话：<asp:Label ID="lbPhone" runat="server" Text=""></asp:Label><br />
                邮箱：<asp:Label ID="lbEmail" runat="server" Text=""></asp:Label><br />
                地址：<asp:Label ID="lbAddress" runat="server" Text=""></asp:Label><br />
                回复：<br />
                <asp:TextBox ID="tbAnswer" runat="server" MaxLength="255" Rows="5" TextMode="MultiLine"
                    Width="99%"></asp:TextBox><br />
                <asp:CheckBox ID="cbIsShow" runat="server" Text="是否在网站显示" /></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:DeleteButton ID="btnDell" runat="server" OnClick="btnDelete_Click" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
