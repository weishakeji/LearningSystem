$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项 

            subjects: [],         //专业
            sbjCurrent: null,          //当前专业
            sbj_children: [],       //子专业，用于顶部专业选择时的使用
            sbjcourses: [],          //用于显示课程的专业 
            sbjid: $api.querystring("sbjid", ""),        //当前专业id
            sbj_load_count: 0,           //加载完成的专业数

            //search_str: $api.querystring("search"),      //搜索字符

            query: { 'orgid': '', 'sbjids': '', 'search': '', 'order': '', 'size': 8, 'index': 0 },
            courses: [],      //数据集，此处是课程列表
            total: 1, //总记录数
            totalpages: 1, //总页数

            loading_cou: false,     //课程加载
            loading: true,
            loading_init: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                        throw arguments[i].config.way + ' ' + data.message;
                    }
                }
                //获取结果
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;
                th.query.orgid = th.organ.Org_ID;
                //获取专业
                $api.get('Subject/TreeFront', { 'orgid': th.organ.Org_ID }).then(function (req) {
                    th.loading_init = false;
                    if (req.data.success) {
                        th.subjects = req.data.result;
                        th.setSbjChilds(th.sbjid);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading_init = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });

            })).catch(function (err) {
                th.loading_init = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        created: function () {
        },
        computed: {
            //是否禁止加载专业下的课程
            'sbjcourses_disabled': function () {
                return this.sbjcourses.length >= this.subjects.length;
            },
            //是否检索
            'issearch': function () {
                return this.query.search != '' || this.sbjCurrent != null;
            }
        },
        watch: {
            'sbjCurrent': {
                handler: function (nv, ov) {
                    if (nv != null && nv.children)
                        this.sbj_children = nv.children;
                    this.query.sbjids = nv == null ? '' : nv.Sbj_ID;
                    //this.load_courses(1);
                    this.load_infinite_datas(true);
                }, deep: true
            },
            'query.search': {
                handler: function (nv, ov) {
                    //this.load_courses(1);
                    this.load_infinite_datas(true);
                }, deep: true
            },
            'issearch': function () {
                this.load_infinite_datas(true);
            }
        },
        methods: {
            //无限下滑加载的方法
            //initial:是否是初始加载，如果为true，则检索的索引页将设置为1
            load_infinite_datas: function (initial) {
                if (!this.issearch) {
                    this.load_sbjcourses();
                } else {
                    if (initial == null || initial == false)
                        this.load_courses();
                    else
                        this.load_courses(1);
                }
            },
            //无限下滑加载的停止加载的判断
            infinite_disabled: function () {
                if (!this.issearch) {
                    return this.sbjcourses.length >= this.subjects.length;
                } else {
                    return this.courses.length >= this.total || this.courses.length == 0
                }
                return false;
            },
            //加载专业下的课程
            load_sbjcourses: function () {
                if (this.sbjcourses.length >= this.subjects.length) return;
                var count = this.sbjcourses.length;
                Vue.set(this.sbjcourses, count, this.subjects[count])
            },
            //加载课程
            load_courses: function (index) {
                if (this.loading_cou) return;
                if (index != null) this.query.index = index;
                else
                    this.query.index++;
                if (this.query.index == 1) this.courses = [];
                var th = this;
                th.loading_cou = true;
                $api.get('Course/ShowPager', th.query).then(function (req) {
                    th.loading_cou = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        //总页数与总记录数
                        th.totalpages = Number(req.data.totalpages);
                        th.total = req.data.total;
                        for (let i = 0; i < result.length; i++) {
                            //th.courses.push(result[i]);
                            th.$set(th.courses, req.data.size * (req.data.index - 1) + i, result[i]);
                        }
                        console.log(th.courses);
                        var t = th.total <= th.courses.length;
                        console.log(t);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading_cou = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
                console.log(this.query);
            },
            //搜索字符串的设置
            search: function (str) {
                var search = $api.querystring("search");
                if (str == null) this.query.search = search;
                this.query.search = search;
                return str;
            },
            //设置当前的专业下级
            //sbj:当前专业的id
            setSbjChilds: function (sbj) {
                //获取当前专业,来自组件的方法
                var getsubject = this.$refs['breadcrumb'].getsubject;
                this.sbjCurrent = getsubject(sbj, this.subjects);
                if (this.sbjCurrent == null) this.sbj_children = this.subjects;
                else
                    this.sbj_children = this.sbjCurrent.children;
                if (sbj != '') {
                    var url = $api.url.set(window.location.href, 'sbjid', sbj);
                    history.pushState({}, "", url);
                } else {
                    var url = $api.url.set(window.location.href, 'sbjid', "");
                    history.pushState({}, "", url);
                }
            },
            //根据索引生成css样式，第四个单独设置样式
            couClass: function (index) {

            }
        }
    });
    //顶部面包屑导航
    Vue.component('breadcrumb', {
        //当前专业，所有专业
        props: ["current", "subjects"],
        data: function () {
            return {
                parentPath: []       //父级路径
            }
        },
        watch: {
            'current': {
                handler: function (nv, ov) {
                    this.parentPath = this.getparents();
                }, immediate: true
            }
        },
        computed: {
        },
        mounted: function () {

        },
        methods: {
            //获取上级路径
            getparents: function () {
                var arr = [];
                if (this.current == null) return arr;
                var parent = this.current;
                do {
                    arr.push(parent);
                    parent = this.getsubject(parent.Sbj_PID, this.subjects);
                } while (parent != null)
                return arr.reverse();
            },
            //获取当前专业
            getsubject: function (sbjid, subjects) {
                var subject = null;
                for (var i = 0; i < subjects.length; i++) {
                    if (sbjid == subjects[i].Sbj_ID) {
                        subject = subjects[i];
                        break;
                    }
                    if (subject == null && subjects[i].children)
                        subject = this.getsubject(sbjid, subjects[i].children);
                }
                return subject;
            },
            //跳转,sbj:专业的id
            gourl: function (sbj) {
                var url = window.location.pathname;
                var search = $api.querystring("search");
                if (search != '') url = $api.url.set(url, 'search', search);
                if (sbj != null) url = $api.url.set(url, 'sbjid', sbj);
                history.pushState({}, "", url);
                vapp.setSbjChilds(sbj);
            }
        },
        // 同样也可以在 vm 实例中像 "this.message" 这样使用
        template: `<div>
        <el-breadcrumb separator-class="el-icon-arrow-right">
            <el-breadcrumb-item><a href="/">首页</a></el-breadcrumb-item>
            <el-breadcrumb-item><b @click="gourl()">课程中心</b></el-breadcrumb-item>
            <el-breadcrumb-item v-for="(item,index) in parentPath">
            <b @click="gourl(item.Sbj_ID)" v-if="index<parentPath.length-1">{{item.Sbj_Name}}</b>
            <span v-else>{{item.Sbj_Name}}</span>
            </el-breadcrumb-item>
        </el-breadcrumb>
    </div>`
    });

}, ["../Components/page_header.js",
    "../Components/page_footer.js",
    "../Components/subject_show.js"]);