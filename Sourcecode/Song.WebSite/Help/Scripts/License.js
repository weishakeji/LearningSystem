
window.vapp = new Vue({
    el: '#vapp',
    data: {
        //版本信息
        editions: [],
        loading: false
    },
    computed: {

    },
    watch: {

    },
    mounted: function () {
        this.getEdition();
    },
    methods: {
        getEdition: function () {
            var th = this;
            th.loading = true;
            $api.get('Platform/EditionsChinese').then(req => {
                if (req.data.success) {
                    th.editions = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        }
    },

});
