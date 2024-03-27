
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            entity: {}, //当前数据对象
            organ: {},           //当前登录账号所在的机构
            rules: {
                Posi_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            }
        },
        watch: {

        },
        computed: {
            //是否新增对象
            isadd: t => { return t.id == null || t.id == ''; },
        },
        created: function () {
            var th = this;
            $api.get('Admin/Organ').then(function (req) {
                if (req.data.success) {
                    th.organ = req.data.result;
                    //如果是新增界面
                    if (th.id == '') {
                        th.entity.Posi_IsUse = true;
                        th.entity.Org_ID = th.organ.Org_ID;
                    } else {
                        $api.get('Position/ForID', { 'id': th.id }).then(function (req) {
                            if (req.data.success) {
                                th.entity = req.data.result;
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                        });
                    }

                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() => { });
        },
        methods: {
            btnEnter: function (formName, isclose) {
                var th = this;
                if (this.loading) return;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var apiurl = this.id == '' ? "Position/Add" : 'Position/Modify';
                        $api.post(apiurl, { 'posi': th.entity }).then(function (req) {
                            if (req.data.success) {
                                th.$notify({
                                    type: 'success', position: 'bottom-left',
                                    message: '操作成功!'
                                });
                                th.operateSuccess(isclose);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                        }).finally(() => { });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.loadDatas', isclose);
            }
        },
    });

});
