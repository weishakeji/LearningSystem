$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},
            organ: {},
            config: {},
            plate: {},


            loading: false
        },
        mounted: function () {
        },
        created: function () {
            //console.log($dom.device());
            //console.log(window.top);
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
            'account': function (nv, ov) {
                var th = this;
                if (!(nv == null || JSON.stringify(nv) === '{}')) {
                    window.setTimeout(function () {
                        th.successful(nv);
                    }, 100);
                }
            },
            'config': function (nv, ov) {
                if (nv.IsRegStudent)
                    $dom("*[v-if='config.IsRegStudent']").hide();
            },
            'plate': function (nv, ov) {
                window.platinfo = nv;
            }
        },
        methods: {
            //已经登录
            successful: function (account) {
                this.account = account;
                this.$refs.header.account = account;
                var th = this;
                window.setTimeout(function () {
                    let referrer = $api.querystring('referrer');
                    if ($api.isnull(referrer)) referrer = $api.storage('singin_referrer');
                    if ($api.isnull(referrer) || referrer == 'undefined') referrer = '/';
                    $api.storage('singin_referrer', null);      //去除本地记录的来源页信息
                    if (referrer != '')
                        window.navigateTo(decodeURIComponent(referrer));
                }, 1000 * 2);
            },
            //退出登录
            logout: function () {
                var th = this;
                th.$confirm('是否确定退出登录？').then(function () {
                    th.account = {};
                    $api.login.out('account', function () {
                        window.setTimeout(function () {
                            if (th.referrer == '')
                                window.location.href = '/web/';
                            else
                                window.location.href = th.referrer;
                        }, 500);
                    });

                }).catch(function () { });
            },
            gourl: function (url) {
                window.location.href = url;
            }
        }
    });
    Vue.component('notices', {
        props: ["org", "count"],
        data: function () {
            return {
                datas: [],
                loading: false
            }
        },
        watch: {
            'org': {
                handler: function (nv, ov) {
                    this.getdatas();
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () {
        },
        methods: {
            getdatas: function () {
                var th = this;
                th.loading = true;
                var orgid = th.org.Org_ID;
                $api.get('Notice/showitems', { 'orgid': orgid, 'type': -1, 'count': th.count }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.datas = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            }
        },
        template: ` <div class="notices" v-if='datas.length>0'>
            <div class="noItem" v-for="(data,i) in datas">
                <div>
                    <b :class="'order_'+i">{{i+1}}.</b>
                    <a :href="'/web/notice/detail.'+data.No_Id" :title="data.No_Ttl">
                    {{data.No_Ttl}}</a>
                </div>
                <div class="nodate">
                    <icon>&#xe81a</icon>{{data.No_StartTime|date("yyyy-MM-dd HH:mm")}}
                </div>
            </div>
        </div>`
    });

}, ['/Utilities/Components/avatar.js',
    '/Utilities/OtherLogin/config.js',      //第三方登录的配置项
    '/Utilities/Components/Sign/Login.js',
    "../Components/subject_rec.js",
    '../scripts/pagebox.js',
]);
$dom.load.css([$dom.path() + 'styles/pagebox.css']);