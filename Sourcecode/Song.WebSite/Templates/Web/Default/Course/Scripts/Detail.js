$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.dot(),        //课程id
            account: {},     //当前登录账号       
            org: {},
            config: {},      //当前机构配置项     
            platinfo: {},

            subjects: [],        //当前机构下的专业
            course: {},         //当前课程对象
            videolog: [],        //课程章节的视频学习记录
            sum: 0,              //购买课程的人数
            teacher: null,     //课程教师
            outlines: [],     //课程章节
            guideCol: [],          //课程通知的分类
            prices: [],          //课程价格
            studied: false,        //是否可以学习课程
            purchase: {},          //课程购买记录
            studied: false,     //是否能够学习
            owned: false,            //是否拥有这个课程（购买或学员组关联）

            testpapers: [],          //试卷
            finaltest: {},           //结课考试

            loading: true,       //加载状态
            loading_init: false,


            guide: {},                   //当前要显示的通知公告
        },
        watch: {
            //当机构加载时
            'org': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    //获取专业
                    var th = this;
                    $api.cache('Subject/TreeFront', { 'orgid': th.org.Org_ID }).then(function (req) {
                        if (req.data.success) {
                            th.subjects = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        console.error(err);
                    });
                }, immediate: true,
            },
            //学员登录后
            'account': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    var th = this;
                    th.loading = true;
                    $api.bat(
                        $api.get('Course/Studied', { 'couid': th.couid }),
                        $api.get('Course/Purchaselog', { 'couid': th.couid, 'stid': th.account.Ac_ID }),
                        $api.cache('Course/LogForOutlineVideo:5', { 'stid': th.account.Ac_ID, 'couid': th.couid }),   //章节的视频学习记录  
                        $api.get('Course/Owned', { 'couid': th.couid, 'acid': th.account.Ac_ID })
                    ).then(([studied, purchase, videolog, owned]) => {
                        //获取结果
                        th.studied = studied.data.result;
                        if (purchase.data.result != null)
                            th.purchase = purchase.data.result;
                        th.videolog = videolog.data.result;
                        th.owned = owned.data.result;
                    }).catch(err => console.error(err))
                        .finally(() => th.loading = false);
                }, immediate: true,
            },
            'nullcourse': function (nv, ov) {
                //console.log(nv);
                this.$nextTick(function () {
                    this.qrcode();
                });
            }
        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); },
            //课程为空,或课程被禁用
            nullcourse: function () {
                return JSON.stringify(this.course) == '{}' || this.course == null || !this.course.Cou_IsUse;
            },
            //是否购买
            purchased: function () {
                if (JSON.stringify(this.purchase) == '{}' || this.purchase == null) return false;
                if (this.purchase.Stc_EndTime.getTime() < (new Date()).getTime())
                    return false;
                if (this.purchase.Stc_IsTry) return false;
                return this.purchase.Stc_Type != 5 && !this.course.Cou_IsFree && this.purchase.Stc_IsEnable;
            },
            //可以学习
            canstudy: function () {
                return this.studied && (this.purchased && this.purchase.Stc_IsEnable);
            },
            //是否可以永久学习
            forever: function () {
                if (!this.purchase) return false;
                if (!this.purchase.Stc_IsEnable) return false;
                if (this.purchase.Stc_Type != 5) return false;
                var time = this.purchase.Stc_EndTime;
                if (time == '' || time == null) return false;
                if ($api.getType(time) != 'Date') return false;
                var year = time.getFullYear();
                return time.getFullYear() - new Date().getFullYear() > 100;
            }
        },
        created: function () {

        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            //当前的机构、登录学员、课程
            $api.bat(
                $api.get('Course/ForID', { 'id': th.couid }),
                $api.cache('Course/ViewNum:60', { 'couid': th.couid, 'num': 1 })
            ).then(([course, viewnum]) => {
                //当前课程
                th.course = course.data.result;
                if (th.course != null) {
                    th.course.Cou_Target = th.clearTag(th.course.Cou_Target);
                    th.course.Cou_Intro = $api.trim(th.course.Cou_Intro);
                    if (Number(viewnum.data.result) >= 0)
                        th.course.Cou_ViewNum = viewnum.data.result;
                    document.title = th.course.Cou_Name;
                }
                if (!th.course) return;
                //课程章节，价格，购买人数,通知，教师，是否购买,购买的记录，是否可以学习（如果课程免费不购买也可以）     
                th.getPrices(th.course);//获取价格          
                $api.bat(
                    $api.cache('Outline/TreeList', { 'couid': th.couid }),
                    $api.get('TestPaper/ShowPager', { 'couid': th.couid, 'search': '', 'diff': '', 'size': 999999, 'index': 1 }),              
                    $api.get('Course/StudentSum', { 'couid': th.couid }),
                    $api.get('Guide/ColumnsTree', { 'couid': th.couid, 'search': '', 'isuse': '' }),
                    $api.get('Teacher/ForID', { 'id': th.course.Th_ID })
                ).then(([outlines, paper, sum, guideCol, teacher]) => {
                    //章节
                    th.outlines = outlines.data.result;
                    //试卷,结课考试
                    let papers = paper.data.result;
                    if (papers != null && papers.length > 0) {
                        for (let i = 0; i < papers.length; i++) {
                            if (papers[i].Tp_IsFinal) {
                                th.finaltest = papers[i];
                                papers.splice(i, 1);
                            };
                        }
                        th.testpapers = papers;
                    }                 
                    th.sum = sum.data.result;
                    th.guideCol = guideCol.data.result;
                    th.teacher = teacher.data.result;
                }).catch(err => console.error(err))
                    .finally(() => th.loading_init = false);
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        methods: {
            //清理Html标签
            clearTag: function (html) {
                if (!html) return "";
                return $api.trim(html.replace(/<\/?.+?>/g, ""));
            },
            //获取价格信息
            getPrices: function (cou) {
                if (cou.Cou_IsFree || cou.Cou_IsLimitFree) return;
                if (!$api.isnull(cou.Cou_Prices) && cou.Cou_Prices.length != 0) {
                    this.prices = $api.parseJson(cou.Cou_Prices);
                    return;
                }
                var th = this;
                $api.put('Course/Prices', { 'uid': cou.Cou_UID }).then(function (req) {
                    if (req.data.success) {
                        th.prices = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //生成二维码
            qrcode: function () {
                var box = $dom("#course-qrcode");
                if (box.length < 1) window.setTimeout(this.qrcode, 200);
                box.each(function () {
                    if ($dom(this).find("img").length > 0) return;
                    let url = window.location.href;
                    new QRCode(this, {
                        text: url,
                        width: 75, height: 75,
                        colorDark: "#000000", colorLight: "#ffffff",
                        render: "canvas", correctLevel: QRCode.CorrectLevel.L
                    });
                });
            },
            //转到学习页
            tolearing: function (course, outline) {
                let url = '';
                if (course.Cou_Type == 0) url = 'study';
                if (course.Cou_Type == 2) url = '../question/course';
                url = $api.url.dot(course.Cou_ID, url);
                url = $api.url.set(url, { 'olid': outline.Ol_ID });
                if (course.Cou_Type == 0)
                    return window.location.href = url;
                if (course.Cou_Type == 2) {
                    var obj = {
                        'url': url,
                        'ico': 'e731', 'min': false, 'showmask': true,
                        'title': '试题练习 - ' + course.Cou_Name,
                        'width': '600',
                        'height': '80%'
                    }
                    let pbox = top.$pagebox.create(obj);
                    pbox.open();
                }
            }
        }
    });
    // 课程内容选项
    Vue.component('course_tabs', {
        props: ["account", "course", "canstudy", "studied", "loading"],
        data: function () {
            return {
                loading_show: false,     //预载中 
                menus: [
                    { name: '课程介绍', tab: 'intro', icon: '&#xe813', size: 20, show: true, evt: null },
                    { name: '章节', tab: 'outline', icon: '&#xe841', size: 20, show: true, evt: null },
                    { name: '课程公告', tab: 'guide', icon: '&#xe697', size: 21, show: true, evt: null },
                    { name: '交流咨询', tab: 'message', icon: '&#xe817', size: 22, show: false, evt: null },
                    { name: '测试/考试', tab: 'test', icon: '&#xe810', size: 21, show: true, evt: null },
                ],
                activeName: $api.querystring('tab', 'intro'),
                curr_menus: {},  //当前点击的按钮项
                outline: {},     //当前点击的章节
                loading: false
            }
        },
        watch: {
            //预载结束，隐藏提示信息
            'loading': function (nv, ov) {
                if (!nv) this.loading_show = false;
            }
        },
        computed: {},
        mounted: function () { },
        methods: {},
        template: `<el-tabs v-model="activeName">
            <el-tab-pane v-for="item in menus" v-if="item.show" :name="item.tab">
                <template slot="label">
                    <icon v-html="item.icon" :style="'font-size:'+item.size+'px'"></icon>
                    {{item.name}}
                </template>  
            <slot :name="item.tab"></slot>      
            </el-tab-pane>        
        </el-tabs>`
    });
}, ['../scripts/pagebox.js',
    'Components/largebutton.js',        //购买课程的按钮
    'Components/breadcrumb.js',         //顶部面包屑
    'Components/guides.js',
    'Components/progress_video.js',
    '../Components/courses.js',
    '/Utilities/Scripts/qrcode.js',]);
$dom.load.css([$dom.path() + 'styles/pagebox.css']);