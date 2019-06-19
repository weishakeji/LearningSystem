<%@ Page Language="C#" MasterPageFile="~/Manage/Course/CourseEdit.Master" AutoEventWireup="true"
    CodeBehind="Courses_Guide.aspx.cs" Inherits="Song.Site.Manage.Course.Courses_Guide" %>

<%@ MasterType VirtualPath="~/Manage/Course/CourseEdit.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div id="guide">
        <iframe id="leftIframe" src="GuideColumns.aspx?couid=<%=couid %>" width="300" height="550"
            marginwidth="0" marginheight="0" scrolling="auto" frameborder="0"></iframe>
   
    <%--右侧区域--%>
  
        <iframe id="rightIframe" src="GuideContent.aspx?couid=<%=couid %>" width="100%" height="550"
            marginwidth="0" marginheight="0" scrolling="auto" frameborder="0"></iframe>
   </div>
</asp:Content>
