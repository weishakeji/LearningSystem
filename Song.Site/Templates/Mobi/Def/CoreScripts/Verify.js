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
* 最后修订：2018年1月22日
*/
(function () {
    var verify = {
        version:"1.1",  //版本号
        //默认属性值
        attr:{
            place:"bottom",     //提示信息显示位置，默认为下方，可以设置right
            backgroundcolor:"#FF3300",     //背景色
            foregroundcolor:"#FFFFFF"      //前景色
        },
        //初始化控件
        //element:页面元素（jquery对象），如果为空，则为全局
        init: function (element) {
            var star = "<span style=\"color:red\">*</span>";
            if (element == null) element = $("body");
            element.find("*[nullable='false'],select[novalue='-1']").each(function () {
                var isstar=verify.getAttr($(this),"star","true");
                if (isstar=="true") $(this).after(star);
            });
            this.submitEvent();
            this.focusEvent();
            //当鼠标点击，或录入时，清除提示框
            $(window).bind("mousedown keydown", function () {
                $(".VerifyShowBox").remove();
            });
        },
        //提交按钮的事件
        submitEvent: function () {
            $("*[verify='true']:enabled").click(function () {
                var group = $(this).attr("group");  //按钮的验证组，识标码                
                return verify.IsPass($(this).parents("form"),group);
            });            
        },
        //失去焦点时的事件
        focusEvent:function(){
            $("form[patter=focus] *[type!=submit][type!=button][type!=image]:visible").focusout(function () {
                   verify.operateSingle(null, $(this));
             });
             $("form[patter!=focus] *[type!=submit][type!=button][type!=image][patter=focus]:visible").focusout(function () {
                   verify.operateSingle(null, $(this));
             });
        },
        //批量验证控件
        IsPass : function (control,group) {
            var isPass = true;
            group = group==null ? "" : group;
            var ctls=control.find("*[type!=submit][type!=button]:visible");
            if(ctls.size()>0){
                ctls.each(function () {
                     isPass = verify.operateSingle(group, $(this));
                     if (!isPass) return false;
                });
            }else{
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
                if (verify.isSamegroup(group,control.attr("group"))) {
                    var passFunc = new veriFunc();
                    for (var f in passFunc) {
                        if (typeof (passFunc[f]) == "function") isPass = passFunc[f](control);
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
        getAttr:function(control,attrName,defValue){
            var attr=control.attr(attrName);
            if(attr==null || attr==""){
                var form=control.parents("form");
                if(form.size()>0)attr=form.attr(attrName);
            }
            return attr==null || attr=="" ? defValue : attr;
        },        
        //显示提示个信息
        //control:要显示提示的控件
        //showTxt:要显示的提示，如果控件有alt属性，则显示alt值作为提示
        //isforce:是否强行显示showTxt
        showBox: function (control, showTxt,isforce) {
            //提示信息
            if(isforce==null || !isforce){
                var alt = $.trim(control.attr("alt"));
                showTxt = alt.length < 1 ? showTxt : alt; 
            }           
            var html = "<div name=\""+control.attr("name")+"\" group=\""+control.attr("group")+"\" class=\"VerifyShowBox\" style=\"display:none\">";
            html += "<div class=\"arrow-up\"> </div><div class=\"errorShow\">"+showTxt+" </div></div>";
            $("body").prepend(html);
            var box = $(".VerifyShowBox[name="+control.attr("name")+"]"); 
            var place=verify.getAttr(control,"place",verify.attr.place);    //显示位置，默认在下方         
            var bgcolor=verify.getAttr(control,"bgcolor",verify.attr.backgroundcolor);  //背景色            
            var fgcolor=verify.getAttr(control,"fgcolor",verify.attr.foregroundcolor);  //前景色
            box.css({ "position": "absolute", "z-index": 5000, "height": 30, "width": "auto" });
            //错误提示文本
            box.find(".errorShow").css({"white-space": "nowrap","width": "auto",
                "font-size": "13px", "line-height": "25px", "text-indent": "10px", "height": "25px",
                "background-color": bgcolor, "color": fgcolor, "border-radius": "2px","padding-right": "10px",
            });
            var offset = control.offset();
            var left=0,top=0;
            //三角符号
            box.find(".arrow-up").css({ "width": "0px", "height": "0px", "font-size": "0px", "line-height": "0px"});
            if(place=="left"){
                box.find(".arrow-up").insertAfter(box.find(".errorShow")).css({"border-top": "5px solid transparent", "border-left": "5px solid "+bgcolor,
                    "border-bottom": "5px solid transparent", "margin-top": "-17px","margin-right": "-5px","float": "right" });
                left=offset.left -box.width() - 15;
                top=offset.top -1;
            }
            if(place=="right"){
                box.find(".arrow-up").css({"border-top": "5px solid transparent", "border-right": "5px solid "+bgcolor,
                    "border-bottom": "5px solid transparent", "margin-top": "7px","float": "left" });
                left=offset.left +control.width() + 5;
                top=offset.top -1;
            }
            if(place=="bottom"){
                box.find(".arrow-up").css({ "border-left": "5px solid transparent", "border-right": "5px solid transparent",
                    "border-bottom": "5px solid "+bgcolor, "margin-left": "10px" });
                left=offset.left;
                top=offset.top + control.height() + 5;
            }
            if(place=="top"){
                box.find(".arrow-up").insertAfter(box.find(".errorShow")).css({ "border-left": "5px solid transparent",
                    "border-right": "5px solid transparent", "border-top": "5px solid "+bgcolor,"margin-left": 7,"float": "left" });
                left=offset.left;
                top=offset.top -box.height()-2;
            }
            box.css({ left: left, top: top});
            if(place=="right")box.find(".errorShow").css("float", "left");
            if(place=="bottom")box.width(control.width() > box.width() ? control.width()+4 : box.width());            
            var alpha = 100;
            box.css("filter", "Alpha(Opacity=" + alpha + ")");
            box.css("display", "block").css("-moz-opacity", alpha / 100).css("opacity", alpha / 100);
            box.slideDown("slow");
            if(verify.getAttr(control,"patter","submit")=="submit") control.focus(); 
            return false;
        },
        //基本等同showBox，供外部调用，会强制显示showTxt，忽悠控件的alt属性
        ShowBox: function (control, showTxt) {
            verify.showBox(control, showTxt,true);
        }
    };
    /***********
    下面是验证录入所用的具体方法
    ***********/
    var veriFunc = function () { }
    //验证是否为空
    veriFunc.prototype.IsNull = function (control) {
        var nullable = control.attr("nullable");
        if (typeof (limit) == "nullable") return true;
        var val = $.trim(control.val());     //取录入的值
        return nullable == "false" && (val == "" || val == control.attr("deftxt")) ? verify.showBox(control, "不得为空！") : true;
    }
    //验证录入的长度区间
    veriFunc.prototype.Lenth = function (control) {
        var limit = control.attr("lenlimit");   //验证属性
        if (typeof (limit) == "undefined") return true;
        //取录入的值长度
        var len = $.trim(control.val()).length;
        if (limit.indexOf("-") < 0) {
            if (len > Number(limit)) return verify.showBox(control, "超出 " + (len - limit) + " 个字符");
        } else {
            var minlen = Number(limit.substring(0, limit.indexOf("-")));
            var maxlen = Number(limit.substring(limit.indexOf("-") + 1));
            if (minlen == maxlen && len != minlen)return verify.showBox(control, "限制 " + minlen + " 个字符");
            if (!(len >= minlen && len <= maxlen))return verify.showBox(control, "请录入 " + minlen + "-" + maxlen + " 个字符");
        }
        return true;
    }
    //验证录入的长度区间
    veriFunc.prototype.NumberLimit = function (control) {
        var limit = control.attr("numlimit");   //验证属性
        if (limit == null) return true;
        var val = $.trim(control.val());     //取录入的值
        var num = Number(val);
        if (num == null) return verify.showBox(control, " 请输入整数数字！");
        if (limit.indexOf("-") < 0) {
            if (num > Number(limit)) return verify.showBox(control, "请输入小于 " + Number(limit) + " 的数字");
        } else {
            var min = Number(limit.substring(0, limit.indexOf("-")));
            var max = Number(limit.substring(limit.indexOf("-") + 1));
            if (!(num >= min && num <= max))return verify.showBox(control, "请录入 " + min + "-" + max + "之间的数字");
        }
        return true;
    }
    //验证输入是否相同
    veriFunc.prototype.IsSame = function (control) {
        var sametarget = control.attr("sametarget");    //验证属性
        if (sametarget == null) return true;
        var val = $.trim(control.val());    //取录入的值
        if (sametarget != null) {
            var same = $("input[name='" + sametarget + "']:visible");
            if (same.size() < 1) same = $("input[name$='" + sametarget + "']:visible");
            if (val != $.trim(same.val())) return verify.showBox(control, "两次输入不相同！");
        }
        return true;
    }
    //验证不允许等于某值
    veriFunc.prototype.Novalue = function (control) {
        var novalue = control.attr("novalue");  //验证属性        
        if (novalue == null) return true;
        var val = $.trim(control.val());    //取录入的值
        var arr = novalue.split("|");
        for (var t in arr) {
            if (val == arr[t]) return verify.showBox(control, "值不合法");
        }
        return true;
    }
    //验证数据类型，验证是否为数字、帐号、email、邮编、手机号
    veriFunc.prototype.DataType = function (control) {
        var datatype = control.attr("datatype");
        if (datatype == null) return true;
        var val = $.trim(control.val());    //取录入的值
        if (val == "") return true;     //如果没有录入，则不验证
        //验证，支持多重验证，用|分隔
        var ispass = { pass: false, err: "" };  //是否通过
        var typeArr = datatype.indexOf("&") > -1 ? datatype.split("&") : datatype.split("|");
        for (var t in typeArr) {
            var type = $.trim(typeArr[t].toLowerCase());
            if (type == "") continue;
            //验证正整数
            if (type == "uint") {
                ispass.pass = new RegExp(/^[0-9]{1,20}$/).test(val);
                if (!ispass.pass) ispass.err = "请输入正整数！";
            }
            //验证数字
            if (type == "number") {
                ispass.pass = new RegExp(/(^\d+$)|(^\d+.\d+$)|(^-\d+$)|(^-\d+.\d+$)/).test(val);
                ispass.err = "请输入数字！";
            }
            //验证电话
            if (type == "tel") {
                ispass.pass = new RegExp(/^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$/).test(val);
                if (!ispass.pass) ispass.err = "请输入正确的电话号码！";
            }
            //验证手机号
            if (type == "mobile") {
                ispass.pass = new RegExp(/^[+]{0,1}(\d){1,1}[ ]?([-]?((\d)|[ ]){10,10})+$/).test(val);
                if (!ispass.pass) ispass.err = "请输入正确的移动电话号码！";
            }
            //邮政编码 
            if (type == "zip") {
                ispass.pass = new RegExp(/^[0-9 ]{6,6}$/).test(val);
                if (!ispass.pass) ispass.err = "请输入正确的邮政编码！";
            }
            //用户帐号
            if (type == "user") {
                ispass.pass = new RegExp(/^[a-zA-Z]{1}([a-zA-Z0-9]|[._]){1,20}$/).test(val);
                if (!ispass.pass) ispass.err = "请用字符或数字且字符开头！";
            }
            //电子信箱
            if (type == "email") {
                ispass.pass = new RegExp(/^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$/).test(val);
                if (!ispass.pass) ispass.err = "请输入正确的电子信箱！";
            }
            //身份证
            if (type == "idcard") {
                ispass.pass = new RegExp(/(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/).test(val);
                if (!ispass.pass) ispass.err = "请输入正确的身份证信息！";
            }
            //网址，
            if (type == "url") {
                ispass.pass = new RegExp(/^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/).test(val);
                if (!ispass.pass) ispass.err = "请输入合法的网络地址！";
            }
            //qq号
            if (type == "qq") {
                ispass.pass = new RegExp(/^[1-9]\d{4,12}$/).test(val);
                if (!ispass.pass) ispass.err = "请输入合规的QQ号码！";
            }
            //中文
            if (type == "chinese") {
                ispass.pass = new RegExp(/^[\u0391-\uFFE5]+$/).test(val);
                if (!ispass.pass) ispass.err = "请输入中文字符！";
            }
            if (datatype.indexOf("|") > -1 && ispass.pass) break;
            if (datatype.indexOf("&") > -1 && !ispass.pass) break;
        }
        if (!ispass.pass) return verify.showBox(control, ispass.err);
        return true;
    }
    //上传文件限制，允许的文件类弄型
    veriFunc.prototype.FileAllow = function (control) {
        var filelimit = control.attr("fileallow");
        if (filelimit == null) return true;
        var val = $.trim(control.val()).toLowerCase();    //当前值
        if (val == "") return true;
        if (val.indexOf(".") > -1) val = val.substring(val.lastIndexOf(".") + 1);
        var arr = filelimit.split("|");
        for (var s in arr) {
            if (arr[s] == val) return true;
        }
        return verify.showBox(control, "上传文件仅限 " + filelimit.replace(/\|/g, "、") + " 格式！");
    }
    //上传文件限制，不允许的文件类型
    veriFunc.prototype.FileLimit = function (control) {
        var filelimit = control.attr("filelimit");
        if (filelimit == null) return true;
        var val = $.trim(control.val()).toLowerCase();    //当前值
        if (val == "") return true;
        if (val.indexOf(".") > -1) val = val.substring(val.lastIndexOf(".") + 1);
        var arr = filelimit.split("|");
        for (var s in arr) {
            if (arr[s] == val) return verify.showBox(control, "不许上传 " + filelimit.replace(/\|/g, "、") + " 格式的文件！");
        }
        return true;
    }
    //以限定字符开头，可以设置多个，用|分隔
    veriFunc.prototype.Begin = function (control) {
        var begin = control.attr("begin");  //验证属性
        if (begin == null) return true;
        var val = $.trim(control.val());    //取录入的值
        var arr = begin.split("|");
        for (var t in arr) {
            if (arr[t].length > val.length) continue;
            if (val.indexOf(arr[t]) == 0) return true;
        }
        return verify.showBox(control, "请以" + arr.join() + "开头");
    }
    //以限定字符结尾，可以设置多个，用|分隔
    veriFunc.prototype.End = function (control) {
        var end = control.attr("end");  //验证属性
        if (end == null) return true;
        var val = $.trim(control.val());    //取录入的值
        var arr = end.split("|");
        for (var t in arr) {
            if (arr[t].length > val.length) continue;
            if (val.indexOf(arr[t]) == val.length - arr[t].length) return true;
        }
        return verify.showBox(control, "请以" + arr.join() + "结尾");
    }
    //正则表达式验证
    veriFunc.prototype.RegExp=function(control){
        var regword = control.attr("regex");  //验证属性
        if (regword == null) return true;
        var val = $.trim(control.val());    //取录入的值
        var regexp= new RegExp("^"+regword+"$","gi");
        if(!regexp.test(val))return verify.showBox(control,"无法满足录入要求");
        return true;
    }
    //
    window.Verify = verify;
    $(function () {
        window.Verify.init();
    });
})();