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
        'selected': function (val, old) {
            this.show = false;
            console.log(this.show);
            this.$emit('change', val);
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
                <div class="login_drag" v-if="true">
                    <span>向右拖动滑块</span>                                         
                    <div class="login_dragbox"></div>
                    <div class="slideway"></div>
                </div>
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
    </div>`
});
