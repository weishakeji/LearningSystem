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
            messages: [{
                role: 'system',
                content: 'You are a helpful assistant.'
            }, {
                role: 'user',
                content: '中国有多少海域？'
            },
            {
                role: 'system',
                content: '不知道，我去问问AI吧！'
            }
            ],         //对话记录
            //加载中
            loading: false,
            loading_init: true
        },
        mounted: function () {
            this.getModelname();
        },
        created: function () {
            /*
            //初始化，设定AI大语言模型的角色
            this.messages.push({
                role: 'system',
                content: 'You are a helpful assistant.'
            });*/
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
                        let result = req.data.result;
                        th.model = result.replace(/^./, match => match.toUpperCase())
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            // 发送
            send: function () {
                if (this.input.trim() == '') return;
                var th = this;
                th.messages.push({ role: 'user', content: th.input });
                th.input = '';
                th.loading = true;
                $api.post('LLM/Communion', { 'character': '', 'messages': th.messages }).then(req => {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.messages.push({ role: 'system', content: result });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => {
                    console.error(err);
                    alert('请求失败');
                }).finally(() => th.loading = false);
                console.error(th.messages);
            },
            //开启新话题
            newCommunion: function () {
                this.messages = [];
            },
            // 格式化文本
            formatText: function (text) {
                const html = marked.parse(text);
                return html;
            }
        }
    });

}, ["../Components/links.js", "Scripts/marked.min.js"]);
