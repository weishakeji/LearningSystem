$ready(['Components/topbar.js',
    '/Utilities/Components/avatar.js',
    '/Utilities/Scripts/marked.min.js'],
    function () {
        window.vapp = new Vue({
            el: '#vapp',
            data: {
                account: {},     //当前登录账号
                platinfo: {},
                org: {},
                config: {},      //当前机构配置项
                //课程相关信息
                couid: $api.querystring("couid") == "" ? $api.dot() : $api.querystring("couid"),        //课程id
                course: {},
                studied: false,        //是否在学习该课程
                owned: false,            //是否拥有该课程，例如购买或学员组关联
                purchase: null,          //课程购买记录    

                model: '',        //大语言模型的引擎名称
                character:'',       //大语言模型的角色内容，来自role.txt

                input: '',         //输入内容
                //当前对话记录
                record: {
                    Llr_ID: 0,      //对话记录ID
                    Ac_ID: 0,            //学员ID
                    Llr_Topic: '',   //对话主题    
                    Llr_Records: []     //对话记录
                },
                records: [],        //所有对话记录
                showhistory: false,   //是否显示历史记录

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
                //课程为空,或课程被禁用
                nullcourse: function () {
                    return JSON.stringify(this.course) == '{}' || this.course == null || !this.course.Cou_IsUse;
                },
                //是否购买记录
                purchased: function () {
                    if (JSON.stringify(this.purchase) == '{}' || this.purchase == null) return false;
                    if (this.purchase.Stc_EndTime.getTime() < (new Date()).getTime())
                        return false;
                    if (this.purchase.Stc_IsTry) return false;
                    return this.purchase.Stc_Type != 5 && !this.course.Cou_IsFree && this.purchase.Stc_IsEnable;
                },
                //可以学习
                canstudy: function () {
                    return this.studied && (this.purchased && this.purchase.Stc_IsEnable);
                },
                //是否可以永久学习
                forever: function () {
                    if (!this.purchase) return false;
                    if (!this.purchase.Stc_IsEnable) return false;
                    if (this.purchase.Stc_Type != 5) return false;
                    var time = this.purchase.Stc_EndTime;
                    if (time == '' || time == null) return false;
                    if ($api.getType(time) != 'Date') return false;
                    var year = time.getFullYear();
                    return time.getFullYear() - new Date().getFullYear() > 100;
                },
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
                            this.getcourse();
                        } else
                            this.loading_init = false;
                    }, immediate: true, deep: true
                }
            },
            methods: {

                //获取课程信息
                getcourse: function () {
                    var th = this;
                    $api.bat(
                        $api.get('Course/ForID', { 'id': th.couid }),
                        $api.get('Course/Studied', { 'couid': th.couid }),
                        $api.get('Course/Owned', { 'couid': th.couid, 'acid': th.account.Ac_ID }),
                        $api.get('Course/Purchaselog', { 'couid': th.couid, 'stid': th.account ? th.account.Ac_ID : 0 }),
                        $api.get("LLM/CourseAIAgentSystemRole", {"couid":th.couid})
                    ).then(([cou, studied, owned, purchase,character]) => {
                        th.course = cou.data.result;
                        th.studied = studied.data.result;
                        th.owned = owned.data.result;
                        if (purchase.data.result != null) th.purchase = purchase.data.result;
                        th.character = character.data.result;
                    }).catch(err => console.error(err))
                        .finally(() => th.loading_init = false);
                },
                //获取大语言模型名称
                getModelname: function () {
                    var th = this;
                    $api.get('LLM/ModelName').then(req => {
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
                    $api.post('LLM/CourseAIAgenCommunion', { 'couid': th.couid, 'messages': th.record.Llr_Records }).then(req => {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.record.Llr_Records.push({ role: 'assistant', content: result });
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
                    record.Cou_ID = th.course.Cou_ID;
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
                    $api.get('LLM/AIAgentRecords', { 'acid': th.account.Ac_ID, 'couid': th.couid, 'count': 1000 }).then(req => {
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
                //删除记录
                btnDelete: function (id) {
                    var th = this;
                    this.$dialog.confirm({
                        title: '删除记录',
                        message: '您是否确定删除当前沟通记录？',
                    }).then(() => {
                        th.delRecord(id);
                    }).catch(() => { });
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
    });