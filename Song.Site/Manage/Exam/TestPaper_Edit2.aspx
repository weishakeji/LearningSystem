<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="TestPaper_Edit2.aspx.cs" Inherits="Song.Site.Manage.Exam.TestPaper_Edit2"
    Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <asp:ScriptManager ID="ScriptManager1" runat="server"  AsyncPostBackTimeout="300"> </asp:ScriptManager>
  <asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
      <div class="loadingBox">
        <p>正在加载<br />
          Loading...</p>
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>
  <div class="quesLeft">
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
      <tr>
        <td width="80px" class="right"> 试卷标题： </td>
        <td colspan="4"><asp:TextBox ID="tbName" runat="server" nullable="false" MaxLength="200" Width="70%"></asp:TextBox>
        <span class="tpType" style="float: right">试卷类型：<b>随机试题</b></span></td>
      </tr>
      <tr>
        <td class="right"> 副标题： </td>
        <td colspan="2"><asp:TextBox ID="tbSubName" runat="server" MaxLength="200" Width="400px"></asp:TextBox>
          &nbsp;
          <asp:CheckBox ID="cbIsUse" runat="server" Text="启用" state="true" Checked="true" ToolTip="启用后，学员才可以进行使用" />
          <asp:CheckBox ID="cbIsRec" runat="server" Text="推荐" state="true" ToolTip="推荐优先显示" /></td>
        <td width="100" rowspan="4" valign="top" style="text-align: right"> <img src="../Images/nophoto.jpg" name="imgShow" width="100" height="100" id="imgShow"
                        runat="server" /><br />
          <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png" Width="100" /></td>
      </tr>
      <tr>
        <td class="right"> 专业/课程： </td>
        <td colspan="3"><asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
              <cc1:DropDownTree ID="ddlSubject" runat="server" IdKeyName="Sbj_ID" ParentIdKeyName="Sbj_PID"
                                TaxKeyName="Sbj_Tax" AutoPostBack="True" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged"> </cc1:DropDownTree>
              &nbsp;课程：
              <cc1:DropDownTree ID="ddlCourse" runat="server" IdKeyName="Cou_ID" ParentIdKeyName="Cou_PID"
                                TaxKeyName="Cou_Tax" AutoPostBack="True" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged"> </cc1:DropDownTree>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="ddlSubject" EventName="SelectedIndexChanged" />
              <asp:AsyncPostBackTrigger ControlID="ddlCourse" EventName="SelectedIndexChanged" />
            </Triggers>
          </asp:UpdatePanel></td>
      </tr>
      <tr>
        <td class="right"> 限时： </td>
        <td colspan="3"><asp:TextBox ID="tbSpan" runat="server" nullable="false" datatype="uint" MaxLength="4"
                        Width="60px"></asp:TextBox>
          分钟&nbsp;&nbsp;总分：
          <asp:TextBox ID="tbTotal" runat="server" nullable="false" datatype="uint" MaxLength="6"
                        Width="60px">100</asp:TextBox>
          &nbsp;&nbsp;及格分：
          <asp:TextBox ID="tbPassScore" runat="server" nullable="false" datatype="uint"
                        MaxLength="6" Width="60px"></asp:TextBox>
          &nbsp;&nbsp;难易度：
          <asp:DropDownList ID="ddlDiff" runat="server">
            <asp:ListItem Value="1">1</asp:ListItem>
            <asp:ListItem Value="2">2</asp:ListItem>
            <asp:ListItem Value="3" Selected="true">3</asp:ListItem>
            <asp:ListItem Value="4">4</asp:ListItem>
            <asp:ListItem Value="5">5</asp:ListItem>
          </asp:DropDownList></td>
      </tr>
      <tr>
        <td class="right"> 组卷人： </td>
        <td colspan="3"><asp:TextBox ID="tbAuthor" runat="server" Width="60px"></asp:TextBox></td>
      </tr>
    </table>
    <!--简介-->
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
      <tr>
        <td width="80px" class="right"> 试卷简介： </td>
        <td>（字数：<span class="count"></span>）</td>
         <td width="80px" class="right">注意事项： </td>
        <td>（字数：<span class="count"></span>）</td>
      </tr>
      <tr>
        <td colspan="2"><WebEditor:Editor ID="tbIntro" runat="server" Height="100px" ThemeType="mini" Width="99%"
                        afterChange="function(){K('.count').html(this.count('text'))}"></WebEditor:Editor></td>
        <td colspan="2"><WebEditor:Editor ID="tbRemind" runat="server" Height="100px" ThemeType="mini" Width="99%"
                        afterChange="function(){K('.count').html(this.count('text'))}"></WebEditor:Editor></td>
      </tr>
    </table>
    <!--出题范围-->
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
      <tr>
        <td width="80px" class="right"> 出题范围： </td>
        <td><asp:RadioButtonList ID="rblFromType" runat="server" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
            <asp:ListItem Selected="True" Value="0">当前课程所有试题 </asp:ListItem>
            <asp:ListItem Value="1">按章节出题 </asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <tr>
        <td></td>
        <td><%--当前课程所有试题--%>
          <dl class="ItemForAll">
            <asp:Repeater ID="rptItemForAll" runat="server">
              <ItemTemplate>
                <dd class="itemRow" index="<%#Container.ItemIndex  %>">
                  <div> <%# GetDataItem()%> ：
                    <asp:TextBox ID="tbItemCount" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox>
                    道试题，
                    占总分
                    <asp:TextBox ID="tbItemScore" runat="server" datatype="uint" MaxLength="3" Width="60px"></asp:TextBox>
                    %，计
                    <asp:Label ID="lbItemNumber" runat="server" Text="0"></asp:Label>
                    分；<span style="display: none">
                    <asp:TextBox
                                            ID="tbItemNumber" runat="server"></asp:TextBox>
                    </span> </div>
                </dd>
              </ItemTemplate>
            </asp:Repeater>
          </dl>
          <%--按章节出题--%>
          <div class="ItemForOutline">
            <div class="PercentBox"> <span class="PercentBoxTit"> 各题型占总分百分比： </span>
              <asp:Repeater ID="rptOutlineScore" runat="server">
                <ItemTemplate> <span index="<%#Container.ItemIndex  %>"> <%# GetDataItem()%>
                  <asp:TextBox ID="tbQuesScore" runat="server" datatype="uint" MaxLength="3" Width="40px"></asp:TextBox>
                  %&nbsp;&nbsp; </span> </ItemTemplate>
              </asp:Repeater>
            </div>
            <asp:UpdatePanel ID="UpdatePanel_ItemForOutline" runat="server" ChildrenAsTriggers="False"
                            UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Repeater ID="rptOutline" runat="server">
                  <ItemTemplate>
                    <div class="olBox">
                      <div class="olname"> <%#Container.ItemIndex+1  %>. <%# Eval("Ol_Name")%> <span style="display:none">
                        <asp:Label ID="lbOlid" runat="server" Text='<%# Eval("Ol_ID")%>'></asp:Label>
                        </span></div>
                      <div class="olCountBox">
                        <asp:Repeater ID="rtpOutlineItem" runat="server">
                          <ItemTemplate> <span index="<%#Container.ItemIndex  %>"> <%# GetDataItem()%>
                            <asp:TextBox ID="tbQuesCount" runat="server" datatype="uint" MaxLength="3" Width="40px"></asp:TextBox>
                            道；&nbsp; </span> </ItemTemplate>
                        </asp:Repeater>
                      </div>
                    </div>
                  </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="ltNoOutline" runat="server" Visible="false">当前课程没有章节。</asp:Literal>
              </ContentTemplate>
            </asp:UpdatePanel>
          </div></td>
      </tr>
    </table>
  </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
  <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
  <cc1:CloseButton ID="btnClose" runat="server" />
</asp:Content>
