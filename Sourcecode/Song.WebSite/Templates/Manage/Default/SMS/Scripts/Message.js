
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            mark: $api.querystring('mark'),
            entity: { text: '' },      //当前接口对象
            organ: {},  //当前机构
            platinfo: {},        //平台信息                    
            rules: {
                text: [{ required: true, message: '不得为空', trigger: 'blur' }]
            },
            loading: false
        },
        watch: {

        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Sms/getItem', { 'mark': th.mark }),
                $api.get('Sms/TemplateSMS', { 'mark': th.mark }),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (item, template, plat, organ) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw arguments[i].config.way + ' ' + data.message;
                    }
                }
                //获取结果
                th.entity = item.data.result;
                //th.entity.text = template.data.result;
                th.$set(th.entity, 'text', template.data.result);
                th.platinfo = plat.data.result;
                th.organ = organ.data.result;
            })).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        computed: {

        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                th.$refs[formName].validate((valid) => {
                    if (valid) {
                        return;
                        th.loading = true;
                        $api.post('Sms/Modify', { 'mark': th.mark, 'account': th.entity.user, 'pwd': th.entity.pw }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.operateSuccess();
                            } else {
                                throw req.data.message;
                            }
                        }).catch(err => alert(err))
                            .finally(() => th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.loadDatas', true);
            }
        },
    });

});
