$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            entity: {},
            organ: {},
            config: {},      //当前机构配置项        
            datas: {},
            loading_init: true
        },
        mounted: function () {
            var th = this;
            //当前试卷
            $api.put('TestPaper/ForID', { 'id': this.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.entity = result;
                    th.Tp_Diff = [];
                    Vue.set(th.Tp_Diff, 0, th.entity.Tp_Diff);
                    Vue.set(th.Tp_Diff, 1, th.entity.Tp_Diff2);
                } else {
                    //th.entity = {};                 
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                //获取结果             
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            })).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
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

});
