//课程视频的学习进度
$dom.load.css([$dom.pagepath() + 'Components/Styles/video_progress.css']);
Vue.component('video_progress', {
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
                this.percent = this.ispurchase ? nv.Stc_StudyScore : 0;
            }, immediate: true
        },
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
        //学习进度的数据项
        'data': {
            handler: function (nv, ov) {
                if (nv == null) return;
                nv.complete = nv.complete >= 100 ? 100 : nv.complete;
                //如果实时计算的学习进度，大于购买记录中的，则记录在购买记录中
                if (nv.complete > this.purchase.Stc_StudyScore) {
                    var th = this;
                    $api.get('Course/LogForVideoRecord', { 'acid': th.stid, 'couid': th.course.Cou_ID, 'rate': nv.complete })
                        .then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                console.log(result);
                                th.purchase.Stc_StudyScore = result;
                                //触发更新事件
                                th.$emit('record', result, th.purchase);
                                //th.$notify({ type: 'success', message: '保存视频学习进度成功' });
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err));
                }
                //console.log(nv);
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
            this.percent = this.ispurchase ? this.purchase.Stc_StudyScore : 0;
            return this.percent + this.tolerance >= 100 ? 100 : this.percent;
        },
        //是否有购买记录
        ispurchase: function () {
            return JSON.stringify(this.purchase) != '{}' && this.purchase != null;
        }
    },
    mounted: function () { },
    methods: {
        onload: function () {
            var th = this;
            th.loading = true;
            $api.cache('Course/LogForVideo:5', { 'couid': this.course.Cou_ID, 'stid': this.stid })
                .then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        if (result != null && result.length > 0) {
                            th.data = result[0];
                            th.data.lastTime = new Date(th.data.lastTime);
                            th.percent = th.data.complete;
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
                .finally(function () {
                    //th.percent = 60;
                });
        },
        gourl: function () {
            var url = "/mobi/course/study";
            url = $api.url.set(url, { 'couid': this.course.Cou_ID });
            window.navigateTo(url);
        }
    },
    template: `<div @click="gourl()">
    <icon>&#xe83a</icon>
    <van-progress :percentage="progress" :color="color"></van-progress>
    </div>`
});