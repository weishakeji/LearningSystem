<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Columns.aspx.cs" Inherits="Song.Site.Manage.Content.Columns" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<div style="height:600px">
    <script type="text/javascript" src="../CoreScripts/Tree.js"></script>

    <!--顶部条 -->
    <div id="contTop">
        <!--预载进度条 -->
        <div id="loadingBar">
            <img src="../Images/loading/load_016.gif" /></div>
    </div>
    <div id="show">
        <font color="red">*</font>拖动即可改变顺序，拖出所在区域即删除。</div>
    <!-- 左侧菜单树的区域 -->
    <div id="MenuTreePanel" onselectstart="return false">
    </div>
    <!--右侧编辑区域 -->
    <div id="MenuEditPanel">
        <!--编辑区域的标题栏 -->
        <div id="EditPanelTitle">
            <div id="editTitle" type="edit">
                修改栏目信息</div>
            <div id="addTitle" type="add">
                添加下级栏目</div>
        </div>
        <div id="Panel">
            <!--子节点编辑区域 -->
            <div id="EditPanel" class="editpanel">
                <p>
                    <b>上级栏目：</b> <span id="editNodeParent" class="nodeParent"></span>
                </p>
                <div style="display: none">
                    <span edit="pid"></span><span edit="tax"></span>
                </div>
                <p>
                    <label>
                        <b>栏目名称：</b>
                        <input name="name" type="text" style="width: 200px" nullable="false" group="save"
                            maxlength="200" id="name" />
                    </label>
                    ID：<span edit="id"></span>
                </p>
                <p>
                    <label>
                        <b>栏目别名：</b>
                        <input name="byname" type="text" style="width: 200px" maxlength="200" id="byname" />
                    </label>
                </p>
                <p class="typeBox" style="display:none">
                    <label>
                        <b>类型：</b>
                        <input type="radio" name="type" checked="checked" value="News" id="type_0" />
                        资讯</label>
                    <label>
                        <input type="radio" name="type" value="Product" id="type_1" />
                        产品</label>
                    <label>
                        <input type="radio" name="type" value="Picture" id="type_2" />
                        图片</label>
                    <label>
                        <input type="radio" name="type" value="Video" id="type_3" />
                        视频</label>
                    <label>
                        <input type="radio" name="type" value="Download" id="type_4" />
                        下载</label>
                    <label>
                        <input type="radio" name="type" value="Article" id="type_5" />
                        单页</label>
                    <br />
                </p>
                <p>
                    <label>
                        <b>栏目标题：</b>
                        <input name="title" type="text" style="width: 300px" maxlength="200" id="title" />
                    </label>
                </p>
                <p>
                    <label>
                        <b>关键字：</b>
                        <input name="keywords" type="text" style="width: 300px" maxlength="200" id="keywords" />
                    </label>
                </p>
                <p>
                    <label>
                        <b>描述：</b>
                        <textarea name="desc" style="width: 300px; height: 80px" id="desc"></textarea>
                    </label>
                </p>
                <p>
                    <label>
                        <b>介绍：</b>
                        <textarea name="intro" style="width: 300px; height: 80px" id="intro"></textarea>
                    </label>
                </p>
                <p>
                    <b>状态：</b>
                    <label>
                        <input name="cbIsUse" type="checkbox" id="cbIsUse" checked="checked" />
                        是否启用
                    </label>
                    <label>
                        <input name="cbIsNote" type="checkbox" id="cbIsNote" checked="checked" />
                        是否允许评论
                    </label>
                </p>
                <p>
                    &nbsp;</p>
                <p>
                    <b>&nbsp;</b>
                    <input type="submit" name="btnSave" id="btnSave" value="修改栏目属性" verify="true" group="save"
                        class="Button" />
                </p>
            </div>
            <!-- 增加子节点的区域 -->
            <div id="AddPanel" class="editpanel">
                <p>
                    <b>上级栏目：</b> <span id="addNodeParent" class="nodeParent"></span>
                </p>
                <div style="display: none">
                    <span edit="addpid"></span><span edit="tax"></span>
                </div>
                <p>
                    <label>
                        <b>新增栏目：</b>
                        <input name="addname" type="text" style="width: 200px" nullable="false" group="add"
                            maxlength="200" id="addname" />
                    </label>
                </p>
                <p>
                    <label>
                        <b>栏目别名：</b>
                        <input name="addbyname" type="text" style="width: 200px" maxlength="200" id="addbyname" />
                    </label>
                </p>
                <p class="typeBox" style="display:none">
                    <label>
                        <b>类型：</b>
                        <input type="radio" name="addtype" checked="checked" value="News" id="addtype_0" />
                        资讯</label>
                    <label>
                        <input type="radio" name="addtype" value="Product" id="addtype_1" />
                        产品</label>
                    <label>
                        <input type="radio" name="addtype" value="Picture" id="addtype_2" />
                        图片</label>
                    <label>
                        <input type="radio" name="addtype" value="Video" id="addtype_3" />
                        视频</label>
                    <label>
                        <input type="radio" name="addtype" value="Download" id="addtype_4" />
                        下载</label>
                    <label>
                        <input type="radio" name="addtype" value="Article" id="addtype_5" />
                        单页</label>
                    <br />
                </p>
                <p>
                    <label>
                        <b>栏目标题：</b>
                        <input name="addtitle" type="text" style="width: 300px" maxlength="200" id="addtitle" />
                    </label>
                </p>
                <p>
                    <label>
                        <b>关键字：</b>
                        <input name="addkeywords" type="text" style="width: 300px" maxlength="200" id="addkeywords" />
                    </label>
                </p>
                <p>
                    <label>
                        <b>描述：</b>
                        <textarea name="adddesc" style="width: 300px; height: 80px" id="adddesc"></textarea>
                    </label>
                </p>
                <p>
                    <label>
                        <b>介绍：</b>
                        <textarea name="addintro" style="width: 300px; height: 80px" id="addintro"></textarea>
                    </label>
                </p>
                <p>
                    <b>状态：</b>
                    <label>
                        <input name="addcbIsUse" type="checkbox" id="addcbIsUse" checked="checked" />
                        是否启用
                    </label>
                    <label>
                        <input name="addcbIsNote" type="checkbox" id="addcbIsNote" checked="checked" />
                        是否允许评论
                    </label>
                </p>
                <p>
                    &nbsp;</p>
                <p>
                    <b>&nbsp;</b>
                    <input type="submit" name="btnAdd" id="btnAdd" value="添加栏目" verify="true" group="add"
                        class="Button" />
                </p>
            </div>
        </div>
    </div></div>
</asp:Content>
