$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {

        },
        mounted: function () {
            this.mobile_to_mobi();
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //如果是1.0的mobile路径，转为现在的mobi
            mobile_to_mobi: function () {
                var href = window.location.href;
                var origin = window.location.origin;
                if (href.indexOf(origin) > -1) {
                    href = href.substring(href.lastIndexOf(origin) + origin.length);
                }
                if (href.indexOf('mobile') > -1) {
                    href = href.replace('mobile', 'mobi');
                    window.location.href = href;
                }               
            }
        }
    });

});
