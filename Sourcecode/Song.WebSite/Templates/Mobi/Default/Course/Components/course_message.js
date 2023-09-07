//留言咨询
$dom.load.css([$dom.pagepath() + 'Components/Styles/course_message.css']);
Vue.component('course_message', {
    props: ['outline', 'account','config'],
    data: function () {
        return {
            messages: [],		//信息列表	
            timer: null,			//	
            input_text: '',
            loading: true //预载中
        }
    },
    watch: {
        'outline': {
            deep: true, immediate: true,
            handler: function (nv, ov) {
                if ($api.isnull(nv) || nv.Ol_ID == null) return;
                this.messages = [];
                this.msgGet();
            }
        }
    },
    computed: {
        //是否登录
        'islogin': t => !$api.isnull(t.account)
    },
    created: function () {
        //定时刷新（加载）咨询留言
        this.timer = window.setInterval(this.msgGet, 1000 * 10);
    },
    methods: {
        //获取当前章节的留言信息
        msgGet: function () {
            var th = this;
            if (!this.islogin) {
                window.setTimeout(function () { th.msgGet(); }, 100);
                return;
            }
            $api.get("message/count", {
                olid: this.outline.Ol_ID, order: 'desc', count: 100
            }).then(function (req) {
                if (req.data.success) {
                    th.messages = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                //alert("留言信息加载异常！详情：\r" + err);
            });
        },
        //发送消息
        msgSend: function () {

            this.msgBlur();
            var th = this;
            var msg = this.input_text;
            if ($api.trim(msg) == '') return;
            var span = Date.now() - Number($api.cookie("msgtime"));
            if (span / 1000 < 10) {
                th.$dialog.alert({
                    message: '不要频繁发消息！',
                    position: 'bottom-right'
                });
                return;
            }
            $api.cookie("msgtime", Date.now());
            var play = vapp.playinfo(this.outline.Ol_ID);
            $api.post("message/add", {
                acc: this.account.Ac_AccName, msg: msg, playtime: play.time,
                couid: this.outline.Cou_ID, olid: this.outline.Ol_ID
            }).then(function (req) {
                var d = req.data;
                if (d.success) {
                    th.input_text = '';
                    th.msgGet();
                } else {
                    var msg = "信息添加发生异常！详情：\r" + d.message;
                    th.$dialog.alert({
                        message: msg,
                        position: 'bottom-right'
                    });
                }
            });
        },
        //留言输入框失去焦点
        msgBlur: function () {
            //document.getElementById("footer").style.display = "";
            //document.getElementById("messageinput").blur();
        },
        //留言输入框获取焦点
        msgFocus: function (e) {
            //document.getElementById("footer").style.display = "none";
            //document.getElementById("messageinput").focus();
        }
    },
    template: `<div id='chatarea'>
	<div class='outline-name'>
		<span v-show='!outline'>正在加载...</span>
		{{outline.Ol_Name}}
		<button id='msginputBtn' class='el-icon-edit' v-on:click='msgFocus'
			v-show='outline'> 留言
		</button>
	</div>
    <dl id='chatlist' v-if='messages.length>0'  v-on:click='msgBlur'>
            <dd v-for='(item,index) in messages'>
                <span :playtime='item.Msg_PlayTime'>
                    <acc><i class='el-icon-chat-dot-round'></i>{{item.Ac_Name}}：</acc>
                    <date>{{item.Msg_CrtTime | date('yyyy-M-d hh:mm:ss')}}</date>
                </span>
                <msg>{{item.Msg_Context}} </msg>
            </dd>
        </dl>
	<dl v-else id='chatlist'><dd  class='nomsg'>没有人留言！</dd></dl>
	<div id='chatbox' remark='留言录入区域'>
		<textarea rows='3' id='messageinput' v-model='input_text' name='messageinput' autofocus
			v-on:keyup.enter='msgSend'></textarea>
		<button id='btnMessage' v-on:click='msgSend'>发送</button>
    </div>
	</div>`
});