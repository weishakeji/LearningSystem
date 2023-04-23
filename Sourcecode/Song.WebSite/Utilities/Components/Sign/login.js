// 登录控件
$dom.load.css(['/Utilities/Components/sign/Styles/login.css']);
Vue.component('login', {
    //config：机构配置项
    props: ['config'],
    data: function () {
        return {

            data: [],
            tabs: [{ name: '账号登录', tag: 'pwd', icon: 'e687', show: true },
            { name: '短信登录', tag: 'sms', icon: 'e76e', show: true }],
            tabActive: '',

            //账号密码登录的表单项与验证码
            acc_form: { acc: '', pwd: '' },
            acc_vcode: { base64: '', md5: '', val: '', loading: false },
            acc_rules: {
                'acc': [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 4, max: 32, message: '长度在 4 到 32 个字符', trigger: 'blur' },
                    { regex: /^[a-zA-Z0-9_-]{4,32}$/, message: '仅限字母与数字', trigger: 'blur' }
                ],
                'pwd': [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 1, max: 32, message: '长度在 6 到 32 个字符', trigger: 'blur' }
                ],
                'vcode': [
                    { required: true, message: '校验码不得为空', trigger: 'blur' },
                    { min: 4, max: 4, message: '仅限4位数字', trigger: 'blur' }
                ]
            },
            //短信登录
            sms_form: { 'phone': '', 'vcode': '' },
            sms_rules: {
                'phone': [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { regex: /^[0-9_-]{11,11}$/, message: '请输入合法手机号', trigger: 'blur' }
                ],
                'sms': [
                    { required: true, message: '验证码不得为空', trigger: 'blur' },
                    { min: 6, max: 6, message: '仅限6位数字', trigger: 'blur' }
                ]
            },
            sms_seconds: 0,        //发送短信后的数秒
            loading: false,
            loading_sms: false,
        }
    },
    watch: {
        //录入变化时
        'acc_form': {
            handler: function (val, old) {
                this.$refs['acc_pwd_drag'].init();
                this.acc_vcode.val = '';
                this.acc_vcode.md5 = '';
                this.acc_vcode.base64 = '';
                this.tips();
            }, deep: true
        },
        'acc_vcode': {
            handler: function (val, old) {
                this.tips();
            }, deep: true
        },
        'sms_form': {
            handler: function (val, old) {
                this.tips();
            }, deep: true
        },
        'config': {
            handler: function (val, old) {
                if (!val || val == undefined || JSON.stringify(val) == '{}') return;
                //是否显示       
                this.tabs[0].show = val.IsLoginForPw;
                this.tabs[1].show = val.IsLoginForSms;
                this.$set(this.tabs, 0, this.tabs[0]);
                this.$set(this.tabs, 1, this.tabs[1]);
                //计算默认选项卡
                var tag = this.tabActive;
                var obj = this.tabs.find(item => item.tag == tag && item.show);
                if (obj == null) obj = this.tabs.find(item => item.show == true);
                if (obj != null) this.tabActive = obj.tag;

            }, immediate: true, deep: true
        }

    },
    computed: {
        //禁止登录
        disabled_login: function () {
            return this.config && !this.config.IsLoginForPw && !this.config.IsLoginForSms;
        }
    },
    created: function () {
        //记录来源页
        var referrer = decodeURIComponent($api.querystring('referrer'));
        if (referrer != '') {
            $api.storage('singin_referrer', referrer);
        }
    },
    methods: {
        //账号密码登录
        submit_accpwd: function (e) {
            //校验输入
            var form = {
                acc: this.acc_form.acc,
                pwd: this.acc_form.pwd,
                vcode: this.acc_vcode.val
            }
            if (!this.verification(form, this.acc_rules)) return;

            var th = this;
            th.loading = true;
            $api.post('Account/Login', {
                'acc': th.acc_form.acc, 'pw': th.acc_form.pwd,
                'vcode': th.acc_vcode.val, 'vmd5': th.acc_vcode.md5
            }).then(function (req) {
                if (req.data.success) {
                    //登录成功
                    var ismobi = $api.ismobi();
                    th.success(req.data.result, ismobi ? '手机端' : '电脑网页', '账号密码登录', '');
                } else {
                    var data = req.data;
                    switch (String(data.state)) {
                        //验证码错误
                        case '1101':
                            th.tips('vcode', false, data.message);
                            break;
                        case '1102':
                            th.tips('pwd', false, '账号或密码错误');
                            break;
                        case '1103':
                            th.tips('acc', false, '账号被禁用');
                            break;
                        default:
                            th.tips('acc', false, '登录失败');
                            console.error(req.data.exception);
                            console.log(req.data.message);
                            break;
                    }
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
            return false;
        },
        //短信登录
        submit_sms: function () {
            //校验输入
            var form = {
                phone: this.sms_form.phone,
                sms: this.sms_form.vcode
            }
            if (!this.verification(form, this.sms_rules)) return;
            //校验验证码
            let storage = 'login_sms_seconds';
            var obj = $api.storage(storage);
            if (obj == undefined) return this.tips('sms', false, '请发送验证码');
            if (obj.expire < new Date()) return this.tips('sms', false, '验证码时效过期');
            if (obj.vcode != $api.md5(form.phone + form.sms)) return this.tips('sms', false, '验证码不正确');
            var th = this;
            th.loading = true;
            $api.post('Account/LoginSms', form).then(function (req) {
                if (req.data.success) {
                    if (req.data.success) {
                        //登录成功
                        var ismobi = $api.ismobi();
                        th.success(req.data.result, ismobi ? '手机端' : '电脑网页', '短信验证登录', '');
                    } else {
                        var data = req.data;
                        switch (String(data.state)) {
                            //验证码错误
                            case '1101':
                                th.tips('phone', false, data.message);
                                break;
                            case '1105':
                                th.tips('sms', false, '验证码错误');
                                break;
                            case '1103':
                                th.tips('phone', false, '账号被禁用');
                                break;
                            default:
                                th.tips('phone', false, '登录失败');
                                console.error(req.data.exception);
                                console.log(req.data.message);
                                break;
                        }
                    }
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
            return;
        },
        //获取短信
        getsms: function () {
            if (!this.verification({ 'phone': this.sms_form.phone }, this.sms_rules)) return;
            var th = this;
            th.loading_sms = true;
            $api.post('Sms/SendLoginVcode', { 'phone': th.sms_form.phone, 'len': 6 }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;   //校验码
                    th.countdown(result);
                    window.login_sms_seconds = window.setInterval(function () {
                        th.countdown();
                    }, 1000);

                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.tips('phone', false, err);
                console.error(err);
            }).finally(() => th.loading_sms = false);
        },
        //倒计时
        countdown: function (vcode) {
            let storage = 'login_sms_seconds';
            var obj = $api.storage(storage);
            if (obj == undefined) obj = {};
            if (vcode != '' && vcode != undefined) {
                obj = { 'second': 60, 'expire': new Date(new Date().getTime() + 1000 * 180), 'vcode': vcode };
            }
            else {
                if (obj.second > 0) obj.second--;
                else window.clearInterval(window.login_sms_seconds);
            }
            this.sms_seconds = obj.second;
            $api.storage(storage, obj);
            console.log(obj.second);
        },
        //显示提示信息
        tips: function (prop, success, msg) {
            if (prop == undefined || prop == null) {
                return $dom('*[prop]').removeClass('login_error');
            }
            var dom = $dom('*[prop="' + prop + '"]');
            if (dom.length > 0) {
                if (success) dom.removeClass('login_error');
                if (!success) {
                    dom.addClass('login_error');
                    dom.find('input').focus();
                }
                dom.find('.tips').html(msg);
            }
            return success;
        },
        //校验数据，data数据项,rules校验规则（基本遵循ElemenuUI相关校验规划）
        verification: function (data, rules) {
            for (d in data) {   //遍历数据项
                if (!check(d, data[d], rules[d], this.tips)) return false;
            }
            //校验方法
            function check(prop, val, rule, func) {
                if (rule == undefined) return true;
                for (let i = 0; i < rule.length; i++) {
                    const item = rule[i];
                    //为空判断
                    if (!!item['required'] && val == '') {
                        return func(prop, false, item['message']);
                    }
                    //判断长度
                    if ((!!item['min'] && val.length < item['min'])
                        || (!!item['max'] && val.length > item['max'])) {
                        return func(prop, false, item['message']);
                    }
                    //正则验证
                    if (!!item['regex']) {
                        var regex = new RegExp(item['regex']);
                        if (!regex.test(val))
                            return func(prop, false, item['message']);
                    }
                }
                return func(prop, true, '');
            }
            return true;
        },
        //滑块拖动完成
        dragfinish: function () {
            console.log('拖动完成');
            //设置验证码输入框为焦点
            $dom('.login_pwd_area .login_vcode input').focus();
            var ref = this.$refs;
            this.getvcode();
        },
        //加载验证码图片
        getvcode: function () {
            var th = this;
            th.acc_vcode.loading = true;
            $api.post('Helper/CheckCodeImg', { 'leng': 4, 'acc': th.acc_form.acc }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.acc_vcode.base64 = result.base64;
                    th.acc_vcode.md5 = result.value;
                } else {
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.acc_vcode.loading = false);
        },
        //登录成功
        success: function (account, source, info, remark) {
            $api.loginstatus('account', account.Ac_Pw, account.Ac_ID);
            $api.login.account_fresh();
            //登录成功的事件
            this.$emit('success', account);
            $api.post('Point/AddForLogin', { 'source': source, 'info': info, 'remark': remark });            
        }
    },
    template: `<div class="login_weisha">
        <div class="login_loding" v-show="loading"><span></span><span></span></div>
        <template  v-show="!loading">
            <div class="login_title_bar">
                <div v-for="t in tabs" @click="tabActive=t.tag" :tag="t.tag" v-if="t.show" :current="tabActive==t.tag">
                    <icon v-html="'&#x'+t.icon"></icon>
                    <span>{{t.name}}</span>
                </div>
            </div>
            <form class="login_pwd_area" v-if="config && config.IsLoginForPw" @submit.prevent="submit_accpwd" v-show="tabActive=='pwd'"  remark="账号密码登录">
                <div class="login_acc_pwd">
                    <div prop="acc">
                        <input type="text" tabindex="1" autocomplete="off" v-model.trim='acc_form.acc' placeholder="账号"/>
                        <span class="tips"></span>
                    </div>
                    <div prop="pwd">
                        <input type="password" tabindex="2" autocomplete="off"  v-model.trim='acc_form.pwd' placeholder="密码"/>
                        <span class="tips"></span>
                    </div>
                </div>    
                <div class="login_vcode"  prop="vcode">
                    <div class="login_vcode_input">
                        <input type="number" tabindex="3" v-model.trim='acc_vcode.val' autocomplete="off" placeholder="校验码"/>
                        <loading bubble v-if="acc_vcode.loading"></loading>
                        <img v-else class="vcode_img" @click="getvcode" :src="acc_vcode.base64" :style="{'opacity': acc_vcode.base64=='' ? 0 : 1}"/>
                    </div>               
                    <dragbox ref="acc_pwd_drag" id="acc_pwd" @dragfinish="dragfinish"></dragbox>
                    <span class="tips"></span>
                </div> 
                <div class="login_btn">
                    <button type="submit" tabindex="4"><icon>&#xe6c6</icon>登录</button>               
                </div>
            </form>

            <form class="login_area" v-if="config && config.IsLoginForSms" @submit.prevent="submit_sms" v-show="tabActive=='sms'" remark="短信登录">               
                <div class="login_sms">
                    <div prop="phone">
                        <input type="text" tabindex="1" autocomplete="off" v-model.trim='sms_form.phone' placeholder="手机号"/>
                        <span class="tips"></span>
                    </div>
                    <div prop="sms">
                        <input type="number" tabindex="2" autocomplete="off"  v-model.trim='sms_form.vcode' placeholder="短信验证码"/>
                        <span class="tips"></span>                      
                        <div class="sms_handler">
                            <loading bubble v-if="loading_sms"></loading>
                            <span v-else-if="sms_seconds>0">{{sms_seconds}}秒后重发</span>
                            <span class="getsms" v-else @click="getsms">获取验证码</span>
                        </div>
                    </div>
                </div>  
                <div class="login_btn">
                    <button type="submit" tabindex="4"><icon>&#xe6c6</icon>登录</button>               
                </div>
            </form>
            <div class="disabled_login" v-if="disabled_login">
                <icon>&#xe774</icon>
                账号登录与短信登录都被禁止
            </div>
            <config ref="config" class="config" :isuse="true" :disabled="false">
                <span slot="item" slot-scope="data" :title='data.item.name' @click="$refs['config'].eventClick(data.item,1)">
                    <img :src="data.img" />
                    {{data.item.obj.Tl_Name}}
                </span>
            </config>
            <div class="login_register">
                <a href="up" v-if="config && !config.IsRegStudent"><icon>&#xe7cd</icon>注册</a>
                <a href="find"><icon>&#xe76a</icon>找回密码</a>
            </div>  
        </template>
    </div>`
});

//滑块
Vue.component('dragbox', {
    //id: 组件的html元素id
    //init:完成度的值
    props: ['id'],
    data: function () {
        return {
            finish: 85,         //拖到百分之多少，算完成
            dragging: {
                init: 0, start: false,
                percent: 0, value: 0,
                finish: false
            }
        }
    },
    watch: {
        'id': {
            handler: function (val, old) {
                this.$nextTick(function () {
                    this.drag(val);
                })
            }, immediate: true
        },
        //拖动中的值变化
        'dragging': {
            handler: function (nv, ov) {
                if (!ov) return;
                //一些计算处理                
                if (!nv.start && !nv.finish) nv.percent = nv.value = 0;    //当停止拖动时
                nv.finish = ov.percent >= this.finish;      //是否为完成状态
                //滑动组件的html元素对象
                var dom = $dom('#' + this.dragid);
                var box = dom.find('.login_dragbox');//滑块元素对象
                var slide = dom.find('.slideway');       //滑块尾部的滑道
                //如果处在拖动中，则滑块去除动态效果
                var initleft = 5;
                if (nv.start) {
                    box.addClass('drag').css({ 'transition': '', 'opacity': 1 }).left(initleft + nv.value);
                    slide.css({ 'transition': '' }).show().width(initleft + nv.value);
                } else {
                    box.css('transition', 'left 0.3s').removeClass('drag').left(initleft);
                    slide.css('transition', 'width 0.3s').width(0);
                }
                //拖动完成
                if (nv.finish) {
                    dom.addClass('complete').css({ 'transition': 'opacity 1s', 'opacity': 0.5 });
                } else {
                    dom.removeClass('complete').css({ 'transition': '', 'opacity': 1 }).show();
                }
                if (nv.finish && !nv.start) {
                    this.$emit('dragfinish');
                    dom.hide();
                }
            }, immediate: true, deep: true
        },
    },
    computed: {
        'dragid': function () {
            return 'dragbox_' + this.id;
        }
    },
    created: function () { },
    methods: {
        //滑块拖动
        drag: function (id) {
            var dom = $dom('#' + this.dragid);
            var box = dom.find('.login_dragbox');       //滑块
            var th = this;
            //滑块的事件;
            box.bind('mousedown,touchstart', function (e) {
                if (e && e.preventDefault) e.preventDefault();
                if (th.dragging.finish) return;
                th.dragging.start = true;
                th.dragging.init = $dom.mouse(e).x; //拖动时的初始鼠标值
            }).bind('mouseup,touchend', function (e) {
                if (e && e.preventDefault) e.preventDefault();
                th.dragging.start = false;
            });
            //整个滑动组件的事件
            dom.bind('mouseleave,touchend', function (e) {
                if (e && e.preventDefault) e.preventDefault();
                th.dragging.start = false;
            }).bind('mousemove,touchmove', function (e) {
                if (e && e.preventDefault) e.preventDefault();
                //计算移动最大宽度范围
                var node = e.target ? e.target : e.srcElement;
                var parent = $dom(node).parent();
                var max = parent.width() - box.width() - 15;
                //计算移动值与完成百分比
                var left = $dom.mouse(e).x - th.dragging.init;
                th.dragging.value = left <= 0 ? 0 : (left >= max ? max : left);
                th.dragging.percent = th.dragging.value / max * 100;
            });
        },
        //初始化
        init: function () {
            this.dragging.start = false;
            this.dragging.percent = 0;
            this.dragging.value = 0;
            this.dragging.finish = false;
        }
    },
    template: `<div :id="dragid" class="login_drag" v-if="true">
            <div>
                <span>向右拖动滑块</span>                                         
                <div class="login_dragbox"></div>
                <div class="slideway"></div>
            </div>
        </div>`
});
