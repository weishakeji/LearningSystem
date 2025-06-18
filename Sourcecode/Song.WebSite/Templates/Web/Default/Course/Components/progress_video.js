
//章节的视频学习完成度
Vue.component('progress_video', {
    //videolog:章节学习记录
    //outline:当前章节
    //text: 默认显示的文本信息，例如没有学习记录时
    props: ["videolog", "course", "outline", "text"],
    data: function () {
        return {}
    },
    watch: {
        //学习记录加载后
        'videolog': {
            handler: function (nv, ov) {
                if (nv != null && nv.length) this.show = true;
            }, immediate: true
        }
    },
    computed: {
        //视频观看的完成度的百分比
        'percentage': function () {
            var logs = this.videolog;
            var percent = 0;
            //如果没有学习记录
            if (!(logs && logs.length)) return percent;
            //如果章节没有设置，返回
            if (!(JSON.stringify(this.outline) != '{}' && this.outline != null)) {
                return percent;
            }
            //取当前章节的学习进度
            for (let i = 0; i < logs.length; i++) {
                const log = logs[i];
                if (log.Ol_ID == this.outline.Ol_ID) {
                    if (log.complete != null) {
                        percent = Number(log.complete);
                        percent = isNaN(percent) ? 0 : percent;
                    }
                    break;
                }
            }
            return Math.floor(percent * 100) / 100;
        },
        //状态（颜色）
        'state': function () {
            if (this.percentage == 0) return 'primary';
            if (this.percentage > 0 && this.percentage < 100) return 'success';
            return 'primary';
        }

    },
    mounted: function () {

    },
    methods: {
    },
    template: `<el-tag type="primary" :type="state" :plain="percentage == 0" @click="$emit('click', course, outline)">
            <template v-if="course.Cou_Type==0">
                <template v-if="percentage>0">
                    {{percentage}} %
                </template>
                <template v-else>
                    {{text}}66
                </template> 
            </template>
            <template v-else>
                   {{text}}
            </template>
        </el-tag> `
});