$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                orgid: '',
                search: '',
                size: 20,
                index: 1
            },
            organs: [],
            loading: false,
            loadingid: 0,        //当前操作中的对象id
            datas: [],          //数据集
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [] //数据表中选中的行
        },
        mounted: function () {
            this.$nextTick(function () {

            });
            this.$refs.btngroup.addbtn({
                text: '卡号查询',
                tips: '按卡号查询所有',
                id: 'cardquery',
                type: 'info', enable: true,
                icon: 'e60f'
            });
        },
        created: function () {
            var th = this;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.form.orgid = result.Org_ID;
                    th.handleCurrentChange(1);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => { });
        },
        computed: {

        },
        methods: {
            //删除
            deleteData: function (datas) {
                var th = this;
                $api.delete('Learningcard/SetDelete', { 'id': datas }).then(function (req) {
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
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 105;
                th.form.size = Math.floor(area / 42);
                $api.get("Learningcard/SetPager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                }).finally(() => { });
            },
            //刷新行数据，
            freshrow: function (id) {
                if (id == null || id == '' || id == 0) return this.handleCurrentChange();
                if (this.datas.length < 1) return;
                //要刷新的行数据
                let entity = this.datas.find(item => item.Lcs_ID == id);
                if (entity == null) return;
                //获取最新数据，刷新
                var th = this;
                th.loadingid = id;
                $api.get('Learningcard/SetForID', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        let index = th.datas.findIndex(item => item.Lcs_ID == id);
                        if (index >= 0) th.$set(th.datas, index, result);
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
            },

            //更改使用状态
            changeUse: function (row) {
                var th = this;
                th.loadingid = row.Lcs_ID;
                $api.post('Learningcard/SetModify', { 'entity': row, 'scope': 1 }).then(function (req) {
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
            //打开子窗体
            childwindow: function (row, file, title, width, height, param) {
                if (row != null) title += ' -“' + row.Lcs_Theme + "”";
                var boxid = "Learningcard_" + (row != null ? row.Lcs_ID + "_" : "") + file;
                var href = file + (row != null ? '?id=' + row.Lcs_ID : "");
                var params = { 'pid': window.name };
                if (param != null) {
                    for (var i in param)
                        params[i] = param[i];
                }
                this.$refs.btngroup.pagebox(href, title, boxid, width, height, params);
            },
        }
    });

});