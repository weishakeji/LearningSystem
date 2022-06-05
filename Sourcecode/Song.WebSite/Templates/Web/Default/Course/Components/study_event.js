//视频事件
Vue.component('study_event', {
    props: ['account', 'state'],
    data: function () {
        return {
            //控件
            player: null, //播放器
            //当前章节的视频信息
            video: {
                url: '', //视频路径
                total: 0, //总时长      
                playTime: 0, //当前播放时间，单位：毫秒     
                playhistime: 0, //历史播放时间
                studytime: 0, //累计学习时间
                percent: 0 //完成度（百分比）
            },
            playtime: 0, //当前播放时间，单位：秒
            playpercent: 0, //当前播放完成度百分比
        }
    },
    watch: {
        'islogin': function (nv, ov) {

        }
    },
    computed: {
        //是否登录
        islogin: function () {
            return JSON.stringify(this.account) != '{}' && this.account != null;
        }
    },
    mounted: function () {
        var css = $dom.path() + 'course/Components/Styles/study_event.css';
        $dom.load.css([css]);
    },
    methods: {

        //视频事件的触发
        videoEvent: function (playtime) {
            var th = this;
            if (th.events.length < 1) return;
            var curr = null;
            for (var ev in th.events) {
                if (th.events[ev].Oe_TriggerPoint == playtime) {
                    curr = th.events[ev];
                    break;
                }
            }
            if (curr == null) return;
            //视频暂停
            if (th.playready()) th.player.pause();
            var box = new MsgBox();
            box.OverEvent = function () {
                if (th.playready()) th.player.play();
            }
            if (curr.Oe_EventType == 1)
                box.Init("提示：" + curr.Oe_Title, curr.Oe_Context, curr.Oe_Width, curr.Oe_Height, "alert").Open();
            if (curr.Oe_EventType == 2)
                box.Init("知识点：" + curr.Oe_Title, curr.Oe_Context, curr.Oe_Width, curr.Oe_Height, "alert").Open();
            if (curr.Oe_EventType == 3) {
                var items = eval(curr.Oe_Datatable);
                var context = curr.Oe_Context + "<div class='quesBox'>";
                for (var i in items) {
                    if (items[i].item == "") continue;
                    context += "<div onclick='th.videoEventClick(" + items[i].iscorrect + ",-1)'>" +
                        (Number(i) + 1) + "、" + items[i].item + "</div>";
                }
                context += "</div>";
                context += "<div id='event_error'>回答错误</div>";
                box.Init("提问" + curr.Oe_Title, context, curr.Oe_Width, curr.Oe_Height, "null").Open();
            }
            if (curr.Oe_EventType == 4) {
                var items = eval(curr.Oe_Datatable);
                var context = curr.Oe_Context + "<div class='quesBox'>";
                for (var i in items) {
                    if (items[i].item == "") continue;
                    context += "<div onclick='th.videoEventClick(null," + items[i].point + ")'>" +
                        (Number(i) + 1) + "、" + items[i].item + " - （跳转到：" + items[i].point + "秒）</div>";
                }
                context += "</div>";
                box.Init("反馈：" + curr.Oe_Title, context, curr.Oe_Width, curr.Oe_Height, "alert").Open();
            }
        },
        //视频事件的点击操作
        videoEventClick: function (iscorrect, seek) {
            var th = this;
            //视频事件的问题
            if (iscorrect != null) {
                if (iscorrect) MsgBox.Close();
                if (!iscorrect) {
                    var err = document.getElementById("event_error");
                    err.style.height = 20 + 'px';
                    err.style.opacity = 1;
                    window.setTimeout(function () {
                        var err = document.getElementById("event_error");
                        err.style.height = 0 + 'px';
                        err.style.opacity = 0;
                    }, 1000);
                }
            }
            //视频跳转
            if (iscorrect == null && seek > 0) {
                if (!th.playready()) return;
                th.player.seek(seek);
                MsgBox.Close();
            }
        },
    },
    template: `<div id="study_video"  :video="state.urlVideo">    
        <div id="videoplayer" v-show="!state.otherVideo && !state.isLive"></div>
        <study_float :account="account" remark="飘浮信息，防录屏"></study_float>

        <div id="videoinfo" v-show="!state.otherVideo && !state.isLive">
            <span style="display: non6e">视频时长：{{video.total}}秒，播放进度：{{playtime}}秒，</span>
            <span>累计学习{{video.studytime}}秒，完成{{video.percent}}%，</span>
            <span style="cursor: pointer"
                v-on:click="videoSeek(video.playhistime)">上次播放到{{video.playhistime}}秒</span>
            <span class="info" v-show="studylogState==1">学习进度提交成功!</span>
            <span class="error" v-show="studylogState==-1">学习进度提交失败!</span>
        </div>
        <div id="videolog" v-show="!state.otherVideo && !state.isLive">
            <template v-for="i in video.total">
                <i>1</i>
            </template>
        </div>
        <iframe id="vedioiframe" height="100%" width="100%"
            v-if="state.outerVideo && state.otherVideo  && !state.isLive" :src="state.urlVideo"
            allowscriptaccess="always" allowfullscreen="true" wmode="opaque" allowtransparency="true"
            frameborder="0" remark="外部视频链接" type="application/x-shockwave-flash"></iframe>
    </div>`
});