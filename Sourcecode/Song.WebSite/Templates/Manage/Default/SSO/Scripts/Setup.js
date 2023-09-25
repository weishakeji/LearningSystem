$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            datas: [],

            search: '',      //按名称检索
            selects: [],  //数据表中选中的行
            loading: false,
            loadingid: 0
        },
        mounted: function () {
            this.loadDatas();
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //加载数据页
            loadDatas: function (index) {
                var th = this;
                th.loading = true;
                $api.get("Sso/All", { 'use': '' }).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //更改使用状态
            changeUse: function (row) {
                var th = this;
                th.loadingid = row.SSO_ID;
                $api.post('Sso/Modify', { 'entity': row }).then(function (req) {
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
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loadingdel = true;
                $api.delete('Sso/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = Number(req.data.result);
                        if (result > 0) {
                            th.$notify({
                                type: 'success',
                                message: '成功删除' + result + '条数据',
                                center: true
                            });
                            th.loadDatas();
                        }
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => alert(err, '错误'))
                    .finally(() => th.loadingdel = false);
            },
            copytext: function (txt) {
                var th = this;
                th.copy(txt, 'textarea').then(function (data) {
                    data.$message({
                        message: '复制 “' + txt + '” 到粘贴板',
                        type: 'success'
                    });
                });
            }
        }
    });

});
