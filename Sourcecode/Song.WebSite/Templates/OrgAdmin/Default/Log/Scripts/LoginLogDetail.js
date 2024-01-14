$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.dot(),      //记录id
            entity: {},          //记录的实体对象
            account: {},        //账号对象
            loading: false,
            loading_bind: ''      //绑定或取消绑定中的状态，
        },
        mounted: function () {
            //this.getAccount();
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //获取当前登录账号
            getAccount: function (func) {
                var th = this;
                th.loading_init = true;
                $api.get('Account/Current').then(function (req) {
                    th.loading_init = false;
                    if (req.data.success) {
                        th.account = req.data.result;
                        if (func != null) func(th.account);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },

        }
    });

}, []);
