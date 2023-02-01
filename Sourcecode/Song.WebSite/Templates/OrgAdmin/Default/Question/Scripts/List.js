$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项
            //专业树形下拉选择器的配置项
            defaultSubjectProps: {
                children: 'children',
                label: 'Sbj_Name',
                value: 'Sbj_ID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            subjects: [],
            sbjids: [],
            //所有课程
            courses_all: [],
            couid: '',
            //试题的查询条件
            form: {
                'orgid': -1, 'sbjid': -1, 'couid': '', 'olid': '',
                'type': '', 'use': '', 'error': '', 'wrong': '', 'search': '', 'size': 20, 'index': 1
            },
            querybox: false,     //更多查询的面板是否显示
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading: true,
            loadingid: 0,
            loading_init: true
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
            ).then(axios.spread(function (organ, types) {
                th.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw arguments[i].config.way + ' ' + data.message;
                    }
                }
                //获取结果
                th.organ = organ.data.result;
                th.form.orgid = th.organ.Org_ID;
                th.config = $api.organ(th.organ).config;
                th.types = types.data.result;
                th.getSubjects();
                th.getCourses();
            })).catch(function (err) {
                th.loading_init = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //当前选择的课程
            'courses': function () {
                var sbjids = this.sbjids;
                if ($api.getType(sbjids) != "Array" || sbjids.length < 1) return this.courses_all;
                //获取选中的所有专业id
                var sbjid = sbjids.length > 0 ? sbjids[sbjids.length - 1] : '';
                var sbjlist = [];
                getsbjid(sbjid, this.subjects);
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
                        if(cou.Sbj_ID == sbj){
                            cou_arr.push(cou);
                        }
                    }                    
                }
                return cou_arr;
            }
        },
        watch: {
            'sbjids': {
                handler: function (nv, ov) {
                    if ($api.getType(nv) == "Array") {                      
                        this.form.sbjid = nv.length > 0 ? nv[nv.length - 1] : '';
                    } else {
                        this.form.sbjid = '';
                    }
                }, deep: true,
            },
        },
        methods: {
            //获取课程专业的数据
            getSubjects: function () {
                var th = this;
                th.loading = true;
                var form = { orgid: vapp.organ.Org_ID, search: '', isuse: null };
                $api.get('Subject/Tree', form).then(function (req) {
                    if (req.data.success) {
                        th.subjects = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                }).finally(function () {
                    //th.getCourses();
                });
            },
            //获取课程
            getCourses: function (val) {
                var th = this;
                th.loading = true;
                var orgid = th.organ.Org_ID;
                var sbjid = 0;
                if (th.sbjids.length > 0) sbjid = th.sbjids[th.sbjids.length - 1];
                $api.cache('Course/Pager', { 'orgid': orgid, 'sbjids': sbjid, 'thid': '', 'search': '', 'order': '', 'size': -1, 'index': 1 }).then(function (req) {
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
                    th.handleCurrentChange(1);
                });
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 42);
                th.form.size = th.form.size <= 10 ? 10 : th.form.size;
                var loading = this.$fulloading();
                $api.get("Question/Pager", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        var result = d.data.result;
                        for (var i = 0; i < result.length; i++) {
                            result[i].Qus_Title = result[i].Qus_Title.replace(/(<([^>]+)>)/ig, "");
                        }
                        th.datas = result;
                        th.$nextTick(function () {
                            loading.close();
                        });
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    th.loading = false;
                    console.error(err);
                });
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                var loading = this.$fulloading();
                $api.delete('Question/Delete', { 'id': datas }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                        th.$nextTick(function () {
                            loading.close();
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    loading.close();
                    console.error(err);
                });
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
                this.loadingid = row.Qus_ID;
                $api.post('Question/ChangeUse', { 'id': row.Qus_ID, 'use': row.Qus_IsUse }).then(function (req) {
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
                    vapp.$alert(err, '错误');
                    th.loadingid = 0;
                });
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
                        th.$alert(err, '错误');
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
        props: ["couid", "courses"],
        data: function () {
            return {
                course: {},
                loading: false
            }
        },
        watch: {
            'couid': {
                handler: function (nv) {
                    if (nv == null || nv == '') return;
                    this.getcourse(nv);
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {
            getcourse: function (couid) {
                if (this.courses) {
                    this.course = this.courses.find(item => item.Cou_ID == couid);
                    if (this.course != undefined) return;
                }
                var th = this;
                th.loading = true;
                $api.cache('Course/ForID', { 'id': couid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.course = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            }
        },
        template: `<span>
            <loading v-if="loading"></loading>
            <template v-else>{{course.Cou_Name}}</template>
        </span>`
    });
}, ['Components/ques_type.js']);
