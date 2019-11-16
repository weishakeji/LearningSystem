<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Index.aspx.cs" Inherits="Song.Site.Manage.Index" Theme="" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
<title>管理登录</title>
<meta http-equiv="Pragma" content="no-cache" />
<meta http-equiv="Cache-Control" content="no-cache" />
<meta http-equiv="Expires" content="0" />
<link href="Styles/Index.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="CoreScripts/jquery.js"></script>
<script type="text/javascript" src="Scripts/Index.js"></script>
</head>
<body>
<div id="topbar">
<a class="topitem" href="/">首页</a>
</div>
<div id="topShow">
  <!--顶部图片 -->
</div>
<form id="form1" runat="server" defaultbutton="imgBtn">
  <div id="loginPanel">
    <div id="loginTitle"> <img src="Images/Index/lock.png" />
      <div class="appName"><span id="consName" runat="server">在线学习系统</span> - 管理中心 </div>
    </div>
    <div id="LoginBox">
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td width="80" class="attr">帐 号：</td>
          <td class="left"><asp:TextBox ID="tbAccName" runat="server" MaxLength="50" CssClass="accName" TabIndex="1"
                                Wrap="False"></asp:TextBox>
            <font color="#ff0000">*</font> </td>
          <td rowspan="3" valign="top"><div id="BtnPanel">
              <asp:ImageButton ID="imgBtn" CssClass="imgBtn" ImageUrl="~/Manage/Images/index/loginbtn.jpg"
                            runat="server" TabIndex="4" OnClick="btnEnter_Click" validationgroup="enter" />
            </div>
            <div id="copyright"><a href="<%= copyright["url"]%>" target="_blank" title="<%= copyright["compay"]%>"><%= copyright["abbr"]%></a><br/><%= copyright["tel"]%><br/>
            
            </div></td>
        </tr>
        <tr>
          <td class="attr">密 码：</td>
          <td class="left"><asp:TextBox ID="tbPw1" runat="server" MaxLength="50" CssClass="pw" TextMode="Password" 
                                TabIndex="2"></asp:TextBox>
            <font color="#ff0000">*</font> </td>
        </tr>
        <tr>
          <td class="attr">验证码：</td>
          <td class="left"><div class="codeInput"><asp:TextBox ID="tbCode" runat="server" CssClass="veri" MaxLength="4" Width="60px" TabIndex="3"></asp:TextBox>
            <font color="#ff0000">*</font> </div><img id="codeImg" src="Utility/CodeImg.aspx?len=4&name=logindex" title="点击更换验证码" /></td>
        </tr>
        <tr>
          <td class="attr"></td>
          <td><div id="ShowPanel"> &nbsp; <span id="showtext" style="color: red;"></span>
              <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="enter"
                            ErrorMessage="验证码错误" OnServerValidate="CustomValidator1_ServerValidate" ControlToValidate="tbCode"
                            Display="Dynamic" SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
              <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="tbPw1"
                            Display="Dynamic" ErrorMessage="密码错误" SetFocusOnError="True" ValidationGroup="enter"></asp:CustomValidator>
            </div></td>
            <td>&nbsp;</td>
        </tr>
      </table>
     
    </div>
  </div>
  
</form>
</body>
</html>
