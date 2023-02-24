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

            loading: true,
            loading_income: false       //页面跳转中的预载
        },
        mounted: function () {
            var th = this;
            this.check(c => th.checked = c);

            console.log(th.checked);
            return;

            var ref = th.getreferrer();
            console.log(ref);

            var ee = window.opener;
            console.log(ee);
            th.getInterface();

            console.log(th.goto);

            var md5 = $api.md5('1');
            console.error(md5);
        },
        created: function () {

        },
        computed: {
            /*
            //接口是否存在
            exist: function () {
                var ref = this.getreferrer();
                if (ref == '' || this.code == '') return false;
                var code = $api.md5(this.appid + this.user + this.name + this.sort);
                if (code != this.code) return false;

                if (this.loading) return false;
                if (!(JSON.stringify(this.interface) != '{}' && this.interface != null)) return false;
                if (this.interface.SSO_Domain != ref) return false;

                return code == this.code;
            }*/
        },
        watch: {

        },
        methods: {
            //校验基础参数是否通过
            check: async function (callback) {
                var ref = this.getreferrer();
                //if (ref == '' || this.code == '' || this.user=='') return callback(false);
                var code = $api.md5(this.appid + this.user + this.name + this.sort);
                if (code != this.code) return callback(false);
                var th = this;
                $api.get('Sso/ForAPPID', { 'appid': th.appid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.interface = req.data.result;
                        if (!(JSON.stringify(th.interface) != '{}' && th.interface != null)) return callback(false);
                        if (!th.interface.SSO_IsUse) return callback(false);
                        if (th.interface.SSO_Domain != ref) return callback(false);
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
                ref = ref.indexOf('?') > -1 ? ref.substring(0, ref.indexOf('?')) : ref;
                ref = ref.indexOf('/') > -1 ? ref.substring(0, ref.indexOf('/')) : ref;
                ref = ref.indexOf('#') > -1 ? ref.substring(0, ref.indexOf('#')) : ref;
                return ref.toLowerCase();
            },
            //获取接口对象
            getInterface: function () {
                var th = this;
                th.loading = true;
                $api.get('Sso/ForAPPID', { 'appid': th.appid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.interface = req.data.result;
                        if (th.exist) {

                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            }
        }
    });

});
