(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            editorid: $api.querystring('editorid'),  //编辑器的id
            input: '',//消息

            messages: [],        //所有对话记录

            loading: false
        },
        mounted: function () {
            this.messages = $api.storage('messages') || [];
        },
        updated: function () {

        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            // 发送
            send: function () {
                if (this.input.trim() == '') return;
                var th = this;
                th.messages.push({ role: 'user', content: th.input });
                th.input = '';
                th.loading = true;
                $api.post('LLM/Communion', { 'character': 'You are a helpful assistant.', 'messages': th.messages }).then(req => {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.messages.push({ role: 'system', content: result });
                        $api.storage('messages', th.messages);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => {
                    console.error(err);
                    alert('请求失败');
                }).finally(() => th.loading = false);

            },
        },
        // 组件
        components: {
            // 用户的消息框
            'user_msg': {
                props: ['message'],
                methods: {
                },
                template: `<div class="user_message">
                    
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
                    },
                    //插入到编辑器中
                    insert:  function (text) { 
                        let editorid=$api.querystring('editorid');  //编辑器的id
                        window.parent.organinfo_action(editorid,this.formatText(this.message));
                    },
                },
                template: `<div class="ai_msg">    
                    <div class="ai_msg_icon"></div>              
                    <div class="ai_msg_content" v-html="formatText(message)"></div>
                    <div class="ai_btn">
                        <button class="ai_btn_copy">复制</button>
                        <button class="ai_btn_fresh">重新生成</button>
                        <button class="ai_btn_fresh" @click="insert">插入到编辑器</button>
                    </div>
                </div >`
            }
        }
    });
})();