<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysMenu.aspx.cs" Inherits="Song.Site.Manage.Sys.SysMenu" Title="无标题页" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
<title>管理登录</title>
<meta http-equiv="Pragma" content="no-cache" />
<meta http-equiv="Cache-Control" content="no-cache" />
<meta http-equiv="Expires" content="0" />
<link href="/manage/Styles/PageWin.css" rel="stylesheet" type="text/css" />
</head>
<body>

<div class="pageWinContext" loyout="row">
  <div style="margin: 10px; margin-bottom:0px;height:auto;"> 
    <script type="text/javascript" src="../CoreScripts/Tree.js"></script> 
    <script>
        document.body.onselectstart = document.body.oncontextmenu = function () { return false; };
        document.body.onpaste = document.body.oncopy = function () { return false; };
        </script>
    <div id="icoPanel"> </div>
    <!--顶部条 -->
    <div id="contTop">
      <div id="treeName"> 当前菜单树名称：
        <asp:Label ID="lbName" runat="server" Text=""></asp:Label>
      </div>
      <!--预载进度条 -->
      <div id="loadingBar"> <img src="../Images/loading/load_016.gif" /></div>
    </div>
    <!-- 左而菜单树的区域 -->
    <div id="MenuTreePanel" onselectstart="return false"> </div>
    <!--右侧编辑区域 -->
    <div id="MenuEditPanel"> 
      <!--编辑区域的标题栏 -->
      <div id="EditPanelTitle">
        <div id="editTitle" type="edit"> 编辑</div>
        <div id="addTitle" type="add"> 新增子节点</div>
      </div>
      <div id="Panel">
        <div> 
          <!--根节点的编辑区域 -->
          <form id="RootEditPanel" class="editpanel">
            <dt>上级菜单项： 无（根节点） </dt>
            <dd class="namebar">
              <div class="name"> 名称：
                <input name="name" type="text" width="200" nullable="false"/>
              </div>
              <div class="icoslect">
                <div class="txt"> 图标：</div>
                <div class="ico"> &nbsp;</div>
              </div>
            </dd>
            <dd> 说 明：
              <input name="intro" type="text" id="intro" value="" size="55" />
            </dd>
            <dd> 样式：
              <label>
                <input type="checkbox" name="isbold"/>
                粗体</label>
              <label>
                <input type="checkbox" name="isitalic"/>
                斜体 </label>
            </dd>
            <dd> 状态：
              <label>
                <input name="isshow" type="checkbox" checked="checked" />
                是否显示 </label>
              <label>
                <input name="isuse" type="checkbox" checked="checked" />
                是否使用 </label>
            </dd>
          </form>
          <!--子节点编辑区域 -->
          <form id="EditPanel" class="editpanel">
            <dt>上级菜单项： <span id="editNodeParent" class="nodeParent"></span></dt>
            <dd style="display: none"> <span name="id"></span>=<span name="patid"></span>-<span name="tax"></span> </dd>
            <dd class="namebar">
              <div class="name"> 名称：
                <input name="name" type="text" width="200" nullable="false"/>
              </div>
              <div class="icoslect">
                <div class="txt"> 图标：</div>
                <div class="ico"> &nbsp;</div>
              </div>
            </dd>            
            <dd> 类 别：
              <label>
                <input name="type" type="radio" value="item" checked="checked" />
                菜单项</label>
              <label>
                <input name="type" type="radio" value="link">
                外链接</label>
              <label>
                <input name="type" type="radio" value="open" />
                弹窗</label>
              (
              <input name="winwidth" type="text" class="numBox"  value="400" size="2" maxlength="3" />
              ×
              <input name="winheight" type="text" class="numBox" value="300" size="2"
                                maxlength="3" />
              px)
              <label>
                <input name="type" type="radio" value="event" />
                Js事件</label>
              <label>
                <input name="type" type="radio" value="node" />
                节点</label>
            </dd>
            <dd> 链 接：
              <input name="link" type="text" size="55" />
            </dd>
            <dd> 标 识：
              <input name="marker" type="text" size="55" />
            </dd>
            <dd> 说 明：
              <input name="intro" type="text" value="" size="55" />
            </dd>
             <dd> 样式：
              <label>
                <input type="checkbox" name="isbold"/>
                粗体</label>
              <label>
                <input type="checkbox" name="isitalic"/>
                斜体 </label>
            </dd>
            <dd> 状态：
              <label>
                <input name="isshow" type="checkbox" checked="checked" />
                是否显示 </label>
              <label>
                <input name="isuse" type="checkbox" checked="checked" />
                是否使用 </label>
            </dd>
          </form>
        </div>
        <!-- 增加子节点的区域 -->
        <form id="AddPanel" class="editpanel">
        <div id="patdata">
          <dd>上级菜单项： <span id="addNodeParent" name="name" read="no" class="nodeParent"></span></dd>
          <dd style="display: none"> <span name="id"></span>-<span name="tax"></span> </dd>
          </div>
          <dd class="namebar">
            <div class="name"> 名称：
              <input name="name" type="text" width="200" nullable="false"/>
            </div>
            <div class="icoslect">
              <div class="txt"> 图标：</div>
              <div class="ico"> &nbsp;</div>
            </div>
          </dd>
          <dd> 类 别：
              <label>
                <input name="type" type="radio" value="item" checked="checked" />
                菜单项</label>
              <label>
                <input name="type" type="radio" value="link">
                外链接</label>
              <label>
                <input name="type" type="radio" value="open" />
                弹窗</label>
              (
              <input name="winwidth" type="text" class="numBox"  value="400" size="2" maxlength="3" />
              ×
              <input name="winheight" type="text" class="numBox" value="300" size="2"
                                maxlength="3" />
              px)
              <label>
                <input name="type" type="radio" value="event" />
                Js事件</label>
              <label>
                <input name="type" type="radio" value="node" />
                节点</label>
            </dd>
          <dd> 链 接：
            <input name="link" type="text" size="55" />
          </dd>
          <dd> 标 识：
            <input name="marker" type="text" size="55" />
          </dd>
          <p> 说 明：
            <input name="intro" type="text" value="" size="55" />
            </dd>
           <dd> 样式：
              <label>
                <input type="checkbox" name="isbold"/>
                粗体</label>
              <label>
                <input type="checkbox" name="isitalic"/>
                斜体 </label>
            </dd>
            <dd> 状态：
              <label>
                <input name="isshow" type="checkbox" checked="checked" />
                是否显示 </label>
              <label>
                <input name="isuse" type="checkbox" checked="checked" />
                是否使用 </label>
            </dd>
         
          </dd>
        </form>
      </div>
    </div>
    <div id="show"> <font color="red">*</font>拖动即可改变顺序，拖出所在区域即删除。</div>
    <div id="alert"> 操作成功 </div>
  </div>
</div>

<div class="pageWinBtn" style="text-align: right" loyout="row" height="50">
  <input type="image" name="btnEnter" id="btnEnter" class="btnEnterButton" verify="true" Text="确定" src="../Images/empty.gif" />
  <input type="image" name="CloseButton" id="btnCloseWin" class="btnCloseWin" src="../Images/empty.gif" onclick="new top.PageBox().Close();" />
</div>
<script type="text/javascript">
    $(".pageWinContext").height(document.documentElement.clientHeight - $(".pageWinBtn").height());
    //当窗口大小变化时
    $(window).resize(function () {
        $(".pageWinContext").height(document.documentElement.clientHeight - $(".pageWinBtn").height());
    });
    </script>

</body>
</html>