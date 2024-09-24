$ready(['/Utilities/Components/Sign/Login.js'],
    function () {
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
                this.check(err => {
                    this.checked = err == '';
                    this.error = err;
                });
            },
            created: function () { },
            computed: {},
            watch: {
                //是否验证通过
                checked: function (nv, ov) {
                    if (nv) {
                        console.log(true);
                        this.loging();
                    }
                }
            },
            methods: {
                //校验基础参数是否通过
                check: async function (callback) {
                    var ref = this.getreferrer();
                    if (ref == '') return callback('来源页错误，需要通过点击超链接跳转');
                    if (this.code == '') return callback('缺少参数: code');
                    if (this.user == '') return callback('缺少参数: user');
                    if (this.appid == '') return callback('缺少参数: appid');
                    //判断参数密文
                    var param = $api.querystring(null, []).filter(item => item.key != 'code' && item.key != 'goto');
                    var code = '';
                    param.forEach(item => code += item.val);
                    code = $api.md5(code);
                    if (code != this.code) return callback('参数校验失败');
                    var th = this;
                    th.loading = true;
                    await $api.get('Sso/ForAPPID', { 'appid': th.appid }).then(function (req) {
                        if (req.data.success) {
                            th.interface = req.data.result;
                            if (!(JSON.stringify(th.interface) != '{}' && th.interface != null)) return callback('登录接口不存在或被禁用');
                            if (!th.interface.SSO_IsUse) return callback('禁用的登录接口');
                            if (th.interface.SSO_Domain != ref) return callback('来源页错误，请求域不匹配');
                            callback('');
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => callback(err)).finally(() => th.loading = false);

                },
                //获取来源页域名
                getreferrer: function () {
                    var ref = document.referrer;
                    if (ref == '') return ref;
                    ref = ref.indexOf('http://') > -1 ? ref.replace('http://', '') : ref;
                    ref = ref.indexOf('https://') > -1 ? ref.replace('https://', '') : ref;
                    //ref = ref.indexOf(':') > -1 ? ref.substring(0, ref.indexOf(':')) : ref;
                    ref = ref.indexOf('?') > -1 ? ref.substring(0, ref.indexOf('?')) : ref;
                    ref = ref.indexOf('/') > -1 ? ref.substring(0, ref.indexOf('/')) : ref;
                    ref = ref.indexOf('#') > -1 ? ref.substring(0, ref.indexOf('#')) : ref;
                    return ref.toLowerCase();
                },
                //获取接口对象
                loging: function () {
                    var th = this;
                    th.loading = true;
                    $api.post('Sso/Login', { 'appid': th.appid, 'user': th.user, 'name': th.name, 'sort': th.sort })
                        .then(function (req) {
                            if (req.data.success) {
                                //登录成功
                                var result = req.data.result;
                                th.loading_income=true;
                                //return;
                                th.$refs['login'].success(result, 'web端', th.getreferrer(), '');
                                console.error(result); 
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => {
                            if (err.indexOf(' ') > -1) err = err.substring(err.indexOf(' '));
                            th.error = err;
                            console.error(err);
                        }).finally(() => th.loading = false);
                },
                //登录成功后的事件,acc:当前登录的账户对象
                successful: function (acc) {
                    var th = this;
                    //跳转到指定页面
                    window.setTimeout(function () {
                        var gourl = th.goto == '' ? '/' : th.goto;
                        window.location.href = gourl;
                    }, 500);
                }
            }
        });

    });
