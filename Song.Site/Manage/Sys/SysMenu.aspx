<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true" CodeBehind="SysMenu.aspx.cs" Inherits="Song.Site.Manage.Sys.SysMenu" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <script type="text/javascript" src="../CoreScripts/Tree.js"></script>
  <!--顶部条 -->
  <div id="contTop">
    <div id="treeName">管理界面右上方的下拉菜单：
      <asp:Label ID="lbName" runat="server" Text=""></asp:Label>
    </div>
    <!--预载进度条 -->
    <div id="loadingBar"><img src="../Images/loading/load_016.gif" /></div>
  </div>
  <!-- 左而菜单树的区域 -->
  <div id="MenuTreePanel"  onselectstart="return false"></div>
  <!--右侧编辑区域 -->
  <div id="MenuEditPanel">
    <!--编辑区域的标题栏 -->
    <div id="EditPanelTitle">
      <div id="editTitle" type="edit">编辑</div>
      <div id="addTitle" type="add">新增子节点</div>
    </div>
    <div id="Panel">
      <div>
        <!--根节点的编辑区域 -->
        <div id="RootEditPanel" class="editpanel"> 菜单树名称 (即 根节点) ：<br/>
          <input name="rootname" type="text" id="rootname" />
        </div>
        <!--子节点编辑区域 -->
        <div id="EditPanel" class="editpanel">
          <p>上级菜单项： <span id="editNodeParent" class="nodeParent"></span> </p>
          <div style="display:none"> <span edit="id"></span><span edit="pid"></span><span edit="tax"></span></div>
          <p>名 称：
            <input name="name" type="text" id="name" />
          </p>
          <p type="nodeType"> 类 别：
            <label>
            <input name="type" type="radio" id="type_0" value="item" checked="checked" />
            菜单项</label>
            <label>
            <input name="type" type="radio"  value="link" id="type_1" />
            外部链接</label>
            <label>
            <input name="type" type="radio"  value="event" id="type_3" />
            Javascript事件</label>
            <label>
            <input name="type" type="radio"  value="open" id="type_2" />
            弹出窗口</label> <span id="whArea" style="display:none">(宽高： <input name="width" type="text" class="numBox" id="width" value="400" size="5" maxlength="3" />
            × <input name="height" type="text" class="numBox" id="height" value="300" size="5"  maxlength="3" />
            px)
            </span>
          </p>
          <p>链 接：
            <input name="link" type="text" id="link" size="55" />
          </p>
          <p> 标 识：
            <input name="marker" type="text" id="marker" size="55" />
          </p>
          <p>说 明：<br/>
            <textarea name="intro" cols="50" rows="4" id="intro"></textarea>
          </p>
          <p>样式：
            <label>
            <input type="checkbox" name="cbIsBold" id="cbIsBold" />
            粗体
            <input type="checkbox" name="cbIsItalic" id="cbIsItalic" />
            斜体 </label>
          </p>
          <p>状态：
            <label>
            <input name="cbIsShow" type="checkbox" id="cbIsShow" checked="checked" />
            是否显示 </label>
            <label>
            <input name="cbIsUse" type="checkbox" id="cbIsUse" checked="checked" />
            是否使用 </label>
          </p>
          <p>&nbsp;</p>
        </div>
      </div>
      <!-- 增加子节点的区域 -->
      <div id="AddPanel" class="editpanel">
        <p>上级菜单项： <span id="addNodeParent" class="nodeParent"></span> </p>
        <div style="display:none"> <span add="id"></span><span edit="pid"></span><span edit="tax"></span></div>
        <p>名 称：
          <input name="addname" type="text" id="addname" />
        </p>
         <p type="nodeType">类 别：
          <label>
          <input name="addtype" type="radio" id="Radio1" value="item" checked="checked" />
          菜单项</label>
          <label>
          <input name="addtype" type="radio"  value="link" id="Radio2" />
          外部链接</label>
          <label>
          <input name="addtype" type="radio"  value="event" id="Radio4" />
          Javascript事件</label>
          <label>
          <input name="addtype" type="radio"  value="open" id="Radio3" />
          弹出窗口</label><span id="addwhArea" style="display:none">(宽高： <input name="addwidth" type="text" class="numBox" id="addwidth" value="400" size="5" maxlength="3" />
            × <input name="addheight" type="text" class="numBox" id="addheight" value="300" size="5"  maxlength="3" />
            px)
            </span>
        </p>
        <p>链 接：
          <input name="addlink" type="text" id="addlink" size="55" />
        </p>
        <p> 标 识：
          <input name="addmarker" type="text" id="addmarker" size="55" />
        </p>
        <p>说 明：<br/>
          <textarea name="addintro" cols="50" rows="4" id="addintro"></textarea>
        </p>
        <p>样式：
          <label>
          <input type="checkbox" name="addcbIsBold" id="addcbIsBold" />
          粗体
          <input type="checkbox" name="addcbIsItalic" id="addcbIsItalic" />
          斜体 </label>
        </p>
        <p>状态：
          <label>
          <input name="addcbIsShow" type="checkbox" id="addcbIsShow" checked="checked" />
          是否显示 </label>
          <label>
          <input name="addcbIsUse" type="checkbox" id="addcbIsUse" checked="checked" />
          是否使用 </label>
        </p>
        <p>&nbsp;</p>
      </div>
    </div>
  </div>
  <div id="show"><font color="red">*</font>拖动即可改变顺序，拖出所在区域即删除。</div>
  <!--以下为按钮区域 -->
  <div class="pageWinBtn">
    <input type="submit" name="ctl00$cphBtn$btnEnter" value="确定" id="ctl00_cphBtn_btnEnter" class="Button" />
  </div>
</asp:Content>
