$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {}, //当前登录账号对象
            loading: false,
            loading_bind: ''      //绑定或取消绑定中的状态，
        },
        mounted: function () {
            this.getAccount();
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
        },
        methods: {
            //获取当前登录账号
            getAccount: function (func) {
                var th = this;
                th.loading_init = true;
                $api.get('Account/Current').then(function (req) {
                    th.loading_init = false;
                    if (req.data.success) {
                        th.account = req.data.result;
                        if (func != null) func(th.account);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //打开弹窗
            openbox: function (url, title, icon, width, height) {
                var pathname = window.location.pathname;
                if (pathname.indexOf('/') > -1)
                    url = pathname.substring(0, pathname.lastIndexOf('/') + 1) + url;
                var obj = {};
                obj = {
                    'url': url, 'ico': icon, 'title': title,
                    'pid': window.name, 'showmask': true, 'min': false, 'max': false,
                    'width': width ? width : 600, 'height': height ? height : 400
                }
                if (window.top.$pagebox)
                    window.top.$pagebox.create(obj).open();
            },
            //刷新页面
            fresh: function () {
                alert('fresh')
                window.location.reload();
            },
            //是否绑定
            isbind: function (tag) {
                let field = 'Ac_' + tag;
                return this.account[field] != '' && this.account[field] != null;
            },
            //取消绑定
            cancelbind: function (tag) {
                var th = this;
                th.loading_bind = tag;
                $api.get('Account/UserUnbind', { 'field': tag }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        window.location.reload();
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_bind = '');
            }
        }
    });
    //第三方平台的绑定信息
    Vue.component('thirdparty', {
        props: ['account', 'tag'],
        data: function () {
            return {
                data: {},        //
                loading: false
            }
        },
        watch: {
            'account': {
                handler: function (nv, ov) {
                    if (nv && nv.Ac_ID != null)
                        this.onload();
                }, immediate: true, deep: true
            }
        },
        computed: {
            //是否存在
            isexist: function () {
                return JSON.stringify(this.data) != '{}' && this.data != null;
            }
        },
        mounted: function () { },
        methods: {
            onload: function () {
                var th = this;
                th.loading = true;
                $api.get('Account/UserThirdparty', { 'acid': th.account.Ac_ID, 'tag': th.tag })
                    .then(function (req) {
                        if (req.data.success) {
                            th.data = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            }
        },
        template: `<div class="thirdparty">
                <loading v-if="loading"></loading>           
                <template v-else-if="isexist">
                    <img :src="data.Ta_Headimgurl" v-if="data.Ta_Headimgurl!=''"/>
                    <icon v-else>&#xe687</icon>
                    <span v-text="data.Ta_NickName"></span>
                </template>
        </div>`
    });
}, ['/Utilities/OtherLogin/config.js']);
