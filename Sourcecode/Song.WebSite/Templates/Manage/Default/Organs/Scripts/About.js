
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {

            account: {}, //当前登录账号对象
            organ: {},       //当前机构
            loading: false
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.post('Admin/Super').then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                    $api.get('Organization/ForID', { 'id': result.Org_ID }).then(function (req) {
                        if (req.data.success) {
                            th.organ = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        th.$alert(err);
                        console.error(err);
                    });
                } else {
                    throw '未登录，或登录状态已失效';
                }
            }).catch(function (err) {
                th.loading = false;
                th.account = null;
                th.$alert(err);
            });
        },
        methods: {
            //提交更改
            updateDetails: function () {
                var th = this;
                th.loading = true;
                $api.post('Organization/Modify', { 'entity': this.organ }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '操作成功!',
                            center: true
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err);
                    console.error(err);
                });
            }
        }
    });
});
