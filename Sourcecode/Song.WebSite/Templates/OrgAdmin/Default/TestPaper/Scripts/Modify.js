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
                var url = "Modify_Type" + type + ".html";
                if (entity == null) {
                    var couid = $api.querystring('couid');
                    if (couid != '')
                        url = $api.url.set(url, 'couid', couid);
                     window.location.href = url;
                } else {
                    url = $api.url.set(url, 'id', entity.Tp_Id);                    
                    window.location.href = url;
                }

            }
        }
    });

});
