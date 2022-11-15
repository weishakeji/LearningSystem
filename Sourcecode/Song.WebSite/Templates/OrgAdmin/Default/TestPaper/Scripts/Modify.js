$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),

            organ: {},
            config: {},      //当前机构配置项        

            entity: null,
            loading: true
        },
        mounted: function () {
            var th = this;
            if (th.id == '') {
                $api.get('Snowflake/Generate').then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.id = req.data.result;
                        //跳转
                        window.setTimeout(function () {
                            th.gourl();
                        }, 100);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
                return;
            }
            $api.get('TestPaper/ForID', { 'id': this.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.entity = result;
                } else {
                }
                //跳转
                window.setTimeout(function () {
                    th.gourl();
                }, 100);
            }).catch(function (err) {
                th.loading = false;
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //根据试卷类型（随机抽题还是固定试题）跳转
            gourl: function () {
                var entity = this.entity;
                var type = entity == null ? 2 : entity.Tp_Type;
                type = type <= 1 ? 1 : type;
                //类型默认是随时抽题，即2
                type = 2;
                var url = "Modify_Type" + type + ".html";
                if (entity == null) {
                    var couid = $api.querystring('couid');
                    if (couid != '')
                        url = $api.url.set(url, { 'couid': couid, 'id': this.id });
                    console.log(url);
                    window.location.href = url;
                } else {
                    url = $api.url.set(url, 'id', entity.Tp_Id);
                    window.location.href = url;
                }

            }
        }
    });

});
