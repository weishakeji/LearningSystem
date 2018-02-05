<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SelfExam.aspx.cs" MasterPageFile="~/Manage/ManagePage.Master"
    Inherits="Song.Site.Manage.Exam.SelfExam" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <fieldset id="todayExam">
        <legend>今天的考试</legend>
        <asp:Repeater ID="rptExamStart" runat="server">
            <ItemTemplate>
                <div class="examBox click" examid=" <%# Eval("Exam_ID")%>">
                    <div class="examRow">
                        <div class="examTit">
                            考试主题：</div>
                        <div class="examCont" tag="theme">
                            <%# Eval("Exam_Title")%>
                        </div>
                    </div>
                    <div class="examRow">
                        <div class="examTit">
                            学科/专业：</div>
                        <div class="examCont" tag="subject">
                            <%# Eval("Sbj_Name")%>
                        </div>
                    </div>
                    <div class="examRow">
                        <div class="examTit" tag="date">
                            开始时间：</div>
                        <div class="examCont">
                            <%# Eval("Exam_Date", "{0:yyyy-MM-dd HH:mm}")%>
                        </div>
                    </div>
                    <div class="examRow">
                        <div class="examTit">
                            考试时长：</div>
                        <div class="examCont">
                            <%# Eval("Exam_Span")%>
                            分钟</div>
                    </div>
                    <div class="examRow">
                        <div class="examTit">
                            考试人员：</div>
                        <div class="examCont" tag="group">
                            <%# getGroupType(Eval("Exam_GroupType", "{0}"), Eval("Exam_UID", "{0}"))%>
</div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </fieldset>
    <fieldset>
        <legend>近期考试</legend>
        <asp:Repeater ID="rtpExamList" runat="server">
            <ItemTemplate>
                <div class="examBox">
                    <div class="examRow">
                        <div class="examTit">
                            考试主题：</div>
                        <div class="examCont">
                            <%# Eval("Exam_Title")%>
                        </div>
                    </div>
                    <div class="examRow">
                        <div class="examTit">
                            学科/专业：</div>
                        <div class="examCont">
                            <%# Eval("Sbj_Name")%>
                        </div>
                    </div>
                    <div class="examRow">
                        <div class="examTit">
                            开始时间：</div>
                        <div class="examCont">
                            <%# Eval("Exam_Date", "{0:yyyy-MM-dd HH:mm}")%>
                        </div>
                    </div>
                    <div class="examRow">
                        <div class="examTit">
                            考试时长：</div>
                        <div class="examCont">
                            <%# Eval("Exam_Span")%>
                            分钟</div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </fieldset>
</asp:Content>
