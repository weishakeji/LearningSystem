
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            user: {},

            loading: false
        },
        computed: {

        },
        watch: {

        },
        mounted: function () {
            var params = $api.querystring();
            var obj = {};
            for (let i = 0; i < params.length; i++) {
                obj[params[i].key] = decodeURIComponent(params[i].val);
            }
            this.user = obj;
        },
        created: function () {

        },
        methods: {

        }
    });

});


