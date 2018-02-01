<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="TestPaper_Edit.aspx.cs" Inherits="Song.Site.Manage.Exam.TestPaper_Edit"
    Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <div class="topBox">
    <div class="typeBox">
      <div class="typeName" href="TestPaper_Edit1.aspx?typd=1&<%= query %>">统一试题</div>
      <div class="typeIntro">
        <ol>
          <li>试题内容固定，包括试题与得分。</li>
          <li>所有学员参与考试时，试卷内容相同。</li>
          <li>统一试题的试卷编辑时稍复杂。</li>
        </ol>
      </div>
    </div>
    <div class="typeBox">
      <div class="typeName" href="TestPaper_Edit2.aspx?typd=2&<%= query %>">随机试题</div>
      <div class="typeIntro">
        <ol>
          <li>试题随机抽取、动态计算。</li>
          <li>学员在参与考试时，试卷内容才会生成，每个学员面临的试卷内容不同，每次考试也不同。</li>
          <li>试题随机的试卷编辑更简单。</li>
        </ol>
      </div>
    </div>
  </div>
  <div class="footerBox">
    <p>*试卷类型一旦选择，将不可更改。</p>

  </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <cc1:CloseButton ID="btnClose" runat="server" />
</asp:Content>
