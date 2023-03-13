$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {}      //当前机构配置项

        },
        mounted: function () {},
        created: function () { },
        computed: {},
        watch: {},
        methods: {
            //获取当前地址
            geturl: function () {
                var url = String(window.document.location.pathname);
                if (url.indexOf("?") > -1)
                    url = url.substring(0, url.lastIndexOf("?"));
                return url;
            }
        }
    });
});
