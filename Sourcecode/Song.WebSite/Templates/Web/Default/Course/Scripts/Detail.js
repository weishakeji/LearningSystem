$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.dot(),        //课程id
            account: {},     //当前登录账号       
            organ: {},
            config: {},      //当前机构配置项     
            subjects: [],        //当前机构下的专业

            course: {},         //当前课程对象
            videolog: [],        //课程章节的视频学习记录
            sum: 0,              //购买课程的人数
            teacher: null,     //课程教师
            outlines: [],     //课程章节
            guideCol: [],          //课程通知的分类
            prices: [],          //课程价格
            isbuy: false,        //是否购买课程
            purchase: {},          //课程购买记录
            canStudy: false,     //是否能够学习

            testpapers: [],          //试卷
            finaltest: {},           //结课考试

            loading: false,       //加载状态
            loading_init: false,


            guide: {},                   //当前要显示的通知公告
        },
        watch: {
            'course': function (nv, ov) {
                this.$nextTick(function () {
                    //this.qrcode();
                });
            },
            'nullcourse': function (nv, ov) {
                console.log(nv);
                this.$nextTick(function () {
                    this.qrcode();
                });
            }
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
            //课程为空,或课程被禁用
            nullcourse: function () {
                return JSON.stringify(this.course) == '{}' || this.course == null || !this.course.Cou_IsUse;
            },
            //是否购买记录
            purchased: function () {
                return JSON.stringify(this.purchase) != '{}' && this.purchase != null;
            }
        },
        created: function () {

        },
        mounted: function () {
            var th = this;
            this.loading_init = true;
            //当前的机构、登录学员、课程
            $api.bat(
                $api.post('Organization/Current'),
                $api.cache('Course/ForID', { 'id': th.couid })
            ).then(axios.spread(function (organ, course) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                    }
                }
                //机构配置信息
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;
                //获取专业
                $api.cache('Subject/TreeFront', { 'orgid': th.organ.Org_ID }).then(function (req) {
                    th.loading_init = false;
                    if (req.data.success) {
                        th.subjects = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading_init = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
                //当前课程
                th.course = course.data.result;
                if (th.course != null) {
                    th.course.Cou_Target = th.clearTag(th.course.Cou_Target);
                    th.course.Cou_Intro = $api.trim(th.course.Cou_Intro);
                    document.title = th.course.Cou_Name;
                }
                if (!th.course) return;

                //课程章节，价格，购买人数,通知，教师，是否购买,购买的记录，是否可以学习（如果课程免费不购买也可以）               
                $api.bat(
                    $api.get('Outline/TreeList', { 'couid': th.couid }),
                    $api.get('TestPaper/ShowPager', { 'couid': th.couid, 'search': '', 'diff': '', 'size': Number.MAX_VALUE, 'index': 1 }),
                    $api.get('Course/Prices', { 'uid': th.course.Cou_UID }),
                    $api.get('Course/StudentSum', { 'couid': th.couid }),
                    $api.get('Guide/ColumnsTree', { 'couid': th.couid, 'search': '', 'isuse': '' }),
                    $api.get('Teacher/ForID', { 'id': th.course.Th_ID }),
                    $api.get('Course/Studied', { 'couid': th.couid })
                ).then(axios.spread(function (outlines, paper, prices, sum, guideCol, teacher, isbuy) {
                    th.loading_init = false;
                    //判断结果是否正常
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i].status != 200)
                            console.error(arguments[i]);
                        var data = arguments[i].data;
                        if (!data.success && data.exception != null) {
                            console.error(data.exception);
                        }
                    }
                    //章节
                    th.outlines = outlines.data.result;
                    //试卷,结课考试
                    var papers = paper.data.result;
                    if (papers != null && papers.length > 0) {
                        for (let i = 0; i < papers.length; i++) {
                            if (papers[i].Tp_IsFinal) {
                                th.finaltest = papers[i];
                                papers.splice(i, 1);
                            };
                        }
                        th.testpapers = papers;
                    }
                    th.prices = prices.data.result;
                    console.log(th.prices);
                    th.sum = sum.data.result;
                    th.guideCol = guideCol.data.result;
                    th.teacher = teacher.data.result;
                    th.isbuy = isbuy.data.result;
                })).catch(function (err) {
                    console.error(err);
                });
            })).catch(function (err) {
                console.error(err);
            });
        },
        methods: {
            //当学员登录后
            forlogin: function (acc) {
                var th = this;
                th.account = acc;
                $api.bat(
                    $api.get('Course/StudyAllow', { 'couid': th.couid }),
                    $api.get('Course/Purchaselog', { 'couid': th.couid, 'stid': th.account.Ac_ID }),
                    $api.get('Course/LogForOutlineVideo:5', { 'stid': th.account.Ac_ID, 'couid': th.couid })   //章节的视频学习记录                    
                ).then(axios.spread(function (canStudy, purchase, videolog) {
                    //判断结果是否正常
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i].status != 200)
                            console.error(arguments[i]);
                        var data = arguments[i].data;
                        if (!data.success && data.exception != null) {
                            //console.error(data.exception);
                            //throw arguments[i].config.way + ' ' + data.message;
                        }
                    }
                    //获取结果
                    th.canStudy = canStudy.data.result;
                    if (purchase.data.result != null)
                        th.purchase = purchase.data.result;
                    th.videolog = videolog.data.result;
                })).catch(function (err) {
                    console.error(err);
                });
            },
            //清理Html标签
            clearTag: function (html) {
                if (!html) return "";
                var txt = html.replace(/<\/?.+?>/g, "");
                txt = $api.trim(txt);
                return txt;
            },
            //生成二维码
            qrcode: function () {
                var box = $("#course-qrcode");
                if (box.size() < 1) {
                    window.setTimeout(this.qrcode, 200);
                }
                box.each(function () {
                    if ($(this).find("img").size() > 0) return;
                    var url = $api.url.dot($api.dot(), window.location.origin + "/mobi/course/Detail");
                    console.log(url);
                    jQuery($(this)).qrcode({
                        render: "canvas", //也可以替换为table
                        width: 75,
                        height: 75,
                        foreground: "#000",
                        background: "#FFF",
                        text: url
                    });
                    //将canvas转换成img标签，否则无法打印
                    var canvas = $(this).find("canvas").hide()[0];  /// get canvas element
                    var img = $(this).append("<img/>").find("img")[0]; /// get image element
                    img.src = canvas.toDataURL();
                });
            },
        }
    });
    // 课程内容选项
    Vue.component('course_tabs', {
        props: ["account", "course", "canstudy", "isbuy", "loading"],
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
            },
            'activeName': function (nv, ov) {
                console.log(nv);
            }
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        mounted: function () {

        },
        methods: {
            tabclick: function (obj) {
                console.log(obj);
            }
        },
        // 同样也可以在 vm 实例中像 "this.message" 这样使用
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
}, ["Components/largebutton.js",        //购买课程的按钮
    "Components/breadcrumb.js",         //顶部面包屑
    "Components/guides.js",
    "Components/progress_video.js",
    "../Components/courses.js",
    '/Utilities/Scripts/jquery.qrcode.min.js',]);
