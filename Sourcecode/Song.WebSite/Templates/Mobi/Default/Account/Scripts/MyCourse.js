$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项          
            loading: true,

            active: [],         //课程列表切换  
            defimg: '',          //默认课程图片
            datas: [],           //课程列表
            finished: false,
            query: {
                'acid': -1, 'search': '', 'enable': true, 'size': 10, 'index': 0
            },
            method_name: 'purchased',      //接口名称，来自选项卡的名称
            total: 0
        },
        mounted: function () {
        },
        created: function () {
            //默认图片
            var img = document.getElementById("default-img");
            this.defimg = img.getAttribute("src");
        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); },
        },
        watch: {
            'account': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    this.query.acid = nv.Ac_ID;
                    this.tabChange(null, 'purchased');
                    this.loading = false;
                }, immediate: true
            },
        },
        methods: {
            login: function () {
                window.navigateTo(this.commonaddr('signin'));
            },
            myself: function () {
                window.navigateTo("/mobi/account/myself");
            },
            //选项卡切换,index没有用，title为选项卡标识，作为排序类型用
            tabChange: function (index, title) {
                this.query.index = 0;
                this.total = 0;
                this.finished = false;
                this.datas = [];
                if (title != null)
                    this.onload(title);
            },
            onload: function (title) {
                var th = this;
                if (title != null)
                    th.method_name = title;
                th.query.index++;
                if (th.query.acid === undefined || th.query.acid == -1) return;
                var query = $api.clone(this.query);
                var apiurl = "Course/" + this.method_name;
                $api.get(apiurl, query).then(function (req) {
                    if (req.data.success) {
                        th.total = req.data.total;
                        var result = req.data.result;
                        for (var i = 0; i < result.length; i++) {
                            th.datas.push(result[i]);
                        }
                        var totalpages = req.data.totalpages;
                        // 数据全部加载完成
                        if (th.datas.length >= th.total || result.length == 0) {
                            th.finished = true;
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.error = err;
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //进入课程详情页
            godetail: function (id) {
                let url = $api.url.dot(id, '/mobi/course/Detail');
                window.navigateTo(url);
            },
            //页面跳转
            gourl: function (page) {
                let url = $api.url.set(page, {});
                window.navigateTo(url);
            },
            //课程单元格滑动时
            cell_swipe: function (event) {
                var position = event.position;
                if (position == "right") {
                    //console.log("显示详情");
                }
            },
        }
    });
    // 课程详情
    Vue.component('course_data', {
        props: ['couid', 'viewnum'],
        data: function () {
            return {
                //课程数据信息
                data: {
                    'outline': 0,
                    'question': 0,
                    'video': 0
                },
                loading: false
            }
        },
        watch: {
            'couid': {
                handler: function (nv, ov) {
                    this.onload();
                }, immediate: true
            }

        },
        computed: {},
        mounted: function () { },
        methods: {
            onload: function () {
                var th = this;
                $api.cache('Course/Datainfo:20', { 'couid': this.couid }).then(function (req) {
                    if (req.data.success) {
                        th.data = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err));
            }
        },
        template: `  <div class="cur_data">
                    <div>
                        <icon outline></icon>
                        章节 {{data.outline}} 
                    </div>
                    <div>
                        <icon question></icon>
                        试题 {{data.question}} 
                    </div>
                    <div>
                        <icon video></icon>
                        视频 {{data.video}}
                    </div>
                    <div>
                        <icon test></icon>
                        测试 {{data.testpaper}}
                    </div>
                    <div>
                        <icon view></icon>
                        关注 {{viewnum}}
                    </div>
                </div>`
    });

}, ['../Components/courses.js',
    'Components/purchase_data.js',
    'Components/video_progress.js',
    'Components/ques_progress.js',
    'Components/exam_test.js']);
