$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('couid', 0),        //课程id
            organ: {},
            config: {},      //当前机构配置项     
            types: [],        //试题类型，来自web.config中配置项

            step: 0,         //步数
            qtype: 0,       //当前题型

            datas: {},

            loading_init: true
        },
        mounted: function () {
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
        },
        created: function () {

        },
        computed: {
            //试题类型的名称
            'tname': function () {
                if (this.qtype <= 0 || this.qtype > this.types.length) return '';
                return this.types[this.qtype - 1];
            }
        },
        watch: {
        },
        methods: {
            //完成导入的事件
            finish: function (count) {
                console.log(count);
            },
        }
    });

}, ["/Utilities/Components/upload-excel.js",
    'Components/ques_type.js']);
