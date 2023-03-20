
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            entity: {},
            rules: {
                Tl_APPID: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Tl_Secret: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Tl_Returl: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            },
            loading: false
        },
        computed: {
            //登录配置项的标识
            tag: function () {
                var url = window.location.href;
                if (url.indexOf('/') > -1) url = url.substring(url.lastIndexOf('/') + 1);
                if (url.indexOf('?') > -1) url = url.substring(0, url.lastIndexOf('?'));
                return url;
            }
        },
        watch: {
            //选择时间区间
            tag: {
                handler(nv, ov) {
                    if (nv == null || nv == '') return;
                    this.entity.Tl_Tag = nv;
                    this.getentity(nv);
                },
                immediate: true,
                deep: true
            }
        },
        created: function () {

        },
        methods: {
            //获取对象
            getentity: function (tag) {
                var th = this;
                th.loading = true;
                $api.get('OtherLogin/GetObject', { 'tag': tag }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        var cof = result.Tl_Config;
                        if (result.Tl_Config != '') {
                            try {
                                var conf = eval('(' + result.Tl_Config + ')');
                                for (k in conf)
                                    result[k] = conf[k];
                            } catch { }
                        }
                        //console.log(result);
                        th.entity = result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                }).finally(function () {
                    th.loading = false;
                });
            },
            //保存
            btnEnter: function (formName, isclose) {
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var th = this;
                        th.loading = true;
                        var entity = $api.clone(th.entity);
                        var conf = {
                            pubAppid: th.entity.pubAppid,
                            pubReturl: th.entity.pubReturl,
                            pubSecret: th.entity.pubSecret
                        };
                        entity.Tl_Config=JSON.stringify(conf);
                        console.log(entity);                       
                        $api.post('OtherLogin/Update', { 'entity': entity }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$notify({
                                    type: 'success',
                                    message: '修改成功',
                                    position: 'bottom-left',
                                    duration: 1000
                                });
                                th.operateSuccess(th.entity.Tl_Tag, isclose);
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        }).finally(function () {
                            th.loading = false;
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function (tag, isclose) {
                if (window.top.$pagebox)
                    window.top.$pagebox.source.tab(window.name, 'vapp.reload("' + tag + '")', isclose);
            }
        }
    });

});


