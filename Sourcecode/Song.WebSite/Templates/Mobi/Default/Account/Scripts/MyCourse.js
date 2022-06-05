$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项          
            loading: true,

            active: [],         //课程列表切换  
            defimg: '',          //默认课程图片
            datas: [],           //课程列表
            finished: false,
            query: {
                'acid': -1, 'search': '', 'size': 10, 'index': 0
            },
            method_name: 'purchased',      //接口名称，来自选项卡的名称
            total: 0
        },
        mounted: function () {
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
                    }
                }
                //获取结果
                vapp.account = account.data.result;
                if (vapp.account && !!vapp.account.Ac_ID)
                    vapp.query.acid = vapp.account.Ac_ID;
                vapp.platinfo = platinfo.data.result;
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                vapp.tabChange(null, 'purchased');
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {
            //默认图片
            var img = document.getElementById("default-img");
            this.defimg = img.getAttribute("src");
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
            login: function () {
                window.location.href = "/mobi/sign/in";
            },
            myself: function () {
                window.location.href = "/mobi/account/myself";
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
                    th.loading = false;
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
                    th.loading = false;
                    th.error = err;
                    console.error(err);
                });
            },
            //进入课程详情页
            godetail: function (id) {
                var url = $api.url.dot(id, '/mobi/course/Detail');
                window.location.href = url;
            },
            //页面跳转
            gourl: function (page) {
                var url = $api.url.set(page, {});
                window.location.href = url;
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
                        //...
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
        // 同样也可以在 vm 实例中像 "this.message" 这样使用
        template: `  <div class="cur_data">
                    <div>
                        <span  class="font_icon">&#xe841</span>
                        章节 {{data.outline}} 
                    </div>
                    <div>
                        <span  class="font_icon">&#xe6b0</span>
                        试题 {{data.question}} 
                    </div>
                    <div>
                        <span  class="font_icon">&#xe83a</span>
                        视频 {{data.video}}
                    </div>
                    <div>
                        <span  class="font_icon">&#xa03a</span>
                        关注 {{viewnum}}
                    </div>
                </div>`
    });
    // 课程购买信息
    Vue.component('purchase_data', {
        props: ['couid', 'stid'],
        data: function () {
            return {
                //课程数据信息
                data: {},
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
                $api.get('Course/Purchaselog:5', { 'couid': this.couid, 'stid': this.stid }).then(function (req) {
                    if (req.data.success) {
                        th.data = req.data.result;
                        //...
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
        // 同样也可以在 vm 实例中像 "this.message" 这样使用
        template: `  <div class="purchase_data"> 
        {{data.Stc_EndTime|date("yyyy-M-d ")}} 过期                 
                </div>`
    });
    //课程视频的学习进度
    Vue.component('video_progress', {
        //config:机构的配置项，其实包括了视频完成度的容差值（VideoTolerance）
        props: ['couid', 'stid', 'config'],
        data: function () {
            return {
                percent: 0,     //完成的真实百分比
                tolerance: 10,       //容差，例如容差为10，则完成90%即可认为是100%
                loading: false
            }
        },
        watch: {
            'couid': {
                handler: function (nv, ov) {
                    this.onload();
                }, immediate: true
            },
            'config': {
                handler: function (nv, ov) {
                    if (nv && nv.VideoTolerance) {
                        this.tolerance = Number(nv.VideoTolerance);
                        this.tolerance = isNaN(this.tolerance) ? 0 : this.tolerance;
                        this.tolerance = this.tolerance <= 0 ? 0 : this.tolerance;
                    }
                }, immediate: true, deep: true
            },
        },
        computed: {
            'color': function () {
                if (this.progress == 0) return '#909399';
                if (this.progress <= 30) return '#F56C6C';
                if (this.progress <= 60) return '#E6A23C';
                if (this.progress <= 80) return '#67C23A';
                return '#409EFF';
            },
            //完成度，加了容差之后的
            'progress': function () {
                return this.percent + this.tolerance >= 100 ? 100 : this.percent;
            },
        },
        mounted: function () { },
        methods: {
            onload: function () {
                var th = this;
                th.loading = true;
                $api.cache('Course/ProgressForVideo:5', { 'couid': this.couid, 'stid': this.stid })
                    .then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            th.percent = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        th.percent = 0
                        console.error(err);
                    });
            }
        },
        // 同样也可以在 vm 实例中像 "this.message" 这样使用
        template: ` <div class="progress"> 
                     <van-progress :percentage="progress" :color="color"></van-progress>              
                </div>`
    });
}, ['../Components/courses.js']);
