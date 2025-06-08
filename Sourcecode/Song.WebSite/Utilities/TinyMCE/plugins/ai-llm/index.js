(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            editorid: $api.querystring('editorid'),  //编辑器的id
            input: '',          //消息
            messages: [],        //所有对话记录

            loading: false
        },
        mounted: function () {
            this.messages = $api.storage('messages') || [];
            for (var i = 0; i < this.messages.length; i++) {
                if (this.messages[i].role == 'system') {
                    this.messages[i].content = this.formatText(this.messages[i].content);
                }
            }
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
            //获取ai的回复
            getaillm: function (messages) {
                var th = this;
                th.loading = true;
                $api.post('LLM/Communion', { 'character': 'You are a helpful assistant.', 'messages': messages }).then(req => {
                    if (req.data.success) {
                        var result = th.formatText(req.data.result);
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
            // 发送
            send: function () {
                if (this.input.trim() == '' || this.loading) return;               
                this.messages.push({ role: 'user', content: this.input });
                this.getaillm(this.messages);
                this.input = '';
            },
            //清空
            clear: function () {
                this.messages = [];
                $api.storage('messages', this.messages);
            },
            //插入到编辑器中
            insert: function (text) {
                window.parent.organinfo_action(this.editorid, text);
            },
            //重新生成
            fresh: function (index) {            
                this.messages = this.messages.splice(0, index);               
                this.getaillm(this.messages);
            },
            //复制到粘贴板
            //text:要复制的文本内容
            //textbox:输入框类型，默认是input,多行文本用textarea
            copy: function (text, textbox) {
                return new Promise(function (resolve, reject) {
                    try {
                        if (textbox == null) textbox = 'input';
                        var oInput = document.createElement(textbox);
                        oInput.value = text;
                        document.body.appendChild(oInput);
                        oInput.select(); // 选择对象
                        document.execCommand("Copy"); // 执行浏览器复制命令           
                        oInput.style.display = 'none';
                        resolve(this);
                    } catch (err) {
                        reject(err);
                    }
                });
            },
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
        },
    });
})();