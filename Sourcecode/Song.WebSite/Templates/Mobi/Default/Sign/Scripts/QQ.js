$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //1为绑定，2为登录
            type: $api.querystring('type'),

            user: {},
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
                //type:1为登录，2为绑定
                if (this.type == 2 || this.type == '2') {
                    var ismobi = $api.ismobi();                  
                    for (k in user) {
                        user[k] = encodeURIComponent(user[k]);
                    }
                    //if (!ismobi) {
                        var url = $api.url.set('/student/OtherLogin/qq', user);
                        //console.error(url);
                        window.location.href = url;
                    //}
                } else {
                    this.user = user;
                }
                //alert(user);
            }
        }
    });

}, ['/Utilities/OtherLogin/qq.js']);
