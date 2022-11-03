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
                //如果实时计算的学习进度，大于购买记录中的，则记录在购买记录中
                if (nv.complete && nv.complete != this.purchase.Stc_StudyScore) {
                    var th = this;
                    $api.get('Course/LogForVideoRecord', { 'acid': th.stid, 'couid': th.course.Cou_ID, 'rate': nv.complete })
                        .then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                //th.$notify({ type: 'success', message: '保存通过率成功' });
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            //alert(err);
                            //Vue.prototype.$alert(err);
                            console.error(err);
                        });
                }
                console.log(nv);
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
            this.percent = this.ispurchase ? this.purchase.Stc_StudyScore : 0;
            this.percent = (this.percent + this.tolerance) >= 100 ? 100 : this.percent;
            return Math.round(this.percent * 100) / 100;
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
                            console.log(th.data);
                        } else {
                            th.data = null;
                            th.percent = 0;
                        }

                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                }).finally(function () {
                    //th.percent = 60;
                });;
        },
        //是否完成
        finished: function (percentage) {
            return percentage >= 100;
        },
        //进度条显示的数值样式
        format(percentage) {
            return this.finished(percentage) ? '完成学习！100%' : '进度：' + percentage + '%';
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
        viewDetail: function (item) {
            var item = this.course;
            var url = $dom.routpath() + 'VideoProgress.' + item.Cou_ID;
            url = '/orgadmin/Student/DetailsView.' + item.Cou_ID,
                url = $api.url.set(url, 'stid', this.stid);
            var obj = {
                'url': url,
                'ico': 'e6ef', 'min': false,
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

        },
        //判断是否是一个有效时间
        judgmenttime: function (date) {
            if ($api.getType(date) != 'Date') return false;
            date = date.setDate(date.getFullYear() + 100);
            if (date < new Date()) return false;
            return true;
        }
    },
    template: `<div class="video_progress">
       <div><span><icon>&#xe761</icon>视频学习</span>
            <template v-if=" JSON.stringify(data) != '{}' && data != null && judgmenttime(data.lastTime)">
                <el-tag type="success"><icon>&#xa039</icon>{{data.lastTime|date('yyyy-MM-dd HH:mm')}}</el-tag>               
            </template>   
        </div>
        <div class="progress">   
            <el-tooltip effect="light" content="点击进度条，查看详情" placement="bottom-end"> 
                <el-progress :text-inside="true" :format="format" :stroke-width="20" :color="color"
                @click.native="viewDetail(course)"
                :status="progress>=100 ? 'success' : ''" :percentage="progress"
                style="width:100%"></el-progress>   
            </el-tooltip>
        </div> 
    </div>`
});