$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                'orgid': '', 'sortid': '', 'use': null, 'acc': '', 'name': '', 'phone': '', 'idcard': '',
                size: 20, index: 1
            },
            querypanel: false,     //查询面板是否显示   
            organ: {},       //当前机构
            sorts: [],      //学员组             

            accounts: [], //账号列表
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading: true,
            loadingid: 0        //当前操作中的对象id
        },
        computed: {
        },
        watch: {
            form: {
                handler: function (nv, ov) {                   
                }, deep: true
            }
        },
        mounted: function () {
            this.$refs.btngroup.addbtn({
                text: '批量禁用', tips: '批量处理',
                id: 'batch', type: 'success',
                class: 'el-icon-magic-stick'
            });
        },
        created: function () {
            var th = this;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    th.organ = req.data.result;
                    th.form.orgid = th.organ.Org_ID;
                    th.getsorts();
                    th.handleCurrentChange(1);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });

        },

        methods: {
            //获取学员组
            getsorts: function (orgid) {
                var th = this;
                $api.get('Account/SortAll', { 'orgid': orgid, 'use': '' }).then(function (req) {
                    if (req.data.success) {
                        th.sorts = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //加载数据页
            handleCurrentChange: function (index) {               
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 67);
                th.loading = true;
                $api.get("Account/Pager", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        for (var i = 0; i < d.data.result.length; i++) {
                            d.data.result[i].isAdminPosi = false;
                        }
                        th.accounts = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        //console.log(th.accounts);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                $api.delete('Account/DeleteBatch', { 'ids': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //显示手机号
            showmobi: function (row) {
                var phone = row.Ac_MobiTel1;
                return phone != '' ? phone : row.Ac_MobiTel2;
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                this.$refs.btngroup.modifyrow(row);
            },
            //复制到粘贴板
            copytext: function (val, textbox) {
                this.copy(val, textbox).then(function (th) {
                    th.$message({
                        message: '复制 “' + val + '” 到粘贴板',
                        type: 'success'
                    });
                });
            },
            //更改使用状态，包括使用、审核
            changeState: function (row) {
                var th = this;
                this.loadingid = row.Ac_ID;
                var form = { 'acid': row.Ac_ID, 'use': row.Ac_IsUse, 'pass': row.Ac_IsPass };
                $api.post('Account/ModifyState', form).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    alert(err, '错误');
                });
            },
            //导出
            output: function (btn) {
                var title = btn.tips;
                this.$refs.btngroup.pagebox('Export', title, null, 640, 480);
            },
            //导入
            input: function (btn) {
                var title = btn.tips;
                this.$refs.btngroup.pagebox('Import', title, null, 800, 600);
            },
            //批量禁用
            batch_disable: function (btn) {
                var title = btn.tips;
                this.$refs.btngroup.pagebox('Batchdisable', title, null, '600px', '60%');
            },
            //打开子页面
            openSubpage: function (page, account, title, width, height, icon) {
                var title = '“' + account.Ac_Name + '(' + account.Ac_AccName + ')”的' + title;
                var param = { ico: icon };
                this.$refs.btngroup.pagebox(page + '?id=' + account.Ac_ID, title, null, width, height, param);
            }
        }
    });
});