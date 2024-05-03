
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            visibleHelp: false,    //帮助面板的显示
            visibleDemo: false,    //示例面板的显示

            //当前数据对象
            entity: {
                SSO_IsUse: true,
                SSO_APPID: ''
            },
            organ: {},           //当前登录账号所在的机构
            demourl: '',         //示例
            rules: {
                SSO_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 1, max: 50, message: '长度在 1 到 50 个字符', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isExist(value).then(res => {
                                if (res) callback(new Error('已经存在!'));
                            });
                        }, trigger: 'blur'
                    }
                ],
                SSO_Domain: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 1, max: 500, message: '长度在 1 到 500 个字符', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (value == undefined || value == '') return callback();
                            if (value == 'localhost') return callback();
                            var pattern = /[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+\.?/;
                            if (!pattern.test(value)) callback(new Error('请输入合法域名'));
                            else if (value.indexOf('?') > -1 || value.indexOf('#') > -1 || value.indexOf('&') > -1) {
                                return callback(new Error('仅限域名，勿带参数'));
                            } else if (value.length >= 'http://'.length && value.substring(0, 7).toLowerCase() == 'http://') {
                                return callback(new Error('勿带 “http://” 前缀'));
                            } else if (value.length >= 'https://'.length && value.substring(0, 8).toLowerCase() == 'https://') {
                                return callback(new Error('勿带 “https://” 前缀'));
                            } else if (value.indexOf('/') > -1) {
                                var tmp = value.indexOf('http://') > -1 ? value.replace('http://', '') : value;
                                tmp = tmp.indexOf('https://') > -1 ? tmp.replace('https://', '') : tmp;
                                if (tmp.indexOf('/') > -1) return callback(new Error('仅限域名，勿带路径'));
                            } else callback();
                        }, trigger: 'blur'
                    }],
                SSO_Phone: [{
                    validator: function (rule, value, callback) {
                        if (value == undefined || value == '') return callback();
                        var pattern = /^((\+86)|(86))?(1)\d{10}$/;
                        if (!pattern.test(value)) callback(new Error('请输入电话!'));
                        else callback();
                    }, trigger: 'blur'
                }],
                SSO_Email: [{ type: 'email', message: '请输入邮箱', trigger: 'blur' }],
                SSO_Info: [{
                    validator: function (rule, value, callback) {
                        if (value == undefined || value == '') return callback();
                        if (value.length > 500) callback(new Error('最多允许 500 字符，当前 ' + value.length + ' 字符'));
                    }, trigger: 'blur'
                }]
            },
            //演示数据
            demo: { user: '', name: '', sort: '', goto: '' },
            rules_demo: {
                user: [{ required: true, message: '不得为空', trigger: 'blur' }],
            },
            loading: false
        },
        watch: {
            'demo': {
                handler: function (val, old) {
                    this.demourl = this.buildemo(val);
                }, deep: true, immediate: true
            },
            'entity': {
                handler: function (val, old) {
                    this.demourl = this.buildemo(val);
                }, deep: true, immediate: true
            }
        },
        created: function () {
            var th = this;
            //如果是新增界面
            if (this.id == '') {
                this.getuid(uid => th.entity.SSO_APPID = uid);
                return;
            }
            //如果是修改界面
            th.loading = true;
            $api.get('Sso/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    th.entity = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(err => alert(err)).finally(() => th.loading = false);
        },
        methods: {
            //获取APPID
            getuid: async function (callback) {
                await $api.get('Systempara/UniqueID').then(function (req) {
                    if (req.data.success) {
                        callback(req.data.result);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //判断是否已经存在
            isExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('Sso/Exist', { 'name': val, 'id': th.id }).then(function (req) {
                        if (req.data.success) {
                            return resolve(req.data.result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        return reject(err);
                    });
                });
            },
            btnEnter: function (formName) {
                if (this.loading) return;
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apiurl = th.id == '' ? "Sso/Add" : 'Sso/Modify';
                        $api.post(apiurl, { 'entity': th.entity }).then(function (req) {
                            if (req.data.success) {
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.operateSuccess();
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err, '错误');
                        }).finally(() => th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //生成演示
            buildemo: function (demo) {
                var href = window.location.origin + '/sso/login?';
                var md5 = $api.md5(this.entity.SSO_APPID + demo.user + demo.name + demo.sort);
                var goto = demo.goto == '' || demo.goto == undefined ? '' : encodeURIComponent(demo.goto);
                href = $api.url.set(href, {
                    appid: this.entity.SSO_APPID,
                    user: demo.user,
                    name: demo.name,
                    sort: demo.sort,
                    code: md5
                });
                href = $api.url.set(href, 'goto', goto);
                return href;
            },
            //复制示例内容
            copydemo: function (formName, txt) {
                this.$refs[formName].validate((valid) => {
                    if (valid) this.copytext(txt, '示例内容');
                });
            },
            //复制文本
            copytext: function (txt, title) {
                var th = this;
                title = title == null ? txt : title;
                th.copy(txt, 'textarea').then(function (data) {
                    data.$message({
                        message: '复制 “' + title + '” 到粘贴板',
                        type: 'success'
                    });
                });
            },

            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.loadDatas', true);
            }
        },
    });

});
