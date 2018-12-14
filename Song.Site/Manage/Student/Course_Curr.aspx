<%@ Page Language="C#" MasterPageFile="~/Manage/Student/Parents.Master" AutoEventWireup="true"
    CodeBehind="Course_Curr.aspx.cs" Inherits="Song.Site.Manage.Student.Course_Curr"
    Title="课程列表" %>

<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="itemBox">
        <div class="itemTitle">
            当前学习的课程</div>
        <asp:Repeater ID="rptCourse" runat="server">
            <ItemTemplate>
                <div class="item">
                    <a class="logo" href="/course.ashx?id=<%# Eval("Cou_ID") %>" target="_blank">
                        <img src="<%# Eval("Cou_LogoSmall") %>" alt="<%# Eval("Cou_Name","{0}") %>" subject="<%# Eval("sbj_Name","[{0}]") %>" /></a>
                    <div class="infoBox">
                        <div class="courseName">
                            <a href="/course.ashx?id=<%# Eval("Cou_ID") %>" target="_blank"><%# Eval("Cou_Name","《{0}》") %></a></div>
                        <div class="itemName">
                            <a href="/Courses.ashx?sbj=<%# Eval("Sbj_ID","{0}") %>" target="_blank">
                                <%# Eval("sbj_Name","[专业：{0}]") %></a></div>
                        <div class="itemName">
                          课程期限：<%# getBuyInfo(Eval("Cou_ID"))%></div>
                        <div class="itemName">
                            最近学习时间：<%# GetLastTime(Eval("Cou_ID", "{0}"))%>  
                            &nbsp;&nbsp;累计学习时间：<%# GetstudyTime(Eval("Cou_ID", "{0}"))%>
                            &nbsp; <a href="#" class="logDetails" onclick="OpenWin('../student/StudyLog_Details.aspx?couid=<%# Eval("Cou_ID")%>',' 《<%# Eval("Cou_Name")%>》',980,80);return false;">
                                详情</a>
                        </div>
                        <div class="itemBtn">
                            <asp:LinkButton ID="lbSelected" CssClass='<%# Convert.ToBoolean(Eval("Cou_IsStudy")) ? "selected" : "noselect"%>'
                                runat="server" CommandArgument='<%# Eval("Cou_ID") %>' OnClick="lbSelected_Click" Visible="false">
                            
                            <%# Convert.ToBoolean(Eval("Cou_IsStudy")) ? "放弃学习" : "我要学习"%>
                            </asp:LinkButton>
                            <a href="/course.ashx?id=<%# Eval("Cou_ID") %>" target="_blank" class="btnStudy">继续学习
                            </a>
                        </div>
                    </div>
                </div>

            </ItemTemplate>
        </asp:Repeater>
        <asp:Panel ID="plNoCourse" runat="server" Visible="false">
        <div class="noCourse">没有学习的课程</div>
        </asp:Panel>
    </div>
    
</asp:Content>
