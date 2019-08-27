<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Article.aspx.cs" Inherits="Song.Site.Manage.Content.Article" Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script language="javascript" src="scripts/tab.js" type="text/javascript"></script>

    <div id="tabTitleBox">
        <div class="titItem curr" tab="tab1">
            基本信息</div>
        <div class="titItem" tab="tab2">
            详细信息</div>
        <div class="titItem" tab="tab3">
            附件</div>
        <div class="titItem" tab="tab4">
            发布设置</div>
        <div class="titItem" tab="tab5">
            自定义信息</div>
    </div>
    <div id="tabContextBox">
        <div class="contItem" tab="tab1">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="right" width="80px">
                        主标题：</td>
                    <td>
                        <asp:TextBox nullable="false" ID="tbTitle" group="ent" runat="server" MaxLength="255"
                            Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        简标题：</td>
                    <td>
                        <asp:TextBox ID="tbTitleAbbr" group="ent" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                        &nbsp;
                        <asp:CheckBox ID="cbIsShow" runat="server" Text="是否显示" state="true" Checked="true" />
                        <asp:CheckBox ID="cbIsTop" runat="server" Text="置顶" state="true" />
                        <asp:CheckBox ID="cbIsRec" runat="server" Text="推荐" state="true" />
                        <asp:CheckBox ID="cbIsHot" runat="server" Text="热点" state="true" /></td>
                </tr>
                <tr>
                    <td class="right">
                        完整标题：</td>
                    <td>
                        <asp:TextBox ID="tbTitleFull" group="ent" runat="server" MaxLength="255" Width="95%"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trCol" runat="server">
                    <td class="right">
                        栏目分类：</td>
                    <td>
                        <asp:DropDownList ID="ddlColumn" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        所属专题：
                    </td>
                    <td>
                        <asp:CheckBoxList ID="cbSpecial" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        </asp:CheckBoxList></td>
                </tr>
                <tr>
                    <td valign="top" class="right">
                        摘要：
                    </td>
                    <td>
                        <asp:TextBox ID="tbIntro" runat="server" MaxLength="255" TextMode="MultiLine" Height="100"
                            Width="99%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right">
                        标签：</td>
                    <td>
                        <asp:TextBox ID="tbLabel" runat="server" MaxLength="255" Width="99%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        作者：</td>
                    <td>
                        <asp:TextBox ID="tbAuthor" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        来源：</td>
                    <td>
                        <asp:TextBox ID="tbSource" state="true" runat="server"></asp:TextBox>
                        <span id="oftenSource">
                            <asp:Literal ID="ltSource" runat="server"></asp:Literal>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                    </td>
                    <td>
                        <asp:CheckBox ID="cbIsImg" runat="server" Text="图片资讯" AutoPostBack="True" OnCheckedChanged="cbIsImg_CheckedChanged" />
                        <cc1:FileUpload ID="fuImg" runat="server" fileallow="png|jpg|gif|bmp" />
                        <br />
                        <%-- 图片资讯的图片--%>
                        <asp:Panel ID="panelImg" runat="server">
                            <div class="imgShow" style="display: none">
                                图片资讯主图</div>
                            <img width="100" height="100" runat="server" id="imgFile" class="imgFile" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <div class="contItem" tab="tab2">
            <WebEditor:Editor ID="tbDetails" runat="server" Height="450px"> </WebEditor:Editor>
        </div>
        <div class="contItem" tab="tab3">
            <cc1:FileUpload ID="fuLoad" runat="server" fileallow="zip|rar|pdf|doc|docx|xls|xlsx"
                group="up" nullable="false" Width="560px" />
            <asp:Button verify="true" group="up" ID="btn" runat="server" Text="上传" OnClick="btn_Click" />
            <br />
            <asp:DataList ID="dlAcc" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <ItemTemplate>
                    <%# Container.ItemIndex   + 1 %>
                    、
                    <asp:HyperLink ID="hl" runat="server" NavigateUrl='<%#Eval("As_FileName") %>' Target="_blank"><%#Eval("As_Name") %></asp:HyperLink>
                    &nbsp; <span class="sizeSpan">(<span class="size"><%#Eval("As_Size") %></span>)</span>&nbsp;
                    &nbsp;<asp:LinkButton ID="lb" runat="server" CommandArgument='<%#Eval("As_Id") %>'
                        ForeColor="Red" OnClick="lb_Click">删</asp:LinkButton>
                    &nbsp;
                    <br />
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div class="contItem" tab="tab4">
            <!--发布设置-->
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="80px" class="right">
                        Keywords：</td>
                    <td>
                        <asp:TextBox ID="tbKeywords" runat="server" MaxLength="255" Width="95%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right" valign="top">
                        Description：</td>
                    <td>
                        <asp:TextBox ID="tbDescr" runat="server" MaxLength="255" TextMode="MultiLine" lenlimit="250"
                            Width="95%" Height="100px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right">
                        上线时间：</td>
                    <td>
                        <asp:TextBox ID="tbPushTime" runat="server" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                            MaxLength="255" Width="160"></asp:TextBox></td>
                </tr>
            </table>
        </div>
        <div class="contItem" tab="tab5">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" group="ent"
        OnClick="btnEnter_Click" ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="btnClose" runat="server" />
</asp:Content>
