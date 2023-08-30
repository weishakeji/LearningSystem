$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项      
            mremove: false,  //移除金额、充值相关

            loading: true,
            show_subject: false, //显示专业面板
            active: 0,         //课程列表切换           
            sear_str: '',
            sbjname: '',         //当前专业名称

            defimg: '',          //默认课程图片
            datas: [],           //课程列表
            finished: false,
            query: {
                'orgid': -1, 'sbjids': 0, 'search': '',
                'order': 'rec', 'size': 2, 'index': 0
            },
            total: 0
        },
        mounted: function () {
            /*
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.post('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                //获取结果
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                th.query.orgid = th.organ.Org_ID;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                //是否移除充值金额相关
                if (!!th.config.IsMobileRemoveMoney)
                    th.mremove = th.config.IsMobileRemoveMoney;
                //
                th.popupSubject();
                th.tabChange(0, 'rec');
            })).catch(function (err) {
                console.error(err);
            });*/
        },
        created: function () {
            //默认图片
            var img = document.getElementById("default-img");
            this.defimg = img.getAttribute("src");
            //默认参数
            this.sear_str = $api.querystring("search");
            this.query.sbjids = $api.querystring("sbjid");
            this.sbjname = decodeURIComponent($api.querystring("sbjname"));
            var th = this;
            if (this.sbjname == '' && this.query.sbjids != '') {
                $api.get('Subject/ForID', { 'id': this.query.sbjids }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.sbjname = result.Sbj_Name;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            }
            //树形菜单
            window.tree = $treemenu.create({ target: '#treemenu', width: "100%" });
            window.tree.onclick(function (s, e) {
                vapp.nodeClick(s, e);
            });
        },
        computed: {},
        watch: {
            'sear_str': function (nv, ov) {
                if (nv == '') this.onSearch();
            },
            'org': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    this.query.orgid = nv.Org_ID;
                    //是否移除充值金额相关
                    if (!!this.config.IsMobileRemoveMoney)
                        this.mremove = this.config.IsMobileRemoveMoney;
                    this.popupSubject();
                    this.tabChange(0, 'rec');
                    this.loading = false;
                }, immediate: true
            },
        },
        methods: {
            onSearch: function () {
                var url = $api.setpara('search', encodeURIComponent(this.sear_str));
                history.pushState({}, "", url); //更改地址栏信息
                this.tabChange(null, null);
            },
            //顶部的专业名称标签，关闭事件
            closeTag: function () {
                var url = $api.setpara('sbjid', '');
                history.pushState({}, "", url);
                history.pushState({}, "", $api.setpara('sbjname', ''));
                this.sbjname = '';
                this.query.sbjids = 0;
                this.tabChange(null, null);
            },
            //专业面板的点击事件
            nodeClick: function (s, e) {
                var sbjid = e.data.id;
                var sbjname = e.data.title;
                var url = $api.setpara('sbjid', sbjid);
                history.pushState({}, "", url);
                url = $api.setpara('sbjname', encodeURIComponent(sbjname));
                history.pushState({}, "", url); //更改地址栏信息
                this.show_subject = false;
                this.sbjname = sbjname;
                this.query.sbjids = sbjid;
                this.tabChange(null, null);
            },
            //选项卡切换,index没有用，title为选项卡标识，作为排序类型用
            tabChange: function (index, title) {
                this.datas = [];
                this.finished = false;
                this.loading = true;
                this.total = 0;
                this.query.index = 0;
                if (title != null)
                    this.query.order = title;
                this.query.search = this.sear_str;

                this.onload();
            },
            onload: function () {
                var th = this;
                th.query.index++;
                if (th.query.orgid === undefined || th.query.orgid == -1) return;
                var query = $api.clone(this.query);
                $api.cache('Course/ShowPager', query).then(function (req) {
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
            //课程单元格滑动时
            cell_swipe: function (event) {
                let position = event.position;
                if (position == "right") {
                    //console.log("显示详情");
                }
            },
            //进入课程详情页
            godetail: function (id) {
                let url = $api.url.set('Detail', {
                    'id': id
                });
                window.navigateTo(url);
            },
            //专业面板打开时
            popupSubject: function () {
                if (!window.tree.isnull()) return;
                if (!vapp.org.Org_ID) return;
                $api.cache('Subject/TreeFront:60', { orgid: vapp.org.Org_ID }).then(function (req) {
                    if (req.data.success) {
                        var result = vapp.nodeconvert(req.data.result);
                        if (window.tree.isnull())
                            window.tree.add(result);
                        window.tree.complete = false;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            /*节点处理方法*/
            nodeconvert: function (obj) {
                var result = '';
                if (typeof (obj) != 'string')
                    result = JSON.stringify(obj);
                result = result.replace(/Sbj_ID/g, "id");
                result = result.replace(/Sbj_Name/g, "title");
                result = result.replace(/Sbj_ByName/g, "tit");
                result = result.replace(/children/g, "childs");
                return JSON.parse(result);
            }
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
        computed: {
        },
        mounted: function () {
            //this.onload();
        },
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
                    //alert(err);
                    console.error(err);
                });
            }
        },
        // 同样也可以在 vm 实例中像 "this.message" 这样使用
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
                            <icon  video></icon>
                            视频 {{data.video}}
                        </div>
                        <div>
                            <icon view></icon>
                            关注 {{viewnum}}
                        </div>
                    </div>`
    });
}, ["/Utilities/Panel/Scripts/ctrls.js",
    "../Scripts/treemenu.js"]);