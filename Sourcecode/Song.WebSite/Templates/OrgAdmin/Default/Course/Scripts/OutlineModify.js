$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('couid'),       //课程id
            id: $api.querystring('olid'),     //当前章节id
            uid: $api.querystring('uid'),       //章节的uid         

            activeName: $api.querystring('active', 'general'),
            outline: {},
            datas: [],       //章节列表
            olSelects: [],      //选择中的章节项

            rules: { 'Ol_Name': [{ required: true, message: '不得为空', trigger: 'blur' }] },
            loading: false
        },
        watch: {
            'outline': {
                handler: function (nv, ov) {

                }, deep: true, immediate: true
            }
        },
        mounted: function () {
            this.getTreeData();
        },
        created: function () {

        },
        computed: {
            //章节是否存在
            isexist: t => { return !$api.isnull(t.outline); }
        },
        methods: {
            //所取所有章节数据，为树形数据
            getTreeData: function () {
                var th = this;
                th.loading = true;
                $api.get('Outline/Tree', { 'couid': th.couid, 'isuse': null }).then(function (req) {
                    if (req.data.success) {
                        th.datas = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => this.getentity());
            },
            //获取章节
            getentity: function () {
                if (this.id == null || this.id == '') return;
                var th = this;
                th.loading = true;
                $api.get('Outline/ForID', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        th.outline = req.data.result;
                        //设置当前节点禁用，防止选择自身
                        var obj = th.traversalQuery(th.outline.Ol_ID, th.datas);
                        if (obj != null) obj.Ol_IsUse = false;
                        th.traversalUse(th.datas);
                        if (th.isexist) {
                            //将当前章节的上级路径，用于在控件中显示
                            var arr = [];
                            arr = th.getParentPath(th.outline, th.datas, arr);
                            th.olSelects = arr;
                            th.$refs['detail_editor'].setContent(th.outline.Ol_Intro);
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //创建新的章节对象
            createobj: function () {

            },
            //获取当前专业的上级路径
            getParentPath: function (entity, datas, arr) {
                var obj = this.traversalQuery(entity.Ol_PID, datas);
                if (obj == null) return arr;
                arr.splice(0, 0, obj.Ol_ID);
                arr = this.getParentPath(obj, datas, arr);
                return arr;
            },
            //从树中遍历对象
            traversalQuery: function (id, datas) {
                var obj = null;
                for (let i = 0; i < datas.length; i++) {
                    const d = datas[i];
                    if (d.Ol_ID == id) {
                        obj = d;
                        break;
                    }
                    if (d.children && d.children.length > 0) {
                        obj = this.traversalQuery(id, d.children);
                        if (obj != null) break;
                    }
                }
                return obj;
            },
            //所有Ol_IsUse取反，主要是Cascader控件的disabled取了Ol_IsUse作为值，它俩的意义相反
            traversalUse: function (datas, use) {
                for (let i = 0; i < datas.length; i++) {
                    const d = datas[i];
                    d.Ol_IsUse = use == null ? !d.Ol_IsUse : use;
                    if (d.children && d.children.length > 0)
                        obj = this.traversalUse(d.children, d.Ol_IsUse ? true : null);
                }
            },
            //当编辑章节中的上级章节变化时
            casader_change: function (data) {
                if (data == null) return;
                this.outline.Ol_PID = data[data.length - 1];
                //关闭级联菜单的浮动层
                this.$refs["outlines"].dropDownVisible = false;
            },
            //编辑当前章节
            btnModify: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var obj = th.modify_obj;
                        obj['Cou_ID'] = th.id;
                        obj['Org_ID'] = th.course.Org_ID;
                        $api.post('Outline/' + th.modify_obj.state, { 'entity': obj }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                $api.put('Outline/ForID', { 'id': obj.Ol_ID });
                                th.updatedEvent();
                                th.modify_show = false;
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            th.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //刷新上级列表
            fresh_parent: function () {
                //如果处于课程编辑页，则刷新
                var pagebox = window.top.$pagebox;
                if (pagebox && pagebox.source.box)
                    pagebox.source.box(window.name, 'vapp.fresh_frame("vapp.getTreeData")', false);
            }
        }
    });

}, ['Components/outline_live.js'                //章节直播的设置
]);
