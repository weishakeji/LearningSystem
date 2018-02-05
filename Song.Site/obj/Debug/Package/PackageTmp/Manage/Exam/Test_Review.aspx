<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Test_Review.aspx.cs" Inherits="Song.Site.Manage.Exam.Test_Review"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <script language="javascript" type="text/javascript">
//当前服务器端时间
var ServerTime=new Date("<%= new WeiSha.Common.Param.Method.ConvertToAnyValue(DateTime.Now.ToString()).JavascriptTime %>");
//当前客户端时间
var ClientTime=new Date();
//试卷ID
var testPagerID=<%= pager.Tp_Id %>;
//考试ID
var examID=0;
//唯一ID
var uid="<%= uid %>";
//考生ID
var stID=<%= st.Ac_ID %>;
    </script>
    <script type="text/javascript" src="Scripts/Mask.js"></script>
    <script type="text/javascript" src="Scripts/QuesLoad.js"></script>
 
<div id="examHeader">
    <fieldset id="emplyee">
    <legend>考生信息</legend>
    <img width="100" height="133" id="empBoxPhoto" src="<%= acc.Acc_Photo%>" />
    <div class="empName">
      <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
          <td width="40" class="right"> 考生：</td>
          <td><%= acc.Acc_Name%> </td>
        </tr>
        <tr>
          <td class="right"> 工号： </td>
          <td><%= acc.Acc_EmpCode%> </td>
        </tr>
        <tr>
          <td class="right"> 部门： </td>
          <td><%= acc.Dep_CnName%> </td>
        </tr>
        <tr>
          <td class="right"> 班组： </td>
          <td><%= acc.Team_Name%> </td>
        </tr>
      </table>
    </div>
    </fieldset>
    <fieldset id="examInfo">
    <legend>考试回顾</legend>
    
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td width="80" class="right"> 考试主题：</td>
            <td> 《<%= exr.Exam_Title%> 》 </td>
          </tr>
          <tr>
            <td width="80" class="right"> 试卷名称：</td>
            <td> 《<%= pager.Tp_Name%> 》 </td>
          </tr>
          <tr>
            <td class="right"> 学科/专业： </td>
            <td><%= pager.Sbj_Name%><span id="sbjid" style="display: none"> <%= pager.Sbj_ID%></span>  </td>
          </tr>
               <tr>
            <td class="right"> 时长： </td>
            <td><span id="timeSpan"> <%=pager.Tp_Span%>  </span>分钟 / <%=pager.Tp_Count%>  </span>道题 </td>
          </tr>
          <tr>
            <td class="right"> 卷面得分： </td>
            <td><span id="Span1"> <%=exr.Exr_Score%> 分 </span> </td>
          </tr>
        </table>
     
    </fieldset>

    <fieldset id="quesState">
    <legend>答题状态 </legend>
    <div id="stateBox">
      <dl>
      </dl>
    </div>
    <div id="runState">
      <div id="runShow"> 
        </div>
      <div id="btnBox">
       
        <div id="btnClose" onclick="new parent.PageBox().Close();"><%--关闭--%>&nbsp;</div>
        
      </div>
    </div>
    </fieldset>
  </div>
    <%--试题展示区--%>
  <div id="quesArea">
    <dl>
    </dl>
  </div>
  <%--控制区--%>
  <div id="controlBox">
  
    <div id="btnNext" class="btn"> <%--下一题--%> &nbsp;</div>
    <div id="btnPrev" class="btn"> <%--上一题--%> &nbsp;</div>
  </div>
</asp:Content>

