/*!
* 主  题：《前端控件》
* 说  明：常用控件的增强。
* 功能描述：
* 1、常用控件的美化与增强；
* 2、增加控件的交互、验证功能；
*
*
* 作  者：宋雷鸣
* 开发时间: 2016年5月22日
*/
(function () {
    $(function () {
        Widget.Initialize();
    });
    //初始化
    function Widget() { };
    Widget.Initialize = function () {
        $("widget").each(function () {
            //替换widget节点为div
            $(this).parent().append("<div></div>");
            var div = $(this).parent().find("div:last");
            Widget.attrCopy($(this), div).html($(this).html());
            div.insertBefore($(this));
            var type = $(this).attr("type");
            if (type == "text") Widget.ForText(div);
            if (type == "password") Widget.ForText(div);
            if (type == "textarea") Widget.ForTextarea(div);
            $(this).remove();
            div.css({ "border": "none" });
        });
    };
    //控件的属性处理，将属性加到新生成的控件上
    Widget.attrCopy = function (wid, control) {
        //返回元素的属性集合（此为二维数组，0为属性名，1为属性值）
        var getAttrs = function (wid) {
            var list = wid.get(0).attributes;
            var arrayObj = new Array();
            for (var i = 0; i < list.length; i++)
                arrayObj.push(new Array(list[i].name, list[i].value));
            return arrayObj;
        };
        //属性处理
        var pretrans = function (wid, control) {
            var mainAttrs = getAttrs(wid); //主元素属性
            var ctlAttr = getAttrs(control); //子元素履性
            for (var i = 0; i < mainAttrs.length; i++) {
                var isexist = false;
                for (var j = 0; j < ctlAttr.length; j++) {
                    if (mainAttrs[i][0] == ctlAttr[j][0]) {
                        isexist = true;
                        break;
                    }
                }
                if (!isexist) control.attr(mainAttrs[i][0], mainAttrs[i][1]);
            }
            return control;
        };
        return pretrans(wid, control);
    };
    //生成控件前面的图标
    Widget.buildICO = function (wid, ctl, width, height) {
        var ico = $.trim(wid.attr("ico"));
        var icoClass = "iconfont";
        if (ico == "") return;
        if (ico.indexOf(" ") > -1) {
            icoClass = ico.substring(ico.indexOf(" "));
            ico = ico.substring(0, ico.indexOf(" "));
        }
        width = width < 1 ? ctl.height() : 40;
        height = height < 1 ? ctl.height() : 40;
        wid.prepend("<div class='WidgetICO " + icoClass + "'>" + ico + "</div>");
        var icobox = wid.find(".WidgetICO");
        icobox.css({ width: width, height: ctl.height(),
            float: 'left', "line-height": height + "px","font-size": "28px",
            'border-style': 'solid', 'border-width': '1px 1px 1px 1px',
            'border-right-style': 'none', 'border-color': ctl.css('border-left-color')
        });
        //alert(ctl.css('border-top-color'));
        var ctlWd = wid.innerWidth() - icobox.outerWidth(true) - (ctl.outerWidth(true) - ctl.width());
        ctl.css({ float: 'left', width: ctlWd <= 0 ? 200 : ctlWd });
    };
    //输入框的处理，包括文本输入框、密码输入框
    Widget.ForText = function (wid) {
        //查询是否已经存在控件
        var type = wid.attr("type");
        wid.css("position", "relative");
        var control = wid.find("input[type=" + type + "]");
        if (control.size() < 1) {
            wid.append("<input type=\"" + type + "\"/>");
            control = wid.find("input[type=" + type + "]");
        }
        Widget.attrCopy(wid, control);
        Widget.buildICO(wid, control, 0, 0);
        //消息框事件
        var setTextInfo = function (txtName, ctl, deftxt) {
            ctl.parent().append("<div id='" + txtName + "_show' class='WidgetShow'></div>");
            var txt = $("#" + txtName + "_show");
            //设置消息的位置
            var offset = ctl.offset();
            txt.css({ "position": "absolute", "z-index": 2000, "text-align": "left",
                top: 0, left: 40, width: ctl.width(), height: ctl.height(), color:"#666",
                "line-height": ctl.height() + "px", "font-size": ctl.css("font-size"), "text-indent": ctl.css("text-indent")
            }).unbind("click").html(deftxt).click(function () {
                var name = $(this).attr("id");
                name = name.substring(0, name.indexOf("_"));
                $("input[name=" + name + "]").focus();
                $(this).remove();
            });
            return txt;
        };
        //消息框初始显示
        if ($.trim(control.val()) == "") {
            var def = $.trim(wid.attr("deftxt"));
            if (def != "") setTextInfo(control.attr("name"), control, def).show();
        }
        control.blur(function () {
            if ($.trim(control.val()) == "") {
                setTextInfo(control.attr("name"), control, $.trim(wid.attr("deftxt"))).show();
            }
        }).focus(function () {
            setTextInfo(control.attr("name"), control, null).remove();
        });
    }
    Widget.ForTextarea = function (wid) {
        //查询是否已经存在控件
        var control = wid.find("textarea");
        wid.css("position", "relative");
        var hg = wid.attr("height");
        if (control.size() < 1) {
            wid.append("<textarea style='height:" + hg + "px;'/>");
            control = wid.find("textarea");
        }
        Widget.attrCopy(wid, control);
        Widget.buildICO(wid, control, 40, 40);
        //消息框事件
        var setTextInfo = function (txtName, ctl, deftxt) {
            ctl.parent().append("<div id='" + txtName + "_show' class='WidgetShow'></div>");
            var txt = $("#" + txtName + "_show");
            //txt.insertAfter(ctl);
            //设置消息的位置
            var offset = ctl.offset();
            txt.css({ "position": "absolute", "z-index": 2000, "text-align": "left",
                top: 0, left: 40, width: ctl.width(), height: 40,
                "line-height": 40 + "px", "font-size": ctl.css("font-size"), "text-indent": ctl.css("text-indent")
            }).unbind("click").html(deftxt).click(function () {
                var name = $(this).attr("id");
                name = name.substring(0, name.indexOf("_"));
                $("textarea").focus();
                $(this).remove();
            });
            return txt;
        };
        //消息框初始显示
        if ($.trim(control.val()) == "") {
            var def = $.trim(wid.attr("deftxt"));
            if (def != "") setTextInfo(control.attr("name"), control, def).show();
        }
        control.blur(function () {
            if ($.trim(control.val()) == "") {
                setTextInfo(control.attr("name"), control, $.trim(wid.attr("deftxt"))).show();
            }
        }).focus(function () {
            setTextInfo(control.attr("name"), control, null).remove();
        });
    }
})();
