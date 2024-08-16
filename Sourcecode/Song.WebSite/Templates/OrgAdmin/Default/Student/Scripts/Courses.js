$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),      //学员id
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项       
            query: {
                'acid': -1, 'search': '', 'enable': null, 'size': 10, 'index': 0
            },
            //左上角的按钮
            methods: [
                { 'name': '学习中的课程', 'label': 'purchased', 'icon': '&#xe813' },
                { 'name': '过期课程', 'label': 'overdue', 'icon': '&#xe671' },
                { 'name': '试学课程', 'label': 'ontrial', 'icon': '&#xe84d' }
            ],
            method_name: 'purchased',      //接口名称，来自选项卡的名称
            datas: [],           //课程列表
            total: 1, //总记录数
            totalpages: 1, //总页数

            loading_init: true,
            loading_id: 0,       //更新状态时的预载
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/ForID', { 'id': th.id }),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(([account, platinfo, organ]) => {
                //获取结果
                th.account = account.data.result;
                if (th.account && !!th.account.Ac_ID)
                    th.query.acid = th.account.Ac_ID;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.handleCurrentChange();

            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
            'method_name': {
                handler: function (nv, ov) {
                    this.handleCurrentChange(1);
                }, immediate: true
            }
        },
        methods: {
            //分页加载数据
            handleCurrentChange: function (index) {
                var th = this;
                th.loading = true;
                if (index != null) this.query.index = index;
                if (th.query.acid === undefined || th.query.acid == -1) return;
                //var query = $api.clone(this.query);
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                //console.log(document.documentElement.clientHeight);
                th.query.size = Math.floor(area / 200);
                var apiurl = "Course/" + this.method_name;
                $api.get(apiurl, th.query).then(function (req) {
                    if (req.data.success) {
                        th.total = req.data.total;
                        var result = req.data.result;
                        //添加一些字段，用于增加学员选修时间的表单
                        for (let i = 0; i < result.length; i++) {
                            result[i]['addtime_show'] = false;
                            result[i]['addtime_value'] = '';
                            result[i]['addtime_loading'] = false;
                        }
                        th.datas = [];
                        th.datas = result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.error = err;
                    console.error(err);
                }).finally(() => th.loading = false);
            },          
            //查看结课成绩的详情
            viewScore: function (item,purchase) {
                //if (!window.top || !window.top.vapp) return;
                var url = "/Student/Course/ScoreDetails";
                url = $api.url.dot(item.Cou_ID, url);
                url = $api.url.set(url, { 'stid': this.account.Ac_ID, 'stcid': purchase.Stc_ID });
                var obj = {
                    'url': url,
                    'ico': 'e6ef', 'min': false,
                    'title': '成绩详情 - ' + item.Cou_Name,
                    'width': '800px',
                    'height': '400px'
                }
                obj['showmask'] = true; //始终显示遮罩
                obj['min'] = false;
                var box = window.top.$pagebox.create(obj);
                box.open();
                //window.top.vapp.open(obj);
            },
            //增加学员选修课程的时间
            purchaseAddTime: function (num, course) {
                console.log(num);
                if (num <= 0) return;
                var th = this;
                course['addtime_loading'] = true;
                $api.get('Course/PurchaseAddTime', { 'stid': th.id, 'couid': course.Cou_ID, 'number': num, 'unit': '天' })
                    .then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            var fuc = th.$refs['purchase_data_' + course.Cou_ID][0].onload;
                            if (fuc != num) fuc();
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    }).finally(function () {
                        course['addtime_loading'] = false;
                    });
            },
            //打开“开课”的窗体
            begincourse: function () {
                var stid = this.id;
                var url = $api.url.dot(stid, $dom.routepath() + 'begincourse');
                //直接创建
                var box = window.top.$pagebox.create({
                    width: 800, height: '70%',
                    resize: true, min: false,
                    ico: 'e813',
                    id: 'begincourse_' + stid,
                    title: '给学员（' + this.account.Ac_Name + ' ' + this.account.Ac_AccName + '）添加选修课程',
                    showmask: true,
                    pid: window.name,
                    url: url
                });
                box.open();
            },
            //禁用学习课程的记录
            //purchase:课程购买记录项
            purchaseEnable: function (purchase, enable) {
                var th = this;
                th.loading_id = purchase.Stc_ID;
                purchase.Stc_IsEnable = enable;
                var params = { 'stid': th.id, 'couid': purchase.Cou_ID, 'enable': enable };
                $api.post('Course/PurchaseEnable', params).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            message: '更新状态成功！',
                            type: 'success'
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_id = 0);
            },
            //删除当前学习记录
            purchaseDel: function (purchase) {
                this.$confirm('此操作将永久删除数据,不可恢复, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    var th = this;
                    th.loading_id = purchase.Stc_ID;
                    var params = { 'stid': purchase.Ac_ID, 'couid': purchase.Cou_ID };
                    $api.delete('Course/PurchaseDelete', params).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.$message({
                                type: 'success',
                                message: '删除成功!'
                            });
                            th.handleCurrentChange();
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    }).finally(() => th.loading_id = 0);

                }).catch(() => { });
            }
        }
    });

}, ['Components/course_data.js',
    'Components/purchase_data.js',      // 课程购买信息
    'Components/video_progress.js',
    'Components/ques_progress.js',
    'Components/exam_test.js']);
