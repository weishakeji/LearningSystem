
$ready(function () {

    var vue = new Vue({
        el: '#app',
        data: {
            account: {}, //当前登录账号对象
        },
        created: function () {
            $api.post('Admin/Super').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    vue.account = result;
                } else {
                    throw '未登录，或登录状态已失效';
                }
            }).catch(function (err) {
                vue.account = null;
            });

        }
    });

});
