$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项

            loading: true,
            loading_bind: ''

        },
        mounted: function () {
            /*
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                vapp.loading = false;
                //获取结果
                th.account = account.data.result;
                if (th.account)
                    th.account.Ac_Sex = String(th.account.Ac_Sex);
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            })).catch(function (err) {
                console.error(err);
            });*/
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); }
        },
        watch: {
            'account': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    this.account.Ac_Sex = String(nv.Ac_Sex);
                    this.loading = false;
                }, immediate: true
            },
        },
        methods: {            
            //是否绑定
            isbind: function (tag) {
                let field = 'Ac_' + tag;
                return this.account[field] != '' && this.account[field] != null;
            },
            //取消绑定
            cancelbind: function (tag) {
                this.$dialog.confirm({
                    message: '是否确定取消绑定？',
                }).then(function () {
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
                }).catch(function () { });
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
            isexist: t => { return !$api.isnull(t.data); }
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
}, ['/Utilities/Components/avatar.js',
    '/Utilities/OtherLogin/config.js']);
