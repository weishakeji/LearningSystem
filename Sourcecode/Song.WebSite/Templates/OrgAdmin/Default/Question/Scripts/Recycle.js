$ready([
    'Components/ques_type.js',
], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            org: {},
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项

            //所有课程
            courses_all: [],
            couid: '',
            //试题的查询条件
            form: {
                'orgid': -1, 'sbjid': '', 'couid': '', 'olid': '',
                'type': '', 'use': '', 'del': true, 'error': '', 'wrong': '', 'search': '', 'size': 1, 'index': 1
            },

            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行
            exception: '',

            loadingid: 0,
            loadstate: {
                recycle: false,         //还原
                course: false,
                get: false,         //加载数据 
                remove: false          //删除数据
            }
        },
        updated: function () {
            this.$mathjax();
        },
        mounted: function () {
            var th = this;
            th.org = window.org;
            th.form.orgid = th.org.Org_ID;
            th.config = window.config;
            th.types = window.questypes;
            th.getCourses();
            th.handleCurrentChange(1);
        },
        created: function () {
            var th = this;
            th.org = window.org;
            th.form.orgid = window.org.Org_ID;
            th.handleCurrentChange(1);
        },
        computed: {
            loading: function () {
                if (!this.loadstate) return false;
                for (let key in this.loadstate) {
                    if (this.loadstate.hasOwnProperty(key)
                        && this.loadstate[key])
                        return true;
                }
                return false;
            },
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
                th.loadstate.course = true;
                var orgid = th.org.Org_ID;
                $api.cache('Course/Pager', {
                    'orgid': orgid, 'sbjids': 0, 'thid': '', 'use': '', 'del': false, 'live': '', 'free': '',
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
                    th.loadstate.course = false;
                });
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                let area = $dom.height() - 110;
                th.form.size = Math.floor(area / 41);
                th.loadstate.get = true;
                th.exception = '';
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
                        console.error(d.data.exception);
                        th.exception = d.data.message;
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.get = false);
            },
            //回收
            recycle: function (btn, datas) {
                var th = this;
                th.$confirm('是否还原选中的数据?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    th.loadstate.recycle = true;
                    $api.post("Question/Recycle", { "qusid": datas })
                        .then(req => {
                            if (req.data.success) {
                                let result = req.data.result;
                                th.handleCurrentChange();
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err))
                        .finally(() => th.loadstate.recycle = false);
                }).catch(() => { });
            },
            //彻底删除
            remove: function (datas) {
                var th = this;
                th.$confirm('此操作将永久删除该内容, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'error'
                }).then(() => {
                    th.loadstate.remove = true;
                    $api.delete("Question/Remove", { "qusid": datas })
                        .then(req => {
                            if (req.data.success) {
                                let result = req.data.result;
                                th.handleCurrentChange();
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err))
                        .finally(() => th.loadstate.remove = false);
                }).catch(() => { });
            },
        },
        filters: {

        },
        components: {

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
                    if (th.couid == '0') return res();
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