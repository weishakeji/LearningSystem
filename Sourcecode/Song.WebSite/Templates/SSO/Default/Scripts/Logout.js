$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            appid: $api.querystring("appid"),
            user: $api.querystring("user", ''),
            name: $api.querystring("name", ''),
            sort: $api.querystring("sort", ''),
            goto: decodeURIComponent($api.querystring("goto", '')),
            code: $api.querystring("code", ''),

            interface: {},      //接口对象
            checked: false,      //是否验证通过

            error: '',       //服务端返回的错误信息
            loading: true,
            loading_income: false       //页面跳转中的预载
        },
        mounted: function () {
            this.check(c => this.checked = c);
        },
        created: function () { },
        computed: {},
        watch: {
            //是否验证通过
            checked: function (nv, ov) {
                if (nv) {
                    console.log(true);
                    $api.loginstatus('account', '');
                      //跳转到指定页面
                      window.setTimeout(function () {                      
                        var gourl = th.goto == '' ? '/' : th.goto;
                        window.location.href = gourl;
                    }, 500);
                }
            }
        },
        methods: {
            //校验基础参数是否通过
            check: async function (callback) {
                var ref = this.getreferrer();
                if (ref == '' || this.code == '' || this.user == '') return callback(false);
                //判断参数密文
                var param = $api.querystring(null, []).filter(item => item.key != 'code' && item.key != 'goto');
                var code = '';
                param.forEach(item => code += item.val);
                code = $api.md5(code);
                if (code != this.code) return callback(false);
                var th = this;
                await $api.get('Sso/ForAPPID', { 'appid': th.appid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.interface = req.data.result;
                        if (!(JSON.stringify(th.interface) != '{}' && th.interface != null)) return callback(false);
                        if (!th.interface.SSO_IsUse) return callback(false);
                        if (th.interface.SSO_Domain != ref) return callback(false);
                        callback(true);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });

            },
            //获取来源页域名
            getreferrer: function () {
                var ref = document.referrer;
                if (ref == '') return ref;
                ref = ref.indexOf('http://') > -1 ? ref.replace('http://', '') : ref;
                ref = ref.indexOf('https://') > -1 ? ref.replace('https://', '') : ref;
                ref = ref.indexOf(':') > -1 ? ref.substring(0, ref.indexOf(':')) : ref;
                ref = ref.indexOf('?') > -1 ? ref.substring(0, ref.indexOf('?')) : ref;
                ref = ref.indexOf('/') > -1 ? ref.substring(0, ref.indexOf('/')) : ref;
                ref = ref.indexOf('#') > -1 ? ref.substring(0, ref.indexOf('#')) : ref;
                return ref.toLowerCase();
            }
        }
    });

});
