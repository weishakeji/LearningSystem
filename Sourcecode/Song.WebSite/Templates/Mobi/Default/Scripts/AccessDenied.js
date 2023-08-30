$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            msg: $api.querystring('msg')
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
            goback: function () {
                this.$dialog.confirm({
                    title: '返回首页',
                    message: '如果系统设置没有更改，返回首页后，仍然会回到这里，是否继续？',
                }).then(function () {
                    window.navigateTo('/mobi/');
                }).catch(function () { });
            }
        }
    });

});
