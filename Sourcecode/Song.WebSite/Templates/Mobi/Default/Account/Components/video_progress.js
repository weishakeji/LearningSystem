//课程视频的学习进度
$dom.load.css([$dom.pagepath() + 'Components/Styles/video_progress.css']);
Vue.component('video_progress', {
    //config:机构的配置项，其实包括了视频完成度的容差值（VideoTolerance）
    props: ['couid', 'stid', 'config'],
    data: function () {
        return {
            percent: 0,     //完成的真实百分比
            tolerance: 10,       //容差，例如容差为10，则完成90%即可认为是100%
            loading: false
        }
    },
    watch: {
        'couid': {
            handler: function (nv, ov) {
                this.onload();
            }, immediate: true
        },
        'config': {
            handler: function (nv, ov) {
                if (nv && nv.VideoTolerance) {
                    this.tolerance = Number(nv.VideoTolerance);
                    this.tolerance = isNaN(this.tolerance) ? 0 : this.tolerance;
                    this.tolerance = this.tolerance <= 0 ? 0 : this.tolerance;
                }
            }, immediate: true, deep: true
        },
    },
    computed: {
        'color': function () {
            if (this.progress == 0) return '#909399';
            if (this.progress <= 30) return '#F56C6C';
            if (this.progress <= 60) return '#E6A23C';
            if (this.progress <= 80) return '#67C23A';
            return '#409EFF';
        },
        //完成度，加了容差之后的
        'progress': function () {
            return this.percent + this.tolerance >= 100 ? 100 : this.percent;
        },
    },
    mounted: function () { },
    methods: {
        onload: function () {
            var th = this;
            th.loading = true;
            $api.cache('Course/ProgressForVideo:5', { 'couid': this.couid, 'stid': this.stid })
                .then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.percent = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.percent = 0
                    console.error(err);
                });
        },
        gourl:function(){
            var url="/mobi/course/study";
            url = $api.url.set(url, { 'couid': this.couid });
            window.location.href = url;
        }
    },
    template: `<div @click="gourl()">
    <icon>&#xe83a</icon>
    <van-progress :percentage="progress" :color="color"></van-progress>
    </div>`
});