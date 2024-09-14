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
            ).then(([org]) => {
                //获取结果             
                th.org = org.data.result;
                //机构配置信息
                th.config = $api.organ(th.org).config;
                th.form.orgid = th.org.Org_ID;
                th.handleCurrentChange(1);
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
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
                var loading = this.$fulloading();
                $api.put("TestPaper/Pager", th.form).then(function (d) {
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
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                var loading = this.$fulloading();
                $api.delete('TestPaper/Delete', { 'id': datas }).then(function (req) {
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
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                //console.log(row);
                this.$refs.btngroup.modify(row[this.$refs.btngroup.idkey], null, {
                    'ico': 'e810'
                });
            },
            //更改使用状态
            changeState: function (row) {
                var th = this;
                th.loadingid = row.Tp_Id;
                $api.post('TestPaper/ModifyState', { 'id': row.Tp_Id, 'use': row.Tp_IsUse, 'rec': row.Tp_IsRec }).then(function (req) {
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
                    var loading = this.$fulloading();
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
                        alert(err);
                    }).finally(() => { });
                }).catch(() => {

                });
            },
            btnadd: function (btn, ctr) {
                let couid = $api.querystring('id');
                var url = $api.url.set(ctr.path, 'couid', couid);
                //console.error(url);
                ctr.add(url, { 'ico': 'e810' });
            },
            //查看成绩
            viewResults: function (row) {
                var url = $api.url.set('../TestPaper/Results', 'tpid', row.Tp_Id);
                this.$refs.btngroup.pagebox(url, '《' + row.Tp_Name + '》的成绩', null, 1000, 600, { 'ico': 'e696' });
            }
        }
    });
    //参加考试的人次
    Vue.component('result_count', {
        props: ["tpid", "index"],
        data: function () {
            return {
                count:0,
                loading: false
            }
        },
        watch: {
            'tpid': {
                handler: function (nv, ov) {
                    if (!$api.isnull(nv)) {
                        if (this.index == 0) this.getData();
                    }
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () {            
        },
        methods: {
            //初始加载
            startInit: function () {
                //加载完成，则加载后一个组件，实现逐个加载的效果
                this.getPrices().finally(() => {
                    var vapp = window.vapp;
                    var ctr = vapp.$refs['result_count_' + (this.index + 1)];
                    if (ctr != null) ctr.startInit();
                });
            },
            //加载
            getData: function () {
                var th = this;
                return new Promise(function (res, rej) {                   
                    th.loading = true;
                    $api.get('TestPaper/ResultsOfCount', { 'tpid': th.tpid }).then(function (req) {
                        if (req.data.success) {
                            th.count = req.data.result;                           
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => {
                            th.loading = false;
                            return res();
                        });
                });

            },          
        },
        template: `<span class="result_count" v-if="count>0">
                ({{count}})
    </span> `
    });
});
