
//通用组件，主要取一些常用值与判断
//事件：
//login:当学员登录
//search: 搜索
//load: 加载成功，三个参数，机构、机构参数、平台参数
Vue.component('generic', {
    props: [],
    data: function () {
        return {
            account: {},        //当前登录的学员账号
            teacher: {},         //当前登录的教师账号
            //平台信息
            platinfo: {},           //平台信息
            config: {},             //当前机构的配置项
            organ: {},        //当前机构
        }
    },
    watch: {
        'config': {
            handler: function (nv, ov) {
                if ($api.isnull(nv)) return;
                //是否在手机中
                let ismobi = $api.ismobi();
                let disenableMweb = nv.DisenableMweb ? nv.DisenableMweb : false;
                if (disenableMweb && ismobi) {
                    return this.goAccessDenied('禁止手机端访问');
                }
                //是否在微信中
                let isWeixin = $api.isWeixin();
                let disenableWeixin = nv.DisenableWeixin ? nv.DisenableWeixin : false;
                if (disenableWeixin && isWeixin) {
                    return this.goAccessDenied('禁止在微信中访问');
                }
                //是否在微信小程序中
                let isWeixinApp = $api.isWeixinApp();
                let disenableMini = nv.DisenableMini ? nv.DisenableMini : false;
                if (disenableMini && isWeixinApp) {
                    return this.goAccessDenied('禁止在微信小程序中访问');
                }
                //是否在手机APP中
                //..(还没有编写)
                //console.log(nv);
            }, immediate: true
        },
    },
    computed: {
        //学员是否登录
        islogin: (t) => { return !$api.isnull(t.account); }
    },
    mounted: function () {
        var th = this;
        th.init();
        //学员登录
        th.loading_login = true;
        $api.login.current('account', function (acc) {
            th.account = acc;
            //触发登录后的事件,第二个参数表示是否登录，用于判断登录判断的操作是否完成
            th.$emit('login', th.account, true);
            th.loading_login = false;
        }, function (err) {
            console.log(err);
            th.$emit('login', {}, true);
            th.loading_login = false;
        });
    },
    methods: {
        init: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(([platinfo, organ]) => {
                //获取结果             
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                document.title += ' - ' + th.organ.Org_PlatformName;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                //加载成功的事件
                th.$emit('load', th.organ, th.config, th.platinfo);
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);;
        },
        //跳转到禁用页
        goAccessDenied(para) {
            window.navigateTo($api.url.set('/mobi/AccessDenied', {
                'msg': para
            }));
        }
    },
    template: ``
});
