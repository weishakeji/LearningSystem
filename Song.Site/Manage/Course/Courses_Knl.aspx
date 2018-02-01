<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Course/CourseEdit.Master"
    AutoEventWireup="true" CodeBehind="Courses_Knl.aspx.cs" Inherits="Song.Site.Manage.Course.Courses_Knl" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

         <div loyout="column" width="300">
         <iframe id="leftIframe" src="KnlColumns.aspx?couid=<%=couid %>" width="300" height="550" marginwidth="0" marginheight="0"
                scrolling="auto" frameborder="0"></iframe>
            <%--<cc1:ListBoxTree ID="ddlKnowledgeSorts" runat="server" Width="180" Height="500" IdKeyName="Kns_ID"
                ParentIdKeyName="Kns_PID" TaxKeyName="Kns_Tax" TypeKeyName="Kns_Type" state="true">
            </cc1:ListBoxTree>--%>
       </div>
        <div id="rightBox"  loyout="column"  overflow="auto">
            <iframe id="rightIframe" src="" width="100%" height="500" marginwidth="0" marginheight="0"
                scrolling="auto" frameborder="0"></iframe>
        </div>
   
</asp:Content>
