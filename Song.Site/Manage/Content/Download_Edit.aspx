<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Download_Edit.aspx.cs" Inherits="Song.Site.Manage.Content.Download_Edit"
    Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
  <script language="javascript" src="scripts/tab.js" type="text/javascript"></script>
  <div id="tabTitleBox">
    <div class="titItem curr" tab="tab1"> 基本信息</div>
    <div class="titItem" tab="tab2"> 详细信息</div>
    <div class="titItem" tab="tab3"> 图片展示</div>
    <div class="titItem" tab="tab4"> 发布设置</div>
    <div class="titItem" tab="tab5"> 自定义信息</div>
  </div>
  <div id="tabContextBox">
    <div class="contItem" tab="tab1">
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td class="right" width="80px"> 资源名称：</td>
          <td><asp:TextBox nullable="false" ID="tbName" lenlimit="2-25" runat="server" Width="95%"
                            MaxLength="100"  group="ent"></asp:TextBox>
          </td>
        </tr>
         <tr>
          <td class="right" > 所属栏目：</td>
          <td><asp:DropDownList ID="ddlColumn" runat="server" Width="150"> </asp:DropDownList>&nbsp;
            <asp:CheckBox ID="cbIsShow" runat="server" Checked="True" Text="显示" />
            <asp:CheckBox ID="cbIsRec" runat="server" Checked="True" Text="推荐" />
            <asp:CheckBox ID="cbIsHot" runat="server" Checked="True" Text="热点" />
            <asp:CheckBox ID="cbIsTop" runat="server" Checked="True" Text="置顶" />
          </td>
        <tr>
        <tr>
          <td class="right" valign="top"> 资源类别：</td>
          <td><asp:DropDownList ID="ddlSort" runat="server" Width="150"> </asp:DropDownList>
            
          </td>
        </tr>
        <tr>
          <td class="right" > 版本号：</td>
          <td><asp:TextBox nullable="false"  group="ent" ID="tbVersion" runat="server" Width="100px" MaxLength="50"></asp:TextBox>
          </td>
        <tr>
          <td class="right" > 更新时间：</td>
          <td><asp:TextBox nullable="false"  group="ent" ID="tbUpdateTime" runat="server" onfocus="WdatePicker()"
                                Width="100px" MaxLength="50"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td class="right" > 适用环境：</td>
          <td><asp:CheckBoxList ID="cblOS" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
              
            </asp:CheckBoxList>
          </td>
        </tr>
        <tr>
          <td valign="top" class="right" > 介绍：</td>
          <td><asp:TextBox ID="tbIntro" runat="server" Height="100px" MaxLength="255" TextMode="MultiLine"
                                Width="90%"></asp:TextBox>
          </td>
        </tr>
        <tr>
        <tr>
          <td class="right" > 所有者：</td>
          <td><asp:TextBox ID="tbAuthor" runat="server"></asp:TextBox>
          </td>
        </tr>
        <tr>
        <tr>
          <td class="right" > 标签：</td>
          <td><asp:TextBox ID="tbLabel" runat="server" Width="90%"></asp:TextBox>
          </td>
        </tr>
        <tr>
        <tr>
          <td class="right" > 上传资源：</td>
          <td><cc1:FileUpload ID="fuSource" runat="server" fileallow="rar|zip|apk" Width="80%" />
            <br />
            资源文件：
            <asp:HyperLink ID="hlSource" runat="server" Target="_blank" Font-Bold="True">[hlSource]</asp:HyperLink>
          </td>
        </tr>
      </table>
    </div>
    <div class="contItem" tab="tab2">
      <WebEditor:Editor ID="tbDetails" runat="server" Height="450px"> </WebEditor:Editor>
    </div>
    <div class="contItem" tab="tab3">
     <div>
                <cc1:FileUpload ID="fuLoad" runat="server" nullable="false" fileallow="jpg|bmp|gif|png" group="upfile"
                    Width="400" />
                <asp:Button ID="btnUpFile" runat="server" Text="上传" group="upfile" verify="true"
                    OnClick="btnUpFile_Click" />
            </div>
            <div class="imgList">
                <asp:Repeater ID="rptPict" runat="server">
                    <HeaderTemplate>
                        <dl>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <dd class="imgBox" style="width: 146px">
                            <div class="picShow" style="width: 140px; height: 140px;background-image:url(<%# Eval("Pic_FilePathSmall", "{0}") %>)"
                                imgid="<%# Eval("Pic_Id") %>">
                                <div style="display:<%# Eval("Pic_IsCover","{0}")=="True" ? "block" : "none" %>" class="cover" >封面</div></div>
                            <div class="control">
                                <asp:LinkButton ID="lbCover" runat="server" CommandArgument='<%# Eval("Pic_Id") %>'
                                    OnClick="lbCover_Click">设为封面</asp:LinkButton>
                                <asp:LinkButton ID="lbDel" runat="server" OnClientClick="return confirm('您确定删除吗？')"
                                    CommandArgument='<%# Eval("Pic_Id")%>' OnClick="lbDel_Click">删</asp:LinkButton>
                                <cc1:StateButton ID="sbUse" OnClick="sbShow_Click" runat="server" TrueText="显" FalseText="隐"
                                    State='<%# Eval("Pic_IsShow","{0}")=="True"%>' CommandArgument='<%# Eval("Pic_Id")%>'></cc1:StateButton>
                            </div>
                        </dd>
                    </ItemTemplate>
                    <FooterTemplate>
                        </dl>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
    </div>
    <div class="contItem" tab="tab4">
      <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
          <td width="80px" class="right"> Keywords：</td>
          <td><asp:TextBox ID="tbKeywords" runat="server" MaxLength="255" Width="95%"></asp:TextBox></td>
        </tr>
        <tr>
          <td class="right" valign="top"> Description：</td>
          <td><asp:TextBox ID="tbDescr" runat="server" MaxLength="255" TextMode="MultiLine" lenlimit="250"
                            Width="95%" Height="100px"></asp:TextBox></td>
        </tr>
        <tr>
          <td class="right"> 上线时间：</td>
          <td><asp:TextBox ID="tbPushTime" runat="server" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                            MaxLength="255" Width="160"></asp:TextBox></td>
        </tr>
        <tr>
          <td class="right" valign="top"><br />
            二维码：</td>
          <td><asp:Image ID="imgQrCode" runat="server" /></td>
        </tr>
      </table>
    </div>
    <div class="contItem" tab="tab5"> </div>
  </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter"  group="ent"/>
  <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
  <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
