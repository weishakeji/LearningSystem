$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项     

            model: '',        //大语言模型的引擎名称

            input: '',         //输入内容
            
            loading_init: true
        },
        mounted: function () {
            this.getModelname();
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //获取大语言模型名称
            getModelname: function () {
                var th = this;
                $api.get('LLM/Model').then(req => {
                    if (req.data.success) {
                        th.model = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            }
        }
    });

}, ["../Components/links.js"]);
