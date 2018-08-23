<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Learningcard_Edit.aspx.cs" Inherits="Song.Site.Manage.Card.Learningcard_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="Kind" %>
<%@ Register Src="../Utility/SortSelect.ascx" TagName="SortSelect" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="right" width="80">
                            主题：
                        </td>
                        <td>
                            <asp:TextBox ID="Lcs_Theme" nullable="false" runat="server" MaxLength="20" Width="95%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            面额：
                        </td>
                        <td>
                            <asp:TextBox ID="Lcs_Price" datatype="uint" nullable="false" runat="server" MaxLength="20"
                                Width="100"></asp:TextBox>
                            元
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            可抵扣：
                        </td>
                        <td>
                            <asp:TextBox ID="Lcs_Coupon" datatype="uint" nullable="false" runat="server" MaxLength="20"
                                Width="100"></asp:TextBox>
                            卡券
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            数量：
                        </td>
                        <td>
                            <asp:TextBox ID="Lcs_Count" datatype="uint" nullable="false" runat="server" MaxLength="20"
                                Width="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            有效期：
                        </td>
                        <td>
                            <asp:TextBox ID="Lcs_LimitStart" runat="server" onfocus="WdatePicker()" Format="yyyy-MM-dd"
                                CssClass="Wdate" EnableTheming="false" MaxLength="20" Width="100"></asp:TextBox>
                            至
                            <asp:TextBox ID="Lcs_LimitEnd" runat="server" onfocus="WdatePicker()" Format="yyyy-MM-dd"
                                CssClass="Wdate" EnableTheming="false" MaxLength="20" Width="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            学习时长：
                        </td>
                        <td>
                            <asp:TextBox ID="Lcs_Span" runat="server" nullable="false" datatype="uint" Width="40"></asp:TextBox>
                            <asp:DropDownList ID="Lcs_Unit" runat="server">
                                <asp:ListItem>日</asp:ListItem>
                                <asp:ListItem>周</asp:ListItem>
                                <asp:ListItem Selected="True">月</asp:ListItem>
                                <asp:ListItem>年</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            简介：
                        </td>
                        <td>
                            <asp:TextBox ID="Lcs_Intro" runat="server" MaxLength="20" Width="95%" TextMode="MultiLine"
                                Height="60"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                        </td>
                        <td>
                            <asp:CheckBox ID="Lcs_IsEnable" runat="server" Checked="true" Text="是否启用" />
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            密钥：
                        </td>
                        <td>
                            <asp:TextBox ID="Lcs_SecretKey" runat="server" nullable="false" lenlimit="10-32"
                                MaxLength="100" Width="95%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                        </td>
                        <td>
                            （密钥将增强学习卡安全性，实际使用中并不使用）
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            学习码长度：
                        </td>
                        <td>
                            <asp:TextBox ID="Lcs_CodeLength" datatype="uint" nullable="false" numlimit="10-15"
                                runat="server" MaxLength="2" Width="30" Text="12"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            密码长度：
                        </td>
                        <td>
                            <asp:TextBox ID="Lcs_PwLength" datatype="uint" nullable="false" numlimit="3-6" runat="server"
                                MaxLength="2" Width="30" Text="3"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="50%" valign="top">
                <span style="display: none">
                    <asp:TextBox ID="tbCourses" runat="server"></asp:TextBox>
                    <asp:Repeater ID="rtpCourses" runat="server">
                        <ItemTemplate>
                            <b couid='<%# Eval("Cou_ID") %>'>
                                <%# Eval("Cou_Name")%></b>
                        </ItemTemplate>
                    </asp:Repeater>
                </span>
                <dl class="courses">
                    <dt>
                        <p>
                            关联的课程：
                            <input href="Learningcard_Courses.aspx" wd="800" hg="600" id="cour_add" type="button"
                                value="编辑" /></p>
                    </dt>
                </dl>
                <div class="noCourse">
                    当前学习卡没有关联任何课程 </br> 点击“编辑”按钮，添加课程
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
