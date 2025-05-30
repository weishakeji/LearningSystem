﻿$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form:
            {
                'orgid': '', 'sbjids': '', 'thid': '', 'use': '', 'live': '', 'free': '',
                'search': '', 'order': 'def',
                'size': 1, 'index': 1
            },
            //排序方式
            orderItem: [
                { text: '默认排序', value: 'def', icon: '&#xb006', div: false },
                { text: '推荐优先', value: 'rec', icon: '&#xe826', div: true },
                { text: '访问量最大', value: 'flux', icon: '&#xa03a', div: false },
                { text: '最新创建', value: 'new', icon: '&#xe81a', div: true },
                { text: '最后创建', value: 'last', icon: '&#xe671', div: false },
                { text: '试题量最大', value: 'quesDesc', icon: '&#xe755', div: true },
                { text: '试题量最小', value: 'quesAsc', icon: '&#xa02e', div: false },
                { text: '视频资源最多', value: 'videoDesc', icon: '&#xe761', div: false },
                { text: '视频资源最少', value: 'videoAsc', icon: '&#xe6bf', div: false }
            ],


            organ: {},
            config: {},      //当前机构配置项      

            teachers: [],        //教师

            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            drawer: false,   //显示滑出的课程详情
            curr: {},        //当前要展示的项       

            loading: false,
            loadingid: 0,
            loading_init: true,
            loading_teach: false //加载教师信息时的预载
        },
        mounted: function () {
            this.$refs.btngroup.addbtn({
                text: '学习记录', tips: '批量处理',
                id: 'log', type: 'success',
                class: 'el-icon-magic-stick'
            });
            this.$refs.btngroup.addbtn({
                text: '更新数据', tips: '更新课程数、试题数、试卷数的统计数据',
                id: 'update_dataall', type: 'warning',
                icon: 'e651'
            });
        },
        created: function () {
            var th = this;
            th.loading_init = true;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    th.organ = req.data.result;
                    th.form.orgid = th.organ.Org_ID;
                    //机构配置信息
                    th.config = $api.organ(th.organ).config;
                    th.handleCurrentChange(1);
                    th.teacher_query('');
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        computed: {

        },
        watch: {
            //当前要展示综述的课程
            'curr': {
                handler: function (nv, ov) {
                    //console.log(nv)
                }, deep: true,
            },
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 57);
                th.loading = true;
                $api.get("Course/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        //console.log(th.datas);
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            orderState: function (val) {
                console.error(val);
                this.form.order = val;
                this.handleCurrentChange();
            },
            //获取教师列表
            teacher_query: function (search) {
                var th = this;
                th.loading_teach = true;
                let query = {
                    orgid: th.organ.Org_ID, titid: '', gender: '-1', isuse: '',
                    search: search, phone: '', acc: '', idcard: '',
                    order: 'pinyin', size: 9999999, index: 1
                };
                $api.get('Teacher/Pager', query).then(function (req) {
                    if (req.data.success) {
                        th.teachers = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_teach = false);

            },
            //刷新单一课程
            freshrow: function (id) {
                if (id == null || id == '' || id == 0) return this.handleCurrentChange();
                if (this.datas.length < 1) return;
                //要刷新的行数据
                let entity = this.datas.find(item => item.Cou_ID == id);
                if (entity == null) return;
                var th = this;
                th.loadingid = id;
                $api.put('Course/ForID', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        var index = th.datas.findIndex(item => {
                            return item.Cou_ID == result.Cou_ID;
                        });
                        if (index >= 0) {
                            th.$set(th.datas, index, result);
                            th.$message({
                                message: '刷新课程 “' + result.Cou_Name + '” 成功',
                                type: 'success'
                            });
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
            },
            //修改教师是否允许编辑
            changeAllowedit: function (row, allow) {
                row.Cou_Allowedit = allow;
                this.changeState(row);
            },
            //更改状态
            changeState: function (row) {
                var th = this;
                this.loadingid = row.Cou_ID;
                $api.post('Course/ModifyState', { 'id': row.Cou_ID, 'use': row.Cou_IsUse, 'rec': row.Cou_IsRec, 'edit': row.Cou_Allowedit }).then(function (req) {
                    this.loadingid = -1;
                    if (req.data.success) {
                        th.$notify({
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
                            }).catch(err => console.error(err))
                                .finally(() => { });
                        }
                    });
                    th.loadingid = 0;
                });
            },
            //批量修改状态
            batchState: function (use) {
                use = Boolean(use);
                var th = this;
                var num = 0;
                for (let i = 0; i < th.datas.length; i++)
                    if (th.datas[i].Cou_IsUse != use) num++;
                if (num == 0) {
                    this.$alert('当前页面所有课程均为“' + (use ? '启用' : '禁用') + '”状态，无须操作', '提示', {
                        confirmButtonText: '确定'
                    });
                } else {
                    var msg = '批量更改当前页面的<b>' + num + '</b>个课程为“' + (use ? '启用' : '禁用') + '”, 是否继续?';
                    this.$confirm(msg, '提示', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        dangerouslyUseHTMLString: true,
                        type: 'warning'
                    }).then(() => {
                        var ids = '';
                        for (var i = 0; i < th.datas.length; i++) {
                            if (th.datas[i].Cou_IsUse != use)
                                ids += th.datas[i].Cou_ID + ',';
                        }
                        th.loading = true;
                        var loading = th.$fulloading();
                        $api.post('Course/ModifyState', { 'id': ids, 'use': use, 'rec': null, 'edit': null }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '成功修改' + result + '个课程的状态!',
                                    center: true
                                });
                                th.handleCurrentChange();
                                th.$nextTick(function () {
                                    loading.close();
                                });
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                        }).finally(() => { });
                    }).catch(() => {

                    }).finally(() => th.loading = false);
                }
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('Course/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vapp.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //更新某个课程的统计数据
            updateDatainfo: function (row) {
                var name = row.Cou_Name
                this.$confirm('重新课程《' + name + '》的课程数、试题数、试卷数等统统计数据, 是否继续?', '更新数据', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    var th = this;
                    th.loadingid = row.Cou_ID;
                    $api.post('Course/updatestatisticaldata', { 'orgid': 0, 'couid': row.Cou_ID, 'asyn': false }).then(req => {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.freshrow(row.Cou_ID);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => th.loadingid = 0);
                }).catch(() => { });
            },
            //更新所有课程的统计数据
            update_dataall: function () {
                var text = '此操作将重新计算所有课程的课程数、试题数、试卷数;';
                var asyn = false;
                if (this.total > 30) {
                    text += '<br/>耗时较长，将采用后台计算。<br/>';
                    asyn = true;
                } else {
                    text += '<br/>耗时较长，';
                    asyn = false;
                }
                this.$confirm(text + '是否继续?', '更新数据', {
                    confirmButtonText: '确定', cancelButtonText: '取消',
                    type: 'warning', dangerouslyUseHTMLString: true
                }).then(() => {
                    var th = this;
                    var loading = this.$fulloading();
                    $api.post('Course/updatestatisticaldata', { 'orgid': th.organ.Org_ID, 'couid': '', 'asyn': asyn }).then(req => {
                        if (req.data.success) {
                            var result = req.data.result;
                            if (!asyn) th.handleCurrentChange();
                            else {
                                th.$alert('已经开始后台计算，请稍后查看结果。', '提示', {
                                    confirmButtonText: '确定'
                                });
                            }
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => {
                            th.$nextTick(function () {
                                loading.close();
                            });
                        });
                }).catch(() => { });
            },
            //打开学习记录的面板
            openlog: function (btn) {
                this.$refs.btngroup.pagebox('StudyLogExport?orgid=' + this.organ.Org_ID,
                    '学习记录导出', window.name + '[studylog]', 800, 500);
            },
            //打开课程的学员管理
            onstudent: function (course, index) {
                if (index == null || index < 0) index = this.datas.findIndex(i => i.Cou_ID == course.Cou_ID);
                let ctrl = this.$refs['course_data_' + index];
                if (ctrl != null) ctrl.onstudent(course.data.student, course);

            },
            //打开编辑界面
            btnmodify: function (id, title, param) {
                this.$refs.btngroup.modify(id, title, param);
            },
            //新增课程的按钮事件
            btnadd: function (btn) {
                this.$refs.btngroup.pagebox('create', '新增', window.name + '[add]', 600, 300);
            },
            //设置教师
            btnsetteacher: function (couid) {
                var url = 'SetupTeacher.' + couid;
                this.$refs.btngroup.pagebox(url, '设置教师', window.name + '[SetupTeacher]', 1000, 600, {
                    'showmask': true, 'min': false, 'ico': 'e650'
                });
            },
            //专业路径
            subjectPath: function (sbj, course) {
                var subjects = this.$refs['subject'];
                if (subjects != null)
                    return subjects.subjectPath(sbj, course);
                return '';
            },
        }
    });

}, ['Components/course_data.js',
    'Components/course_income.js',
    'Components/course_prices.js']);
