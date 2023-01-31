/*!
 * 主 题：登录框
 * 说 明：
 * 1、支持支持滑块；
 * 2、可加载验证码
 * 3、异步加载验证
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * 开发时间: 2020年3月25日
 * 最后修订：2020年3月31日
 * github开源地址:https://github.com/weishakeji/WebdeskUI
 */
(function (win) {
    var login = function (param) {
        if (param == null || typeof (param) != 'object') param = {};
        this.attrs = {
            target: '', //所在Html区域          
            width: '',
            height: '',
            title: '', //标题
            ico: 'a003', //图标的字体符号
            icoimg: '', //图标的图片样式
            buttontxt: '登录',       //提交按钮上的文字
            company: '', //公司名称
            website: '', //公司的网址
            tel: '', //联系电话
            user: '', //账号
            pw: '', //密码
            vcode: '', //验证码,输入的验证码
            vcodelen: 4, //验证码长度
            vcodebase64: '', //验证的图片base64编码
            vcodemd5: '', //验证码的md5编码
            id: '',
            drag: false, //是否处于拖动状态
            draggvalue: 0,   //  拖动值，即离左侧起始位置的x坐标值
            dragfinish: false, //拖动完成
            success: false, //是否登录成功
            loading: false //加载中
        };
        for (var t in param) this.attrs[t] = param[t];
        eval($ctrl.attr_generate(this.attrs));
        /* 自定义事件 */
        //layout:布局完成; resize:改变大小；dragfinish:拖动完成; full:标签项全屏
        eval($ctrl.event_generate(['layout', 'resize', 'dragfinish', 'change', 'vefiry', 'submit', 'success', 'error']));
        //以下不支持双向绑定
        this.dom = null; //控件的html对象
        this.domtit = null; //控件标签栏部分的html对象
        this.dombody = null;
        this.domfoot = null;
        this.customVerify = []; //自定义验证方法
        this.dragfinish_value = 85;       //滑块拖动完成达到这个值，就表示拖动完成
        this.$slot = null;         //插槽
        //
        if (!this._id) this._id = 'login_' + new Date().getTime();
        $ctrls.add({
            id: this.id,
            obj: this,
            dom: this.dom,
            type: 'login'
        });
    };
    var fn = login.prototype;
    fn._initialization = function () {
        var area = $dom(this.target);
        if (area.length < 1) {
            console.log('Login所在区域不存在');
            return;
        }
        this.$slot = area.html();
        area.html('');
        if (!this._id) this._id = 'login_' + new Date().getTime();
    };
    //当属性更改时触发相应动作
    fn._watch = {
        'width': function (obj, val, old) {
            if (obj.dom) obj.dom.width(val);
            obj.trigger('resize', {
                width: val,
                height: obj._height
            });
        },
        'height': function (obj, val, old) {
            if (obj.dom) obj.dom.height(val);
            obj.trigger('resize', {
                width: obj._width,
                height: val
            });
        },
        'title': function (obj, val, old) {
            if (obj.domtit) {
                if (val != '')
                    obj.domtit.find('login_tit').html(val).css('display','');
                else
                    obj.domtit.find('login_tit').hide();
            }
        },
        'icoimg': function (obj, val, old) {
            if (obj.domtit) {
                var ico = obj.domtit.find('login_ico');
                if (obj.icoimg != '') {
                    ico.find('i').remove();
                    if (ico.find('img').length < 1)
                        ico.add('img').attr('src', obj.icoimg);
                }
                if (obj.icoimg == '') {
                    ico.find('img').remove();
                    if (ico.find('i').length < 1)
                        ico.add('i').html('&#x' + obj.ico);
                }
            }
        },
        'company': function (obj, val, old) {
            if (obj.domfoot) {
                obj.domfoot.find('login_company a').html(val);
                if (val == '') obj.domfoot.find('login_company').hide();
                else
                    obj.domfoot.find('login_company').show();
            }
        },
        'website': function (obj, val, old) {
            if (obj.domfoot) obj.domfoot.find('login_company a').attr('href', val);
        },
        'tel': function (obj, val, old) {
            if (obj.domfoot) {
                obj.domfoot.find('login_tel').html(val);
                if (val == '') obj.domfoot.find('login_tel').hide();
                else
                    obj.domfoot.find('login_tel').show();
            }
        },
        'user': function (obj, val, old) {
            if (obj.dom) obj.dom.find('input[name=\'login_user\']').val(val);
            obj.dragfinish = false;
        },
        'pw': function (obj, val, old) {
            if (obj.dom) obj.dom.find('input[name=\'login_pw\']').val(val);
            obj.dragfinish = false;
        },
        'vcode': function (obj, val, old) {
            if (val == old) return;
            if (obj.dom) obj.dom.find('input[name=\'login_vcode\']').val(val);
        },
        //验证码的base64编码
        'vcodebase64': function (obj, val, old) {
            if (val == old) return;
            if (obj.dom) obj.dom.find('img[class=\'vcode_img\']').attr('src', val);
        },
        //滑块是否在拖动,val为true为拖动中
        'drag': function (obj, val, old) {
            if (!obj.dom) return;
            var box = obj.dom.find('login_dragbox');
            if (!val) {
                box.css('transition', 'left 0.3s').removeClass('drag');
                var parent = box.parent();
                var perecnt = obj.dragfinish_value; //拖动完成百分多少，算完成             
                if (parseInt(box.css('left')) / (parent.width() - box.width()) * 100 >= perecnt) {
                    box.left(parent.width() - box.width() - 5);
                    if (!obj.dragfinish) obj.dragfinish = true;
                } else {
                    box.left(5);
                }
                //隐藏滑道，渐隐
                obj.dom.find('slideway').css({ 'transition': 'width 0.3s', 'width': '0px' });
                window.setTimeout(function () {
                    obj.dom.find('slideway').hide();
                }, 300);
            }
            //如果处在拖动中，则滑块去除动态效果
            if (val) {
                box.addClass('drag').css({ 'transition': '', 'opacity': 1 });
                obj.dom.find('slideway').css({ 'transition': '' }).show();
            }
        },
        //拖动值，滑块在拖动区间的百分比位置
        draggvalue: function (obj, val, old) {
            var slideway = obj.dom.find('slideway');
            if (val > 10)
                slideway.css('width', 'calc(' + val + '% - 10px)');
            else
                slideway.css('width', 'calc(' + val + '%)');
        },
        //滑块拖动完成
        'dragfinish': function (obj, val, old) {
            //滑块拖动区域
            var box = obj.dom.find('login_drag');
            //要隐藏的元素，当未滑动时，输入框与验证码都隐藏        
            var elements = box.prev().merge(box.prev().prev());
            if (val) {
                box.addClass('complete').css({ 'transition': 'opacity 1s', 'opacity': 0 });
                window.setTimeout(function () {
                    if (obj.dragfinish) box.hide();
                }, 1000);
                obj.trigger('dragfinish');
                obj.vcode = '';
                obj.dom.find('input[name=\'login_vcode\']').focus().parent().removeClass('login_error');
                elements.show();
                //隐藏滑道，渐隐
                obj.dom.find('slideway').css({ 'transition': 'width 0.3s', 'width': '0px' });
                window.setTimeout(function () {
                    obj.dom.find('slideway').hide();
                }, 300);
                box.prev().focus();
            } else {
                box.removeClass('complete').css({ 'transition': '', 'opacity': 1 }).show();
                obj.dom.find('login_dragbox').left(5);
                elements.hide();
            }
        },
        //加载状态变化时
        loading: function (obj, val, old) {
            if (obj.dom) {
                if (val) {
                    obj.dom.find('form.login_body').hide();
                    obj.dom.find('login_titlebar').hide();
                    obj.dom.find('div.slot').hide();
                    obj.dom.find('div.login_loding').show();
                } else {
                    obj.dom.find('form.login_body').show();
                    obj.dom.find('div.slot').show();
                    obj.dom.find('login_titlebar').show();
                    obj.dom.find('div.login_loding').hide();
                }
            }
        }
    };
    fn.open = function () {
        this._initialization();
        //创建登录框，以及基础事件
        for (var t in this._builder) this._builder[t](this);
        for (var t in this._baseEvents) this._baseEvents[t](this);
        //
        if (this._width != '') this.width = this._width;
        if (this._height != '') this.height = this._height
        this.user = this._user;
        this.pw = this._pw;
        this.loading = this._loading;
        this.inputs = {
            user: this.dom.find('input[type=text][name=login_user]'),
            pw: this.dom.find('input[type=password][name=login_pw]'),
            vcode: this.dom.find('input[type=number][name=login_vcode]')
        }
        var th = this;
        window.setTimeout(function () {
            th.trigger('layout', {
                'target': th.dom,
                'data': th.attrs
            });
            th.dom.show();
        }, 200);

        return this;
    };
    fn._builder = {
        //生成外壳
        shell: function (obj) {
            var area = $dom(obj.target);
            if (area.length < 1) {
                console.log('Login所在区域不存在');
                return;
            }
            area.addClass('loginbox').attr('ctrid', obj.id);
            obj.dom = area;
        },
        title: function (obj) {
            obj.domtit = obj.dom.add('login_titlebar');
            var ico = obj.domtit.add('login_ico');
            if (obj.icoimg != '') ico.add('img').attr('src', obj.icoimg);
            if (obj.icoimg == '') ico.add('i').html('&#x' + obj.ico);
            obj.domtit.add('login_tit').html(obj.title);
        },
        body: function (obj) {
            obj.dombody = obj.dom.add('form').addClass('login_body');
            //账号
            var user = obj.dombody.add('login_row');
            user.addClass('login_user').add('input').attr({
                'type': 'text',
                'name': 'login_user',
                'autocomplete': 'off',
                'placeholder': '账号'
            });
            //密码
            var pw = obj.dombody.add('login_row');
            pw.addClass('login_pw').add('input').attr({
                'type': 'password',
                'name': 'login_pw',
                'autocomplete': 'off',
                'placeholder': '密码'
            });
            //验证码
            var code = obj.dombody.add('login_row');
            code.add('img').addClass('vcode_img');
            code.addClass('login_code').add('input').attr({
                'type': 'number',
                'name': 'login_vcode',
                'autocomplete': 'off',
                'maxlength': obj.vcodelen,
                'placeholder': '验证码'
            });
            //拖动滑块
            var drag = code.add('login_drag');
            drag.add('div').html('<span>向右拖动滑块</span>').add('login_dragbox');
            drag.find('div').first().add('slideway').hide();   //滑道
            //登录按钮
            var btnarea = obj.dombody.add('login_row');
            btnarea.add('button').attr('type', 'submit').html(obj.buttontxt);
            //各项提示框
            obj.dombody.find('login_row').add('login_tips');
            //.html('不得为空！');
        },
        loading: function (obj) {
            var loadbox = obj.dom.add('div').addClass('login_loding');
            //var effect = loadbox.add('div').addClass('loading_effect');
            loadbox.add('span');
            loadbox.add('span');
            loadbox.add('b').text('loading......');
        },
        footer: function (obj) {
            obj.domfoot = obj.dom.add('login_footbar');
            var company = obj.domfoot.add('login_company');
            if (obj.company == '') company.hide();
            company.add('a').html(obj.company).attr({
                'target': '_blank',
                'href': obj.website
            });
            //联系电话
            var tel = obj.domfoot.add('login_tel');
            if (obj.tel == '') tel.hide();
            tel.html(obj.tel);
        },
        slot: function (obj) {
            var slot = obj.dom.add('div').addClass("slot");
            slot.html(obj.$slot);
            slot.hide();
        }
    };
    //基础事件
    fn._baseEvents = {
        submit: function (obj) {
            var form = obj.dom.find('form');
            form.bind('submit', function (e) {
                console.log(e);
                e.preventDefault();
                return false;
            });
        },
        //登录按钮事件
        login: function (obj) {
            obj.dom.find('button').click(function (e) {
                var obj = login._getObj(e);
                obj.trigger('submit', {
                    user: obj.user,
                    pw: obj.pw,
                    vcode: obj.vcode
                });
                e.preventDefault();
                return false;
            });
        },
        //滑块拖动
        drag: function (obj) {
            obj.dom.find('login_dragbox').bind('mousedown,touchstart', function (e) {
                if (e && e.preventDefault) e.preventDefault();
                var obj = login._getObj(e);
                if (obj.dragfinish) return;
                obj.drag = true;
                obj._drag_init_x = $dom.mouse(e).x; //拖动时的初始鼠标值
            }).bind('mouseup,touchend', function (e) {
                if (e && e.preventDefault) e.preventDefault();
                var obj = login._getObj(e);
                obj.drag = false;
            });
            obj.dom.find('login_drag>div').bind('mouseleave,touchend', function (e) {
                if (e && e.preventDefault) e.preventDefault();
                var obj = login._getObj(e);
                obj.drag = false;
            });
            obj.dom.find('login_drag>div').bind('mousemove,touchmove', function (e) {
                if (e && e.preventDefault) e.preventDefault();
                var obj = login._getObj(e);
                if (obj.dragfinish) return; //如果拖动完成，则不能拖动
                //计算移动最大宽度范围
                var node = e.target ? e.target : e.srcElement;
                var parent = $dom(node).parent();
                var min = 5;
                var dragbox = obj.dom.find('login_dragbox');
                var max = parent.width() - dragbox.width() - 10;
                //
                var mouse = $dom.mouse(e);
                var left = mouse.x - obj._drag_init_x;
                left = left <= min ? min : (left >= max ? max : left);
                if (obj.drag) {
                    dragbox.left(left);
                    var perecnt = obj.dragfinish_value;       //允许完成拖动的百分比，即达到这个值就算拖动成功
                    //console.error(perecnt);
                    var per = parseInt(dragbox.css('left')) / (parent.width() - dragbox.width()) * 100;
                    if (per >= perecnt) {
                        obj.dom.find("login_drag").addClass('complete').css("opacity", 0.8);
                    } else {
                        obj.dom.find("login_drag").removeClass('complete').css("opacity", 1);
                    }
                    obj.draggvalue = per;
                }
            });
        },
        //输入更改时
        change: function (obj) {
            obj.dom.find('form input').bind('input', function (e) {
                var input = e.target ? e.target : e.srcElement;
                var word = e.data ? e.data : ''; //新输入的字符
                var val = input.value; //当前输入框中的字符串
                if (val == '') return;
                //触发事件
                var obj = login._getObj(e);
                obj.trigger('change', {
                    'action': input.name.substring(input.name.indexOf('_') + 1),
                    'word': word,
                    'value': val,
                    'target': input
                });
            });
        },
        //验证码点击
        vcodeclick: function (obj) {
            obj.dom.find('img[class=\'vcode_img\']').click(function (e) {
                obj.trigger('dragfinish');
            });
        }
    };
    //登录时的基础验证
    fn._baseVefiry = {
        //非空验证
        notempty: function (obj, ctrl) {
            var val = $dom.trim(ctrl.val());
            var placeholder = ctrl.attr('placeholder');
            var name = ctrl.attr('name');
            if (name == 'login_vcode' && !obj.dragfinish) return true;
            if (val == '') {
                return obj.tips(ctrl, false, '“' + placeholder + '”不得为空 ！');
            } else {
                return obj.tips(ctrl, true);
            }
        },
        //滑块验证
        drag: function (obj, ctrl) {
            var name = ctrl.attr('name');
            if (name != 'login_vcode') return true;
            if (!obj.dragfinish) {
                return obj.tips(ctrl, false, '请将滑块滑向右侧！');
            }
            return obj.dragfinish;
        },
        //格式验证
        format: function (obj, ctrl) {
            return true;
        }
    };
    //添加自定义验证方法
    fn.verify = function (ctrl, regex, tips) {
        if (arguments.length == 1) {
            //如果为对象
            if (Object.prototype.toString.call(ctrl) === '[object Object]') {
                return this.verify(ctrl.ctrl, ctrl.regex, ctrl.tips);
            }
            if (ctrl instanceof Array) {
                for (var t in ctrl) this.verify(ctrl[t]);
                return;
            }
        }
        var input = null;
        if (ctrl == 'user') input = this.dom.find('input[type=text][name=login_user]');
        if (ctrl == 'pw') input = this.dom.find('input[type=password][name=login_pw]');
        if (ctrl == 'vcode') input = this.dom.find('input[type=number][name=login_vcode]');
        this.customVerify.push({
            ctrl: input,
            regex: regex,
            tips: tips
        });
    };
    //显示提示框
    fn.tips = function (ctrl, success, msg) {
        var parent = ctrl.parent(); //行
        var tips = parent.find('login_tips');
        if (success) {
            parent.removeClass('login_error');
        } else {
            parent.addClass('login_error');
            tips.html(msg);
            ctrl[0].focus();
        }
        return success;
    };
    /*** 
    以下是静态方法
    *****/
    login.create = function (param) {
        if (param == null) param = {};
        var obj = new login(param);
        //当输入更改时
        obj.onchange(function (s, e) {
            if (e.action == 'user') s._user = e.value;
            if (e.action == 'pw') s._pw = e.value;
            if (e.action == 'vcode') s._vcode = e.value;
            if (e.action == 'user' || e.action == 'pw') s.dragfinish = false;
            if (e.action != 'vcode') s.vcodemd5 = ''; //图片验证码的加密信息，清空
            //验证当前输入框
            var pass = true;
            for (var t in s._baseVefiry) {
                var res = s._baseVefiry[t](s, $dom(e.target));
                if (res == false || res == undefined) pass = false;
            };
            //自定义验证方法
            for (var t in s.customVerify) {
                if (s.customVerify[t].ctrl.attr('name') == e.target.getAttribute('name')) {
                    var val = $dom.trim(s.customVerify[t].ctrl.val());
                    var regex = new RegExp(s.customVerify[t].regex);
                    if (!regex.test(val)) {
                        pass = s.tips(s.customVerify[t].ctrl, false, s.customVerify[t].tips);
                    } else {
                        s.tips(s.customVerify[t].ctrl, true);
                    }
                }
            }
            if (pass) s.tips($dom(e.target), true);
        });
        //当提交表单时
        obj.onsubmit(function (s, e) {
            //验证表单
            var inputs = s.dom.find('input[type=text],input[type=number],input[type=password]');
            var pass = false;
            pass = inputs.each(function () {
                for (var t in s._baseVefiry) {
                    var res = s._baseVefiry[t](s, $dom(this));
                    if (res == false || res == undefined) return false;
                };
                //自定义验证方法
                for (var t in s.customVerify) {
                    if (s.customVerify[t].ctrl.attr('name') == this.getAttribute('name')) {
                        var val = $dom.trim(s.customVerify[t].ctrl.val());
                        var regex = new RegExp(s.customVerify[t].regex);
                        if (!regex.test(val)) {
                            return s.tips(s.customVerify[t].ctrl, false, s.customVerify[t].tips);
                        } else {
                            s.tips(s.customVerify[t].ctrl, true);
                        }
                    }
                }
                return true;
            }, 1);
            //只要有一个未通过，则禁止提交
            if (pass instanceof Array) {
                for (var i = 0; i < pass.length; i++) {
                    if (pass[i] == false || pass[i] == undefined) return false;
                }
            }
            return pass;
        });
        obj.onchange(function (s, e) {
            for (var t in s.vefiry) {
                var res = s.vefiry[t](s, $dom(e.target));
                if (res == false || res == undefined) return false;
            }
        });
        return obj.open();
    };
    login._getObj = function (e) {
        var node = e.target ? e.target : e.srcElement;
        while (!node.getAttribute('ctrid')) node = node.parentNode;
        var ctrl = $ctrls.get(node.getAttribute('ctrid'));
        return ctrl.obj;
    }
    win.$login = login;
})(window);