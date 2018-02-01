<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Archives.aspx.cs" MasterPageFile="~/Manage/ManagePage.Master"
    Inherits="Song.Site.Manage.Teacher.Archives" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <div class="searchBox">
            考试主题：<asp:TextBox ID="tbTheme" runat="server"></asp:TextBox>
            时间：<asp:DropDownList ID="ddlTime" runat="server">
                <asp:ListItem Value="-1">--所有时间--</asp:ListItem>
                <asp:ListItem Value="1">今天</asp:ListItem>
                <asp:ListItem Value="2">本周</asp:ListItem>
                <asp:ListItem Value="3">本月</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <asp:Repeater ID="rtpExamThem" runat="server" 
        onitemdatabound="rtpExamThem_ItemDataBound">
        <ItemTemplate>
        <div class="examBox">
            <div class="themeBox">
              
                <div class="title">  <span class="tax">
                    <!--序号 -->
                    <%# Container.ItemIndex + Pager1.Size * (Pager1.Index - 1) + 1%>、
                </span>
                    <asp:Label ID="lbTitle" CssClass='<%# Eval("Exam_UID")%>' runat="server" Text='<%# Eval("Exam_Title")%>'></asp:Label></div>
                    <div class="viewScore" onclick="OpenWin('Statistics_Details.aspx?id=<%# Eval("Exam_ID") %>','《<%# Eval("Exam_Title")%>》-成绩详情',980,90);return false;">成绩详情</div>
            </div>
            <div class="groupBox">
                参考人员：<%# getGroupType(Eval("exam_grouptype").ToString(), Eval("exam_uid").ToString())%></div>
            <div class="examList">
                <div class="itemHeader">
                    <div class="name">
                        考试场次</div>
                    <div class="subject">
                        课程</div>
                    <div class="total">
                        满分</div>
                   <div class="span">
                       平均分</div>
                       <div class="men">
                       考生数</div>
                    <div class="btn">
                        &nbsp;</div>
                </div>
                <asp:Repeater ID="rtpExam" runat="server">
                    <ItemTemplate>
                        <div class="itemRow">
                            <div class="name">
                                <%# Eval("Exam_Name")%></div>
                            <div class="subject">
                               <%# Eval("sbj_Name")%></div>
                            <div class="total">
                                 <%# Eval("exam_total")%></div>
                           <div class="span">
                       <%# getAvg4Exam(Eval("Exam_ID"))%>分</div>
                        <div class="men">
                       <%# getNumber4Exam(Eval("Exam_ID"))%>人</div>
                            <div class="btn" id="btnCorrect" runat=server visible='<%# getIsCorrect(Eval("Exam_ID"))%>'>
                                <a class="button" onclick="OpenWin('correct.aspx?id=<%# Eval("Exam_ID")%>','<%# Eval("Exam_Name","考试：《{0}》")%>',980,95);return false;" href="#">批阅试卷</a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <uc2:Pager ID="Pager1" runat="server" Size="10" OnPageChanged="BindData" />
</asp:Content>
