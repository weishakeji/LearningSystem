//课程视频的学习进度
Vue.component('course_progress', {
    //config:机构的配置项，其实包括了视频完成度的容差值（VideoTolerance）
    props: ['course', 'stid', 'config'],
    data: function () {
        return {
            data: {},        //进度信息
            percent: 0,     //完成的真实百分比
            tolerance: 10,       //容差，例如容差为10，则完成90%即可认为是100%
            loading: false
        }
    },
    watch: {
        'course': {
            handler: function (nv, ov) {
                this.onload();
            }, immediate: true, deep: true
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
        onload: function () {
            var th = this;
            th.loading = true;
            $api.get('Course/LogForVideo:5', { 'couid': this.course.Cou_ID, 'stid': this.stid })
                .then(function (req) {                 
                    if (req.data.success) {
                        var result = req.data.result;
                        if (result != null && result.length > 0) {
                            th.data = result[0];
                            th.data.lastTime = new Date(th.data.lastTime);
                            th.percent = th.data.complete;
                            th.percent = 100;
                            //console.log(th.data);
                        } else {
                            th.data = null;
                            th.percent = 0;
                        }

                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //是否完成
        finished: function (percentage) {
            return percentage >= 100;
        },
        //进度条显示的数值样式
        format(percentage) {
            return this.finished(percentage) ? '100%' : percentage + '%';
        },
        //累计学习时间
        studyTime: function (time) {
            if (time < 60) return time + "秒";
            if (time >= 60) {
                var ss = time % 60;
                var mm = Math.floor(time / 60);
                if (mm < 60) {
                    return mm + "分" + ss + "秒";
                } else {
                    var hh = Math.floor(mm / 60);
                    mm = mm % 60;
                    return hh + '小时' + mm + "分" + ss + "秒";
                }
            }
            return time;
        },
        //查看课程学习记录详情
        viewDetail: function () {
            var item = this.course;
            var url = $dom.routepath() + 'VideoProgress.' + item.Cou_ID;
            url = '/Student/Course/VideoProgress.' + item.Cou_ID,
                url = $api.url.set(url, 'stid', this.stid);
            var obj = {
                'url': url,
                'ico': 'e6ef', 'min': false,pid:window.name,
                'title': item.Cou_Name + ' - 学习进度',
                'width': '60%',
                'height': '50%',
                'full': true,
                'showmask': true
            }
            if (window.top.$pagebox) {
                var pbox = window.top.$pagebox.create(obj);
                pbox.open();
            }

        }
    },
    template: `<div class="progress"  @click="viewDetail()">         
            <el-progress type="circle" :format="format" :stroke-width="15" :color="color"
             :percentage="progress" :width="80"></el-progress>
    </div>`
});