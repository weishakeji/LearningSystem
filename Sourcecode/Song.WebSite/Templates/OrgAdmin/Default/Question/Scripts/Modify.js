
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
            ).then(axios.spread(function (organ, types) {
                th.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw arguments[i].config.way + ' ' + data.message;
                    }
                }
                //获取结果
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;
                th.types = types.data.result;
            })).catch(function (err) {
                th.loading_init = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
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
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.entity = result;
                        th.gourl(th.entity.Qus_Type);
                    } else {
                        throw '未查询到数据';
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                });
            },            
            //转向
            gourl: function (type) {
                var url = "Modify_Type" + type;
                var params = $api.url.params();
                for (let i = 0; i < params.length; i++)
                    url = $api.url.set(url, params[i].key, params[i].val);
                url = $api.url.set(url, 'id', '');
                if (this.id != '') url = $api.url.dot(this.id, url);
                var loading = this.$fulloading();
                window.setTimeout(function () {
                    window.location.href = url;
                }, 500);               
            }
        },

    });
}, ['Components/ques_type.js']);
