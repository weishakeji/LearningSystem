
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            account: {},    //当前账号
            entity: {}, //当前对象    
            levels: [], //机构等列表
            domain: '',  //主域
            lv_id: '',   //当前机构等级
            rules: {
                Org_PlatformName: [
                    { required: true, message: '平台名称不得为空', trigger: 'blur' }
                ],
                Org_Name: [
                    { required: true, message: '机构名称不得为空', trigger: 'blur' }
                ],
                Org_AbbrName: [
                    { required: true, message: '机构简称不得为空', trigger: 'blur' }
                ]
            },
            mapshow: false,      //是否显示地图信息
            loading: false
        },
        watch: {
            'lv_id': function (nl, ol) {
                this.entity.Olv_ID = nl;
            },
        },
        created: function () {
            var th = this;
            $api.post('Admin/Super').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                    $api.get('Organization/ForID', { 'id': result.Org_ID }).then(function (req) {
                        if (req.data.success) {
                            th.entity = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                } else {
                    throw '未登录，或登录状态已失效';
                }
            }).catch(function (err) {
                th.account = null;
                alert(err);
            });
        },
        methods: {

            //保存信息
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        $api.post('Organization/Modify', { 'entity': th.entity, 'exclude': '' }).then(function (req) {
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
                        }).catch(function (err) {
                            //window.top.ELEMENT.MessageBox(err, '错误');
                            th.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vue.handleCurrentChange', true);
            }
        },
    });

}, ['/Utilities/baiduMap/convertor.js',
    '/Utilities/baiduMap/map_setup.js']);
