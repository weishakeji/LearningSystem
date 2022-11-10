$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                'acid': '', 'isused': false, 'isback': false, 'isdisable': false, 'code': '',
                'index': 1, 'size': 20
            },
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数

            useCardShow: false,      //使用学习卡的面板是否打开
            useCardForm: {
                card: ''
            },
            useCardRules: {
                'card': [
                    { required: true, message: '不得为空', trigger: ["blur", "change"] },
                    {
                        validator: function (rule, value, callback) {
                            if (value.indexOf('-') < 0) {
                                callback(new Error('格式为“学习卡-密码”，破折号不可缺少'));
                            } else {
                                var arr = value.split('-');
                                if (!(/^\d+(\.\d+)?$/.test($api.trim(arr[0])))) {
                                    callback(new Error('卡号必须为数字'));
                                }
                                if (arr.length > 1) {
                                    if (!(/^\d+(\.\d+)?$/.test($api.trim(arr[1])))) {
                                        callback(new Error('密码必须为数字'));
                                    }
                                }
                            }
                            return callback();

                        }, trigger: ["blur", "change"]
                    }
                ]
            },
            account: {},
            totalCard: 0,           //学员所有的学习卡

            detail: {},       //详情查看
            detailShow: false,      //是否显示详情

            loading_init: true,
            loading: false,
            loading_up: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.get('Account/Current').then(function (req) {
                th.loading_init = false;
                if (req.data.success) {
                    th.account = req.data.result;
                    th.form.acid = th.account.Ac_ID;
                    th.getTatolCardCount();
                    th.handleCurrentChange(1);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 51);
                th.loading = true;
                $api.get("Learningcard/AccountCards", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        var result = d.data.result;
                        for (let i = 0; i < result.length; i++) {
                            result[i]['count'] = 0;
                        }
                        th.datas =result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        //console.log(th.accounts);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            //获取学员全部所有学习卡数量，不按条件判断，只要有就计算
            getTatolCardCount: function () {
                var th = this;
                $api.get('Learningcard/AccountOfCount', { 'acid': th.account.Ac_ID }).then(function (req) {
                    if (req.data.success) {
                        th.totalCard = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {

                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //在列中显示信息，包含检索
            showInfo: function (txt) {
                if (txt != '' && this.form.code != '') {
                    var regExp = new RegExp(this.form.code, 'g');
                    txt = txt.replace(regExp, `<red>${this.form.code}</red>`);
                }
                return txt;
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                var cardset = row.cardset;
                $api.get('Learningcard/SetCourses', { 'id': row.Lcs_ID }).then(function (req) {
                    var courses = req.data.success ? req.data.result : [];
                    var title = "学习卡号：" + row.Lc_Code + " - " + row.Lc_Pw;
                    var txt = title;
                    txt += "\r\n有效时间：" + cardset.Lcs_LimitStart.format("yyyy-MM-dd") + " 至 " + cardset.Lcs_LimitEnd.format("yyyy-MM-dd");
                    txt += "\r\n学习时长：" + cardset.Lcs_Span + cardset.Lcs_Unit;
                    txt += "\r\n面　　额：" + cardset.Lcs_Price + "元";
                    txt += "\r\n课　　程：（" + courses.length + "）";
                    for (var i = 0; i < courses.length; i++) {
                        var cour = courses[i];
                        txt += "\r\n　　　　　" + (i + 1) + "." + cour.Cou_Name;
                    }
                    vapp.copy(txt, title);

                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });

            },
            //复制到粘贴板
            copy: function (val, title) {
                navigator.clipboard.writeText(val)
                    .then(() => {
                        vapp.$message({
                            message: '复制 “' + title + '” 到粘贴板',
                            type: 'success'
                        });
                    })
                    .catch(err => {
                        if (textbox == null) textbox = 'input';
                        var oInput = document.createElement('textarea');
                        document.body.appendChild(oInput);
                        oInput.value = val;
                        oInput.select(); // 选择对象
                        document.execCommand("Copy"); // 执行浏览器复制命令           
                        oInput.style.display = 'none';
                    });

            },
            useCard: function (formName, func) {
                var th = this;
                var obj = eval('this.' + func);
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        obj(th.useCardForm.card);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //使用学习卡
            usecode: function (code) {
                var th = this;
                th.loading_up = true;
                $api.get('Learningcard/UseCode', { 'code': code }).then(function (req) {
                    th.loading_up = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$alert('通过使用该学习卡，您成功选修 ' + result.length + ' 门课程。', '操作成功', {
                            confirmButtonText: '确定',
                            callback: action => {
                                th.handleCurrentChange();
                                th.useCardForm.card = '';
                                th.useCardShow = false;
                            }
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading_up = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //暂存学习卡
            acceptcode: function (code) {
                var th = this;
                th.loading_up = true;
                $api.get('Learningcard/AcceptCode', { 'code': code }).then(function (req) {
                    th.loading_up = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$alert('操作成功，学习卡被暂存在名下，后续可以在合适时间使用它。', '操作成功', {
                            confirmButtonText: '确定',
                            callback: action => {
                                th.handleCurrentChange();
                                th.useCardForm.card = '';
                                th.useCardShow = false;
                            }
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading_up = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //立即使用
            usenow: function (row) {
                this.useCardForm.card = row.Lc_Code + '-' + row.Lc_Pw;
                this.useCardShow = true;
            },
            //是否临近过期，离过期七天以内
            nearexpire: function (c) {
                var time = new Date().setDate((new Date().getDate() - 7));
                var end = c.Lc_LimitEnd;
                return end > time && end < new Date();
            },
            //是否过期
            expire: function (c) {
                return c.Lc_LimitEnd < new Date();
            },
            //学习卡使用结束时间
            useendtime: function (c) {
                var time = new Date(c.Lc_UsedTime.getTime());
                time = time.setDate(time.getDate() + c.Lc_Span);
                return time;
            },
        }
    });
    //获取卡号的设置项
    Vue.component('cardset', {
        props: ['card'],
        data: function () {
            return {
                cardset: {},
                courses: [],     //学习卡的关联课程
                loading: false
            }
        },
        watch: {
            'card': {
                handler: function (nv, ov) {
                    this.getcardset();
                    this.getcourses();
                }, deep: true, immediate: true
            }
        },
        computed: {
        },
        mounted: function () { },
        methods: {
            //获取卡片设置项
            getcardset: function () {
                if (this.card == null) return;
                var th = this;
                $api.get('Learningcard/SetForID', { 'id': th.card.Lcs_ID }).then(function (req) {
                    th.loading++;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.cardset = result;
                        th.card['cardset'] = result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading++;
                    console.error(err);
                });
            },
            //获取关联的课程
            getcourses: function () {
                if (this.card == null) return;
                var th = this;
                $api.get('Learningcard/SetCourses', { 'id': th.card.Lcs_ID }).then(function (req) {
                    th.loading++;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.courses = result;
                        th.card['courses'] = result;
                        th.card['count'] = result.length;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading++;
                    console.error(err);
                });
            }
        },
        // 同样也可以在 vm 实例中像 "this.message" 这样使用
        template: `<span class="theme">
           {{cardset.Lcs_Theme}}          
        </span>`
    });
});
