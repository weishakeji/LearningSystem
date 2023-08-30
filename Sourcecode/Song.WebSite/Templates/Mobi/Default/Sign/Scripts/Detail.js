$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            acid: $api.querystring('acid'),      //刚注册的学员id
            uid: $api.querystring('uid'),           //校验码
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项    
            sorts: [],   //学员组列表
            accsort: {},      //当前学员所在的分组
            sortpanel: false,
            sortquery: { 'orgid': '', 'use': true, 'search': '', 'index': 1, 'size': 10 },
            sort: {
                loading: true,
                total: 0,
                paper: 0
            },

            account: {},     //要操作的学员账号
            accrules: {
                input: [
                    { required: true, message: '账号不得为空', trigger: 'blur' },
                    { min: 5, max: 50, message: '长度在 5 到 50 个字符', trigger: 'blur' }
                ]
            },
            loading: false,
        },
        mounted: function () { },
        created: function () {
            var th = this;
            $api.bat(
                $api.get('Account/ForID', { 'id': th.acid }),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                //获取结果
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            })).catch(function (err) {
                console.error(err);
            });
        },
        computed: {
            //学员的组是否存在
            sortexist: function () {
                return JSON.stringify(this.accsort) != '{}' && this.accsort != null && !!this.accsort.Sts_ID;
            }
        },
        watch: {
            'organ': {
                handler: function (nv, ov) {
                    if (JSON.stringify(nv) == '{}' || nv == null) return;
                    this.sortquery.orgid = nv.Org_ID;
                    this.sortpaper(1);
                }, immediate: true
            },
        },
        methods: {
            //获取当前学员
            getaccount: function () {

            },
            //分页获取学员组
            sortpaper: function (index) {
                if (index != null) this.sortquery.index = index;
                var th = this;
                th.sort.loading = true;
                $api.get("Account/SortPager", th.sortquery).then(function (d) {
                    th.sort.loading = false;
                    if (d.data.success) {
                        th.sorts = d.data.result;
                        th.sort.paper = Number(d.data.totalpages);
                        th.sort.total = d.data.total;
                        //console.log(th.accounts);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    th.sort.loading = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //选择学员组
            sortselect: function (item) {
                this.sortpanel = false;
                this.accsort = item;
                this.account.Sts_ID = item.Sts_ID;
                this.account.Sts_Name = item.Sts_Name;
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = 'Account/RegisterModify';
                        $api.post(apipath, { 'acc': th.account, 'acid': th.acid, 'uid': th.uid }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                window.setTimeout(function () {
                                    th.goback();
                                }, 200);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err, '错误');
                        }).finally(th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //跳过
            btnJump: function () {
                this.$confirm('是否确定要跳过当前操作?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    this.goback();
                }).catch(() => { });
            },
            //成功后的跳转
            goback: function () {
                var referrer = $api.querystring('referrer');
                if (referrer == null || referrer == '')
                    referrer = $api.storage('singin_referrer');
                referrer = decodeURIComponent(referrer);
                window.navigateTo(referrer != '' ? referrer : '/');
            }
        }
    });

}, ['/Utilities/ElementUi/index.js',
    '/Utilities/Components/education.js']);
