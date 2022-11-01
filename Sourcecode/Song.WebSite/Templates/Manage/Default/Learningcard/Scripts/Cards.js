$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            form: {
                lsid: '',
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
            current: {},     //查看当前行
            currentVisible: false,       //是否显示账号详情
            cardsetVisible: false,       //是否显示设置项
            cardsetLoading: false,       //是否在加载设置信息
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
            'cardsetVisible': function (nl, vl) {
                if (nl) this.getdatainfo();
            }
        },
        computed: {

        },
        created: function () {
            var th = this;
            this.form.lsid = $api.querystring('id');
            th.cardsetLoading = true;
            $api.get('Learningcard/SetForID', { 'id': th.form.lsid }).then(function (req) {
                th.cardsetLoading = false;
                if (req.data.success) {
                    th.cardset = req.data.result;
                    th.cardset.courses = [];
                    $api.get('Learningcard/SetCourses', { 'id': vue.form.lsid }).then(function (req) {
                        if (req.data.success) {
                            th.cardset.courses = [];
                            th.cardset['courses'] = req.data.result;                           
                        }
                    }).catch(function (err) {
                        th.$alert(err);
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.$alert(err);
                th.cardsetLoading = false;
                console.error(err);
            });
            this.handleCurrentChange();
        },
        methods: {
            //获取学习卡数据统计信息
            getdatainfo: function () {
                $api.get('Learningcard/SetDataInfo', { 'id': this.form.lsid }).then(function (req) {
                    if (req.data.success) {
                        vue.num = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
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
                        th.loading = false;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //在列中显示信息，包含检索
            showInfo: function (txt) {
                if (txt != '' && this.form.card != '') {
                    var regExp = new RegExp(this.form.card, 'g');
                    txt = txt.replace(regExp, `<red>${this.form.card}</red>`);
                }
                return txt;
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                var title = "学习卡号：" + row.Lc_Code + " - " + row.Lc_Pw;
                var txt = title;
                txt += "\r\n有效时间：" + this.cardset.Lcs_LimitStart.format("yyyy-MM-dd") + " 至 " + this.cardset.Lcs_LimitEnd.format("yyyy-MM-dd");
                txt += "\r\n学习时长：" + this.cardset.Lcs_Span + this.cardset.Lcs_Unit;
                txt += "\r\n面　　额：" + this.cardset.Lcs_Price + "元";
                txt += "\r\n课　　程：（" + this.cardset.courses.length + "）";
                for (var i = 0; i < this.cardset.courses.length; i++) {
                    var cour = this.cardset.courses[i];
                    txt += "\r\n　　　　　" + (i + 1) + "." + cour.Cou_Name;
                }
                this.copy(txt, title, 'textarea');
            },
            //复制到粘贴板
            copy: function (val, title, textbox) {
                if (textbox == null) textbox = 'input';
                var oInput = document.createElement(textbox);
                oInput.value = val;
                document.body.appendChild(oInput);
                oInput.select(); // 选择对象
                document.execCommand("Copy"); // 执行浏览器复制命令           
                oInput.style.display = 'none';
                this.$message({
                    message: '复制 “' + title + '” 到粘贴板',
                    type: 'success'
                });
            },
            //显示激活学习卡的账号的信息
            acccountInfo: function (row) {
                this.current = row;
                if (this.current.account) {
                    this.currentVisible = true;
                    return;
                }
                var acc = row.Ac_AccName;
                $api.get('Account/ForAcc', { 'acc': acc }).then(function (req) {
                    if (req.data.success) {
                        row.account = req.data.result;
                        vue.currentVisible = true;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //导出二维码
            OutputQrcode: function () {
                var file = 'OutputQrcode';
                //文件路径
                var url = window.location.href;
                url = url.substring(0, url.lastIndexOf("/") + 1) + file;
                var boxid = "Learningcard_" + this.form.lsid + "_" + file;
                //创建
                var box = window.top.$pagebox.create({
                    width: 800,
                    height: 400,
                    resize: true,
                    full: true,
                    id: boxid,
                    pid: window.name,
                    ico: 'a053',
                    url: url + '?id=' + $api.querystring('id')
                });
                //parent.full = true;
                box.title = '学习卡“' + this.cardset.Lcs_Theme + "”导出二维码";
                box.open();
                window.setTimeout(function () {
                    box.full = true;
                }, 1000);
            },
            //导出excel，打开窗口
            OutputExcel: function () {
                var file = 'OutputExcel';
                //文件路径
                var url = window.location.href;
                url = url.substring(0, url.lastIndexOf("/") + 1) + file;
                var boxid = "Learningcard_" + this.form.lsid + "_" + file;
                //创建
                var box = window.top.$pagebox.create({
                    width: 600,
                    height: 400,
                    resize: true,
                    id: boxid,
                    pid: window.name,
                    ico: 'e60f',
                    url: url + '?id=' + $api.querystring('id')
                });
                box.title = '学习卡“' + this.cardset.Lcs_Theme + "”导出Excel";
                box.open();
            },
            //回滚学习卡
            goback: function (row) {
                if (row) {
                    //单个回滚
                    this.$confirm('请选择回滚方式?', '确认', {
                        distinguishCancelAndClose: true,
                        confirmButtonText: '回滚，且清除学习记录',
                        cancelButtonText: '回滚，但保留学习记录',
                        type: 'warning'
                    }).then(() => {
                        this.gobackFunc(row, true);
                    }).catch(action => {
                        if (action == 'cancel')
                            this.gobackFunc(row, false);
                    });
                } else {
                    //批量回滚
                    if (this.selects.length < 1) {
                        this.$message({
                            type: 'error',
                            message: '没有选择要回滚的学习卡'
                        });
                    } else {
                        //可以回滚的数量（已经使用学习卡可以回滚，未使用的不用回滚）
                        var num = 0;
                        for (var i = 0; i < vue.selects.length; i++) {
                            if (vue.selects[i].Lc_IsUsed && vue.selects[i].Lc_State != -1) { num++; }
                        }
                        if (num == 0) {
                            this.$message({
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
                            for (var i = 0; i < vue.selects.length; i++) {
                                if (vue.selects[i].Lc_IsUsed)
                                    this.gobackFunc(vue.selects[i], true);
                            }
                        }).catch(action => {
                            if (action == 'cancel') {
                                for (var i = 0; i < vue.selects.length; i++) {
                                    if (vue.selects[i].Lc_IsUsed)
                                        this.gobackFunc(vue.selects[i], false);
                                }
                            }
                        });
                    }
                }
            },
            //回滚学习的具体方法
            gobackFunc: function (row, isclear) {
                row.Lc_State = -100;
                $api.post('Learningcard/CardRollback', { 'code': row.Lc_Code, 'pw': row.Lc_Pw, 'clear': isclear }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        for (var i = 0; i < vue.datas.length; i++) {
                            if (result.Lc_ID == vue.datas[i].Lc_ID) {
                                vue.$set(vue.datas, i, result);
                            }
                        }
                        vue.$message({
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
                });
            },
            //更改启用禁用
            changeEnable: function (row) {
                var th = this;
                this.loadingid = row.Lc_ID;
                var para = { 'code': row.Lc_Code, 'pw': row.Lc_Pw, 'isenable': row.Lc_IsEnable };
                $api.post('Learningcard/CardChangeEnable', para).then(function (req) {
                    if (req.data.success) {
                        vue.$notify({
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
                    vue.$alert(err, '错误');
                });
            }
        }
    });

});