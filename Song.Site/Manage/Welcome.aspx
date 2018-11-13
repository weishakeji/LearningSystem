<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="Song.Site.Manage.Welcome" %>
<%@ Register Src="Utility/QrBuilder.ascx" TagName="QrBuilder" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html>
<html>
<head runat="server">
<title>无标题页</title>
</head>
<body>
<script type="text/javascript" src="/Utility/echarts/echarts-all.js"></script>
<form id="form1" runat="server">
  <dl class="numbox">
    <dd>
      <dl class="num">
        <dt>课程数量</dt>
        <dd><%= ViewState["counum"] %></dd>
      </dl>
    </dd>
    <dd>
      <dl class="num">
        <dt>试题数量</dt>
        <dd><%= ViewState["quesnum"] %></dd>
      </dl>
    </dd>
    <dd>
      <dl class="num">
        <dt>学员数量</dt>
        <dd><%= ViewState["accnum"] %></dd>
      </dl>
    </dd>
    <dd>
      <dl class="num">
        <dt>在线人数</dt>
        <dd><%= ViewState["onlinenum"] %></dd>
      </dl>
    </dd>
  </dl>
  <div class="ChartsArea">
    <div id="courCharts" class="charts" style="height: 400px; width: 45%;"> </div>
    <div id="courChartsPie" class="charts"  style="height: 400px; width: 45%;"> </div>
  </div>
  <div class="ChartsArea">
    <div id="accoutsCharts" class="charts" style="height: 400px; width: 95%;"> </div>
  </div>
  <div class="ChartsArea">
    <div id="quesCharts" class="charts"  style="height: 400px; width: 95%;"> </div>
  </div>
  <%--数据输出--%>
  <div id="dataArea" style="display: none">
    <div id="course">
      <asp:Repeater ID="rptCourse" runat="server">
        <ItemTemplate>
          <row>
            <orgid><%# Eval("Org_ID")%></orgid>
            <name><%# Eval("Org_Name")%></name>
            <pname><%# Eval("Org_PlatformName")%></pname>
            <abbr><%# Eval("Org_AbbrName")%></abbr>
            <count><%# Eval("count") %></count>
          </row>
        </ItemTemplate>
      </asp:Repeater>
    </div>
    <div id="accouts">
      <asp:Repeater ID="rptAccouts" runat="server">
        <ItemTemplate>
          <row>
            <orgid><%# Eval("Org_ID")%></orgid>
            <name><%# Eval("Org_Name")%></name>
            <pname><%# Eval("Org_PlatformName")%></pname>
            <abbr><%# Eval("Org_AbbrName")%></abbr>
            <count><%# Eval("count") %></count>
          </row>
        </ItemTemplate>
      </asp:Repeater>
    </div>
    <div id="question">
      <asp:Repeater ID="rptQues" runat="server">
        <ItemTemplate>
          <row>
            <orgid><%# Eval("Org_ID")%></orgid>
            <name><%# Eval("Org_Name")%></name>
            <pname><%# Eval("Org_PlatformName")%></pname>
            <abbr><%# Eval("Org_AbbrName")%></abbr>
            <count><%# Eval("count") %></count>
          </row>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </div>
  <div class="footer"> <span>硬件环境：CPU
    <asp:Literal ID="ltHz" runat="server"></asp:Literal>
    GHz×
    <asp:Literal
            ID="ltCpucount" runat="server"></asp:Literal>
    内存
    <asp:Literal ID="ltRamSize" runat="server"></asp:Literal>
    Gb</span> <span>软件环境：
    <asp:Literal ID="ltOs" runat="server"></asp:Literal>
    IIS
    <asp:Literal ID="ltIISver" runat="server"></asp:Literal>
    .Net
    <asp:Literal ID="ltDotNetver" runat="server"></asp:Literal>
    </span> <span>主机地址：
    <asp:Literal
                    ID="ltIp" runat="server"></asp:Literal>
    </span> <span>物理路径：
    <asp:Literal ID="ltPath" runat="server"></asp:Literal>
    </span> </div>
</form>
</body>
</html>
