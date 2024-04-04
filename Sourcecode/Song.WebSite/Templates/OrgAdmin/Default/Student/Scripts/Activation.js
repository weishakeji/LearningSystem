$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: { 'orgid': '', 'stsid': '', 'acc': '', 'name': '', 'mobi': '', 'idcard': '', 'code': '', 'orderby': '', 'orderpattr': '', 'size': 20, 'index': 1 },
            rules: {
                name: [{ min: 1, max: 10, message: '长度限 1 到 10 个字符', trigger: 'blur' }],
                mobi: [
                    {
                        required: false, type: 'number', message: '请输入数字', trigger: 'change', transform(value) {
                            return Number(value)
                        }
                    }
                ]
            },
            organ: {},       //当前机构
            photopath: '',        //学员图片的路径
            sorts: [],      //学员组             

            accounts: [], //账号列表
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading: false,
            loading_init:true,
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
        },
        created: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current'),
                $api.get('Platform/UploadPath', { 'key': 'Student' })
            ).then(axios.spread(function (org, path) {
                th.organ = org.data.result;
                th.photopath = path.data.result.virtual;
                th.form.orgid = th.organ.Org_ID;
                th.getsorts();
                th.handleCurrentChange(1);
            })).catch(err => alert(err))
                .finally(() => th.loading_init = false);
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
                $api.cache("Student/Activation", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.accounts = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch((err) => {
                    alert(err);
                    console.error(err);
                }).finally(() => { });
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
                }).finally(() => { });
            },
            //显示手机号
            showmobi: function (row) {
                let phone = row.Ac_MobiTel1;
                return phone != '' ? phone : row.Ac_MobiTel2;
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
            //打开子页面
            openSubpage: function (page, account, title, width, height, icon) {
                var title = '“' + account.Ac_Name + '(' + account.Ac_AccName + ')”的' + title;
                var param = { ico: icon };
                this.$refs.btngroup.pagebox(page + '?id=' + account.Ac_ID, title, null, width, height, param);
            }
        },
        filters: {
            //时间离现在多久
            untilnow: function (value) {
                if (value == null || value == '') return '';
                let type = $api.getType(value);
                if (type != 'Date') return '';
                return '( ' + value.untilnow() + ' )';
            }
        }

    });
});