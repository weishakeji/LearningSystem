
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            entity: {},     //当前数据对象
            account: {},           //当前账号
            isSuper: false,      //当前登录管理员，是否为超管
            organ: {},       //当前机构
            pay: {},         //支付接口
            fromType: ['管理员充扣', '充值码充值', '在线支付', '购买课程']
        },
        watch: {
            'loading': function (val, old) {
                console.log('loading:' + val);
            }
        },
        created: function () {
            var th = this;
            this.getEntity();
            //当前是否为超管登录
            $api.get('Admin/IsSuper').then(function (req) {
                if (req.data.success) {
                    th.isSuper = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        methods: {
            //获取数据实体
            getEntity: function () {
                var th = this;
                $api.get('Money/ForID', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        th.entity = req.data.result;
                        $api.bat(
                            $api.get('Account/ForID', { 'id': th.entity.Ac_ID }),
                            $api.get('Organization/ForID', { 'id': th.entity.Org_ID })
                        ).then(axios.spread(function (account, organ) {
                            //获取结果
                            th.account = account.data.result;
                            th.organ = organ.data.result;
                        })).catch(function (err) {
                            console.error(err);
                        });
                        //如果是在线支付
                        if (th.entity.Ma_From == 3) {
                            $api.get('Pay/ForID', { 'id': th.entity.Pai_ID }).then(function (req) {
                                if (req.data.success) {
                                    th.pay = req.data.result;
                                } else {
                                    console.error(req.data.exception);
                                    throw req.data.message;
                                }
                            }).catch(function (err) {
                                alert(err);
                                console.error(err);
                            });
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //来源
            getFrom: function (from) {
                return this.fromType[from - 1];
            },
            //确认金额的按钮事件
            btnConfirm: function () {
                var th = this;
                var msg = '在线充值扣款成功，但该平台内没有增加金额时，<br/>\
                        此处操作可以确认金额。<br/>\
                        请认真核对流水号，确认该订单在支付平台已经成功。<br/>\
                        流水号：'+ this.entity.Ma_Serial;
                this.$confirm(msg, '提示', {
                    dangerouslyUseHTMLString: true,
                    confirmButtonText: '经核对，确实充值成功',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    var txt = '确认订单成功后，该学员(' + this.entity.Ac_Name + ')的资金账户将增加' + this.entity.Ma_Money + '元。<br/>';
                    txt += '是否确认？<br/>\
                        流水号：'+ this.entity.Ma_Serial;
                    th.$confirm(txt, '再次提示', {
                        dangerouslyUseHTMLString: true,
                        confirmButtonText: '确认当前订单为成功',
                        cancelButtonText: '取消',
                        type: 'warning'
                    }).then(() => {
                        th.funcConfirm();
                    }).catch(() => { });
                }).catch(() => { });
            },
            //确认金额的具体方法
            funcConfirm: function () {
                var th = this;
                tthhis.loading = true;
                $api.post('Money/ConfirmSerial', { 'serial': th.entity.Ma_Serial }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '操作成功!'
                        });
                        th.getEntity();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    alert(err);
                    console.error(err);
                });
            },
            //复制到粘贴板
            copytext: function (val, title) {
                this.copy(val, 'textarea').then(function (th) {
                    th.$message({
                        message: '复制 “' + title + '” 到粘贴板',
                        type: 'success'
                    });
                });
            },
        }
    });
});
