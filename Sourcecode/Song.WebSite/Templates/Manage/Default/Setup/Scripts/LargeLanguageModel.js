
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //要操作的对象
            apikey: '',
            rules: {
                'apikey': [
                    { required: false, message: '请输入阿里云百炼平台的AppKey', trigger: 'blur' }
                ]
            },
            error: '',       //错误提示
            //当前登录的管理员
            admin: null,
            //当前机构
            org: {},
            loading: false,
            loading_init: false
        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.admin); },
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.get('Admin/Super').then(function (req) {
                if (req.data.success) {
                    th.admin = req.data.result;
                    if (!th.islogin) return;
                    th.loading = true;
                    $api.bat(
                        $api.post('LLM/GetApikey'),
                        $api.get('Organization/ForID', { 'id': th.admin.Org_ID })
                    ).then(([apikey, org]) => {
                        th.apikey = apikey.data.result;
                        th.org = org.data.result;
                    }).catch(err => alert(err))
                        .finally(() => th.loading = false);
                } else {
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        methods: {
            //保存信息
            btnEnter: function () {
                var th = this;
                if (th.loading) return;
                this.$refs['entity'].validate(function (valid) {
                    if (valid) {
                        th.loading = true;
                        $api.post("LLM/SetAppkey", { 'apikey': th.apikey }).then(function (req) {
                            if (req.data.success) {
                                //th.apikey = req.data.result;
                                th.$message({ message: '操作成功', type: 'success' });
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                        }).finally(() => th.loading = false);
                    }
                });
            },
        }
    });

});


