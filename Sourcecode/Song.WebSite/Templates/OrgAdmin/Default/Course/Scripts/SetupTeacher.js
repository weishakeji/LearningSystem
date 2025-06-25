$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.dot(),       //课程id       
            organ: {},
            config: {},      //当前机构配置项

            course: {},     //当前课程
            teacher: {},     //当课程的教师
            //教师查询
            query: {
                orgid: '', titid: '', gender: '-1', isuse: '',
                search: '', phone: '', acc: '', idcard: '',
                order: '', size: 20, index: 1
            },
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数

            drawer: false,       //展示详情
            drawerobj: {},           //要展示详情的对象

            loading: false,
            loading_sel: false,      //选中教师的执行状态
            loading_init: false
        },
        mounted: function () {
            var th = this;
            $api.put('Course/ForID', { 'id': this.couid }).then(function (req) {
                th.loading_init = true;
                if (req.data.success) {
                    var result = req.data.result;
                    th.course = result;
                    th.getTeacher();
                    $api.get('Organization/ForID', { 'id': th.course.Org_ID }).then(function (req) {
                        if (req.data.success) {
                            th.organ = req.data.result;
                            //机构配置信息
                            th.config = $api.organ(th.organ).config;
                            th.query.orgid = th.organ.Org_ID;
                            th.getdatas(1);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    }).finally(() => th.loading_init = false);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.loading_init = false;
                Vue.prototype.$alert('课程：' + err + '');
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //当前课程是否有教师
            existteach: function () {
                return JSON.stringify(this.teacher) != '{}' && this.teacher != null;
            }
        },
        watch: {
        },
        methods: {
            //获取当前课程的教师
            getTeacher: function () {
                if (this.course.Th_ID <= 0) return;
                var th = this;
                $api.get('Teacher/ForID', { 'id': th.course.Th_ID }).then(function (req) {
                    if (req.data.success) {
                        th.teacher = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //加载数据页
            getdatas: function (index) {
                if (index != null) this.query.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 180;
                th.query.size = Math.floor(area / 42);
                th.loading = true;
                $api.get("Teacher/Pager", th.query).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //选择教师
            selected: function (teach) {
                var th = this;
                th.loading_sel = true;
                $api.post('Teacher/SetCourse', { 'couid': th.couid, 'teachid': teach != null ? teach.Th_ID : 0 })
                    .then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.teacher = teach;
                            th.operateSuccess();
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    }).finally(() => th.loading_sel = false);
            },
            //显示教师详情
            showteach: function (teach) {
                this.drawer = true;
                this.drawerobj = teach;
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.freshrow("' + this.couid + '")', false);
            }
        }
    });

}, ["/Utilities/Components/education.js"]);
