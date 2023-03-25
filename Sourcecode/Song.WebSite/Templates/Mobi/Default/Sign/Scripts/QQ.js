$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //1为绑定，2为登录
            type: $api.querystring('type'),

            loading: true
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
            loaduser: function (user) {
                alert(user);
            }
        }
    });

}, ['/Utilities/OtherLogin/qq.js']);
