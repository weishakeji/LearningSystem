$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            couid: $api.querystring('couid'),   //课程id

            //课程公告分类
            columns: [],         //课程公告的分类          
            //当前要操作的公告的对象
            entity: {
                Gu_IsUse: true, Gu_IsShow: true
            },
            guide_rules: {
                Gu_Title: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 2, max: 200, message: '长度在 2 到 200 个字符', trigger: 'blur' }
                ]
            },
            activeName: 'general',       //选项卡
            loading: false,
            loading_sumbit: false,
        },
        mounted: function () {
            this.getColumnsTree();
            var th = this;
            th.loading = true;
            th.getguide().then(function (data) {
                th.entity = data;
            }).catch(err => {
                alert(err);
                console.error(err);
            }).finally(() => th.loading = false);
        },
        created: function () {

        },
        computed: {
            //是否新增账号
            isadd: t => t.id == null || t.id == '' || this.id == 0,
        },
        watch: {
        },
        methods: {
            //获取分类的数据，为树形数据
            getColumnsTree: function () {
                var th = this;
                th.loading = true;
                $api.put('Guide/ColumnsTree', { 'couid': th.couid, 'search': '', 'isuse': '' }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.columns = req.data.result;
                    } else {
                        th.columns = [];
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //获取信息
            getguide: function () {
                var th = this;
                return new Promise(function (resolve, reject) {
                    if (th.id == '' || th.id == 0) {
                        resolve(th.entity);
                    } else {
                        $api.get('Guide/ForID', { 'id': th.id }).then(function (req) {
                            if (req.data.success) {
                                resolve(req.data.result);
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => reject(err));
                    }
                });
            },
            guideEnter: function (formName, isclose) {
                var th = this;
                this.$refs[formName].validate((valid,fields) => {
                    if (valid) {
                        var obj = th.entity;
                        obj['Cou_ID'] = th.couid;
                        if (obj.Gc_UID && $api.getType(obj.Gc_UID) == "Array") {
                            if (obj.Gc_UID.length > 0)
                                obj.Gc_UID = obj.Gc_UID[obj.Gc_UID.length - 1];
                        }
                        $api.post('Guide/' + (th.isadd ? 'Add' : 'Modify'), { 'entity': obj }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.fresh_parent(isclose);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            th.$alert(err, '错误');
                        });
                    } else {
                         //未通过验证的字段
                         let field = Object.keys(fields)[0];
                         let label = $dom('label[for="' + field + '"]');
                         while (label.attr('tab') == null)
                             label = label.parent();
                         th.activeName = label.attr('tab');
                         console.log('error submit!!');
                         return false;
                    }
                });
            },
            //刷新上级列表
            fresh_parent: function (isclose) {
                //如果处于课程编辑页，则刷新
                var pagebox = window.top.$pagebox;
                if (pagebox && pagebox.source.box)
                    pagebox.source.box(window.name, 'vapp.fresh_frame("vapp.handleCurrentChange()")', isclose);
            }
        }
    });

});
