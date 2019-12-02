/*!
 * 主  题：《表单录入验证》
 * 说  明：通过控件属性实现验证，包括生成星号标注、非法提示等；
 * 功能描述：
 * 1、当录入框必填时，在控件后生成红色星号（设置star=false时不显示星号）；
 * 2、根据控件属性判断需要录入的数据格式，如果非法则阻止提交并弹出提示
 * 3、支持分组验证
 * 4、可脱离微厦在线学习系统独立使用（需要JQuery库支持）
 *
 * 作  者：微厦科技_宋雷鸣_10522779@qq.com
 * 开发时间: 2013年8月5日
 * 最后修订：2018年2月4日
 * github开源地址:https://github.com/weishakeji/Verify_Js
 */
(function () {
    var verify = {
        version: "1.6",  //版本号
        //默认属性值
        attr: {
            place: "bottom",     //提示信息显示位置，默认为下方，可以设置right
            backgroundcolor: "#FF3300",     //背景色
            foregroundcolor: "#FFFFFF",      //前景色
            showheight: 25,        //提示框的高度
            showsize: 13        //提示框字体大小
        },
        //验证的错误提示
        errmsg: {
            nullable: { def: "不能为空", msg1: "请选择", msg2: "该项必须选中" },
            lenlimit: { def: "限制{0}字符", msg1: "超出{0}个字符", msg2: "请录入{0}-{1}个字符" },
            numlimit: { def: "请输入数字", msg1: "请输入小于等于{0}的数字", msg2: "请录入{0}-{1}之间的数字", msg3: "只能录入{0}" },
            sametarget: { def: "两次输入不相同" },
            novalue: { def: "值不合法" },
            datatype: {
                def: "值不合法！",
                uint: "请输入正整数",
                number: "请输入数字",
                float: "请输入浮点数",
                tel: "请输入正确的电话号码",
                mobile: "请输入移动电话号码",
                zip: "请输入邮政编码",
                user: "请用字符开头的字母数字组合",
                email: "请输入正确的电子信箱",
                idcard: "请输入正确的身份证信息",
                url: "请输入合法的网络地址",
                qq: "请输入合规的QQ号码",
                chinese: "请输入中文字符"
            },
            fileallow: { def: "上传文件仅限{0}格式！" },
            filelimit: { def: "不许上传{0}格式的文件！" },
            begin: { def: "请以{0}开头！" },
            end: { def: "请以{0}结尾！" },
            regex: { def: "无法满足录入要求！" }
        },
        //初始化控件
        //element:页面元素（jquery对象），如果为空，则为全局
        init: function (element) {
            var star = "<span style=\"color:red\">*</span>";
            if (element == null) element = document;
            var ctrls = element.querySelectorAll("*[nullable='false'], select[novalue='-1']");
            this.toArray(ctrls).forEach(function (item) {
                var isstar = verify.getAttr($(item), "star", "true");
                if (isstar == "true") item.outerHTML += star;
            });
            this.toArray(element.querySelectorAll("*[verify='true']")).forEach(function (item) {
                item.addEventListener("click", function (e) {
                    if (this.getAttribute("disabled") == "disabled") return;     //如果按钮是禁用的，则不验证
                    var group = this.getAttribute("group");  //按钮的验证组，识标码
                    var form = $(e).parents("form");
                    form = form.size() > 0 ? form : $("body");
                    var ret = verify.IsPass(form, group);
                    if (!ret) e.preventDefault();
                }, false);
            });
            //提交按钮的事件
            //$("*[verify='true']").click(function () {

            //});
            //失去焦点时的事件
            $("form[patter=focus] *[type!=submit][type!=button][type!=image]:visible").focusout(function () {
                verify.operateSingle(null, $(this));
            });
            $("form[patter!=focus] *[type!=submit][type!=button][type!=image][patter=focus]:visible").focusout(function () {
                verify.operateSingle(null, $(this));
            });
            //当鼠标点击，或录入时，清除提示框
            $(window).bind("mousedown keydown scroll onmousewheel", function () {
                $(".VerifyShowBox").remove();
            });
        },
        //批量验证控件
        IsPass: function (control, group) {
            var isPass = true;
            group = group == null ? "" : group;
            var ctls = control.find("*[type!=submit][type!=button]:visible");
            if (ctls.size() > 0) {
                ctls.each(function () {
                    isPass = verify.operateSingle(group, $(this));
                    if (!isPass) return false;
                });
            } else {
                return verify.operateSingle(group, control);
            }
            return isPass;
        },
        //逐个验证控件
        operateSingle: function (group, control) {
            var isPass = true;
            var name = control.get(0).tagName.toLowerCase();
            if (name == "input" || name == "select" || name == "textarea") {
                //按钮与控件的分组码不同，则跳过
                if (group == "all" || verify.isSamegroup(group, control.attr("group"))) {
                    for (var attr in verify.verifyfunc) {
                        var attrval = control.attr(attr);    //属性的值
                        if (typeof (attrval) == "undefined") continue;
                        isPass = verify.verifyfunc[attr](control, attrval, $.trim(control.val()));
                        if (!isPass) return false;
                    }
                }
            }
            return isPass;
        },
        //判断两个组是否有交叠
        isSamegroup: function (btnGroup, ctlGroup) {
            btnGroup = btnGroup == null ? "" : btnGroup;
            ctlGroup = ctlGroup == null ? "" : ctlGroup;
            if (btnGroup == ctlGroup) return true;
            var arr1 = btnGroup.split("|");
            var arr2 = ctlGroup.split("|");
            for (var a in arr1) {
                for (var b in arr2) {
                    if (arr1[a] == arr2[b]) return true;
                }
            }
            return false;
        },
        //获取属性，如果控件没有该属性，则取form上的同名属性
        //control:控件
        //attrName:属性
        //defValue:默认值
        getAttr: function (control, attrName, defValue) {
            var attr = control.attr(attrName);
            if (attr == null || attr == "") {
                var form = control.parents("form");
                if (form.size() > 0) attr = form.attr(attrName);
            }
            return attr == null || attr == "" ? defValue : attr;
        },
        //将节点列表转为数组
        toArray: function (nodes) {
            var array = null;
            try {
                array = Array.prototype.slice.call(nodes, 0);//非ie浏览器  ie8-将NodeList实现为COM对象，不能用这种方式检测
            } catch (ex) {//ie8-
                array = new Array();
                for (var i = 0; i < nodes.length; i++) {
                    array.push(nodes[0]);
                }
            }
            return array;
        },
        //显示提示个信息
        //control:要显示提示的控件
        //showTxt:要显示的提示，如果控件有alt属性，则显示alt值作为提示
        //isforce:是否强行显示showTxt
        showBox: function (control, showTxt, isforce) {
            //提示信息
            if (isforce == null || !isforce) {
                var alt = $.trim(control.attr("alt"));
                showTxt = alt.length < 1 ? showTxt : alt;
            }
            var html = "<div name=\"" + control.attr("name") + "\" group=\"" + control.attr("group") + "\" class=\"VerifyShowBox\" style=\"display:none\">";
            html += "<div class=\"arrow-up\"> </div><div class=\"errorShow\">" + showTxt + " </div></div>";
            $("body").prepend(html);
            var box = $(".VerifyShowBox[name='" + control.attr("name") + "']");
            var place = verify.getAttr(control, "place", verify.attr.place);    //显示位置，默认在下方         
            var bgcolor = verify.getAttr(control, "bgcolor", verify.attr.backgroundcolor);  //背景色            
            var fgcolor = verify.getAttr(control, "fgcolor", verify.attr.foregroundcolor);  //前景色
            var showheight = Number(verify.getAttr(control, "showheight", verify.attr.showheight));  //提示信息高度
            showheight = (place == "left" || place == "right") && showheight == verify.attr.showheight ? control.outerHeight() : showheight;
            var size = Number(verify.getAttr(control, "showsize", verify.attr.showsize));  //提示信息字体大小
            box.css({ "position": "absolute", "z-index": 5000, "height": showheight, "width": "auto" });
            //错误提示文本
            box.find(".errorShow").css({
                "white-space": "nowrap", "width": "auto",
                "font-size": size, "line-height": showheight + "px", "text-indent": "10px", "height": showheight,
                "background-color": bgcolor, "color": fgcolor, "border-radius": "2px", "padding-right": "10px"
            });
            var offset = control.offset();
            var left = 0, top = 0;
            //三角符号
            box.find(".arrow-up").css({ "width": "0px", "height": "0px", "font-size": "0px", "line-height": "0px" });
            if (place == "left") {
                box.find(".arrow-up").insertAfter(box.find(".errorShow")).css({
                    "border-top": "5px solid transparent", "border-left": "5px solid " + bgcolor,
                    "border-bottom": "5px solid transparent", "margin-top": -showheight / 2 - 5, "margin-right": "-5px", "float": "right"
                });
                left = offset.left - box.width() - 15;
                top = offset.top - 1;
            }
            if (place == "right") {
                box.find(".arrow-up").css({
                    "border-top": "5px solid transparent", "border-right": "5px solid " + bgcolor,
                    "border-bottom": "5px solid transparent", "margin-top": showheight / 2 - 5, "float": "left"
                });
                left = offset.left + control.width() + 5;
                top = offset.top - 1;
                box.find(".errorShow").css("float", "left");
            }
            if (place == "bottom") {
                box.find(".arrow-up").css({
                    "border-left": "5px solid transparent", "border-right": "5px solid transparent",
                    "border-bottom": "5px solid " + bgcolor, "margin-left": "10px"
                });
                left = offset.left;
                top = offset.top + control.height() + 5;
                box.width(control.width() > box.width() ? control.width() + 4 : box.width());
            }
            if (place == "top") {
                box.find(".arrow-up").insertAfter(box.find(".errorShow")).css({
                    "border-left": "5px solid transparent",
                    "border-right": "5px solid transparent", "border-top": "5px solid " + bgcolor, "margin-left": 7, "float": "left"
                });
                left = offset.left;
                top = offset.top - box.height() - 2;
            }
            box.css({ left: left, top: top });
            box.css("display", "block").css({ opacity: .9 }).slideDown("slow");
            if (verify.getAttr(control, "patter", "submit") == "submit") control.focus();
            return false;
        },
        //基本等同showBox，供外部调用，会强制显示showTxt，忽悠控件的alt属性
        ShowBox: function (control, showTxt) {
            return verify.showBox(control, showTxt, true);
        },
        //格式化字符串
        format: function (str, args) {
            if (arguments.length < 1) return "";
            var primary = arguments[0];
            for (var i = 1; i < arguments.length; i++) {
                primary = primary.replace(eval('/\\{' + (i - 1) + '\\}/g'), arguments[i]);
            }
            return primary;
        },
        //获取控件，通过name属性
        getctl: function (name) {
            var ctl = $("input[name='" + name + "']:visible");
            if (ctl.size() < 1) ctl = $("input[name$='" + name + "']:visible");
            return ctl;
        },
        //获取花括号中的值
        getbrace: function (str) {
            var result = str.match(/\{(\w[^\}]+)\}/);
            if (result != null && result.length >= 2) return result[1];
            return null;
        },
        //验证方法
        verifyfunc: {
            //不能为空,
            //control:当前验证的录入控件
            //attrval:验证属性的值
            //input:用户录入的值
            nullable: function (control, attrval, input) {
                var name = control.get(0).tagName.toLowerCase();
                var type = control.attr("type");
                //选择框
                if (name == "select") {
                    return attrval == "false" && (input == "" || input == control.attr("deftxt")) ? verify.showBox(control, verify.errmsg.nullable.msg1) : true;
                }
                //复选框
                if (name == "input" && type == "checkbox") {
                    return control.is(':checked') ? true : verify.showBox(control, verify.errmsg.nullable.msg2);
                }
                return attrval == "false" && (input == "" || input == control.attr("deftxt")) ? verify.showBox(control, verify.errmsg.nullable.def) : true;
            },
            //长度区间
            lenlimit: function (control, attrval, input) {
                //取录入的值长度
                var len = input.length;
                if (attrval.indexOf("-") < 0) {
                    if (len > Number(attrval)) return verify.showBox(control, verify.format(verify.errmsg.lenlimit.msg1, len - Number(attrval)));
                } else {
                    var minlen = Number(attrval.substring(0, attrval.indexOf("-")));
                    var maxlen = Number(attrval.substring(attrval.indexOf("-") + 1));
                    if (minlen == maxlen && len != minlen) return verify.showBox(control, verify.format(verify.errmsg.lenlimit.def, minlen));
                    if (!(len >= minlen && len <= maxlen)) return verify.showBox(control, verify.format(verify.errmsg.lenlimit.msg2, minlen, maxlen));
                }
                return true;
            },
            //数值区间
            numlimit: function (control, attrval, input) {
                var num = Number(input);    //取录入的值
                if (isNaN(num) || num == null) return verify.showBox(control, verify.errmsg.numlimit.def);
                var min = 0, max = 0;
                if (attrval.indexOf("-") < 0) {
                    var target = verify.getbrace(attrval);
                    min = target != null ? Number(verify.getctl(target).val()) : Number(attrval);
                    if (num > min) return verify.showBox(control, verify.format(verify.errmsg.numlimit.msg1, min));
                } else {
                    var pre = attrval.substring(0, attrval.indexOf("-"));
                    var pos = attrval.substring(attrval.indexOf("-") + 1);
                    var preCtl = verify.getbrace(pre);
                    min = preCtl != null ? Number(verify.getctl(preCtl).val()) : Number(pre);
                    var posCtl = verify.getbrace(pos);
                    max = posCtl != null ? Number(verify.getctl(posCtl).val()) : Number(pos);
                    if (min == max && num != min) return verify.showBox(control, verify.format(verify.errmsg.numlimit.msg3, min, max));
                    if (!(num >= min && num <= max)) return verify.showBox(control, verify.format(verify.errmsg.numlimit.msg2, min, max));
                }
                return true;
            },
            //验证输入是否相同
            sametarget: function (control, attrval, input) {
                var same = verify.getctl(attrval);
                if (same.size() < 1) return true;
                if (input != $.trim(same.val())) return verify.showBox(control, verify.errmsg.sametarget.def);
                return true;
            },
            //验证不允许等于某值
            novalue: function (control, attrval, input) {
                var arr = attrval.split("|");
                for (var t in arr) {
                    if (input == arr[t]) return verify.showBox(control, verify.errmsg.novalue.def);
                }
                return true;
            },
            //上传文件限制，允许的文件类弄型
            fileallow: function (control, attrval, input) {
                if (input == "") return true;
                if (input.indexOf(".") > -1) input = input.substring(input.lastIndexOf(".") + 1);
                var arr = attrval.split("|");
                for (var s in arr) {
                    if (arr[s] == input.toLowerCase()) return true;
                }
                return verify.showBox(control, verify.format(verify.errmsg.fileallow.def, attrval.replace(/\|/g, "、")));
            },
            //上传文件限制，不允许的文件类型
            filelimit: function (control, attrval, input) {
                if (input == "") return true;
                if (input.indexOf(".") > -1) input = input.substring(input.lastIndexOf(".") + 1);
                var arr = attrval.split("|");
                for (var s in arr) {
                    if (arr[s] == input.toLowerCase())
                        return verify.showBox(control, verify.format(verify.errmsg.filelimit.def, attrval.replace(/\|/g, "、")));
                }
                return true;
            },
            //以限定字符开头，可以设置多个，用|分隔
            begin: function (control, attrval, input) {
                var arr = attrval.split("|");
                for (var t in arr) {
                    if (arr[t].length > input.length) continue;
                    if (input.indexOf(arr[t]) == 0) return true;
                }
                return verify.showBox(control, verify.format(verify.errmsg.begin.def, arr.join()));
            },
            //以限定字符结尾，可以设置多个，用|分隔
            end: function (control, attrval, input) {
                var arr = attrval.split("|");
                for (var t in arr) {
                    if (arr[t].length > input.length) continue;
                    if (input.indexOf(arr[t]) == input.length - arr[t].length) return true;
                }
                return verify.showBox(control, verify.format(verify.errmsg.end.def, arr.join()));
            },
            //正则表达式验证
            regex: function (control, attrval, input) {
                var regexp = new RegExp("^" + attrval + "$", "gi");
                if (!regexp.test(input)) return verify.showBox(control, verify.errmsg.regex.def);
                return true;
            },
            //验证数据类型，验证是否为数字、帐号、email、邮编、手机号
            datatype: function (control, attrval, input) {
                if (input == "") return true;     //如果没有录入，则不验证
                //验证，支持多重验证，用|分隔
                var ispass = { pass: false, err: verify.errmsg.datatype.def };  //是否通过
                var typeArr = attrval.indexOf("&") > -1 ? attrval.split("&") : attrval.split("|");
                //正则表达式
                var regexp = {
                    uint: { exp: /^[0-9]{1,}$/gi },     //正整数
                    number: { exp: /(^-?[0-9]\d*\.\d*|-0\.\d*[0-9]\d*$)|(^-?[0-9]\d*$)/gi }, //数字
                    float: { exp: /^(-?\d+)(\.\d+)?$/gi },     //浮点数
                    tel: { exp: /^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$/gi },     //固定电话
                    mobile: { exp: /^1[0-9]{10}$/gi },  //移动电话
                    zip: { exp: /^[0-9]{6,6}$/gi },         //邮政编码
                    user: { exp: /^[a-zA-Z]{1}([a-zA-Z0-9]|[._]){1,20}$/gi },  //账号
                    email: { exp: /^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$/gi },    //电子邮箱
                    idcard: { exp: /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/gi },   //身份证
                    url: { exp: /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/gi },  //网址
                    qq: { exp: /^[1-9]\d{4,12}$/gi },     //qq号
                    chinese: { exp: /^[\u0391-\uFFE5]+$/gi }     //中文
                };
                for (var t in typeArr) {
                    var type = $.trim(typeArr[t].toLowerCase());
                    for (var ex in regexp) {
                        if (type == ex) {
                            ispass.pass = regexp[ex].exp.test(input);
                            if (!ispass.pass) ispass.err = eval("verify.errmsg.datatype." + ex);
                            break;
                        }
                    }
                    if (attrval.indexOf("|") > -1 && ispass.pass) break;
                    if (attrval.indexOf("&") > -1 && !ispass.pass) break;
                }
                if (!ispass.pass) return verify.showBox(control, ispass.err);
                return true;
            }
            //
        }
    };
    //将内部变量赋给window，成为全局变量
    window.Verify = verify;
    $(function () {
        window.Verify.init();
    });
})();