$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //1为绑定，2为登录
            type: $api.querystring('type'),
            //微信用户信息
            user: {},
            /** 示例
            {
                "openid": "ohjXUjtd56L30DEbXM81fwk0che0", 
                "nickname": "宋", 
                "sex": 0, 
                "language": "", 
                "city": "", 
                "province": "", 
                "country": "", 
                "headimgurl": "https://thirdwx.qlogo.cn/mmopen/vi_32/DYAIOgq83eo6KrCia9leuX73Y84YRzcsWhUicECVCE04q7ESOAeWJFJFj3nbgFjxxaBX1aticCgxWnVn7154H5H7A/132", 
                "privilege": [ ], 
                "unionid": "oviR-0ujiC7OLI45s0xt0ja1-YSI"
            }
            */
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
                        var url = $api.url.set('/student/OtherLogin/weixin', user);
                        //console.error(url);
                        window.location.href = url;
                    }
                }
                //alert(user);
            }
        }
    });

}, ['/Utilities/OtherLogin/weixin.js']);
