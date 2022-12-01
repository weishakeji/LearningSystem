$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            course: {},     //当前课程
            //课程公告分类
            columns: [],         //课程公告的分类       
            columnsMofidy: false,        //是否处于编辑状态
            columnsVisible: false,       //分类的编辑框是否显示
            columnsObject: null,         //当前要操作的分类的对象
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
            //公告内容
            selects: [],
            guides: [],
            form: { 'couid': '', 'uid': '', 'show': '', 'search': '', 'size': 10, 'index': 1 },
            total: 1, //总记录数
            totalpages: 1, //总页数
            guideVisible: false,       //公告的编辑框是否显示      
            guide_title: '',
            //当前要操作的公告的对象
            guide_form: {
                Gu_IsUse: true
            },
            guide_rules: {
                Gu_Title: [
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
            $api.put('Course/ForID', { 'id': this.id }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    var result = req.data.result;
                    th.course = result;
                    if (th.course) {
                        document.title += th.course.Cou_Name;
                        th.getTreeData();
                        th.handleCurrentChange(1);
                    }
                    th.$nextTick(function () {
                        $dom('#vapp>div').show();
                    });
                    window.setTimeout(function () {
                        th.loading_init = false;
                    }, 500);
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
            //所取分的数据，为树形数据
            getTreeData: function () {
                var th = this;
                this.loading = true;
                $api.put('Guide/ColumnsTree', { 'couid': th.id, 'search': '', 'isuse': '' }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.columns = req.data.result;

                    } else {
                        th.columns = [];
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
                var arr = th.tree2array(this.columns);
                $api.post('Guide/ColumnsUpdateTaxis', { 'items': arr }).then(function (req) {
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
                        var apipath = 'Guide/Columns' + (th.columnsObject == null ? 'Add' : 'Modify');
                        var obj = th.column_form;
                        obj['Cou_ID'] = th.id;
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
                                th.getTreeData();
                                th.columnsShow(false, null);
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
            columnState: function (data, field) {
                data[field] = !data[field];
                var th = this;
                this.loadingid = data.Gc_ID;
                $api.post('Guide/ColumnsModify', { 'entity': data }).then(function (req) {
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
                th.loading_sumbit = true;
                $api.delete('Guide/ColumnsDelete', { 'id': data.Gc_ID }).then(function (req) {
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
                $api.get("Guide/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.guides = d.data.result;
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
            //公告的编辑状态
            //show：是否显示编辑面板
            //obj:要编辑的对象，如果是新增则为null
            guideShow: function (show, obj) {              
                this.guide_title = obj == null ? '新增课程公告' : '编辑课程公告';
                var th = this;
                if (obj == null) {
                    $api.get('Snowflake/Generate').then(function (req) {
                        if (req.data.success) {
                            th.guide_form = {};
                            th.guide_form.Gu_ID = req.data.result;
                            th.guide_form.state = 'add';
                            th.guideVisible = show;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {

                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
                } else {
                    this.guide_form = $api.clone(obj);
                    th.guide_form.state = 'Modify';
                    th.guideVisible = show;
                }
                this.$refs['details_editor'].setContent(this.guide_form.Gu_Details);
            },
            guideEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var obj = th.guide_form;
                        obj['Cou_ID'] = th.id;
                        if (obj.Gc_UID && $api.getType(obj.Gc_UID) == "Array") {
                            if (obj.Gc_UID.length > 0)
                                obj.Gc_UID = obj.Gc_UID[obj.Gc_UID.length - 1];
                        }
                        $api.post('Guide/' + th.guide_form.state, { 'entity': obj }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.handleCurrentChange();
                                th.guideShow(false, null);
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
            //删除课程公告
            deleteData: function (datas) {
                if (datas == '') return;
                var th = this;
                $api.delete('Guide/Delete', { 'id': datas }).then(function (req) {
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
            }
        }
    });

});
