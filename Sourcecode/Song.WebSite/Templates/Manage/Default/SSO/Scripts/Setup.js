$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {              
            datas: [],
            selects: [],  //数据表中选中的行
            loading: false
        },
        mounted: function () {
           
        },
        created: function () {

        },
        computed: {
           
        },
        watch: {
        },
        methods: {
             //删除
             deleteData: function (datas) {
                var th = this;
                th.loadingdel = true;
                $api.delete('Account/DeleteBatch', { 'ids': datas }).then(function (req) {
                    th.loadingdel = false;
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
                    th.loadingdel = false;
                    th.$alert(err);
                });
            },
        }
    });

});
