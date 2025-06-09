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
            loading_init: true,
            loadingid: 0
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
                let isnewTopic = $api.storage('isnewTopic');
                if (isnewTopic == null) return true;        //默认是新话题
                return isnewTopic == 'true' || isnewTopic === true;
            },
            //历史记录
            historys: function () {
                let datas = [
                    { span: 1, text: '今天' },
                    { span: 2, text: '昨天' },
                    { span: 7, text: '7天内' },
                    { span: 30, text: '30天内' },
                    { span: -1, text: '更多...' },
                ];
                let records = $api.clone(this.records);
                for (let i = 0; i < datas.length; i++) {
                    let data = datas[i];
                    data.list = [];
                    //时间界限
                    const now = new Date();
                    const deadline = new Date(now.getFullYear(), now.getMonth(), now.getDate());
                    deadline.setDate(now.getDate() + 1 - data.span);
                    //按时间分割聊天记录   
                    for (let j = 0; j < records.length; j++) {
                        let record = records[j];
                        if (data.span > 0) {
                            if (record.Llr_LastTime > deadline) {
                                data.list.push(record);
                                records.splice(j, 1);
                                j--;
                            }
                        } else {
                            data.list.push(record);
                            records.splice(j, 1);
                            j--;
                        }
                    }
                }
                //console.error(datas);
                return datas;
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
                    this.loading_init = false;
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
                        throw req.data.message;
                    }
                }).catch(err => {
                    alert('请求失败；详情：' + err);
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
                            th.record = result;
                            th.selectRecord(th.record);
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
            },
            //删除对话记录
            delRecord: function (id) {
                var th = this;
                th.loadingid = id;
                $api.delete('LLM/RecordsDelete', { 'ids': id }).then(req => {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.loadRecords(function (records) {
                            if (records.length < 1) th.newTopic();
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
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
                        text = marked.parse(text);
                        //text = text.replace(/\n/g, '<br/>');
                        text = text.replace(/\\times/g, "&times;");
                        text = text.replace(/\\div/g, "&divide;");
                        text = text.replace(/\\approx/g, "&asymp;");
                        text = text.replace(/\\text{([^}]*)}/g, "$1");
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

}, ['/Utilities/Components/avatar.js',
    '/Utilities/Scripts/marked.min.js']);
