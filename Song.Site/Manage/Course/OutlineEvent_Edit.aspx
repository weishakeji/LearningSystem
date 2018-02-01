<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="OutlineEvent_Edit.aspx.cs" Inherits="Song.Site.Manage.Course.OutlineEvent_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80px">
                事件主题：
            </td>
            <td>
                <asp:TextBox ID="tbTitle" nullable="false" Width="300" MaxLength="200" runat="server"></asp:TextBox>
                <asp:CheckBox ID="cbIsUse" runat="server" Text="是否启用" Checked="true" />
            </td>
        </tr>
        <tr>
            <td class="right">
                窗口：
            </td>
            <td>
                宽
                <asp:TextBox ID="tbWidth" nullable="false" Width="40" datatype="uint" numlimit="100-1000"
                    MaxLength="3" runat="server"></asp:TextBox>像素 &#215; 高
                <asp:TextBox ID="tbHeight" nullable="false" Width="40" datatype="uint" numlimit="100-1000"
                    MaxLength="3" runat="server"></asp:TextBox>像素
            </td>
        </tr>
        <tr>
            <td class="right">
                时间：
            </td>
            <td>
                当视频播放到
                <asp:TextBox ID="tbPoint" nullable="false" Width="60" datatype="uint" MaxLength="4"
                    runat="server"></asp:TextBox>秒时，触发该事件
            </td>
        </tr>
        <tr>
            <td class="right">
                类型：
            </td>
            <td>
                <asp:RadioButtonList ID="rblTypes" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"
                    RepeatLayout="Flow" OnSelectedIndexChanged="tblTypes_SelectedIndexChanged" ToolTip="类型一旦确定，无法更改">
                    <asp:ListItem Selected="True" Value="1">提醒</asp:ListItem>
                    <asp:ListItem Value="2">知识展示</asp:ListItem>
                    <asp:ListItem Value="3">课程提问</asp:ListItem>
                    <asp:ListItem Value="4">实时反馈</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="Panel1" runat="server">
        <asp:TextBox ID="tbContext1" runat="server" TextMode="MultiLine" Width="99%" Height="250"></asp:TextBox>
        <br />
        <br />
        说明：提示信息以弹窗的形式出现，并暂停视频播放；文字限300字，超出部分将截取；
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
        <WebEditor:Editor ID="tbContext2" runat="server" Width="99%" ThemeType="simple" Height="250">
        </WebEditor:Editor>
        <br />
        说明：展示图文资料，并暂停视频；
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="right" width="80px">
                    问题：
                </td>
                <td>
                    <asp:TextBox ID="tbQuesTit" runat="server" TextMode="MultiLine" Width="99%" Height="100"></asp:TextBox>
                </td>
            </tr>
 <tr>
                <td class="right" width="80px">
                    选项：
                </td><td>
         <asp:GridView ID="gvAnswer" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText="序号">
                    <ItemStyle CssClass="center" Width="40px" />
                    <ItemTemplate>
                        <%# Container.DataItemIndex   +   1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="答案">
                    <ItemStyle CssClass="center" Width="40px" />
                    <ItemTemplate>
                        <asp:RadioButton ID="rbAns" CssClass="rbAns" runat="server" Checked='<%# Eval("iscorrect","{0}")=="True"%>'>
                        </asp:RadioButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="选项">
                    <ItemStyle CssClass="left" />
                    <ItemTemplate>
                        <asp:TextBox ID="itemTxt" Width="98%" MaxLength="200" runat="server" Text='<%# Eval("item")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
       </td></tr>
       
            <tr>
                <td class="right">
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="right">
                </td>
                <td>
                    说明：只限单选题；学员回答正确，视频才会继续播放。
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="right" width="80px">
                    问题：
                </td>
                <td>
                    <asp:TextBox ID="tbQuesTit4" runat="server" TextMode="MultiLine" Width="99%" Height="60"></asp:TextBox>
                </td>
            </tr>
            <asp:Repeater ID="rptFeedback" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="right">
                            <%# Container.ItemIndex + 1%>
                            .
                        </td>
                        <td>
                            <asp:TextBox ID="tbItem" runat="server" Width="60%" MaxLength="100" Text='<%# Eval("item") %>'></asp:TextBox>
                            选择后跳转到<asp:TextBox ID="tbPoint" Width="30" datatype="uint" MaxLength="4" Text='<%# Eval("point") %>'
                                runat="server"></asp:TextBox>秒
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td class="right">
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="right">
                </td>
                <td>
                    说明：根据学员反馈跳转播放视频。
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
