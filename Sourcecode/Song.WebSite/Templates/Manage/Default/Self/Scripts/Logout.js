
$ready(function () {
    var vue = new Vue({
        el: '#vapp',
        data: {
            account: {} //当前登录账号对象
        },
        created: function () {
            var th = this;
            $api.post('Admin/Super').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                } else {
                    throw '未登录，或登录状态已失效';
                }
            }).catch(function (err) {
                //alert(err);
                th.account = null;
            });
        },
        methods: {
            btnLogout: function () {
                $api.login.out('admin', function () {
                    window.top.location.reload();
                });
            }
        },
    });
});
