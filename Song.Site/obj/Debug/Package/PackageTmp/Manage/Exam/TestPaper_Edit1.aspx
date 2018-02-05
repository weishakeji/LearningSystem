<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="TestPaper_Edit1.aspx.cs" Inherits="Song.Site.Manage.Exam.TestPaper_Edit1"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="modifyArea">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td width="80px" class="right">
                    试卷名称：
                </td>
                <td colspan="3">
                    <asp:TextBox ID="tbName" runat="server" nullable="false" MaxLength="200" Width="400px"></asp:TextBox>
                    &nbsp;
                    <asp:CheckBox ID="cbIsUse" runat="server" Text="采用" state="true" Checked="true" />
                    <asp:CheckBox ID="cbIsRec" runat="server" Text="推荐" state="true" />
                    &nbsp; <span class="tpType">试卷类型：统一试题</span>
                </td>
            </tr>
            <tr>
                <td class="right">
                    专业/课程：
                </td>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <cc1:DropDownTree ID="ddlSubject" runat="server" IdKeyName="Sbj_ID" ParentIdKeyName="Sbj_PID"
                                TaxKeyName="Sbj_Tax" Width="120" AutoPostBack="True" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                            </cc1:DropDownTree>
                            &nbsp; 课程：<cc1:DropDownTree ID="ddlCourse" runat="server" IdKeyName="Cou_ID" ParentIdKeyName="Cou_PID"
                                TaxKeyName="Cou_Tax">
                            </cc1:DropDownTree>
                           
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlSubject" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="right">
                    难易度：
                </td>
                <td>
                    <asp:DropDownList ID="ddlDiff" runat="server">
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3" Selected="true">3</asp:ListItem>
                        <asp:ListItem Value="4">4</asp:ListItem>
                        <asp:ListItem Value="5">5</asp:ListItem>
                    </asp:DropDownList>
                    -
                    <asp:DropDownList ID="ddlDiff2" runat="server">
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3" Selected="true">3</asp:ListItem>
                        <asp:ListItem Value="4">4</asp:ListItem>
                        <asp:ListItem Value="5">5</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="300" rowspan="7">
                    试卷简介： （字数：<span class="count"></span>）<br />
                    <WebEditor:Editor ID="tbIntro" runat="server" Height="165px" ThemeType="mini" Width="99%"
                        afterChange="function(){K('.count').html(this.count('text'))}"></WebEditor:Editor>
                </td>
                <td width="155" rowspan="7" valign="top" style="width: 155px">
                    标识图片：<br />
                    <img src="../Images/nophoto.jpg" name="imgShow" width="150" height="150" id="imgShow"
                        runat="server" /><br />
                    <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png" Width="150" />
                </td>
            </tr>
            <tr>
                <td class="right">
                    时长：
                </td>
                <td>
                    <asp:TextBox ID="tbSpan" runat="server" nullable="false" datatype="uint" MaxLength="4"
                        Width="60px"></asp:TextBox>
                    分钟 &nbsp;&nbsp;&nbsp;&nbsp;总分：<asp:TextBox ID="tbTotal" runat="server" nullable="false"
                        datatype="uint" MaxLength="6" Width="60px">100</asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;及格分：
                    <asp:TextBox ID="tbPassScore_" runat="server" nullable="false" datatype="uint" MaxLength="6"
                        Width="60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    填空题：
                </td>
                <td>
                    有
                    <asp:TextBox ID="tbItem5Count" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox>
                    道题；占总分
                    <asp:TextBox ID="tbItem5Score" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox>
                    %；计<asp:Label ID="lbItem5Number" runat="server" Text="0"></asp:Label>分；<span style="display: none"><asp:TextBox
                        ID="tbItem5Number" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    单选题：
                </td>
                <td>
                    有
                    <asp:TextBox ID="tbItem1Count" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox>
                    道题；占总分
                    <asp:TextBox ID="tbItem1Score" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox>
                    %；计<asp:Label ID="lbItem1Number" runat="server" Text="0"></asp:Label>分；<span style="display: none"><asp:TextBox
                        ID="tbItem1Number" runat="server"></asp:TextBox></span>
                </td>
            </tr>
            <tr>
                <td class="right">
                    多选题：
                </td>
                <td>
                    有
                    <asp:TextBox ID="tbItem2Count" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox>
                    道题；占总分
                    <asp:TextBox ID="tbItem2Score" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox>
                    %；计<asp:Label ID="lbItem2Number" runat="server" Text="0"></asp:Label>分；<span style="display: none"><asp:TextBox
                        ID="tbItem2Number" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    判断题：
                </td>
                <td>
                    有
                    <asp:TextBox ID="tbItem3Count" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox>
                    道题；占总分
                    <asp:TextBox ID="tbItem3Score" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox>
                    %；计<asp:Label ID="lbItem3Number" runat="server" Text="0"></asp:Label>分；<span style="display: none"><asp:TextBox
                        ID="tbItem3Number" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    简答题：
                </td>
                <td>
                    有
                    <asp:TextBox ID="tbItem4Count" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox>
                    道题；占总分
                    <asp:TextBox ID="tbItem4Score" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox>
                    %；计<asp:Label ID="lbItem4Number" runat="server" Text="0"></asp:Label>分；<span style="display: none"><asp:TextBox
                        ID="tbItem4Number" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div id="quesArea">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="btnClose" runat="server" />
</asp:Content>
