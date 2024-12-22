$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                'orgid': '', 'sortid': '', 'use': 'null', 'acc': '', 'name': '', 'phone': '', 'idcard': '',
                'gender': '-1', 'orderby': '', 'orderpattr': '',
                'size': 20, 'index': 1
            },
            rules: {
                name: [{ min: 1, max: 10, message: '长度限 1 到 10 个字符', trigger: 'blur' }],
                phone: [
                    {
                        required: false, type: 'number', message: '请输入数字', trigger: 'change', transform(value) {
                            return Number(value)
                        }
                    }
                ]
            },
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
                id: 'batch', type: 'warning',
                icon: 'e7ad'
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
            }).catch(err => alert(err))
                .finally(() => { });

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
                }).finally(() => { });
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
                    if (d.data.success) {
                        th.accounts = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        //console.log(th.accounts);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //当表格的排序条件发生变化的时候会触发该事件
            sortchange: function (column) {
                //排序方式
                if (column.order == null || column.order == '') {
                    this.form.orderby = '';
                    this.form.orderpattr = '';
                } else {
                    let order = column.order.substring(0, 3);
                    if (order != 'asc') order = 'desc';
                    this.form.orderpattr = order;
                    this.form.orderby = column.prop;
                }
                this.handleCurrentChange();
            },
            //刷新行数据，
            freshrow: function (id) {
                if (id == null || id == '' || id == 0) return this.handleCurrentChange();
                if (this.accounts.length < 1) return;
                //要刷新的行数据
                let entity = this.accounts.find(item => item.Ac_ID.toString() == id);
                if (entity == null) return;
                //获取最新数据，刷新
                var th = this;
                th.loadingid = id;
                $api.get('Account/ForID', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        let index = th.accounts.findIndex(item => item.Ac_ID.toString() == id);
                        if (index >= 0) th.$set(th.accounts, index, result);
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
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
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //显示手机号
            showmobi: function (row) {
                var phone = row.Ac_MobiTel1;
                return phone != '' ? phone : row.Ac_MobiTel2;
            },
            //时间是否为空
            timeisnull: function (time) {
                if (!(Object.prototype.toString.call(time) === '[object Date]'))
                    return true;
                if (time.getTime() == 0) return true;
                //if (time.format('yyyy-MM-dd') == '1970-01-01') return true;
                return false;
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

                }).catch(function (err) {
                    alert(err, '错误');
                }).finally(() => th.loadingid = 0);
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
            //资金流水
            capitalRecords: function (row) {
                var file = 'capitalRecords?id=' + row.Ac_ID;
                var title = '“' + row.Ac_Name + '(' + row.Ac_AccName + ')”的资金流水'
                var boxid = 'capitalRecords_' + file;
                this.$refs.btngroup.pagebox(file, title, boxid, 800, 600,
                    { pid: window.name, resize: true });
            },
            //打开子页面
            openSubpage: function (page, account, title, width, height, icon) {
                var title = '“' + account.Ac_Name + '(' + account.Ac_AccName + ')”的' + title;
                var param = { ico: icon };
                this.$refs.btngroup.pagebox(page + '?id=' + account.Ac_ID, title, null, width, height, param);
            }
        }
    });
    //学员的课程数
    Vue.component('course_count', {
        //acid:学员id
        props: ['acid'],
        data: function () {
            return {
                count: 0,
                loading: false
            }
        },
        watch: {
            'acid': {
                handler: function (nv, ov) {
                    if (!$api.isnull(nv)) this.getcount();
                }, immediate: true, deep: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {
            getcount: function () {
                var th = this;
                th.loading = true;
                $api.get('Account/CourseCount', { 'acid': th.acid }).then(function (req) {
                    if (req.data.success) {
                        th.count = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },

        },
        template: `<span :class="{ 'disabled': count<1 }">
            <loading asterisk v-if="loading">...</loading>
            <template v-else>
                <icon>&#xe813</icon>
                <template v-if="count>0">
                    课程 ({{count}})
                </template>
                <template v-else>
                    课程 (0)
                </template>
            </template>            
        </span>`
    });
});