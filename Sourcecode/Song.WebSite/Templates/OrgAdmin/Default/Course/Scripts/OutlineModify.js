$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('couid'),       //课程id
            id: $api.querystring('olid'),     //当前章节id
            pid: $api.querystring('pid'),     //章节父id
            uid: $api.querystring('uid'),       //章节的uid         

            activeName: $api.querystring('active', 'general'),
            //当前章节对象，下面是一些新增时的默认值
            outline: {
                Cou_ID: $api.querystring('couid'),
                Ol_IsUse: true,
                Ol_IsFinish: true,
                Ol_IsChecked: true,
                Ol_IsFree: false
            },
            livestream: {},      //直播流

            datas: [],       //所有章节，用于选择上级章节
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
            //是否新增章节
            isadd: t => { return t.id == null || t.id == ''; },
            //是否设置了直播
            islive: t => { return t.outline.Ol_IsLive && !$api.isnull(t.livestream); },
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
                    .finally(() => this.setentity());
            },
            //设置当前章节的信息
            setentity: function () {
                var th = this;
                th.loading = true;
                th.getentity().then(function (ol) {
                    th.outline = ol;
                    //设置当前节点禁用，防止选择自身
                    var obj = th.traversalQuery(th.outline.Ol_ID, th.datas);
                    if (obj != null) obj.Ol_IsUse = false;
                    th.traversalUse(th.datas);
                    th.getlivestream();
                    //将当前章节的上级路径，用于在控件中显示
                    var arr = [];
                    arr = th.getParentPath(th.outline, th.datas, arr);
                    th.olSelects = arr;
                    if (th.outline.Ol_Intro && th.$refs['detail_editor'])
                        th.$refs['detail_editor'].setContent(th.outline.Ol_Intro);
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //获取章节对象，如果不存在则创建新的章节对象
            getentity: function () {
                var th = this;
                return new Promise((resolve, reject) => {
                    if (th.isadd) {
                        $api.get('Snowflake/Generate').then(function (req) {
                            if (req.data.success) {
                                th.outline.Ol_ID = req.data.result;
                                th.outline.Ol_PID = th.pid;
                                resolve(th.outline);
                            } else {
                                reject(req.config.way + ' ' + req.data.message);
                            }
                        });
                    } else {
                        $api.get('Outline/ForID', { 'id': th.id }).then(function (req) {
                            if (req.data.success) {
                                resolve(req.data.result);
                            } else {
                                reject(req.config.way + ' ' + req.data.message);
                            }
                        }).catch(err => console.error(err))
                            .finally(() => th.loading = false);
                    }
                });
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
            btnEnter: function (formName, isclose) {
                var th = this;
                if (th.loading) return;
                //是新增还是编辑
                var modify_state = th.isadd ? 'add' : 'Modify';
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        th.loading = true;
                        var obj = th.outline;
                        $api.post('Outline/' + modify_state, { 'entity': obj }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.outline = result;
                                th.$message({
                                    type: 'success',
                                    message: isclose ? '操作成功,关闭当前窗体' : '操作成功!',
                                    center: true
                                });
                                th.getlivestream();
                                $api.put('Outline/ForID', { 'id': obj.Ol_ID });
                                th.fresh_parent(isclose);
                                //th.modify_show = false;
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err, '错误');
                        }).finally(() => th.loading = false);
                    } else {
                        //未通过验证的字段
                        let field = Object.keys(fields)[0];
                        let label = $dom('label[for="' + field + '"]');
                        while (label.attr('tab') == null)
                            label = label.parent();
                        th.activeName = label.attr('tab');
                        return false;
                    }
                });
            },
            //获取直播流信息
            getlivestream: function () {
                var th = this;
                if (th.outline.Ol_LiveID == null || th.outline.Ol_LiveID == '') return;
                $api.get('Live/StreamInfo', { 'name': th.outline.Ol_LiveID }).then(function (req) {
                    if (req.data.success) {
                        th.livestream = req.data.result;
                        console.log(th.livestream);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //复制到粘贴板
            copytext: function (val) {
                this.copy(val, null).then(function (th) {
                    th.$message({
                        message: '复制 “' + val + '” 到粘贴板',
                        type: 'success'
                    });
                });
            },
            //刷新上级列表
            fresh_parent: function (isclose) {
                //console.error(isclose);
                //this.test();
                //如果处于课程编辑页，则刷新
                var pagebox = window.top.$pagebox;
                if (pagebox && pagebox.source.top)
                    pagebox.source.top(window.name, 'vapp.fresh_frame("vapp.updatedEvent(true)")', isclose);
            },
            test: function () {
                let p = window.top.$pagebox.source.top(window.name);
                console.error(p);
                //alert(p);
            }
        }
    });

}, []);
