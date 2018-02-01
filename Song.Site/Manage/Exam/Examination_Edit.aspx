<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="Examination_Edit.aspx.cs" Inherits="Song.Site.Manage.Exam.Examination_Edit"
    Title="考试" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <div class="quesLeft">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td width="10%" class="right">
                    考试主题：
                </td>
                <td colspan="3">
                    <asp:TextBox ID="tbName" runat="server" nullable="false" MaxLength="200" Width="80%"></asp:TextBox>
                    &nbsp;
                    <asp:CheckBox ID="cbIsUse" runat="server" Text="启用" state="true" Checked="true" />
                </td>
            </tr>
            <tr>
                <td class="right" valign="top" width="10%">
                    简介：
                </td>
                <td width="40%">
                    <asp:TextBox ID="tbIntro" runat="server" Height="100px" Width="99%" 
                        TextMode="MultiLine"></asp:TextBox>
                </td>
                <td class="right" valign="top" width="10%">
                    参考人员：<br />
                    <asp:RadioButtonList ID="rblGroup" runat="server" CssClass="rblGroup" RepeatLayout="Flow">
                        <asp:ListItem Selected="True" Value="1">全体学生</asp:ListItem>
                        <asp:ListItem Value="2">限定分组</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td width="40%">
                    <%--按分类选择参考范围--%>
                    <div id="selStudentSort" class="selGroup" style="display: none">
                        <asp:ListBox ID="lbSort" CssClass="lbSort" runat="server" Height="100" Width="130">
                        </asp:ListBox>
                        <div class="btnBox">
                            <input class="btnGo" id="btnAddSort" type="button" value=">>" title="添加" />
                            <input type="button" class="btnGo" id="btnSortRemove" value="<<" title="去除" />
                            <input type="button" class="btnGo" id="btnSortRemoveAll" value="<<<" title="去除所有" /></div>
                        <asp:ListBox ID="lbSelected" CssClass="lbSelected" runat="server" Height="100" Width="130">
                        </asp:ListBox>
                        <span style="display: none">
                            <asp:TextBox ID="tbSortSelected" runat="server"></asp:TextBox></span>
                    </div>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td width="10%" class="right">
                            考试时间：
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblExamTimeType" runat="server" RepeatDirection="Horizontal"
                                OnSelectedIndexChanged="rblExamTimeType_SelectedIndexChanged" 
                                AutoPostBack="True">
                                <asp:ListItem Selected="True" Value="1">定时开始</asp:ListItem>
                                <asp:ListItem Value="2">设定时间区间</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                         <asp:Panel ID="plDateTimeType1" runat="server" Visible="true">
                         按各场次所设置的具体时间开始考试，准点开始考试。
                         </asp:Panel>
                            <asp:Panel ID="plDateTimeType2" runat="server" Visible="false">
                           
                            
                            <asp:TextBox runat="server" ID="tbStartTime" EnableTheming="false" CssClass="Wdate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"
                                Width="130px"></asp:TextBox>
                            至
                            <asp:TextBox runat="server" ID="tbStartOver" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"
                                Width="130px" EnableTheming="false" CssClass="Wdate" ></asp:TextBox>之间可以随时考试，各场次所设置的时间将不再起作用。 </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" class="right">
                           
                        </td>
                        <td>
                            <asp:CheckBox ID="cbExam_IsToggle" Text="允许切换窗口" runat="server" />
                            <asp:CheckBox ID="cbExam_IsShowBtn" Text="显示确认按钮" runat="server" />
                            <asp:CheckBox ID="cbExam_IsRightClick" Text="禁用右键" Checked="true" runat="server" />  
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" class="right">
                            场次：
                        </td>
                        <td>
                            <asp:Label ID="lbMaxnum" runat="server" Text="6" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="rblExamTimeType" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:GridView ID="GvItem" CssClass="GridView" name="examItems" runat="server" AutoGenerateColumns="False"
            OnRowDataBound="GvItem_RowDataBound">
            <EmptyDataTemplate>
                没有任何满足条件的考试！
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="序号">
                    <ItemStyle CssClass="center" Width="40px" />
                    <ItemTemplate>
                        <%# Container.DataItemIndex   +   1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="专业">
                    <ItemStyle Width="120px" />
                    <ItemTemplate>
                        <cc1:DropDownTree ID="ddlSubject" runat="server" Width="120px" IdKeyName="Sbj_ID"
                            ParentIdKeyName="Sbj_PID" TaxKeyName="Sbj_Tax">
                        </cc1:DropDownTree>
                        <span style="display: none">
                            <asp:Label ID="lbID" runat="server" Text='<%# Eval("Exam_ID","{0}")%>'></asp:Label></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="采用试卷">
                    <ItemStyle Width="150px" />
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlTestPager" runat="server" Width="150px">
                        </asp:DropDownList>
                        <span style="display: none">
                            <asp:TextBox ID="tbTestPager" runat="server" Text='<%# Eval("Tp_ID","{0}")%>'></asp:TextBox></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="标题">
                    <ItemStyle />
                    <ItemTemplate>
                        <asp:TextBox ID="tbName" runat="server" Width="100%" Text='<%# Eval("Exam_Name","{0}")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="及格/总分">
                    <ItemStyle CssClass="center" Width="80px" />
                    <ItemTemplate>
                        <asp:Label ID="lbPassScore" CssClass="lbPassScore" runat="server" Text='<%# Eval("Exam_PassScore","{0}")%>'></asp:Label>
                        <span style="display: none">
                            <asp:TextBox ID="tbPassScore" runat="server" Text='<%# Eval("Exam_PassScore","{0}")%>'></asp:TextBox></span>
                        /
                        <asp:Label ID="lbTotal" CssClass="lbTotal" runat="server" Text='<%# Eval("Exam_Total","{0}")%>'></asp:Label>
                        <span style="display: none">
                            <asp:TextBox ID="tbTotal" runat="server" Text='<%# Eval("Exam_Total","{0}")%>'></asp:TextBox></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="开始时间">
                    <ItemStyle CssClass="center" Width="130px" />
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="tbDate"  EnableTheming="false" CssClass="Wdate" Text='<%# Eval("Exam_Date","{0:yyyy-MM-dd HH:mm}")%>'
                            onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" Width="99%"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="限时">
                    <ItemStyle CssClass="center" Width="50px" />
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="tbSpan" Text='<%# Eval("Exam_Span","{0}")%>' datatype="uint"
                            Width="50px" ToolTip="单位：分钟"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="header" />
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="btnClose" runat="server" />
</asp:Content>
