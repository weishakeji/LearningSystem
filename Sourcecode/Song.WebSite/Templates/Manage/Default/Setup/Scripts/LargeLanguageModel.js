﻿
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //要操作的对象
            entity: {
                'apikey': '',   //apikey
                'model': ''     //大模型名称
            },

            rules: {
                'apikey': [
                    { min: 3, message: '长度在 3 到 5 个字符', trigger: 'blur' }
                ]
            },
            error: '',       //错误提示
            //当前登录的管理员
            admin: null,
            //配置项
            cfg: {},
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
                        $api.post('LLM/GetApikey'),     //获取apikey
                        $api.post('LLM/ModelCode'),     //模型的代码
                        $api.get('LLM/AliyunConfiguration') //配置项，即所有模型
                    ).then(([apikey,mcode, cfg]) => {
                        th.entity.apikey = apikey.data.result;
                        th.entity.model = mcode.data.result;
                        th.cfg = cfg.data.result;
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
                        $api.post("LLM/SetModel", th.entity).then(function (req) {
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


