// 登录控件
$dom.load.css(['/Utilities/Components/sign/Styles/login.css']);
Vue.component('login', {
    //config：机构配置项
    props: ['config'],
    data: function () {
        return {

            data: [],
            tabs: [{ name: '密码登录', tag: 'pwd', icon: 'e687', show: true },
            { name: '短信登录', tag: 'sms', icon: 'e76e', show: true }],
            tabActive: 'pwd',

            show: false,         //是否显示
            search: '',      //用于检索的字符
            loading: false
        }
    },
    watch: {
        'config': function (val, old) {
            //this.$emit('change', val);
            this.$nextTick(function () {
                //this.drag();
            });
        }
    },
    computed: {

    },
    created: function () {

    },
    methods: {

    },
    template: `<div class="login_weisha">
        <div class="login_title_bar">
            <div v-for="t in tabs" @click="tabActive=t.tag" :tag="t.tag" v-if="t.show" :current="tabActive==t.tag">
                <icon v-html="'&#x'+t.icon"></icon>
                <span>{{t.name}}</span>
            </div>
        </div>
        <div class="login_pwd_area" v-if="tabActive=='pwd'">
            <div class="login_acc_pwd">
                <div><input type="text" tabindex="1" autocomplete="off" placeholder="账号"/></div>
                <div><input type="password" tabindex="2" autocomplete="off" placeholder="密码"/></div>
            </div>    
            <div class="login_vcode">
                <div class="login_vcode_input">
                    <input type="text" tabindex="3" autocomplete="off" placeholder="校验码"/>
                    <img class="vcode_img" src=""/>
                </div>               
                <dragbox id="acc_pwd"></dragbox>
            </div> 
            <div class="login_btn">
                <button type="submit" tabindex="4"><icon>&#xe6c6</icon>登录</button>               
            </div>
        </div>
        <div class="login_area" v-if="tabActive=='sms'">
            短信的登录
        </div>
        <div class="login_register">
            <a href="up" v-if="!config.IsRegStudent"><icon>&#xe7cd</icon>注册</a>
            <a href="find"><icon>&#xe76a</icon>找回密码</a>
        </div>  
        <config ref="config" class="config" :isuse="true">
            <div slot="item" slot-scope="data" :title='data.item.name'>
                <img :src="data.img" />
                {{data.item.obj.Tl_Name}}
            </div>
        </config>
    </div>`
});

//滑块
Vue.component('dragbox', {
    //id
    //finish:完成度的值
    props: ['id'],
    data: function () {
        return {
            dragvalue: 0,        //当前拖动x坐标值
            dragper: 0,          //拖动完成度

            finish: 70,
            //drag_start: false,
            //drag_end: false,

            //drag_init_x: 0,       //初始坐标
            dragging: {
                init: 0,
                start: false,
                end: false,
                percent: 0,
                value: 0,
                finish: false,
            },
            //dragfinish: false        //拖动是否完成
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
        'drag_start': function (nv, ov) {
            console.log(nv);
        },
        'dragging': {
            handler: function (nv, ov) {
                console.log(nv);
                if (!nv.start) {
                    var dom = $dom('#' + this.dragid);
                    dom.find('.login_dragbox').left(5);
                }
            }, immediate: true, deep: true
        },
    },
    computed: {
        'dragid': function () {
            return 'dragbox_' + this.id;
        }
    },
    created: function () {

    },
    methods: {
        //滑块拖动
        drag: function (id) {
            var dom = $dom('#' + this.dragid);
            var th = this;
            //滑块的事件;
            dom.find('.login_dragbox').bind('mousedown,touchstart', function (e) {
                if (e && e.preventDefault) e.preventDefault();
                if (th.dragging.finish) return;
                th.dragging.start = true;
                th.dragging.end = false;
                th.dragging.finish = false;
                th.dragging.init = $dom.mouse(e).x; //拖动时的初始鼠标值
            }).bind('mouseup,touchend', function (e) {
                if (e && e.preventDefault) e.preventDefault();
                th.dragging.start = false;
                th.dragging.value = 0;
            });
            dom.bind('mouseleave,touchend', function (e) {
                if (e && e.preventDefault) e.preventDefault();
                th.dragging.start = false;
                th.dragging.value = 0;
            });
            dom.bind('mousemove,touchmove', function (e) {
                if (e && e.preventDefault) e.preventDefault();
                if (th.dragging.finish) return;
                //var obj = login._getObj(e);
                //if (obj.dragfinish) return; //如果拖动完成，则不能拖动
                //计算移动最大宽度范围
                var node = e.target ? e.target : e.srcElement;
                var parent = $dom(node).parent();
                var min = 5;
                var dragbox = dom.find('div.login_dragbox');
                var max = parent.width() - dragbox.width() - 10;
                //
                var mouse = $dom.mouse(e);
                var left = mouse.x - th.dragging.init;
                left = left <= min ? min : (left >= max ? max : left);
                if (th.dragging.start) {
                    dragbox.left(left);
                    var perecnt = th.finish;       //允许完成拖动的百分比，即达到这个值就算拖动成功
                    //console.error(perecnt);
                    var per = parseInt(dragbox.css('left')) / (parent.width() - dragbox.width()) * 100;
                    if (per >= perecnt) {
                        dom.find(".login_drag").addClass('complete').css("opacity", 0.8);
                    } else {
                        dom.find(".login_drag").removeClass('complete').css("opacity", 1);
                    }
                    th.dragging.percent = per;
                }
            });
        },
    },
    template: `<div :id="dragid" class="login_drag" v-if="true">
            <span>向右拖动滑块</span>                                         
            <div class="login_dragbox"></div>
            <div class="slideway"></div>
        </div>`
});
