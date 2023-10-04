
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            entity: {}, //当前数据对象
            organ: {},           //当前登录账号所在的机构
            rules: {
                Title_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            }
        },
        watch: {
            'loading': function (val, old) {
                console.log('loading:' + val);
            }
        },
        computed: {
            //是否新增对象
            isadd: t => { return t.id == null || t.id == ''; },
        },
        created: function () {
            //如果是新增界面
            if (this.isadd) {
                var th = this;
                th.entity.Title_IsUse = true;
                $api.get('Admin/Organ').then(function (req) {
                    if (req.data.success) {
                        th.organ = req.data.result;
                        th.entity.Org_ID = th.organ.Org_ID;

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
            $api.get('Admin/TitleForID', { 'id': this.id }).then(function (req) {
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
            btnEnter: function (formName, isclose) {
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var th = this;
                        if (th.loading) return;
                        var apiurl = this.id == '' ? "Admin/TitleAdd" : 'Admin/TitleModify';
                        $api.post(apiurl, { 'entity': th.entity }).then(function (req) {
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
                            alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function (isclose) {
                //刷新列表页中的单条数据
                //window.top.$pagebox.source.tab(window.name, 'vapp.freshrow("' + this.id + '")', isclose);
                
                //刷新列表页，原本只用刷新单条数据，考虑到编辑页中有排序号的更改
                window.top.$pagebox.source.tab(window.name, 'vapp.loadDatas()', isclose);
            }
        },
    });

});
