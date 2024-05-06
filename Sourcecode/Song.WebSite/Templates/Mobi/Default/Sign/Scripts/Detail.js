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
                loading: true, total: 0, paper: 0
            },

            account: {},     //要操作的学员账号
            accrules: {
                Ac_Name: [
                    { required: true, message: '姓名不得为空', trigger: 'blur' },
                    { min: 1, max: 10, message: '长度在 1 到 10 个字符', trigger: 'blur' }
                ],
                Ac_IDCardNumber: [
                    {
                        validator: function (rule, value, callback) {
                            value = $api.trim(value);
                            if (value == '') return callback();
                            try {
                                if (vapp.IdentityCodeValid(value)) return callback();
                            } catch (err) {
                                callback(new Error(err));
                            }
                        }, trigger: ['blur', 'change']
                    }
                ],
                Ac_Email: [
                    { type: 'email', message: '请输入正确的邮箱地址', trigger: ['blur', 'change'] }
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
            ).then(([account, platinfo, organ]) => {
                //获取结果
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            }).catch(err => console.error(err));
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
                    if ($api.isnull(nv)) return;
                    this.sortquery.orgid = nv.Org_ID;
                    this.sortpaper(1);
                }, immediate: true
            },
        },
        methods: {
            //分页获取学员组
            sortpaper: function (index) {
                if (index != null) this.sortquery.index = index;
                var th = this;
                th.sort.loading = true;
                $api.get("Account/SortPager", th.sortquery).then(function (d) {
                    if (d.data.success) {
                        th.sorts = d.data.result;
                        th.sort.paper = Number(d.data.totalpages);
                        th.sort.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.sort.loading = false);
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
            //支持地址编码、出生日期、校验位验证
            IdentityCodeValid: function (code) {
                let city = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江 ", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北 ", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏 ", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外 " };
                let tip = "";
                let pass = true;
                //验证身份证格式（6个地区编码，8位出生日期，3位顺序号，1位校验位）
                if (!code || !/^\d{6}(18|19|20)?\d{2}(0[1-9]|1[012])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/i.test(code)) {
                    tip = "身份证号格式错误";
                    pass = false;
                }
                else if (!city[code.substr(0, 2)]) {
                    tip = "地址编码错误";
                    pass = false;
                }
                else {
                    //18位身份证需要验证最后一位校验位
                    if (code.length == 18) {
                        code = code.split('');
                        //∑(ai×Wi)(mod 11)
                        //加权因子
                        let factor = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2];
                        //校验位
                        let parity = [1, 0, 'X', 9, 8, 7, 6, 5, 4, 3, 2];
                        let sum = 0, ai = 0, wi = 0;
                        for (let i = 0; i < 17; i++) {
                            ai = code[i];
                            wi = factor[i];
                            sum += ai * wi;
                        }
                        let last = parity[sum % 11];
                        if (parity[sum % 11] != code[17]) {
                            tip = "校验位错误";
                            pass = false;
                        }
                    }
                }
                if (!pass) throw tip;
                return pass;
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
