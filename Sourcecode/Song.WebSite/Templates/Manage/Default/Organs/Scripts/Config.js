
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            organ: {},
            config: {},      //当前机构配置项   
            loading: false
        },
        watch: {

        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.get('Organization/ForID', { 'id': th.id }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.organ = req.data.result;
                    //机构配置信息
                    th.config = $api.organ(th.organ).config;
                    console.log(th.config);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        methods: {

        },
    });

});
