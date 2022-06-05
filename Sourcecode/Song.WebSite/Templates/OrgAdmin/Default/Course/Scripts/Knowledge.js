$ready(function () {
    Vue.use(VueHtml5Editor, {
        showModuleName: true,
        image: {
            sizeLimit: 512 * 1024,
            compress: true,
            width: 500,
            height: 400,
            quality: 80
        }
    });
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            course: {},     //当前课程
            //课程知识分类
            sorts: [],         //课程知识库的分类      
            sort_current: null,        //当前要检索的分类（当点击右侧树形节点时） 
            sortsVisible: false,       //分类的编辑框是否显示
            sortsObject: null,         //当前要操作的分类的对象
            sort_title: '',
            sort_form: {},
            sort_rules: {
                Kns_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 2, max: 200, message: '长度在 2 到 200 个字符', trigger: 'blur' }
                ],
                Kns_Intro: [
                    { min: 0, max: 1000, message: '长度在 0 到 1000 个字符', trigger: 'blur' }
                ]
            },
            //知识内容
            selects: [],
            knls: [],
            form: { 'couid': '', 'uid': '', 'isuse': '', 'search': '', 'size': 10, 'index': 1 },
            total: 1, //总记录数
            totalpages: 1, //总页数
            knlVisible: false,       //知识的编辑框是否显示
            knlObject: null,         //当前要操作的知识的对象
            knl_title: '',
            knl_form: {},
            knl_rules: {
                Kn_Title: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 2, max: 200, message: '长度在 2 到 200 个字符', trigger: 'blur' }
                ]
            },

            loading: false,
            loading_sumbit: false,
            loadingid: false,
            loading_init: true
        },
        mounted: function () {
            var th = this;
            th.form.couid = th.id;
            $api.get('Course/ForID', { 'id': this.id }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    var result = req.data.result;
                    th.course = result;
                    if (th.course) {
                        document.title += th.course.Cou_Name;
                        th.getTreeData();
                        th.handleCurrentChange(1);
                    }
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
        },
        methods: {
            //获取分类的数据，为树形数据
            getTreeData: function () {
                var th = this;
                this.loading = true;
                $api.get('Knowledge/SortTree', { 'couid': th.id, 'search': '', 'isuse': '' }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.sorts = req.data.result;
                    } else {
                        th.sorts = [];
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            //分类的拖动改变顺序
            handleDragEnd(draggingNode, dropNode, dropType, ev) {
                var th = this;
                th.loading_sumbit = true;
                var arr = th.tree2array(this.sorts);
                $api.post('Knowledge/sortUpdateTaxis', { 'items': arr }).then(function (req) {
                    th.loading_sumbit = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '更改排序成功!',
                            center: true
                        });
                        th.getTreeData();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //将树形数据转到数据列表，用于递交到服务端更改专业的排序
            tree2array: function (datas) {
                var list = [];
                list = toarray(datas, '0', 1, list);
                return list;
                function toarray(arr, pid, level, list) {
                    for (let i = 0; i < arr.length; i++) {
                        const d = arr[i];
                        var obj = {
                            'Kns_ID': d.Kns_ID,
                            'Kns_PID': pid,
                            'Kns_Tax': i + 1
                        }
                        list.push(obj);
                        if (d.children && d.children.length > 0) {
                            list = toarray(d.children, d.Kns_UID, ++level, list);
                        }
                    }
                    return list;
                }
            },
            //分类的编辑状态
            //show：是否显示编辑面板
            //obj:要编辑的对象，如果是新增则为null
            sortsShow: function (show, obj) {
                this.sortsVisible = show;
                this.sortsObject = obj;
                this.sort_title = obj == null ? '新增分类' : '编辑分类';
                this.sort_form = obj != null ? $api.clone(obj) : {};
            },
            //新增或保存分类
            sortsEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var apipath = 'Knowledge/Sort' + (th.sortsObject == null ? 'Add' : 'Modify');
                        var obj = th.sort_form;
                        obj['Cou_ID'] = th.id;
                        if (th.sortsObject == null) {
                            obj['Kns_PID'] = '0';
                            obj['Kns_IsUse'] = true;
                            obj['Kns_UID'] = new Date().getTime();
                        }
                        $api.post(apipath, { 'entity': obj }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.getTreeData();
                                th.sortsShow(false, null);
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
            //修改分类的状态
            sortstate: function (data, field) {
                data[field] = !data[field];
                var th = this;
                this.loadingid = data.Kns_ID;
                $api.post('Knowledge/SortModify', { 'entity': data }).then(function (req) {
                    th.loadingid = -1;
                    if (req.data.success) {
                        th.$message({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                        th.getTreeData();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    th.loadingid = -1;
                });
            },
            //移除分类
            sortRemove: function (node, data) {
                var th = this;
                this.$confirm('删除分类，其下知识点也会被删除，请确认是否继续删除?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    if (!!data.children && data.children.length > 0) {
                        var msg = '当前分类“' + data.Kns_Name + '”下还有 <b>' + data.children.length
                            + '</b> 个子分类，会被同步删除，请确认是否继续删除。'
                        th.$confirm(msg, '再次确认', {
                            confirmButtonText: '确定',
                            cancelButtonText: '取消',
                            dangerouslyUseHTMLString: true,
                            type: 'warning'
                        }).then(() => {
                            th.sortDelete(data);
                        }).catch(() => { });
                    } else {
                        th.sortDelete(data);
                    }
                }).catch(() => { });
            },
            //删除课程知识的分类
            sortDelete: function (data) {
                var th = this;
                th.loading_sumbit = true;
                $api.delete('Knowledge/SortDelete', { 'id': data.Kns_ID }).then(function (req) {
                    th.loading_sumbit = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '删除成功!',
                            center: true
                        });
                        th.getTreeData();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //分类节点击事件
            nodeclick: function (data) {
                this.sort_current = data;
                this.form.uid = data.Kns_UID;
                this.handleCurrentChange(1);
            },
            nodeclose: function () {
                this.sort_current = null;
                this.form.uid = '';
                this.handleCurrentChange(1);
            },
            /*
            
            */
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 41);
                if ($api.getType(th.form.uid) == "Array" && th.form.uid.length > 0) {
                    th.form.uid = th.form.uid[th.form.uid.length - 1];
                }
                $api.get("Knowledge/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.knls = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);

                });
            },
            //知识的编辑状态
            //show：是否显示编辑面板
            //obj:要编辑的对象，如果是新增则为null
            knlShow: function (show, obj) {
                this.knlVisible = show;
                this.knlObject = obj;
                this.knl_title = obj == null ? '新增知识点' : '编辑知识点';
                this.knl_form = obj != null ? $api.clone(obj) : {};
            },
            knlEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var obj = th.knl_form;
                        obj['Cou_ID'] = th.id;
                        if (th.knlObject == null) {
                            obj['Kn_IsUse'] = true;
                        }
                        if (obj.Kns_UID && $api.getType(obj.Kns_UID) == "Array") {
                            if (obj.Kns_UID.length > 0)
                                obj.Kns_UID = obj.Kns_UID[obj.Kns_UID.length - 1];
                        }
                        if (!obj.Kn_Uid || obj.Kn_Uid == '') obj.Kn_Uid = new Date().getTime();
                        var apipath = 'Knowledge/' + (th.knlObject == null ? 'Add' : 'Modify');
                        $api.post(apipath, { 'entity': obj }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.handleCurrentChange();

                                th.knlShow(false, null);
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
            //删除课程知识
            deleteData: function (datas) {
                if (datas == '') return;
                var th = this;
                $api.delete('Knowledge/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            knlstate: function (data, field) {
                data[field] = !data[field];
                var th = this;
                this.loadingid = data.Kn_ID;
                $api.post('Knowledge/Modify', { 'entity': data }).then(function (req) {
                    th.loadingid = -1;
                    if (req.data.success) {
                        th.$message({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                        th.getTreeData();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    th.loadingid = -1;
                });
            }
        }
    });

}, ["/Utilities/editor/vue-html5-editor.js"]);
