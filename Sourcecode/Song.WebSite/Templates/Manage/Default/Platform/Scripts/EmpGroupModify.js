
$ready(function () {

    window.vue = new Vue({
        el: '#app',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            entity: {}, //当前数据对象
            organ: {},           //当前登录账号所在的机构
            rules: {
                EGrp_Name: [
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
            //如果是新增界面
            if (this.id == '') {
                this.entity.EGrp_IsUse = true;
                $api.get('Admin/Organ').then(function (req) {
                    if (req.data.success) {
                        vue.organ = req.data.result;
                        vue.entity.Org_ID = vue.organ.Org_ID;
                       
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });                
                return;
            }
            //如果是修改界面
            var th = this;
            $api.get('Admin/GroupForID', { 'id': this.id }).then(function (req) {
                if (req.data.success) {
                    th.entity = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
            });
        },
        methods: {
            btnEnter: function () {
                var th = this;
                if (this.loading) return;
                var apiurl = this.id == '' ? "Admin/GroupAdd" : 'Admin/GroupModify';
                $api.post(apiurl, { 'entity': vue.entity }).then(function (req) {
                    if (req.data.success) {
                        vue.$message({
                            type: 'success',
                            message: '操作成功!',
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
                window.top.$pagebox.source.tab(window.name, 'vapp.loadDatas', true);
            }
        },
    });

});
