$ready(function () {
    window.vue = new Vue({
        el: '#app',
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
            datas: [],      //卡号列表数据
            codesetArr: [],      //学习卡主题的数组（当前数据页）
            current: {},     //查看当前行
            codeset: {},     //当前充值码主题
            currentVisible: false,       //是否显示账号详情
            codesetVisible: false,       //学习卡主题详情的显示
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
                $api.get('RechargeCode/CodePager', this.form).then(function (req) {
                    if (req.data.success) {
                        th.datas = req.data.result;
                        th.totalpages = Number(req.data.totalpages);
                        th.total = req.data.total;
                        th.loading = false;
                        th.getcodeset4Pager(req.data.result);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //获取当前数据页的学习卡主题信息，可能是多个
            getcodeset4Pager: function (cards) {
                //当前数据页的所有主题id
                var setarr = [];
                for (let i = 0; i < cards.length; i++) {
                    const card = cards[i];
                    let isexist = false;
                    for (let j = 0; j < setarr.length; j++) {
                        if (card.Rs_ID == setarr[j]) {
                            isexist = true;
                            break;
                        }
                    }
                    if (!isexist) setarr.push(card.Rs_ID);
                }
                //获取主题信息
                this.codesetArr = [];
                for (let i = 0; i < setarr.length; i++) {
                    $api.cache('RechargeCode/SetForID', { 'id': setarr[i] }).then(function (req) {
                        if (req.data.success) {
                            vue.codesetArr.push(req.data.result);
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
            //显示主题
            getTheme: function (rs_id) {
                let set = {};
                for (let i = 0; i < this.codesetArr.length; i++) {
                    if (this.codesetArr[i].Rs_ID == rs_id) {
                        set = this.codesetArr[i];
                        break;
                    }
                }
                return set;
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
                let codeset = this.getTheme(row.Rs_ID);
                var title = "充值码：" + row.Rc_Code + " - " + row.Rc_Pw;
                var txt = title;
                txt += "\r\n有效时间：" + codeset.Rs_LimitStart.format("yyyy-MM-dd") + " 至 " + codeset.Rs_LimitEnd.format("yyyy-MM-dd");
                txt += "\r\n面　　额：" + codeset.Rs_Price + "元";
                this.copy(txt, title, 'textarea');

            },
            //复制到粘贴板
            copy: function (val, title) {
                navigator.clipboard.writeText(val)
                    .then(() => {
                        vue.$message({
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
                       throw req.data.message;
                    }
                }).catch(function (err) {
                    vue.$alert(err, '提示', {
                        confirmButtonText: '确定',
                        callback: action => {}
                    });
                    console.error(err);
                });
            },
            //更改启用禁用
            changeEnable: function (row) {
                var th = this;
                this.loadingid = row.Rc_ID;
                var para = { 'code': row.Rc_Code, 'pw': row.Rc_Pw, 'isenable': row.Rc_IsEnable };
                $api.post('RechargeCode/CodeChangeEnable', para).then(function (req) {
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
        },
        components: {
            'codeset': {
                props: ['theme'],
                methods: {
                    showtheme: function () {
                        var th = this;
                        vue.codesetVisible = true;
                        vue.codeset = th.theme;
                        //当前主题的数据
                        $api.get('RechargeCode/SetDataInfo', { 'id': this.theme.Rs_ID }).then(function (req) {
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
                    }
                },
                template: '<template>\
                     <el-tooltip class="item" effect="light" content="点击查看详情" placement="right">\
                        <el-link @click="showtheme()">{{theme.Rs_Theme }}</el - link >\
                     </el-tooltip>\
                </template > '
            }
        }
    });

});