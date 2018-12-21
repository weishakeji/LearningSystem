<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Students_Courses.aspx.cs" Inherits="Song.Site.Manage.Admin.Students_Courses"
    Title="学员课程" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<div class="itemBox">
        <div class="itemTitle">
            
    <asp:Label ID="lbAccName" runat="server" Text=""></asp:Label> 选修的课程
            <input id="btnPrint" type="button" value=" 打 印 " href="../student/Students_Details.aspx?id=<%= id %>" />
            </div>
        <asp:Repeater ID="rptCourse" runat="server">
            <ItemTemplate>
                <div class="item" isfree="<%# GetStc(Eval("Cou_ID","{0}")).Stc_IsFree %>" istry="<%# GetStc(Eval("Cou_ID","{0}")).Stc_IsTry %>">
                    <a class="logo" href="/course.ashx?id=<%# Eval("Cou_ID") %>" target="_blank">
                        <img src="<%# Eval("Cou_LogoSmall") %>" alt="<%# Eval("Cou_Name","{0}") %>" subject="<%# Eval("sbj_Name","[{0}]") %>" /></a>
                    <div class="infoBox">
                        <div class="courseName">
                            <a href="/course.ashx?id=<%# Eval("Cou_ID") %>" target="_blank"><%# Eval("Cou_Name","《{0}》") %></a></div>
                        <div class="itemName">
                            <a href="/Courses.ashx?sbj=<%# Eval("Sbj_ID","{0}") %>" target="_blank">
                                <%# Eval("sbj_Name","[专业：{0}]") %></a></div>
                        <div class="itemName">
                          课程期限：<%# Eval("Cou_ID", "{0}")%><%# getBuyInfo(Eval("Cou_ID"))%></div>
                        <div class="itemName">
                            <span>最近学习时间：<%# GetLastTime(Eval("Cou_ID", "{0}"))%>  </span>
                            <span>累计学习时间：<%# GetstudyTime(Eval("Cou_ID", "{0}"))%>  </span>
                            <span>完成度：<%# GetComplete(Eval("Cou_ID", "{0}"))%>%  </span>
                            <a href="#" class="logDetails" onclick="OpenWin('../student/StudyLog_Details.aspx?couid=<%# Eval("Cou_ID")%>&acid=<%= id %>',' 《<%# Eval("Cou_Name")%>》',980,80);return false;">
                                查看详情</a>
                        </div>
                        <div class="itemBtn">
                            <asp:LinkButton ID="lbSelected" CssClass='<%# Convert.ToBoolean(Eval("Cou_IsStudy")) ? "selected" : "noselect"%>'
                                runat="server" CommandArgument='<%# Eval("Cou_ID") %>' OnClick="lbSelected_Click" Visible="true">
                            
                            <%# Convert.ToBoolean(Eval("Cou_IsStudy")) ? "放弃学习" : "没有学习"%>
                            </asp:LinkButton>
                           
                        </div>
                    </div>
                </div>

            </ItemTemplate>
        </asp:Repeater>
        <asp:Panel ID="plNoCourse" runat="server" Visible="false">
        <div class="noCourse">该学员未选修任何课程</div>
        </asp:Panel>
    </div>
    
       <iframe src="" id="iframeExportDetails" link="Students_Details.aspx?sts=-1" scrolling="auto" style="display:none;" height="30" width="100%"></iframe>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
