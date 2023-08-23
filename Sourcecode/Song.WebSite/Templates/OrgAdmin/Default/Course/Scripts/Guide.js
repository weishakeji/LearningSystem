$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            course: {},     //当前课程         
            columns: [],         //课程公告的分类
            //公告内容
            selects: [],
            guides: [],
            form: { 'couid': '', 'uid': '', 'show': '', 'search': '', 'size': 10, 'index': 1 },
            total: 1, //总记录数
            totalpages: 1, //总页数         

            loading: false,
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
                        //document.title += th.course.Cou_Name;
                        th.getColumnsTree();
                        th.handleCurrentChange(1);
                    }
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //获取分类的数据，为树形数据
            getColumnsTree: function () {
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
            //分类的编辑    
            columnsModify: function () {
                let url = $api.url.set('GuideColumns', { 'couid': this.id });
                let toolsbar = this.$refs['btngroup'];
                toolsbar.pagebox(url, '分类管理', this.id, 500, 400, null);              
            },
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
