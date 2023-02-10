$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("id") == "" ? $api.dot() : $api.querystring("id"),        //课程id
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项     

            course: {},         //当前课程对象
            videolog: [],        //课程章节的视频学习记录
            datainfo: {},              //课程的相关数据统计
            teacher: null,     //课程教师
            outlines: [],     //课程章节
            guides: [],          //课程通知
            prices: [],          //课程价格
            studied: false,        //是否在学习该课程
            owned: false,            //是否拥有该课程，例如购买或学员组关联
            purchase: null,          //课程购买记录           

            loading: true,       //加载状态
            loading_init: false,

            showState: 1,         //内容显示的切换状态
            guides_show: false,           //通知公告是否显示
            guide: {},                   //当前要显示的通知公告
            tabActive: 0
        },
        watch: {
            'curr_sbjid': function (nv, ov) {
                console.log(nv);
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
            this.loading_init = true;
            //当前的机构、登录学员、课程
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current'),
                $api.get('Course/ForID', { 'id': th.couid }),
                $api.post('Course/ViewNum', { 'couid': th.couid, 'num': 1 })
            ).then(axios.spread(function (account, platinfo, organ, course, viewnum) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                    }
                }
                //当前登录学员、平台信息         
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                //机构配置信息
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;
                //当前课程
                th.course = course.data.result;
                th.course.Cou_Target = th.clearTag(th.course.Cou_Target);
                th.course.Cou_Intro = $api.trim(th.course.Cou_Intro);
                if (Number(viewnum.data.result) >= 0)
                    th.course.Cou_ViewNum = viewnum.data.result;
                document.title = th.course.Cou_Name;
                if (!th.course) return;

                //课程章节，价格，购买人数,通知，教师，是否购买,购买的记录，是否可以学习（如果课程免费不购买也可以）               
                $api.bat(
                    $api.cache('Outline/TreeList:3', { 'couid': th.couid }),
                    $api.get('Course/Prices', { 'uid': th.course.Cou_UID }),
                    $api.get('Course/Datainfo', { 'couid': th.couid }),
                    $api.cache('Guide/Guides:3', { 'couid': th.couid, 'count': 20 }),
                    $api.get('Teacher/ForID', { 'id': th.course.Th_ID })
                ).then(axios.spread(function (outlines, prices, datainfo, guides, teacher) {
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
                    //获取结果
                    th.outlines = outlines.data.result;
                    th.prices = prices.data.result;
                    th.datainfo = datainfo.data.result;
                    th.guides = guides.data.result;
                    th.teacher = teacher.data.result;
                    //如果已经登录
                    if (th.islogin) {
                        th.loading = true;
                        $api.bat(
                            $api.get('Course/Studied', { 'couid': th.couid }),
                            $api.get('Course/Owned', { 'couid': th.couid, 'acid': th.account.Ac_ID }),
                            $api.cache('Course/Purchaselog', { 'couid': th.couid, 'stid': th.account ? th.account.Ac_ID : 0 }),
                            $api.cache('Course/LogForOutlineVideo:10', { 'stid': th.account.Ac_ID, 'couid': th.couid })   //章节的视频学习记录
                        ).then(axios.spread(function (studied, owned, purchase, videolog) {
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
                            th.studied = studied.data.result;
                            th.owned = owned.data.result;
                            if (purchase.data.result != null)
                                th.purchase = purchase.data.result;
                            th.videolog = videolog.data.result;
                        })).catch(function (err) {
                            console.error(err);
                        });
                    }
                })).catch(function (err) {
                    console.error(err);
                }).finally(function () {
                    th.loading = false;
                });
            })).catch(function (err) {
                console.error(err);
            });
        },
        methods: {
            //清理Html标签
            clearTag: function (html) {
                var txt = html.replace(/<\/?.+?>/g, "");
                txt = $api.trim(txt);
                return txt;
            }
        }
    });
}, ["Components/course_menus.js",
    "Components/progress_video.js"]);
