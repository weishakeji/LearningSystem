//课程视频的学习进度
$dom.load.css([$dom.pagepath() + 'Components/Styles/exam_test.css']);
Vue.component('exam_test', {
    //config:机构的配置项，其实包括了视频完成度的容差值（VideoTolerance）
    props: ['course', 'stid', 'config', 'purchase'],
    data: function () {
        return {
            datas: [],        //试卷信息
            finaltest: {},       //结课考试
            highest: -1,          //结课考试最高分
            loading: false
        }
    },
    watch: {
        'course': {
            handler: function (nv, ov) {
                this.onload();
            }, immediate: true, deep: true
        }
    },
    computed: {
        //是否存在结课考试
        final: function () {
            return JSON.stringify(this.finaltest) != '{}' && this.finaltest != null;
        }
    },
    mounted: function () { },
    methods: {
        onload: function () {
            var th = this;
            var couid = this.course.Cou_ID;
            th.loading = true;
            $api.get('TestPaper/ShowPager', { 'couid': couid, 'search': '', 'diff': '', 'size': 999999, 'index': 1 })
                .then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        var papers = result;
                        if (papers != null && papers.length > 0) {
                            for (let i = 0; i < papers.length; i++) {
                                if (papers[i].Tp_IsFinal) {
                                    th.finaltest = papers[i];
                                    papers.splice(i, 1);
                                };
                            }
                            th.datas = papers;
                        }
                        //如果有结课考试，则计算结课的最高成绩
                        if (th.final) {
                            th.getfinal_highest(th.stid, th.finaltest.Tp_Id);
                        } else {
                            th.highest = -1;
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.datas = [];
                    th.finaltest = {};
                    console.error(err);
                }).finally(() => th.loading = false);
        },
        //获取结课考试的最高成绩
        getfinal_highest: function (stid, tpid) {
            var th = this;
            if (stid <= 0 || tpid <= 0) return;
            th.loading = true;
            th.highest = -1;
            $api.get('TestPaper/ResultsAll', { 'stid': stid, 'tpid': tpid }).then(function (req) {
                if (req.data.success) {
                    var results = req.data.result;
                    var highest = -1;
                    for (let i = 0; i < results.length; i++) {
                        const el = results[i];
                        if (el.Tr_Score > highest) highest = el.Tr_Score;
                    }
                    th.highest = highest;
                    //如果结课成绩与课程购买记录中的不一致，更新购买记录中的结课成绩
                    if (th.purchase && highest >= 0 && th.purchase.Stc_ExamScore != highest) {
                        var form = { 'acid': stid, 'couid': th.purchase.Cou_ID, 'score': highest }
                        $api.post('TestPaper/ResultLogRecord', form).then(function (req) {
                            if (req.data.success) {
                                th.purchase.Stc_ExamScore = highest;
                                th.$notify({ type: 'success', message: '更新结课考试成绩' });
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err));
                    }
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }

            }).catch(function (err) {
                th.results = [];
                th.highest = -1;
                alert(err);
                console.error(err);
            }).finally(() => th.loading = false);
        },
        //跳转的地址
        //isgo:是否跳
        gourl: function (isgo) {
            if (!isgo) return "#";
            var url = '/web/course/detail';
            url = $api.url.dot(this.course.Cou_ID, url);
            url = $api.url.set(url, { 'tab': 'test' });
            return url;
        },
        gofinal: function (isgo) {
            if (!isgo) return "#";
            var url = '/web/test/paper';
            url = $api.url.dot(this.finaltest.Tp_Id, url);
            //url = $api.url.set(url, { 'tab': 'test' });
            return url;
        }
    },
    template: `<div class="exam_test">
        <div><span><icon>&#xe810</icon>测试/结课考试</span>           
        </div>
        <div class="exam_btns">
            <a :href="gourl(datas.length>0)" :target="datas.length>0 ? '_blank' : ''" :disabled="datas.length<1" 
                :title="'有'+datas.length+'个模拟测试'"
                :style="{'color':datas.length>0 ? '#67C23A' : ''}">
                    <icon>&#xe72f</icon>
                    <span v-if="datas.length>0">{{datas.length}}场测试</span>
                    <span v-else>没有测试</span>
            </a>
            <a :href="gofinal(final)" target="_blank" 
             :style="{'color':final ? '#409EFF' : ''}"  :target="final ? '_blank' : ''" :disabled="!final">
                <icon>&#xe816</icon>
                <span v-if="highest>=0">结课 ({{highest}}分)</span>
                <span v-else>结课考试</span>
             </a>
        </div>
    </div>`
});