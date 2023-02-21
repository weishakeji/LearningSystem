
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            entity: {}, //当前对象             

            loading: false
        },
        watch: {

        },
        created: function () {
            var th = this;
            if (th.id == '') return;
            th.loading = true;
            $api.get('Pay/ForID', { 'id': th.id }).then(function (req) {
                window.setTimeout(function () {
                    th.loading = false;
                }, 300);

                if (req.data.success) {
                    th.entity = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        computed: {
            //实体是否不为空
            'entity_exist': function () {
                return JSON.stringify(this.entity) != '{}' && this.entity != null;
            }
        },
        methods: {

        },
    });

});
