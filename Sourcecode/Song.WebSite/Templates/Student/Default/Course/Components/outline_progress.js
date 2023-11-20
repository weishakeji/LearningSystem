
//章节的视频学习完成度
Vue.component('outline_progress', {
    //videolog:章节学习记录
    //outline:当前章节
    //loading: 加载状态
    props: ["videolog", "outline", "loading"],
    data: function () {
        return {
            data: null,      //当前章节的学习记录的数据
            show: false
        }
    },
    watch: {
        //学习记录加载后
        'videolog': {
            handler: function (nv, ov) {
                if (nv && nv.length) {
                    this.show = true;
                    this.data = this.getdata(nv);
                }
            }, immediate: true
        }
    },
    computed: {
        //视频观看的完成度的百分比
        percentage: function () {
            if (this.data == null) return 0;
            if (this.data.complete) {
                var percent = Number(this.data.complete);
                percent = isNaN(percent) ? 0 : percent;
                percent = Math.floor(percent * 100) / 100;
                return percent;
            }
            return 0;
        }
    },
    mounted: function () {
    },
    methods: {
        //获取当前章节的学习记录
        getdata: function (logs) {
            //如果没有学习记录
            if (!(logs && logs.length)) return null;
            var data = null;
            //取当前章节的学习进度
            for (let i = 0; i < logs.length; i++) {
                const log = logs[i];
                if (log.Ol_ID == this.outline.Ol_ID) {
                    data = log;
                    break;
                }
            }
            //console.log(data.lastTime);
            if (data && !$api.isnull(data.lastTime) && data.lastTime != '')
                data.lastTime = new Date(data.lastTime);
            return data;
        },
        /*
        //视频观看的完成度的百分比
        percentage: function () {
            if (this.data == null) return 0;
            if (this.data.complete) {
                var percent = Number(this.data.complete);
                percent = isNaN(percent) ? 0 : percent;
                console.log(percent);
                return percent;
            }
            return 0;
        },*/
        'state': function (percent) {
            if (percent < 10) return 'danger';
            else if (percent <= 30) return 'warning';
            else if (percent <= 80) return 'info';
            else return 'primary';
        },
        //累计学习时间
        studyTime: function (time) {
            if (time === '' || time === null) return '';
            if (time < 60) return "0:" + time + "";
            if (time >= 60) {
                var ss = time % 60;
                var mm = Math.floor(time / 60);
                if (mm < 60) {
                    return mm + ":" + ss + "";
                } else {
                    var hh = Math.floor(mm / 60);
                    mm = mm % 60;
                    return hh + ':' + mm + ":" + ss + "";
                }
            }
            return time;
        },
        //跳转到学习页
        gourl: function () {
            var url = '/web/course/study.258?olid=3406';    //示例，其中的数值会被后面替换
            url = $api.url.dot(this.outline.Cou_ID, url);
            url = $api.url.set(url, { 'olid': this.outline.Ol_ID });
            return url;
        }
    },
    template: `<div class="outline_progress" :style="'padding-left:'+(outline.Ol_Level*20)+'px'">
            <span>
                <span class="order">{{outline.Ol_XPath}}</span>
                <template v-if="percentage==100 || !outline.Ol_IsVideo">{{outline.Ol_Name}}</template>
                <a :href="gourl()" target="_blank" v-else>{{outline.Ol_Name}}</a>
                <icon v-if="outline.Ol_IsVideo">&#xe83a</icon>
            </span>
        <div v-if="data!=null && outline.Ol_IsVideo" class="time_info">
            <span class="lastTime" title="最后学习时间">{{data.lastTime|date('yyyy-MM-dd HH:mm')}}</span>
            <span class="studyTime" title="累计学习时间">{{studyTime(data.studyTime)}}</span>
            <span class="playTime" v-if="studyTime(data.playTime)!=''">
                <span title="播放进度">{{studyTime(Math.floor(data.playTime/1000))}}</span>
                <span class="slant"> / </span>
                <span title="视频时长">{{studyTime(Math.floor(data.totalTime/1000))}}</span>
            </span>
        </div>
        <template v-if="outline.Ol_IsVideo">
            <el-tag type="primary" :type="state(percentage)" plain v-if="!loading">            
                <template v-if="percentage==100">{{percentage}} %</template>
                <a :href="gourl()" target="_blank" v-else>{{percentage}} %</a>
            </el-tag>
            <el-tag type="info" v-else><loading bubble></loading></el-tag>
        </template>
    </div> `
});