$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('id'),
            org: {},
            config: {},      //当前机构配置项

            form: {
                'orgid': '', 'sbjid': '', 'couid': $api.querystring('id'),
                'search': '', 'isuse': '', 'diff': '', 'size': 20, 'index': 1
            },

            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            drawer: false,
            curr: {},        //当前要查看的试卷

            loading: false,
            loadingid: 0,
            loading_init: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (org) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果             
                th.org = org.data.result;
                //机构配置信息
                th.config = $api.organ(th.org).config;
                th.form.orgid = th.org.Org_ID;
                th.handleCurrentChange(1);
            })).catch(function (err) {
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
                th.form.size = Math.floor(area / 64);
                th.loading = true;
                var loading = this.showloading();
                $api.put("TestPaper/Pager", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        var result = d.data.result;
                        th.datas = result;
                        th.$nextTick(function () {
                            loading.close();
                        });
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    th.loading = false;
                    console.error(err);
                });
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                var loading = this.showloading();
                $api.delete('TestPaper/Delete', { 'id': datas }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                        th.$nextTick(function () {
                            loading.close();
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    console.error(err);
                });
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                //console.log(row);
                this.$refs.btngroup.modify(row[this.$refs.btngroup.idkey]);
            },
            //更改使用状态
            changeState: function (row) {
                var th = this;
                this.loadingid = row.Tp_Id;
                $api.post('TestPaper/ModifyState', { 'id': row.Tp_Id, 'use': row.Tp_IsUse, 'rec': row.Tp_IsRec }).then(function (req) {
                    this.loadingid = -1;
                    if (req.data.success) {
                        vapp.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    vapp.$alert(err, '错误');
                    th.loadingid = 0;
                });
            },
            //批量修改状态
            batchState: function (use) {
                use = Boolean(use);
                var th = this;
                this.$confirm('批量更改当前页面的试卷为“' + (use ? '启用' : '禁用') + '”, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    var ids = '';
                    for (var i = 0; i < th.datas.length; i++) {
                        ids += th.datas[i].Tp_Id;
                        if (i < th.datas.length - 1) ids += ',';
                    }
                    var loading = this.showloading();
                    $api.post('TestPaper/ModifyState', { 'id': ids, 'use': use, 'rec': null }).then(function (req) {
                        if (req.data.success) {
                            th.$notify({
                                type: 'success',
                                message: '修改状态成功!',
                                center: true
                            });
                            th.handleCurrentChange();
                            th.$nextTick(function () {
                                loading.close();
                            });
                        } else {
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        th.$alert(err, '错误');
                    });
                }).catch(() => {

                });
            },
            //显示全屏Loading
            showloading: function () {
                return this.$loading({
                    lock: true,
                    text: 'Loading',
                    spinner: 'el-icon-loading',
                    background: 'rgba(255, 255, 255, 0.3)'
                });
            },
            btnadd: function (btn, ctr) {
                let couid = $api.querystring('id');
                var url = $api.url.set(ctr.path, 'couid', couid);
                console.log(url);
                ctr.add(url);
            },
            //查看成绩
            viewResults: function (tpid) {
                var url = $api.url.set('../TestPaper/Results', 'tpid', tpid);
                this.$refs.btngroup.pagebox(url, '成绩管理', null, 800, 600, { 'ico': 'e696' });
            }
        }
    });

});
