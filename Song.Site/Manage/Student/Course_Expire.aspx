<%@ Page Language="C#" MasterPageFile="~/Manage/Student/Parents.Master" AutoEventWireup="true"
    CodeBehind="Course_Expire.aspx.cs" Inherits="Song.Site.Manage.Student.Course_Expire"
    Title="课程列表" %>

<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="itemBox" id="divCourseExpire" runat="server">
        <div class="itemTitle">
            过期课程</div>
        <asp:Repeater ID="rptCourseExpire" runat="server">
            <ItemTemplate>
                <div class="item" isfree="<%# GetStc(Eval("Cou_ID","{0}")).Stc_IsFree %>" istry="<%# GetStc(Eval("Cou_ID","{0}")).Stc_IsTry %>">
                    <a class="logo" href="/course.ashx?id=<%# Eval("Cou_ID") %>" target="_blank">
                        <img src="<%# Eval("Cou_LogoSmall") %>" alt="<%# Eval("Cou_Name","{0}") %>" subject="<%# Eval("sbj_Name","[{0}]") %>" /></a>
                    <div class="infoBox">
                        <div class="courseName">
                            <%# Eval("Cou_Name","《{0}》") %><span></span></div>
                        <div class="itemName">
                            <a href="/Courses.ashx?sbj=<%# Eval("Sbj_ID","{0}") %>" target="_blank">
                                <%# Eval("sbj_Name","[专业：{0}]") %></a></div>
                        <div class="itemName">
                            累计学习时间：<%# GetstudyTime(Eval("Cou_ID", "{0}"))%>
                            &nbsp; <a href="#" class="logDetails" onclick="OpenWin('../student/StudyLog_Details.aspx?couid=<%# Eval("Cou_ID")%>',' 《<%# Eval("Cou_Name")%>》',980,80);return false;">
                                详情</a></div>
                        <div class="itemName">
                            最近学习时间：<%# GetLastTime(Eval("Cou_ID", "{0}"))%>
                        </div>
                        <div class="itemBtn">
                            <asp:LinkButton ID="lbSelected" CssClass='<%# Convert.ToBoolean(Eval("Cou_IsStudy")) ? "selected" : "noselect"%>'
                                runat="server" CommandArgument='<%# Eval("Cou_ID") %>' OnClick="lbSelected_Click">
                            
                            <%# Convert.ToBoolean(Eval("Cou_IsStudy")) ? "删除课程" : "我要学习"%>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <asp:Panel ID="plNoCourse" runat="server" Visible="false">
        <div class="noCourse">
            没有过期的课程</div>
    </asp:Panel>
</asp:Content>
