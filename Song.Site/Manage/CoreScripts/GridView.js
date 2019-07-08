//当页面加载时
function GridView_Init() {
    //取gridview
    //var gv=$(".GridView");
    var gv = $("table[ControlType='GridView']");
    if (gv.length == 0) {
        return;
    }
    //gv.hide();
    //设置一般性样式
    setStyle(gv);
    //全选、反选、取消三事件
    setSelectEvent(gv);
    //设置行事件，如悬停、点击事件
    setRowEvent(gv);
    //
    var tr = gv.find("tr[RowType='DataRow']");
    ChangeRowState(tr, "select");
    //
    gv.show();
}

//设置一般性样式
//gv:GridView表格
function setStyle(gv) {
    //取gridview
    var gv = $(".GridView");
    //头部样式
    gv.find("tr:first").addClass("header");
    //尾部部样式
    gv.find("tr[RowType='Footer']").addClass("footer");
    //数据行样式
    gv.find("tr[RowType='DataRow']").addClass("DataRow");
    //复数行
    gv.find("tr[RowType='DataRow']:odd").addClass("DataRowOdd");
    //单数行
    gv.find("tr[RowType='DataRow']:even").addClass("DataRowEven");
    //复选框box所在单元格宽度与样式
    gv.find("td[ItemType='SelectBox']").width(40);
    gv.find("td[ItemType='SelectBox']").attr("align", "center");
    gv.find("tr:first td:first").attr("align", "center");
    //脚底选择按钮
    var handler = gv.find("td[ItemType='SelectHandler']");
    var handTxt = handler.html();
    handTxt = " <a href='#'>全选</a> /";
    handTxt += " <a href='#'>反选</a> /";
    handTxt += " <a href='#'>取消</a>";
    handler.html(handTxt);
}
//设置选择事件(全选、反选、取消)
function setSelectEvent(gv) {
    //左上角的复选框
    var handler = gv.find("#selectBox");
    //全选、反选消
    handler.click(function () {
        var state = $(this).prop("checked");
        //选择操作的名称
        var name = $.trim($(this).text());
        var td = gv.find("tr[RowType='DataRow'] td[ItemType='SelectBox']");
        //复选框的集合
        var box = td.find("span[ControlType='SelectBox'] input[type='checkbox']");
        if (state)
            box.prop("checked", "checked");
        else
            box.removeAttr("checked");
        //对行状态进行操作
        var tr = gv.find("tr[RowType='DataRow']");
        ChangeRowState(tr, "select");
    });
}
//设置行事件，如悬停、点击事
function setRowEvent(gv) {
    var row = gv.find("tr[RowType='DataRow']");
    //鼠标悬停样式
    row.hover(function () {
        var isClick = $(this).attr("isClick");
        if (isClick == "true") return;
        //将原有css样式记录下来
        var oldCss = $(this).attr("class");
        $(this).attr("oldCss", oldCss);
        $(this).attr("class", "RowOver");
    }, function () {
        var isClick = $(this).attr("isClick");
        if (isClick == "true") return;
        var oldCss = $(this).attr("oldCss");
        $(this).attr("class", oldCss);
    });
    //鼠标单击事件
    row.click(function () {
        ChangeRowState($(this), "click");
    });
    //获取工具条,如果没有工具条，则不加载双击事件
    var toolbar = $(".toolsBar");
    if (toolbar.size() > 0) {
        //鼠标双击
        row.dblclick(function () {
            //var id = $(this).attr("DataKey");
            var keyvalue = $(this).attr("EncryptKey");
            var keyname = $(this).attr("PrimaryKey");
            if (keyvalue == null) return false;
            OnRowDbClick(keyname, keyvalue, null, window.name);
            return false;
        });
        //行内的编辑按钮
        gv.find(".RowEdit").click(function () {
            var isJsEvent = $(this).attr("IsJsEvent") == "false" ? false : true;
            if (isJsEvent == false) return true;
            var keyvalue = $(this).parents("tr").attr("EncryptKey");
            var keyname = $(this).parents("tr").attr("PrimaryKey");
            if (keyvalue == null) return false;
            OnRowDbClick(keyname, keyvalue, "编辑", window.name);
            return false;
        });
        gv.find(".RowView").click(function () {
            var keyvalue = $(this).parents("tr").attr("EncryptKey");
            var keyname = $(this).parents("tr").attr("PrimaryKey");
            if (keyvalue == null) return false;
            OnRowDbClick(keyname, keyvalue, "查看", window.name);
            return false;
        });
    }
}
//改变行状态
//tr:数据行集合
//eventSource:事件来源，是点击click，还是选择select
function ChangeRowState(tr, eventSource) {
    tr.each(function () {
        var td = $(this).find("td[ItemType='SelectBox']");
        var box = td.find("span[ControlType='SelectBox'] input[type='checkbox']");
        //如果是行点击来的事件
        if (eventSource == "click") {
            //当前复选框是否选中
            var val = box.prop("checked");
            //当前行，是否处于点中状态
            var isClick = $(this).attr("isClick");
            if (isClick == "true") {
                var oldCss = $(this).attr("oldCss");
                $(this).attr("class", oldCss);
                $(this).attr("isClick", "false");
                box.removeAttr("checked");
            } else {
                $(this).attr("class", "onClick");
                $(this).attr("isClick", "true");
                box.prop("checked", "checked");
            }
        }
        //如果来自于选择操作
        if (eventSource == "select") {
            //当前复选框是否选中
            var val = box.prop("checked");
            //当前行，是否处于点中状态
            var isClick = $(this).attr("isClick");
            if (val == true) {
                //复选框为选中状态，行没有选中
                if (isClick != "true") {
                    //将原有css样式记录下来
                    var oldCss = $(this).attr("class");
                    $(this).attr("oldCss", oldCss);
                    $(this).attr("class", "onClick");
                    $(this).attr("isClick", "true");
                    //box.prop("checked","checked");
                }
            } else {
                if (isClick == "true") {
                    var oldCss = $(this).attr("oldCss");
                    $(this).attr("class", oldCss);
                    $(this).attr("isClick", "false");
                    //box.removeAttr("checked");
                }
            }
        }
    });
}

//获取选中的行,返回主键id，如果没有主键，返回-1，否则以逗号分隔的字符串；如1,3,8
//gvName：GridView的id;
function GetKeyValues(gvName) {
    var gv = $("table[ControlId='" + gvName + "']");
    var tr = gv.find("tr[RowType='DataRow']");
    //主键值
    var keys = "";
    tr.each(function () {
        //行主键
        var key = $(this).attr("DataKey");
        if (key == null || key == "") return -1;
        //复选框
        var cb = $(this).find("td:first span[ControlType='SelectBox'] input[type='checkbox']");
        if (cb.prop("checked")) {
            keys += key + ",";
        }
    });
    if (keys.indexOf(",") > -1) {
        keys = keys.substr(0, keys.lastIndexOf(","));
    }
    return keys;
}