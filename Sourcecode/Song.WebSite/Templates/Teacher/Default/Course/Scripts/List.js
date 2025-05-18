$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form:
            {
                'orgid': '',
                'sbjids': '', 'thid': '', 'use': '', 'live': '', 'free': '',
                'search': '',
                'order': 'new',
                'size': 20, 'index': 1
            },
            checkRec: false,     //是否推荐的选项

            organ: {},
            teacher: {},         //当前登录的教师
            config: {},      //当前机构配置项      

            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            drawer: false,   //显示滑出的课程详情
            curr: {},        //当前要展示的项

            batchVisible: false,     //批量处理的面板显示
            fileloading: false,
            progressloading: false,          //进度
            excelProgress: {},       //批量生成的进度

            loading: false,
            loadingid: 0,
            loading_init: true
        },
        mounted: function () {
            this.$refs.btngroup.addbtn({
                text: '批量', tips: '批量处理',
                id: 'bat', type: 'success',
                class: 'el-icon-magic-stick'
            });
        },
        created: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current'),
                $api.get('Teacher/Current')
            ).then(([org, teach]) => {
                //获取结果              
                th.organ = org.data.result;
                th.teacher = teach.data.result;
                th.form.orgid = th.organ.Org_ID;
                th.form.thid = th.teacher ? th.teacher.Th_ID : 0;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.handleCurrentChange(1);

            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.teacher) != '{}' && this.teacher != null;
            },
        },
        watch: {
            //是否查询推荐的课程
            'checkRec': function (nv, ov) {
                if (nv) this.form.order = 'rec';
                else
                    this.form.order = '';
            },
            //当前要展示综述的课程
            'curr': {
                handler: function (nv, ov) {
                    //console.log(nv)
                }, deep: true,
            },
            //批量处理的面板显示
            'batchVisible': function (nv, ov) {
                if (nv && window.excelProgress == null) {
                    this.batch2excelProgress();
                }
            }
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (this.form.thid <= 0 || this.form.thid == '') return;
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 162);
                th.loading = true;
                $api.get("Course/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        for (var i = 0; i < d.data.result.length; i++) {
                            d.data.result[i].isAdminPosi = false;
                        }
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        //console.log(th.accounts);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //刷新单一课程
            freshrow: function (id) {
                var th = this;
                th.loadingid = id;
                $api.get('Course/ForID', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        var index = th.datas.findIndex(item => {
                            return item.Cou_ID == result.Cou_ID;
                        });
                        th.$set(th.datas, index, result);
                        th.$message({
                            message: '刷新课程 “' + result.Cou_Name + '” 成功',
                            type: 'success'
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
            },
            //更改状态
            changeState: function (row) {
                var th = this;
                this.$alert('课程的启用与禁用，需要管理员操作', '警告', {
                    confirmButtonText: '确定',
                    type: 'warning',
                    callback: action => {
                        row.Cou_IsUse = !row.Cou_IsUse;
                    }
                });

                return;
                this.loadingid = row.Cou_ID;
                $api.post('Course/ModifyState', { 'id': row.Cou_ID, 'use': row.Cou_IsUse, 'rec': row.Cou_IsRec, 'edit': row.Cou_Allowedit }).then(function (req) {
                    this.loadingid = -1;
                    if (req.data.success) {
                        vapp.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    th.$alert(err, '错误', {
                        callback: function () {
                            $api.get('Course/ForID', { 'id': row.Cou_ID }).then(function (req) {
                                if (req.data.success) {
                                    var result = req.data.result;
                                    for (let i = 0; i < th.datas.length; i++) {
                                        if (th.datas[i].Cou_ID == result.Cou_ID) {
                                            th.datas[i] = result;
                                            th.$set(th.datas, i, result);
                                            break;
                                        }
                                    }
                                } else {
                                    console.error(req.data.exception);
                                    throw req.data.message;
                                }
                            }).catch(function (err) {
                                console.error(err);
                            });
                        }
                    });
                    th.loadingid = 0;
                });
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('Course/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //打开编辑界面
            btnmodify: function (id) {
                this.$refs.btngroup.modify(id);
            },
            //新增课程的按钮事件
            btnadd: function (btn) {
                if (!this.islogin) {
                    this.$alert('教师未登录，无法创建课程', '提示', {
                        confirmButtonText: '确定',
                        type: 'warning'
                    });
                    return;
                }
                var url = '/orgadmin/course/create';
                url = $api.url.set(url, {
                    'thid': this.teacher.Th_ID
                });
                this.$refs.btngroup.pagebox(url, '新增', window.name + '[add]', 600, 300, {
                    'ico': 'e813'
                });
                console.log(btn);
            },
            //设置教师
            btnsetteacher: function (couid) {
                var url = 'SetupTeacher.' + couid;
                this.$refs.btngroup.pagebox(url, '设置教师', window.name + '[SetupTeacher]', 800, 600, {
                    'showmask': true, 'min': false
                });
            },
            subjectPath: function (sbj, course) {
                var subjects = this.$refs['subject'];
                if (subjects != null)
                    return subjects.subjectPath(sbj, course);
                return '';
            },
            //批量导出excel
            batch2excel: function () {
                var th = this;
                th.fileloading = true;
                window.setTimeout(function () {
                    th.batch2excelProgress();
                }, 3000);

                $api.get('Course/StudentsLogBatExcel', { 'orgid': th.organ.Org_ID }).then(function (req) {

                    if (req.data.success) {
                        var result = req.data.result;
                        //...
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //获取进度
            batch2excelProgress: function () {
                getprogress();
                window.excelProgress = window.setInterval(function () { getprogress(); }, 3000);
                function getprogress() {
                    vapp.fileloading = true;
                    $api.get('Course/StudentsLogBatExcelProgress', { 'orgid': vapp.organ.Org_ID }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            vapp.excelProgress = result;
                            vapp.excelProgress["progress"] = Math.floor(result["complete"] / result["total"] * 1000) / 10;
                            if (vapp.excelProgress["progress"] >= 100) {
                                vapp.fileloading = false;
                                window.clearInterval(window.excelProgress);
                            }
                        } else {
                            vapp.fileloading = false;
                            window.clearInterval(window.excelProgress);
                            // console.log('没有数据');
                            //console.error(req.data.exception);
                            //throw req.data.message;
                        }
                    }).catch(err => console.error(err));
                }
            }
        }
    });

}, ['Components/course_data.js',
    'Components/course_prices.js']);
