<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true" CodeBehind="Depart.aspx.cs" Inherits="Song.Site.Manage.Sys.Depart" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <%--<script type="text/javascript" src="../JavaScript/MenuTree.js"></script>--%>
  <script type="text/javascript" src="Scripts/Depart/DepartTree.js"></script>
  <script type="text/javascript" src="Scripts/Depart/DepartNode.js"></script>
  <!--顶部条 -->
  <div id="contTop">
    <!--预载进度条 -->
    <div id="loadingBar"><img src="../Images/loading/load_016.gif" /></div>
  </div>
  <!-- 左而菜单树的区域 -->
  <div id="MenuTreePanel"  onselectstart="return false"></div>
  <!--右侧编辑区域 -->
  <div id="MenuEditPanel">
    <!--编辑区域的标题栏 -->
    <div id="EditPanelTitle">
      <div id="editTitle" type="edit">修改院系信息</div>
      <div id="addTitle" type="add">添加下级院系</div>
      <%--<div id="purviewTitle" type="purview" target="purview.aspx">管理权限</div>--%>
     <%-- <div id="artPurTitle" type="purview" target="Depart_ArtColumn.aspx">文章编辑权限</div>--%>
    </div>
    <div id="Panel">
      <!--子节点编辑区域 -->
      <div id="EditPanel" class="editpanel">
        <p>上级菜单项： <span id="editNodeParent" class="nodeParent"></span> </p>
        <div style="display:none"> <span edit="id"></span><span edit="pid"></span><span edit="tax"></span></div>
        <p>
          <label>院系名称：
          <input name="name" type="text" id="name" />
          </label>
          <label>中文简称：
          <input name="CnAbbr" type="text" id="CnAbbr" size="10" />
          </label>
        </p>
        <label>英文名称：
        <input name="enname" type="text" id="enname" />
        </label>
        <label>英文简称：
        <input name="EnAbbr" type="text" id="EnAbbr" size="10" />
        </label>
        <p>
          <label>院系代码：
          <input name="code" type="text" id="code" size="10" />
          </label><br/>
         状态： <label>
          <input name="cbIsShow" type="checkbox" id="cbIsShow" checked="checked" />
          是否显示 </label>
           <label>
          <input name="cbIsUse" type="checkbox" id="cbIsUse" checked="checked" />
          是否启用 </label>
        </p>
        <p>职能介绍：<br/>
          <textarea name="func" cols="50" rows="4" id="func"></textarea>
        </p>
        <p>联系方式：<br/>
          <label>电话：
          <input name="phone" type="text" id="phone" />
          </label>
          <br/>
          <label>传真：
          <input name="fax" type="text" id="fax" />
          </label>
          <br/>
          <label>邮箱：
          <input name="email" type="text" id="email" />
          </label>
          <br/>
          <label>Msn：
          <input name="msn" type="text" id="msn" />
          </label>
          <br/>
          <label>办公地址：
          <input name="WorkAddr" type="text" id="WorkAddr" size="40" />
          </label>
          <br/>
        </p>
        <p>&nbsp;</p>
      </div>
      <!-- 增加子节点的区域 -->
      <div id="AddPanel" class="editpanel">
        <p>上级院系： <span id="addNodeParent" class="nodeParent"></span> </p>
        <div style="display:none"> <span add="id"></span><span edit="addpid"></span><span edit="tax"></span></div>
        <p>
          <label>院系名称：
          <input name="addname" type="text" id="addname" />
          </label>
          <label>中文简称：
          <input name="addCnAbbr" type="text" id="addCnAbbr" size="10" />
          </label>
        </p>
        <label>英文名称：
        <input name="addenname" type="text" id="addenname" />
        </label>
        <label>英文简称：
        <input name="addEnAbbr" type="text" id="addEnAbbr" size="10" />
        </label>
        <p>
          <label>院系代码：
          <input name="addcode" type="text" id="addcode" size="10" />
          </label><br/>状态：
          <label>
          <input name="addcbIsShow" type="checkbox" id="addcbIsShow" checked="checked" />
          是否显示 </label>
           <label>
          <input name="addcbIsUse" type="checkbox" id="addcbIsUse" checked="checked" />
          是否启用 </label>
        </p>
        <p>职能介绍：<br/>
          <textarea name="addfunc" cols="50" rows="4" id="addfunc"></textarea>
        </p>
        <p>联系方式：<br/>
          <label>电话：
          <input name="addphone" type="text" id="addphone" />
          </label>
          <br/>
          <label>传真：
          <input name="addfax" type="text" id="addfax" />
          </label>
          <br/>
          <label>邮箱：
          <input name="addemail" type="text" id="addemail" />
          </label>
          <br/>
          <label>Msn：
          <input name="addmsn" type="text" id="addmsn" />
          </label>
          <br/>
          <label>办公地址：
          <input name="addWorkAddr" type="text" id="addWorkAddr" size="40" />
          </label>
          <br/>
        </p>
        <p>&nbsp;</p>
      </div>
    </div>
  </div>
  <div id="show"><font color="red">*</font>拖动即可改变顺序，拖出所在区域即删除。</div>
  <!--以下为按钮区域 -->
  <div class="pageWinBtn">
    <label>
    <input type="button" name="btnEnter" id="btnEnter" value="确定"  class="Button"/>
    </label>
   
  </div>
</asp:Content>
