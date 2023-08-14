$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项     

            datas: {},
            loading_init: true
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
        }
    });

}, ["../Components/links.js",
    '../Components/page_header.js']);
