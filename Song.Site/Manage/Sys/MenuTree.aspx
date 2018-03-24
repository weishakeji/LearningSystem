<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="MenuTree.aspx.cs" Inherits="Song.Site.Manage.Sys.MenuTree" Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <script type="text/javascript" src="../CoreScripts/Tree.js"></script>
  <div id="icoPanel"></div>
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
        <div id="RootEditPanel" class="editpanel">
          <p> 上级菜单项： 无（根节点） </p>
          <div class="namebar">
            <div class="name">名称：
              <input name="rootname" type="text" width="200" id="rootname" />
            </div>
            <div class="icoslect"><div class="txt">图标：</div><div class="ico">&nbsp;</div></div>
          </div>
          <p> 说 明：<br />
            <textarea name="rtintro" cols="50" rows="5" id="intro"></textarea>
          </p>
          <p>样式：
            <label>
            <input type="checkbox" name="rtcbIsBold" id="rtcbIsBold" />
            粗体
            <input type="checkbox" name="rtcbIsItalic" id="rtcbIsItalic" />
            斜体 </label>
          </p>
          <p>状态：
            <label>
            <input name="rtcbIsShow" type="checkbox" id="rtcbIsShow" checked="checked" />
            是否显示 </label>
            <label>
            <input name="rtcbIsUse" type="checkbox" id="rtcbIsUse" checked="checked" />
            是否使用 </label>
          </p>
        </div>
        <!--子节点编辑区域 -->
        <div id="EditPanel" class="editpanel">
          <p> 上级菜单项： <span id="editNodeParent" class="nodeParent"></span> </p>
          <div style="display: none"> <span edit="id"></span><span edit="pid"></span><span edit="tax"></span> </div>
          <div class="namebar">
            <div class="name">名称：
              <input name="name" type="text" width="200" id="name" />
            </div>
            <div class="icoslect"><div class="txt">图标：</div><div class="ico">&nbsp;</div></div>
          </div>
          <p> 移动到：
            <asp:DropDownList ID="ddlMove" runat="server"> </asp:DropDownList>
            复制到：
            <asp:DropDownList ID="ddlCopy" runat="server"> </asp:DropDownList>
          </p>
          <p> 类 别：
            <label>
            <input name="type" type="radio" id="type_0" value="item" checked="checked" />
            菜单项</label>
            <label>
            <input name="type" type="radio" value="link" id="type_1" />
            外部链接</label>
            <label>
            <input name="type" type="radio"  value="open" id="type_2" />
            弹出窗口</label>
            <input name="type" type="radio"  value="node" id="type_3" />
            节点</label>
          </p>
          <p> 链 接：
            <input name="link" type="text" id="link" size="55" />
          </p>
          <p> 标 识：
            <input name="marker" type="text" id="marker" size="55" />
          </p>
          <p> 说 明：<br />
            <textarea name="intro" cols="50" rows="5" id="intro"></textarea>
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
          <p>&nbsp; </p>
        </div>
      </div>
      <!-- 增加子节点的区域 -->
      <div id="AddPanel" class="editpanel">
        <p> 上级菜单项： <span id="addNodeParent" class="nodeParent"></span> </p>
        <div style="display: none"> <span add="id"></span><span edit="pid"></span><span edit="tax"></span> </div>
         <div class="namebar">
            <div class="name">名称：
              <input name="addname" type="text" width="200" id="addname" />
            </div>
            <div class="icoslect"><div class="txt">图标：</div><div class="ico">&nbsp;</div></div>
          </div>
        <p> 类 别：
          <label>
          <input name="addtype" type="radio" id="addtype_0" value="item" checked="checked" />
          菜单项</label>
          <label>
          <input name="addtype" type="radio" value="link" id="addtype_1" />
          外部链接</label>
          <label>
          <input name="addtype" type="radio"  value="open" id="addtype_2" />
          弹出窗口</label>
        </p>
        <p> 链 接：
          <input name="addlink" type="text" id="addlink" size="55" />
        </p>
        <p> 标 识：
          <input name="addmarker" type="text" id="addmarker" size="55" />
        </p>
        <p> 说 明：<br />
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
        <p>&nbsp; </p>
      </div>
    </div>
  </div>
  <div id="show"> <font color="red">*</font>拖动即可改变顺序，拖出所在区域即删除。</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <cc1:EnterButton  ID="btnEnter" runat="server"  Text="确定"/>
  <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
