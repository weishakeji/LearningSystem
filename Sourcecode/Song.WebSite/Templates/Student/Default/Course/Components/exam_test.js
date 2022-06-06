//课程视频的学习进度
$dom.load.css([$dom.pagepath() + 'Components/Styles/exam_test.css']);
Vue.component('exam_test', {
    //config:机构的配置项，其实包括了视频完成度的容差值（VideoTolerance）
    props: ['course', 'stid', 'config', 'purchase'],
    data: function () {
        return {
            datas: [],        //试卷信息
            finaltest: {},       //结课考试
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
            $api.get('TestPaper/ShowPager', { 'couid': couid, 'search': '', 'diff': '', 'size': Number.MAX_VALUE, 'index': 1 })
                .then(function (req) {
                    th.loading = false;
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
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
        },
        //跳转的地址
        //isgo:是否跳
        gourl: function (isgo) {
            if (!isgo) return "#";
            var url = '/web/course/detail';
            url = $api.url.dot(this.course.Cou_ID, url);
            url = $api.url.set(url, { 'tab': 'test' });
            return url;
        }
    },
    template: `<div class="exam_test">
        <div><span><icon>&#xe810</icon>测试/考试</span>           
        </div>
        <div class="exam_btns">
            <a :href="gourl(datas.length>0)" :target="datas.length>0 ? '_blank' : ''" :disabled="datas.length<1" 
             :style="{'color':datas.length>0 ? '#67C23A' : ''}">
                <icon>&#xe72f</icon>模拟测试
                <span>({{datas.length}})</span>
            </a>
            <a :href="gourl(final)" target="_blank" 
             :style="{'color':final ? '#409EFF' : ''}"  :target="final ? '_blank' : ''" :disabled="!final"><icon>&#xe816</icon>结课考试</a>
        </div>
    </div>`
});