$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项 

            form: { 'orgid': 0, 'search': '', 'size': 20, 'index': 1 },

            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading: false,
            loading_init: true,
            loadingid: 0
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(([organ]) => {
                //获取结果             
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.form.orgid = th.organ.Org_ID;
                th.handleCurrentChange(1);
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
        },
        watch: {
            'selects': function (nv, ov) {
                console.log(nv);
            }
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 42);
                th.loading = true;
                $api.get("Notice/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //刷新行数据，
            freshrow: function (id) {
                if (id == null || id == '' || id == 0) return this.handleCurrentChange();
                if (this.datas.length < 1) return;
                //要刷新的行数据
                let entity = this.datas.find(item => item.No_Id == id);
                if (entity == null) return;
                //获取最新数据，刷新
                var th = this;
                th.loadingid = id;
                $api.get('Notice/ForID', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        let index = th.datas.findIndex(item => item.No_Id == id);
                        if (index >= 0) th.$set(th.datas, index, result);
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('Notice/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //更改使用状态
            changeState: function (row) {
                var th = this;
                th.loadingid = row.No_Id;
                $api.post('Notice/Modify', { 'entity': row }).then(function (req) {
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
                    th.$alert(err, '错误');
                }).finally(() => th.loadingid = 0);
            }
        }
    });

});
