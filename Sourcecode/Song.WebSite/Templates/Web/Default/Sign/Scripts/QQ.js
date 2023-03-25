$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //1为绑定，2为登录
            type: $api.querystring('type'),

            user:{},
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
                if (this.type == 1 || this.type == '1') {
                    var ismobi = $api.ismobi();
                    //let json = eval('('+user+')');
                    //let json = JSON.parse(user);
                    for (k in user) {
                        user[k] = encodeURIComponent(user[k]);
                    }
                    if (!ismobi) {
                        var url = $api.url.set('/student/OtherLogin/qq', user);
                        //console.error(url);
                        window.location.href = url;
                    }
                }
                //alert(user);
            }
        }
    });

}, ['/Utilities/OtherLogin/qq.js']);
