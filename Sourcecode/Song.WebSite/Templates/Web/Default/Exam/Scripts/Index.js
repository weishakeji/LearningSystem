$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        

            tabmenu: 'my_exam',     //选项卡

            loading: true,
            loading_init: true,      //初始数所据加载
            finished: false,

            form: { 'search': '', 'size': 5, 'index': 0 },
            total: 0,

            myexam: [],      //当前学员今天以及之后的考试
            allexam: [],             //所有考试（此为考试主题)
            scoreexam: []           //成绩回顾
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                th.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果           
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                document.title += th.platinfo.title;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {
            $dom.load.css([$dom.path() + 'styles/pagebox.css']);
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
            //当前学员今天以及之后的考试
            my_exam: function (index) {
                if (!this.islogin) {
                    this.tabmenu = 'all_exam';
                    return;
                }
                var th = this;
                if (index != null) this.form.index = index;
                this.loading = true;
                var form = $api.clone(this.form);
                form['acid'] = this.account.Ac_ID;
                console.log(form);
                $api.get('Exam/SelfExam4Todaylate', form).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.total = req.data.total;
                        var result = req.data.result;
                        th.myexam = result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            },
            //获取所有考试
            all_exam: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                var form = $api.clone(this.form);
                form['orgid'] = this.organ.Org_ID;
                form['start'] = '';
                form['end'] = '';
                this.loading = true;
                $api.get('Exam/ThemePager', form).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.total = req.data.total;
                        th.allexam = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            },
            //成绩回顾的加载
            score_exam: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                var form = $api.clone(this.form);
                form['acid'] = this.account.Ac_ID;
                form['orgid'] = -1;
                form['sbjid'] = -1;
                this.loading = true;
                $api.get('Exam/Result4Student', form).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.total = req.data.total;
                        th.scoreexam = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            },
            //查询
            onsearch: function (search, tabitem) {
                this.loading = true;
                this.total = 0;
                this.myexam = [];
                this.allexam = [];
                this.scoreexam = [];

                this.form.search = search;
                eval('this.' + tabitem.tag + '')(1);
            }
        }
    });
    //顶部的选项卡
    Vue.component('exam_tabs', {
        //islogin:学员是否登录,
        //tab:当前选中的项
        props: ["islogin", "tab"],
        data: function () {
            return {
                //选项卡，search为搜索字符
                tabs: [
                    { 'name': '我的考试', 'tag': 'my_exam', 'icon': 'e811', 'curr': true, 'search': '', 'login': true },
                    { 'name': '所有考试', 'tag': 'all_exam', 'icon': 'e810', 'curr': false, 'search': '', 'login': false },
                    { 'name': '成绩查看', 'tag': 'score_exam', 'icon': 'e6ef', 'curr': false, 'search': '', 'login': true },
                ],
            }
        },
        watch: {
            //当前选中的项
            'tab': {
                handler: function (nv, ov) {
                    var curr = null;
                    for (let i = 0; i < this.tabs.length; i++) {
                        if (this.tabs[i].tag == nv) {
                            this.tabs[i].curr = true;
                            curr = this.tabs[i];
                        } else {
                            this.tabs[i].curr = false;
                        }
                    }
                    if (curr == null) {
                        if (this.islogin) {
                            this.tabs[0].curr = true;
                        } else {
                            for (let i = 0; i < this.tabs.length; i++) {
                                if (!this.tabs[i].login) {
                                    this.tabs[i].curr = true;
                                    break;
                                }
                            }
                        }
                        for (let i = 0; i < this.tabs.length; i++) {
                            if (this.tabs[i].curr == true) {
                                curr = this.tabs[i];
                                break;
                            }
                        }
                    }
                    if (nv != ov) {
                        var existEvent = this.$listeners['change'];
                        if (existEvent) this.$emit('change', this.tab, curr);
                        this.search(curr);
                    }
                }, immediate: true
            }
        },
        computed: {
            //当前选中的项
            'selected': function () {
                var curr = null;
                for (let i = 0; i < this.tabs.length; i++) {
                    if (this.tabs[i].curr == true) {
                        curr = this.tabs[i];
                        break;
                    }
                }
                this.tab = curr.tag;
                return curr;
            }
        },
        mounted: function () {
        },
        methods: {
            //选项卡点击事件
            clickEvent: function (tab, item) {
                this.selected = item;
                this.tab = tab;
            },
            //搜索事件
            search: function (item) {
                var existEvent = this.$listeners['search'];
                if (existEvent) this.$emit('search', item.search, item);
            },
            //当搜索框内容变更多时触，如果为空，则触发搜索事件
            changesearch: function (item) {
                item.search = $api.trim(item.search);
                if (item.search == '') this.search(item);
            }
        },
        template: `<div class="tabs-box" :login="islogin">
            <div v-for="(item,index) in tabs" :current="item.tag==tab " 
                v-if="islogin || (!islogin && !item.login)" @click="clickEvent(item.tag,item)">
                <icon v-html="'&#x'+item.icon"></icon>
                <span>{{item.name}}</span>
            </div>
            <el-input class="tab_search" v-for="item in tabs"  v-model="item.search" clearable  @input="changesearch(item)"
                v-show="item.tag==tab" placeholder="搜索" @keyup.enter.native="search(item)">
                <el-button slot="append" icon="el-icon-search" @click="search(item)"></el-button>
            </el-input>
        </div>`
    });
    //考试详情（用于“我的考试”）
    Vue.component('exam_data', {
        //exam:考试场次
        props: ['exam', 'index', 'account'],
        data: function () {
            return {
                paper: {},       //试卷
                subject: {},     //专业
                loading: false
            }
        },
        watch: {},
        computed: {},
        mounted: function () {
            var th = this;
            //获取“试卷详情”
            $api.bat(
                $api.cache('TestPaper/ForID', { 'id': this.exam.Tp_Id }),
                $api.cache('Subject/ForID', { 'id': this.exam.Sbj_ID })
            ).then(axios.spread(function (paper, subject) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        //throw data.message;
                    }
                }
                //获取结果
                th.paper = paper.data.result;
                th.subject = subject.data.result;
            })).catch(function (err) {
                console.error(err);
            });
        },
        methods: {
            goexaming: function (exam) {
                var url=$api.url.set("/web/exam/doing",{"id":exam.Exam_ID});
                //window.location.href = "/web/exam/doing?id=" + exam.Exam_ID;
                return url
            }
        },
        template: `<card>
        <card-title>{{index+1}}.《{{exam.Exam_Name}}》
            <a type="button" :examid="exam.Exam_ID" :href="goexaming(exam)" target="_blank">
                参加考试<icon>&#xe6c6</icon>
            </a>
        </card-title>
        <card-context>
        <div class="item"> {{exam.Exam_Title}}</div>
          <div class="item">时间：
            <span v-if="exam.Exam_DateType==1">
              <!--准时开始-->
              {{exam.Exam_Date|date("yyyy-M-dd HH:mm")}} （准时开始）
            </span>
            <span v-else>
              <!--区间时间-->
              {{exam.Exam_Date|date("yyyy-M-dd HH:mm")}} 至
              {{exam.Exam_DateOver|date("yyyy-M-dd HH:mm")}} 之间
            </span>
          </div>
          <div class="item" v-if="paper">限时：{{exam.Exam_Span}}分钟 &nbsp; 题量：{{paper.Tp_Count}}道</div>
          <div class="item">总分：{{exam.Exam_Total}}分（{{exam.Tp_PassScore}}分及格）</div>        
          <div class="item">专业：{{subject.Sbj_Name}}</div>
          <div class="item" v-if="paper">课程：{{paper.Cou_Name}}</div>        
        </card-context>
      </card>`
    });
    //考试主题（用于所有考试）
    Vue.component('exam_theme', {
        props: ['theme', 'index', 'account'],
        data: function () {
            return {
                group: '',
                exams: [],     //考试场次
                loading: false
            }
        },
        watch: {},
        computed: {},
        mounted: function () {
            this.getgroup();
            this.onload();
        },
        methods: {
            //参考人员
            getgroup: function () {
                var th = this;
                if (this.theme.Exam_GroupType == 1) {
                    th.group = '全体学员';
                    return;
                }
                $api.cache('Exam/GroupType', { 'type': this.theme.Exam_GroupType, 'uid': this.theme.Exam_UID })
                    .then(function (req) {
                        if (req.data.success) {
                            th.group = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
            },
            //获取“考试场次”
            onload: function () {
                var th = this;
                $api.get('Exam/Exams', { 'uid': this.theme.Exam_UID }).then(function (req) {
                    if (req.data.success) {
                        th.exams = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }
        },
        template: `<card class="theme">
        <card-title>{{index+1}}.《{{theme.Exam_Title}}》 </card-title>
        <card-context>
        <div class="item">参考人员：{{group}} </div>     
        <theme_item v-for="(e,index) in exams" :exam="e" :index="index" :account="account"></exam_data>
        </card-context>
      </card>`
    });
    //考试主题中的详情（用于所有考试）
    Vue.component('theme_item', {
        props: ['exam', 'index', 'account'],
        data: function () {
            return {
                paper: {},       //试卷
                subject: {},     //专业
                loading: false
            }
        },
        watch: {
        },
        computed: {
        },
        mounted: function () {
            this.onload();
        },
        methods: {
            //获取“试卷详情”
            onload: function () {
                var th = this;
                $api.bat(
                    $api.cache('TestPaper/ForID', { 'id': this.exam.Tp_Id }),
                    $api.cache('Subject/ForID', { 'id': this.exam.Sbj_ID })
                ).then(axios.spread(function (paper, subject) {
                    //判断结果是否正常
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i].status != 200)
                            console.error(arguments[i]);
                        var data = arguments[i].data;
                        if (!data.success && data.exception != null) {
                            console.error(data.exception);
                            throw data.message;
                        }
                    }
                    //获取结果
                    th.paper = paper.data.result;
                    th.subject = subject.data.result;
                })).catch(function (err) {
                    console.error(err);
                });
            }
        },
        template: `<div>
        <div class="item"><b>第（{{index+1}}）场.《{{exam.Exam_Name}}》   </b>    
        </div>           
          <div class="item">时间：
            <span v-if="exam.Exam_DateType==1">
              <!--准时开始-->
              {{exam.Exam_Date|date("yyyy-M-dd HH:mm")}} （准时开始）
            </span>
            <span v-else>
              <!--区间时间-->
              {{exam.Exam_Date|date("yyyy-M-dd HH:mm")}} 至
              {{exam.Exam_DateOver|date("yyyy-M-dd HH:mm")}} 之间
            </span>
          </div>
          <div class="item" v-if="paper">限时：{{exam.Exam_Span}}分钟 &nbsp; 题量：{{paper.Tp_Count}}道</div>
          <div class="item" v-if="paper">总分：{{exam.Exam_Total}}分（{{exam.Exam_PassScore}}分及格）</div>        
          <div class="item">专业：{{subject.Sbj_Name}} </div>
          <div class="item" v-if="paper">课程：{{paper.Cou_Name}}</div>        
       
      </div>`
    });
    //成绩查看的考试项（用于成绩回顾）
    Vue.component('score_item', {
        props: ['result', 'index', 'account'],
        data: function () {
            return {
                paper: {},       //试卷
                subject: {},     //专业
                exam: {},        //考试
                loading: false
            }
        },
        watch: {},
        computed: {},
        mounted: function () {
            var th = this;
            //获取“试卷详情”
            $api.bat(
                $api.cache('Exam/ForID', { 'id': this.result.Exam_ID }),
                $api.cache('TestPaper/ForID', { 'id': this.result.Tp_Id }),
                $api.cache('Subject/ForID', { 'id': this.result.Sbj_ID })
            ).then(axios.spread(function (exam, paper, subject) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw data.message;
                    }
                }
                //获取结果
                th.exam = exam.data.result;
                th.paper = paper.data.result;
                th.subject = subject.data.result;
            })).catch(function (err) {
                console.error(err);
            });
        },
        methods: {
            //得分样式
            scoreStyle: function (score) {
                //总分和及格分
                var total = this.exam.Exam_Total;
                var passscore = this.paper ? this.paper.Tp_PassScore : -1;
                if (score == total) return "praise";
                if (score < passscore) return "nopass";
                if (score < total * 0.8) return "general";
                if (score >= total * 0.8) return "fine";
                return "";
            },
            gourl: function () {
                var url = $api.url.set("/student/exam/review", {
                    "examid": this.result.Exam_ID,
                    "exrid": this.result.Exr_ID
                });
                var obj = {
                    'url': url, 'ico': 'e6ef',
                    'title': this.exam.Exam_Name,
                    'width': '80%',
                    'height': '80%'
                };
                obj['showmask'] = true; //始终显示遮罩
                obj['min'] = false;
                $pagebox.create(obj).open();
            }
        },
        template: `<card>
        <card-title style="cursor: pointer" @click="gourl">{{index+1}}.《{{exam.Exam_Name}}》
        <score :class="scoreStyle(result.Exr_ScoreFinal)">{{result.Exr_ScoreFinal}} 分</score>
        </card-title>
        <card-context>
            <div class="item"> {{exam.Exam_Title}}</div>          
            <div class="item">限时：{{exam.Exam_Span}}分钟 &nbsp; 题量：{{paper.Tp_Count}}道</div>
            <div class="item">交卷时间：{{result.Exr_SubmitTime|date("yyyy-MM-dd HH:mm:ss")}}</div>
            <div class="item">得分：{{result.Exr_ScoreFinal}} 分
            （满分{{exam.Exam_Total}}分，{{exam.Tp_PassScore}}分及格）</div>           
        </card-context>
      </card>`
    });
}, ["../Components/courses.js",
    "../Components/course.js",
    '../scripts/pagebox.js']);
