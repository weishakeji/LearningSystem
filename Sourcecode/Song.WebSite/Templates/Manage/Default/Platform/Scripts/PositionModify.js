
$ready(function () {

    window.vue = new Vue({
        el: '#app',
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
            'loading': function (val, old) {
                console.log('loading:' + val);
            }
        },
        created: function () {
            var th = this;
            $api.get('Admin/Organ').then(function (req) {
                if (req.data.success) {
                    vue.organ = req.data.result;
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
            });
        },
        methods: {
            btnEnter: function () {
                var th = this;
                if (this.loading) return;
                //如果是新增界面
                if (this.id == '') {
                    $api.post('Position/Add', { 'posi': vue.entity }).then(function (req) {
                        if (req.data.success) {
                            vue.$message({
                                type: 'success',
                                message: '添加成功!',
                                center: true
                            });
                            th.operateSuccess();
                        } else {
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                    });

                    return;
                }
                //如果是修改
                $api.post('Position/Modify', { 'posi': vue.entity }).then(function (req) {
                    if (req.data.success) {
                        vue.$message({
                            type: 'success',
                            message: '修改成功!',
                            center: true
                        });
                        th.operateSuccess();

                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    vue.$alert(err, '错误');
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vue.loadDatas', true);
            }
        },
    });

});
