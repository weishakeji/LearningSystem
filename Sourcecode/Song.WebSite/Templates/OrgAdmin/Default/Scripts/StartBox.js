$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},
            //是否不再显示
            not_show: false,
            loading: false,
        },
        mounted: function () {
            var show = $api.storage('not_show');
            show = show == null ? false : JSON.parse(show);
            this.not_show = show;
        },
        created: function () {

        },
        computed: {

        },
        watch: {
            'not_show': {
                handler: function (newVal, oldVal) {
                    $api.storage('not_show', newVal);
                }//, immediate: true,
            }
        },
        methods: {
        }
    });
});
