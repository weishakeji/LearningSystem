
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项

            entity: {},         //当前试题实体
            loading: false,
            loading_init: true,
        },
        watch: {
        },
        created: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current'),
                $api.cache('Question/Types:99999')
            ).then(([organ, types]) => {
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;
                th.types = types.data.result;
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
            this.getEntity();
        },
        mounted: function () {

        },
        methods: {
            //获取试题
            getEntity: function () {
                var th = this;
                if (th.id == '') return;
                th.loading = true;
                $api.put('Question/ForID', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.entity = result;
                        th.gourl(th.entity.Qus_Type, th.types[th.entity.Qus_Type]);
                    } else {
                        throw '未查询到数据';
                    }
                }).catch(err => alert(err, '错误'))
                    .finally(() => th.loading = false);
            },
            //转向
            //type:试题类型的数值
            //typename:试题类型的名称，如单选题、多选题
            gourl: function (type, typename) {
                var url = "Modify_Type" + type;
                var params = $api.url.params();
                for (let i = 0; i < params.length; i++)
                    url = $api.url.set(url, params[i].key, params[i].val);
                url = $api.url.set(url, 'typename', encodeURIComponent(typename));
                if (this.id != '') url = $api.url.dot(this.id, url);
                var loading = this.$fulloading();
                window.setTimeout(function () {
                    window.location.href = url;
                }, 500);
            }
        },

    });
}, ['Components/ques_type.js']);
