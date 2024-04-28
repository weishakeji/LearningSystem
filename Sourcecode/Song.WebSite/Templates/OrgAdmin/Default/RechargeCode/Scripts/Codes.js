$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                rsid: '',
                isused: false,
                isdisable: false,
                code: '',
                account: '',
                size: 20,
                index: 1
            },
            codeset: {},     //充值卡的设置项
            datas: [],      //卡号列表数据
            current: {},     //查看当前行
            currentVisible: false,       //是否显示账号详情
            cardsetVisible: false,       //是否显示设置项
            codesetLoading: false,       //是否在加载设置信息
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
            th.form.rsid = $api.querystring('id');
            th.codesetLoading = true;
            $api.get('RechargeCode/SetForID', { 'id': th.form.rsid }).then(function (req) {
                if (req.data.success) {
                    th.codeset = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() => th.codesetLoading = false);
            this.handleCurrentChange();
        },
        methods: {
            //获取学习卡数据统计信息
            getdatainfo: function () {
                var th = this;
                $api.get('RechargeCode/SetDataInfo', { 'id': th.form.rsid }).then(function (req) {
                    if (req.data.success) {
                        th.num = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => { });
            },
            //加载数据页
            handleCurrentChange: function (index) {
                this.loading = true;
                var th = this;
                if (index != null) this.form.index = index;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 41);
                $api.get('RechargeCode/CodePager', this.form).then(function (req) {
                    if (req.data.success) {
                        th.datas = req.data.result;
                        th.totalpages = Number(req.data.totalpages);
                        th.total = req.data.total;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                var title = "充值码：" + row.Rc_Code + " - " + row.Rc_Pw;
                var txt = title;
                txt += "\r\n有效时间：" + this.codeset.Rs_LimitStart.format("yyyy-MM-dd") + " 至 " + this.codeset.Rs_LimitEnd.format("yyyy-MM-dd");
                txt += "\r\n面　　额：" + this.codeset.Rs_Price + "元";
                this.copy(txt, 'textarea').then(function (th) {
                    th.$message({
                        message: '复制 “' + title + '” 到粘贴板',
                        type: 'success'
                    });
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
                    console.error(err);
                    th.$alert(err, '提示', {
                        confirmButtonText: '确定',
                        callback: action => { }
                    });
                }).finally(() => { });
            },
            //导出二维码
            OutputQrcode: function () {
                var file = 'OutputQrcode';
                //文件路径
                var url = window.location.href;
                url = url.substring(0, url.lastIndexOf("/") + 1) + file;
                var boxid = "RechargeCode_" + this.form.rsid + "_" + file;
                //创建
                var box = window.top.$pagebox.create({
                    width: 800,
                    height: 400,
                    resize: true,
                    full: true,
                    id: boxid,
                    pid: window.name,
                    url: url + '?id=' + $api.querystring('id')
                });
                //parent.full = true;
                box.title = '充值码“' + this.codeset.Rs_Theme + "”导出二维码";
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
                var boxid = "RechargeCode_" + this.form.rsid + "_" + file;
                //创建
                var box = window.top.$pagebox.create({
                    width: 600,
                    height: 400,
                    resize: true,
                    id: boxid,
                    pid: window.name,
                    url: url + '?id=' + $api.querystring('id')
                });
                box.title = '学习卡“' + this.codeset.Rs_Theme + "”导出Excel";
                box.open();
            },

            //更改启用禁用
            changeEnable: function (row) {
                var th = this;
                th.loadingid = row.Rc_ID;
                var para = { 'code': row.Rc_Code, 'pw': row.Rc_Pw, 'isenable': row.Rc_IsEnable };
                $api.post('RechargeCode/CodeChangeEnable', para).then(function (req) {
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
                }).catch(function (err) {
                    alert(err, '错误');
                }).finally(() => th.loadingid = 0);
            }
        }
    });

});