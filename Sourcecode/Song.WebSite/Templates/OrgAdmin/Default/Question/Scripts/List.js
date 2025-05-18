$ready([
    'Components/ques_type.js',
], function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项

            //所有课程
            courses_all: [],
            couid: '',
            //试题的查询条件
            form: {
                'orgid': -1, 'sbjid': '', 'couid': '', 'olid': '',
                'type': '', 'use': '', 'error': '', 'wrong': '', 'search': '', 'size': 1, 'index': 1
            },

            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading: false,
            loadingid: 0,
            loading_init: true  //是否正在初始化
        },
        updated: function () {
            this.$mathjax();
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current'),
                $api.cache('Question/Types:99999')
            ).then(([organ, types]) => {
                //获取结果
                th.organ = organ.data.result;
                th.form.orgid = th.organ.Org_ID;
                th.config = $api.organ(th.organ).config;
                th.types = types.data.result;
                th.getCourses();
                th.handleCurrentChange(1);
            }).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
            //当前选择的课程
            'courses': function () {
                if (this.form.sbjid == '') return this.courses_all;
                //获取选中的所有专业id
                var sbjid = this.form.sbjid;
                var sbjlist = this.$refs['subject'].subjects;
                getsbjid(sbjid, sbjlist);
                function getsbjid(sbjid, sbjs) {
                    for (let i = 0; i < sbjs.length; i++) {
                        if (sbjid == 0) {
                            sbjlist.push(sbjs[i].Sbj_ID);
                            if (sbjs[i].children && sbjs[i].children.length > 0)
                                getsbjid(0, sbjs[i].children);
                            continue;
                        }
                        if (sbjs[i].Sbj_ID == sbjid) {
                            sbjlist.push(sbjs[i].Sbj_ID);
                            if (sbjs[i].children && sbjs[i].children.length > 0)
                                getsbjid(0, sbjs[i].children);
                        } else {
                            if (sbjs[i].children && sbjs[i].children.length > 0)
                                getsbjid(sbjid, sbjs[i].children);
                        }
                    }
                }
                //console.log(sbjlist);
                //所有专业下的课程（包括子专业）
                var cou_arr = [];
                for (let j = 0; j < sbjlist.length; j++) {
                    var sbj = sbjlist[j];
                    for (let i = 0; i < this.courses_all.length; i++) {
                        const cou = this.courses_all[i];
                        if (cou.Sbj_ID == sbj) {
                            cou_arr.push(cou);
                        }
                    }
                }
                return cou_arr;
            }
        },
        watch: {

        },
        methods: {
            //获取课程
            getCourses: function (val) {
                var th = this;
                th.loading = true;
                var orgid = th.organ.Org_ID;
                $api.cache('Course/Pager', {
                    'orgid': orgid, 'sbjids': 0, 'thid': '', 'use': '', 'live': '', 'free': '',
                    'search': '', 'order': '', 'size': -1, 'index': 1
                }).then(function (req) {
                    if (req.data.success) {
                        th.courses_all = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(function () {
                    //th.handleCurrentChange(1);
                });
            },
            //加载数据页
            handleCurrentChange: function (index) {
                var th = this;
                if (index != null) this.form.index = index;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 42);
                th.form.size = th.form.size <= 10 ? 10 : th.form.size;
                var loading = this.$fulloading();
                $api.get("Question/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        var result = d.data.result;
                        for (let i = 0; i < result.length; i++) {
                            result[i].Qus_Title = result[i].Qus_Title.replace(/(<([^>]+)>)/ig, "");
                        }
                        th.datas = result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => {
                    th.$nextTick(function () {
                        loading.close();
                    });
                });
            },
            //刷新行数据，
            freshrow: function (id) {
                if (id == null || id == '') return this.handleCurrentChange();
                if (this.datas.length < 1) return;
                //要刷新的行数据
                let entity = this.datas.find(item => item.Qus_ID == id);
                if (entity == null) return;
                //获取最新数据，刷新
                var th = this;
                th.loadingid = id;
                $api.get('Question/ForID', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        result.Qus_Title = result.Qus_Title.replace(/(<([^>]+)>)/ig, "");
                        let index = th.datas.findIndex(item => item.Qus_ID == id);
                        if (index >= 0) th.$set(th.datas, index, result);
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                //th.loading = true;
                var loading = this.$fulloading();
                var quesid = datas.split(',');
                var form = { 'qusid': quesid };
                //要删除的试题,当删除后要重新统计章节、课程、专业下的试题数，所以需要提交更多id
                var ques = th.getques_selected(quesid);
                form['olid'] = th.getques_keys(ques, 'Ol_ID'); //章节id              
                $api.delete('Question/Delete', form).then(function (req) {
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
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => {
                    th.$nextTick(function () {
                        loading.close();
                        //th.loading = false;
                    });
                });
            },
            //获取选中的（要删除的）试题
            getques_selected: function (ids) {
                var arr = [];
                for (let i = 0; i < ids.length; i++) {
                    const id = ids[i];
                    var q = this.datas.find(el => el.Qus_ID == id);
                    if (q != null) arr.push(q);
                }
                return arr;
            },
            //获取试题的章节
            getques_keys: function (ques, key) {
                var arr = [];
                for (let i = 0; i < ques.length; i++)
                    arr.push(ques[i][key]);
                return Array.from(new Set(arr));
            },
            //导出
            output: function (btn) {
                var title = btn.tips;
                this.$refs.btngroup.pagebox('Export', title, null, 800, 600);
            },
            //导入
            input: function (btn) {
                var title = btn.tips;
                this.$refs.btngroup.pagebox('Import', title, null, 900, 650);
            },
            //更改使用状态
            changeState: function (row) {
                var th = this;
                th.loadingid = row.Qus_ID;
                $api.post('Question/ChangeUse', { 'id': row.Qus_ID, 'use': row.Qus_IsUse }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err, '错误');
                }).finally(() => th.loadingid = 0);
            },
            //批量修改状态
            batchState: function (use) {
                use = Boolean(use);
                var th = this;
                this.$confirm('批量更改当前页面的试题为“' + (use ? '启用' : '禁用') + '”, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    var ids = '';
                    for (var i = 0; i < th.datas.length; i++) {
                        ids += th.datas[i].Qus_ID;
                        if (i < th.datas.length - 1) ids += ',';
                    }
                    var loading = this.$fulloading();
                    $api.post('Question/ChangeUse', { 'id': ids, 'use': use }).then(function (req) {
                        if (req.data.success) {
                            th.$notify({
                                type: 'success',
                                message: '修改状态成功!',
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
                    });
                }).catch(() => {

                });
            }
        }
    });
    //显示课程名称
    Vue.component('course_name', {
        //couid:当前试题的id
        //courses：所有课程
        props: ["couid", "courses", "index"],
        data: function () {
            return {
                course: {},
                loading: false
            }
        },
        watch: {
            'couid': {
                handler: function (nv) {
                    if (!$api.isnull(nv)) {
                        if (this.index == 0) this.startInit();
                    }
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {
            //初始加载
            startInit: function () {
                //加载完成，则加载后一个组件，实现逐个加载的效果
                this.getcourse().finally(() => {
                    var vapp = window.vapp;
                    var ctr = vapp.$refs['couname_' + (this.index + 1)];
                    if (ctr != null) {
                        window.setTimeout(ctr.startInit, 50);
                    }
                });
            },
            //获取课程信息
            getcourse: function () {
                var th = this;
                return new Promise(function (res, rej) {
                    if (th.courses) {
                        th.course = th.courses.find(item => item.Cou_ID == th.couid);
                        if (th.course != undefined) return res();
                    }
                    th.loading = true;
                    $api.cache('Course/ForID', { 'id': th.couid }).then(function (req) {
                        if (req.data.success) {
                            th.course = req.data.result;
                        } else {
                            th.loading = false;
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => {
                            th.loading = false;
                            return res();
                        });
                });
            }
        },
        template: `<span>
            <i class="el-icon-loading" v-if="loading"></i>
            <template v-else-if="course">{{course.Cou_Name}}</template>
        </span>`
    });
});
