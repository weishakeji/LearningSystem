//交流咨询
Vue.component('study_chat', {
    props: ['course', 'outline', 'account', 'playtime'],
    data: function () {
        return {
            messages: [], //咨询留言
            error: '',      //加载错误的信息

            subtitle: false,     //是否启用弹幕
            switchSubtitle: true,        //弹幕开关状态   
            enable_del: true,        //是否允许学员删除自身的发言         

            send_msg: '',       //当前要发送的消息
            loading: false,
            loadingid: 0
        }
    },
    watch: {
        'outline': {
            handler: function (nv, ov) {
                if ($api.isnull(nv) || nv.Ol_ID == null) return;
                this.msgGet();
            }, immediate: true,
        }
    },
    computed: {
        //是否登录
        'islogin': t => !$api.isnull(t.account)
    },
    mounted: function () {
        var css = $dom.path() + 'course/Components/Styles/study_chat.css';
        $dom.load.css([css]);

        var th = this;
        //定时刷新（加载）咨询留言        
        window.setInterval(function () {
            th.msgGet();
        }, 1000 * 10);

    },
    methods: {
        //获取当前章节的留言信息
        msgGet: function () {
            var th = this;
            if (!this.islogin) {
                window.setTimeout(function () { th.msgGet(); }, 100);
                return;
            }
            var olid = this.outline ? this.outline.Ol_ID : 0;
            if (olid <= 0) return;

            $api.post("message/count", { olid: olid, order: 'asc', count: 100 })
                .then(function (req) {
                    var d = req.data;
                    if (d.success) {
                        th.messages = d.result;
                        th.scroll_to_bottom();
                    } else {
                        th.error = "留言信息加载异常！详情：\r" + d.message;
                    }
                }).catch(function (err) {
                    //alert("msgGet方法存在错误："+err);
                });
        },
        //发送消息
        msgSend: function () {
            var th = this;
            if (th.send_msg == '') return;
            var span = Date.now() - Number($api.cookie("msgtime"));
            if (span / 1000 < 10) {
                return th.$notify({
                    message: '不要频繁发消息！',
                    position: 'bottom-right'
                });
            }
            $api.cookie("msgtime", Date.now());
            th.loading = true;
            var olid = this.outline ? this.outline.Ol_ID : 0;
            var couid = this.course ? this.course.Cou_ID : 0;
            var acc = this.account ? this.account.Ac_AccName : '';
            $api.post("message/add", {
                acc: acc, msg: th.send_msg,
                playtime: th.playtime, couid: couid, olid: olid
            }).then(function (req) {
                th.loading = false;
                if (req == null) return;
                if (req.data.success) {
                    th.send_msg = '';
                    th.$set(th.messages, th.messages.length, req.data.result);
                    th.$nextTick(th.scroll_to_bottom);
                } else {
                    th.error = "信息添加发生异常！详情：\r" + d.message;
                }
            }).catch(function (err) {
                th.loading = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });;
        },
        //删除留言
        msgDel: function (item) {
            var th = this;
            this.$confirm('是否删除当前留言, 是否继续?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                th.loadingid = item.Msg_Id;
                $api.delete('Message/Delete', { 'msid': item.Msg_Id }).then(function (req) {
                    th.loadingid = -1;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '删除成功!'
                        });
                        var index = th.messages.findIndex((n) => {
                            return n.Msg_Id == item.Msg_Id;
                        });
                        th.messages.splice(index, 1);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loadingid = -1;
                    alert(err);
                    console.error(err);
                });

            }).catch(() => {
            });
        },
        //滚动到底部
        scroll_to_bottom: function () {
            window.setTimeout(function () {
                var dl = document.getElementById("chatlist");
                if (dl != null) {
                    var max = getClientHeight();
                    var height = dl.offsetHeight > max ? dl.offsetHeight : max;
                    dl.scrollTop = height * 2;
                }
            }, 300);
            function getClientHeight() {
                var clientHeight = 0;
                if (document.body.clientHeight && document.documentElement.clientHeight) {
                    var clientHeight = (document.body.clientHeight < document.documentElement.clientHeight) ? document.body.clientHeight : document.documentElement.clientHeight;
                }
                else {
                    var clientHeight = (document.body.clientHeight > document.documentElement.clientHeight) ? document.body.clientHeight : document.documentElement.clientHeight;
                }
                return clientHeight;
            }
        }
    },
    template: `<div id="study_chat">    
        <dl id="chatlist">
            <dd v-if="messages.length<1">没有消息</dd>
            <template v-else>
                <dd v-if="item.Msg_Id>0" v-for="(item,i) in messages" :playtime="item.Msg_PlayTime">    
                    <div>             
                        <acc>
                            <del class="el-icon-close" v-if="enable_del && (islogin && account.Ac_ID==item.Ac_ID)" @click="msgDel(item)"></del>
                            <i class="el-icon-chat-dot-round" v-else></i>
                            {{item.Ac_Name}}
                        </acc>
                        <date>{{item.Msg_CrtTime | date('yyyy-M-d hh:mm:ss')}}</date>
                    </div>
                    <msg>{{item.Msg_Context}} </msg>
                </dd>    
            </template>        
        </dl>
        <div :class="{'chatbox':true,'subtitle':subtitle}">
            <div :class="['switch',switchSubtitle ? 'switch-on' : '']" v-if="subtitle"
                v-on:click="switchSubtitle=!switchSubtitle"></div>
            <loading v-if="loading">sending ...</loading>
            <div class="msgbox" v-else>
                <input type="text" id="messageinput" v-model.trim="send_msg" v-on:keyup.enter="msgSend" />
                <div id="btnMessage" @click="msgSend">发送</div>
            </div>
        </div>
    </div>`
});