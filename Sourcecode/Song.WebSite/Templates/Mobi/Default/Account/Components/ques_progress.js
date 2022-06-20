//课程试题练习的通过率
$dom.load.css([$dom.pagepath() + 'Components/Styles/ques_progress.css']);
Vue.component('ques_progress', {
    //config:机构的配置项，其实包括了视频完成度的容差值（VideoTolerance）
    //purchase:购买记录
    props: ['course', 'stid', 'config', 'purchase'],
    data: function () {
        return {
            data: {},        //进度信息
            percent: 0,     //完成的真实百分比
            tolerance: 10,       //容差，例如容差为10，则完成90%即可认为是100%
            loading: false
        }
    },
    watch: {
        'purchase': {
            handler: function (nv, ov) {
                this.percent = nv.Stc_QuesScore;
            }, immediate: true
        }
    },
    computed: {
        'color': function () {
            if (this.progress == 0) return '#909399';
            if (this.progress < 30) return '#F56C6C';
            if (this.progress < 60) return '#E6A23C';
            if (this.progress < 90) return '#67C23A';
            if (this.progress < 100) return 'rgb(106 179 255)';
            return '#409EFF';
        },
        //完成度，加了容差之后的
        'progress': function () {
            return this.percent + this.tolerance >= 100 ? 100 : this.percent;
        },
    },
    mounted: function () { },
    methods: {
        //是否完成
        finished: function (percentage) {
            return percentage >= 100;
        },
        //进度条显示的数值样式
        format(percentage) {
            return this.finished(percentage) ? '完成学习！100%' : '通过率：' + percentage + '%';
        },
        gourl:function(){
            var url="/mobi/question/course";
            url = $api.url.set(url, { 'couid': this.course.Cou_ID });
            window.location.href = url;
        }
    },
    template: `<div @click="gourl()">
    <icon>&#xe75e</icon>
    <van-progress :percentage="progress" :color="color"></van-progress>
    </div>`
});