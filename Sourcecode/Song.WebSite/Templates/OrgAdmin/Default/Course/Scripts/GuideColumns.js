$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('couid'),       //课程id

            columns: [],         //课程公告的分类 
            column_title: '',
            column_form: {},
            column_rules: {
                Gc_Title: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 2, max: 200, message: '长度在 2 到 200 个字符', trigger: 'blur' }
                ],
                Gc_Intro: [
                    { min: 0, max: 1000, message: '长度在 0 到 1000 个字符', trigger: 'blur' }
                ]
            },
            filterText: '',
            columnsVisible: false,
            loading: false,
            loadingid: false,
            loading_sumbit: false,

        },
        mounted: function () {
            this.getColumnsTree();
        },
        created: function () {
        },
        computed: {
        },
        watch: {
            filterText: function (val) {
                this.$refs.tree.filter(val);
            }
        },
        methods: {
            //获取分类的数据，为树形数据
            getColumnsTree: function () {
                var th = this;
                if (th.loading) return;
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
            //过滤树形
            filterNode: function (value, data) {
                if (!value) return true;
                var txt = $api.trim(value.toLowerCase());
                //console.log(txt.length);
                if (txt == '') return true;
                return data.Gc_Title.toLowerCase().indexOf(txt) !== -1;
            },
            //分类的拖动改变顺序
            handleDragEnd(draggingNode, dropNode, dropType, ev) {
                var th = this;
                if (th.loading_sumbit) return;
                th.loading_sumbit = true;
                var arr = th.tree2array(this.columns);
                $api.post('Guide/ColumnsUpdateTaxis', { 'items': arr }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '更改排序成功!',
                            center: true
                        });
                        th.getColumnsTree();
                        th.fresh_parent(false);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_sumbit = false);
            },
            //将树形数据转到数据列表，用于递交到服务端更改专业的排序
            tree2array: function (datas) {
                var list = [];
                list = toarray(datas, 0, 1, list);
                return list;
                function toarray(arr, pid, level, list) {
                    for (let i = 0; i < arr.length; i++) {
                        const d = arr[i];
                        var obj = {
                            'Gc_ID': d.Gc_ID,
                            'Gc_PID': pid,
                            'Gc_Tax': i + 1
                        }
                        list.push(obj);
                        if (d.children && d.children.length > 0) {
                            list = toarray(d.children, d.Gc_UID, ++level, list);
                        }
                    }
                    return list;
                }
            },
            //分类的编辑状态
            //show：是否显示编辑面板
            //obj:要编辑的对象，如果是新增则为null
            columnsShow: function (show, obj) {
                this.columnsVisible = show;
                this.columnsObject = obj;
                this.column_title = obj == null ? '新增分类' : '编辑分类';
                this.column_form = obj != null ? $api.clone(obj) : {};
            },
            //新增或保存分类
            columnsEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        if (th.loading_sumbit) return;
                        th.loading_sumbit = true;
                        var apipath = 'Guide/Columns' + (th.columnsObject == null ? 'Add' : 'Modify');
                        var obj = th.column_form;
                        obj['Cou_ID'] = th.couid;
                        if (th.columnsObject == null) {
                            obj['Gc_PID'] = '0';
                            obj['Gc_IsUse'] = true;
                            obj['Gc_UID'] = new Date().getTime();
                        }
                        $api.post(apipath, { 'entity': obj }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.getColumnsTree();
                                th.fresh_parent(false);
                                th.columnsShow(false, null);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                        }).finally(() => th.loading_sumbit = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //修改分类的状态
            columnState: function (data, field) {
                data[field] = !data[field];
                var th = this;
                if (th.loadingid > 0) return;
                th.loadingid = data.Gc_ID;
                $api.post('Guide/ColumnsModify', { 'entity': data }).then(function (req) {
                    if (req.data.success) {
                        th.$message({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                        th.getColumnsTree();
                        th.fresh_parent(false);
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err, '错误');
                }).finally(() => th.loadingid = -1);
            },
            //移除分类
            columnRemove: function (node, data) {
                var th = this;
                this.$confirm('删除课程公告的分类，其下公告信息也会被删除，请确认是否继续删除?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    if (!!data.children && data.children.length > 0) {
                        var msg = '当前分类“' + data.Gc_Title + '”下还有 <b>' + data.children.length
                            + '</b> 个子分类，会被同步删除，请确认是否继续删除。'
                        th.$confirm(msg, '再次确认', {
                            confirmButtonText: '确定',
                            cancelButtonText: '取消',
                            dangerouslyUseHTMLString: true,
                            type: 'warning'
                        }).then(() => {
                            th.columnDelete(data);
                        }).catch(() => { });
                    } else {
                        th.columnDelete(data);
                    }
                }).catch(() => { });
            },
            //删除课程公告的分类
            columnDelete: function (data) {
                var th = this;
                if (th.loading_sumbit) return;
                th.loading_sumbit = true;
                $api.delete('Guide/ColumnsDelete', { 'id': data.Gc_ID }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '删除成功!',
                            center: true
                        });
                        th.getColumnsTree();
                        th.fresh_parent(false);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_sumbit = false);
            },
            //刷新上级列表
            fresh_parent: function (isclose) {
                //如果处于课程编辑页，则刷新
                var pagebox = window.top.$pagebox;
                if (pagebox && pagebox.source.top)
                    pagebox.source.top(window.name, 'vapp.fresh_frame("vapp.getColumnsTree(true)")', isclose);
            }
        }
    });

});
