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
            //当前对话记录
            record: {
                Llr_ID: 0,      //对话记录ID
                Ac_ID: 0,            //学员ID
                Llr_Topic: '',   //对话主题    
                Llr_Records: []     //对话记录
            },
            records: [],        //所有对话记录

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
            //是否登录
            islogin: t => !$api.isnull(t.account),
            //对话记录
            messages: function () {
                if (!this.islogin || !this.record.Llr_Records) return [];
                return this.record.Llr_Records;
            },
            //是否为新话题
            isnewTopic: function () {
                let  isnewTopic = $api.storage('isnewTopic');
                if(isnewTopic==null)return true;        //默认是新话题
                return isnewTopic=='true' || isnewTopic===true;
            }
        },
        watch: {
            //当学员登录时
            'account': {
                handler: function (nv, ov) {
                    if (nv && nv.Ac_ID != null) {
                        var th = this;
                        this.loadRecords(function (records) {
                            if (records.length > 0 && !th.isnewTopic)
                                th.record = $api.clone(records[0]);
                            th.$nextTick(function () {
                                window.setTimeout(function () {
                                    let area = $dom("div.messages_area")[0];
                                    th.$mathjax(area);
                                }, 2000);

                            });
                        });
                    }
                }, immediate: true, deep: true
            }
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
            //开启新话题
            newTopic: function () {
                this.record.Llr_ID = 0;
                this.record.Llr_Topic = '';
                this.record.Llr_Records = [];
                //记录新话题的状态
                $api.storage('isnewTopic', true);

            },
            //选择历史对话记录
            selectRecord: function (record) {
                this.record = $api.clone(record);
                $api.storage('isnewTopic', false);
            },
            // 发送
            send: function () {
                if (this.input.trim() == '') return;
                var th = this;
                th.record.Llr_Records.push({ role: 'user', content: th.input });
                th.input = '';
                th.loading = true;
                $api.post('LLM/Communion', { 'character': 'You are a helpful assistant.', 'messages': th.record.Llr_Records }).then(req => {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.record.Llr_Records.push({ role: 'system', content: result });
                        th.saveRecords(th.record);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => {
                    console.error(err);
                    alert('请求失败');
                }).finally(() => th.loading = false);

            },
            //保存对话记录
            saveRecords: function (record) {
                if (!this.islogin) return;
                var th = this;
                let apiurl = record.Llr_ID == 0 ? 'LLM/RecordsAdd' : 'LLM/RecordsUpdate';
                //计算话题，取用户的第一条消息
                record.Llr_Topic = record.Llr_Records[0].content;
                record.Ac_ID = th.account.Ac_ID;
                $api.post(apiurl, { 'record': record }).then(req => {
                    if (req.data.success) {
                        let result = req.data.result;
                        if (result != null) {
                            result.Llr_Records = JSON.parse(result.Llr_Records);
                            console.error(result);
                            th.record = result;
                        }
                        th.loadRecords();
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //加载对话记录,加载完成执行func方法
            loadRecords: function (func) {
                if (!this.islogin) return;
                var th = this;
                $api.get('LLM/RecordsAll', { 'acid': th.account.Ac_ID }).then(req => {
                    if (req.data.success) {
                        let result = req.data.result;
                        for (let i = 0; i < result.length; i++) {
                            result[i].Llr_Records = JSON.parse(result[i].Llr_Records);
                        }
                        th.records = result;
                        if (func) func(th.records);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            }
        },
        // 组件
        components: {
            // 用户的消息框
            'user_msg': {
                props: ['account', 'message'],
                methods: {
                },
                template: `<div class="user_message">
                    <avatar :account="account" circle="true" size="50"></avatar>
                    <div class="user_message_content" v-html="message"></div>
                </div >`
            },
            //Ai的消息框
            'ai_msg': {
                props: ['message'],
                methods: {
                    //格式化文本
                    formatText: function (text) {
                        if (text == null || text == "") return "";
                        //text = marked.parse(text);
                        text = text.replace(/\\times/g, "&times;");
                        text = text.replace(/\\div/g, "&divide;");

                        text = text.replace(/\\approx/g, "&asymp;");

                        text = text.replace(/\\text{([^}]*)}/g, "$1");
                        text = marked.parse(text);
                        return text;
                    }
                },
                template: `<div class="ai_msg">    
                    <div class="ai_msg_icon"></div>              
                    <div class="ai_msg_content" v-html="formatText(message)"></div>
                </div >`
            }
        }
    });

}, ['../Components/links.js',
    '/Utilities/Components/avatar.js',
    'Scripts/marked.min.js']);
