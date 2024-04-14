$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                lsid: 0,
                isused: false,
                isback: false,
                isdisable: false,
                card: '',
                account: '',
                size: 20,
                index: 1
            },
            cardset: { 'courses': [] },     //学习卡的设置项
            datas: [],      //卡号列表数据
            cardsetArr: [],      //学习卡主题的数组（当前数据页）
            current: {},     //查看当前行

            currentVisible: false,       //是否显示账号详情
            cardsetVisible: false,       //学习卡主题详情的显示
            loading: false,
            loadingid: false,
            selects: [], //数据表中选中的行   
            total: 1, //总记录数
            totalpages: 1, //总页数      
            num: {
                total: 0,
                used: 0,     //已经使用的个数
                rollbak: 0,      //回滚的个数
                disable: 0       //禁用个数
            }
        },
        watch: {

        },
        computed: {

        },
        created: function () {
            this.handleCurrentChange();
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                this.loading = true;
                var th = this;
                if (index != null) this.form.index = index;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 41);
                $api.get('Learningcard/CardPager', this.form).then(function (req) {
                    if (req.data.success) {
                        th.datas = req.data.result;
                        th.totalpages = Number(req.data.totalpages);
                        th.total = req.data.total;
                        th.getCardset4Pager(req.data.result);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //获取当前数据页的学习卡主题信息，可能是多个
            getCardset4Pager: function (cards) {
                //当前数据页的所有主题id
                var setarr = [];
                for (let i = 0; i < cards.length; i++) {
                    const card = cards[i];
                    let isexist = false;
                    for (let j = 0; j < setarr.length; j++) {
                        if (card.Lcs_ID == setarr[j]) {
                            isexist = true;
                            break;
                        }
                    }
                    if (!isexist) setarr.push(card.Lcs_ID);
                }
                //获取主题信息
                this.cardsetArr = [];
                var th = this;
                for (let i = 0; i < setarr.length; i++) {
                    $api.cache('Learningcard/SetForID', { 'id': setarr[i] }).then(function (req) {
                        if (req.data.success) {
                            th.cardsetArr.push(req.data.result);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    }).finally(() => { });
                }
            },
            //显示主题
            getTheme: function (lcs_id) {
                let set = {};
                for (let i = 0; i < this.cardsetArr.length; i++) {
                    if (this.cardsetArr[i].Lcs_ID == lcs_id) {
                        set = this.cardsetArr[i];
                        break;
                    }
                }
                return set;
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                let cardset = this.getTheme(row.Lcs_ID);
                var th = this;
                $api.get('Learningcard/SetCourses', { 'id': cardset.Lcs_ID }).then(function (req) {
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
                    th.copy(txt, 'textarea').then(function (data) {
                        th.$message({
                            message: '复制 “' + title + '” 到粘贴板',
                            type: 'success'
                        });
                    });

                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });

            },
            //显示激活学习卡的账号的信息
            acccountInfo: function (row) {
                this.current = row;
                if (this.current.account) {
                    this.currentVisible = true;
                    return;
                }
                var th = this;
                var acc = row.Ac_AccName;
                $api.get('Account/ForAcc', { 'acc': acc }).then(function (req) {
                    if (req.data.success) {
                        row.account = req.data.result;
                        th.currentVisible = true;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => { });
            },
            //回滚学习卡
            goback: function (row) {
                var th = this;
                if (row) {
                    //单个回滚
                    th.$confirm('请选择回滚方式?', '确认', {
                        distinguishCancelAndClose: true,
                        confirmButtonText: '回滚，且清除学习记录',
                        cancelButtonText: '回滚，但保留学习记录',
                        type: 'warning'
                    }).then(() => {
                        th.gobackFunc(row, true);
                    }).catch(action => {
                        if (action == 'cancel')
                            th.gobackFunc(row, false);
                    });
                } else {
                    //批量回滚
                    if (th.selects.length < 1) {
                        th.$message({
                            type: 'error',
                            message: '没有选择要回滚的学习卡'
                        });
                    } else {
                        //可以回滚的数量（已经使用学习卡可以回滚，未使用的不用回滚）
                        var num = 0;
                        for (var i = 0; i < th.selects.length; i++) {
                            if (th.selects[i].Lc_IsUsed && th.selects[i].Lc_State != -1) { num++; }
                        }
                        if (num == 0) {
                            th.$message({
                                type: 'error',
                                message: '您选中的学习卡，均未使用，不可以回滚'
                            });
                            return;
                        }
                        var msg = "";
                        if (this.selects.length > num) {
                            msg = "您选中了" + this.selects.length + "个学习卡，其中有" + (this.selects.length - num) + "个未使用或已经回滚。"
                            msg += "<br/>在可以回滚的" + num + "个学习卡中，请选择回滚方式。"
                        } else {
                            msg = "您选中了" + this.selects.length + "个学习卡，请选择回滚方式。"
                        }
                        this.$confirm(msg, '确认', {
                            distinguishCancelAndClose: true,
                            dangerouslyUseHTMLString: true,
                            confirmButtonText: '回滚，且清除学习记录',
                            cancelButtonText: '回滚，但保留学习记录',
                            type: 'warning'
                        }).then(() => {
                            for (var i = 0; i < th.selects.length; i++) {
                                if (th.selects[i].Lc_IsUsed)
                                    this.gobackFunc(th.selects[i], true);
                            }
                        }).catch(action => {
                            if (action == 'cancel') {
                                for (var i = 0; i < th.selects.length; i++) {
                                    if (th.selects[i].Lc_IsUsed)
                                        this.gobackFunc(th.selects[i], false);
                                }
                            }
                        });
                    }
                }
            },
            //回滚学习的具体方法
            gobackFunc: function (row, isclear) {
                row.Lc_State = -100;
                var th = this;
                $api.post('Learningcard/CardRollback', { 'code': row.Lc_Code, 'pw': row.Lc_Pw, 'clear': isclear }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        for (let i = 0; i < th.datas.length; i++) {
                            if (result.Lc_ID == th.datas[i].Lc_ID)
                                th.$set(th.datas, i, result);
                        }
                        th.$message({
                            type: 'success',
                            message: '回滚成功!'
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => { });
            },
            //更改启用禁用
            changeEnable: function (row) {
                var th = this;
                this.loadingid = row.Lc_ID;
                var para = { 'code': row.Lc_Code, 'pw': row.Lc_Pw, 'isenable': row.Lc_IsEnable };
                $api.post('Learningcard/CardChangeEnable', para).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            position: 'bottom-right',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    alert(err, '错误');
                }).finally(() => { });
            }
        },
        components: {
            'cardset': {
                props: ['theme'],
                methods: {
                    showtheme: function () {
                        var th = this;
                        var vapp = window.vapp;
                        vapp.cardsetVisible = true;
                        //当前主题的数据
                        $api.get('Learningcard/SetDataInfo', { 'id': th.theme.Lcs_ID }).then(function (req) {
                            if (req.data.success) {
                                vapp.num = req.data.result;
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        }).finally(() => { });
                        //当前主题的课程信息
                        vapp.cardset['courses'] = [];
                        $api.get('Learningcard/SetCourses', { 'id': th.theme.Lcs_ID }).then(function (req) {
                            if (req.data.success) {
                                th.theme['courses'] = req.data.result;
                            } else {
                                th.theme['courses'] = [];
                            }
                            vapp.cardset = th.theme;
                        }).catch(err => console.error(err))
                            .finally(() => { });
                    }
                },
                template: '<template>\
                     <el-tooltip class="item" effect="dark" content="点击查看详情" placement="bottom">\
                        <el-link @click="showtheme()">{{theme.Lcs_Theme }}</el - link >\
                     </el-tooltip>\
                </template > '
            }
        }
    });

});