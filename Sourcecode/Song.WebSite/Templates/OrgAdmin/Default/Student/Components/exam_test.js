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

            showFinaldata: false,    //显示结课成绩
            //测试成绩
            results: [],
            result_query: {
                'stid': -1, 'tpid': -1, 'size': 9999, 'index': 1
            },
            total: 1, //总记录数
            totalpages: 1, //总页数
            loading_result: false,
            loading_id: -1,
            loading: false,


        }
    },
    watch: {
        'course': {
            handler: function (nv, ov) {
                this.onload();
            }, immediate: true, deep: true
        },
        'stid': {
            handler: function (nv, ov) {
                this.result_query.stid = nv;
            }, immediate: true
        },
        'finaltest': {
            handler: function (nv, ov) {
                var tpid = Number(nv.Tp_Id);
                tpid = isNaN(tpid) ? 0 : tpid;
                this.result_query.tpid = tpid;

                this.getresults(1);
            }, immediate: true, deep: true
        },
        //当打开结课成绩面板时
        showFinaldata: function (nv, ov) {
            if (nv == true) {
                //this.onload();
                //this.getresults(1);
            }
        }
    },
    computed: {
        //是否存在结课考试
        final: function () {
            return JSON.stringify(this.finaltest) != '{}' && this.finaltest != null;
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
            th.finaltest = {};
            var couid = this.course.Cou_ID;
            th.loading = true;
            $api.get('TestPaper/ShowPager', { 'couid': couid, 'search': '', 'diff': '', 'size': 999999, 'index': 1 })
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
                });
        },
        //获取结课考试的最高成绩
        getfinal_highest: function (stid, tpid) {
            var th = this;
            if (stid <= 0 || tpid <= 0) return;
            th.loading = true;
            th.highest = -1;
            $api.get('TestPaper/ResultsAll', { 'stid': stid, 'tpid': tpid }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    var results = req.data.result;
                    var highest = -1;
                    for (let i = 0; i < results.length; i++) {
                        const el = results[i];
                        if (el.Tr_Score > highest) highest = el.Tr_Score;
                    }
                    th.highest = highest;
                    //如果结课成绩与课程购买记录中的不一致，更新购买记录中的结课成绩
                    if (th.ispurchase && highest >= 0 && th.purchase.Stc_ExamScore != highest) {
                        var form = { 'acid': stid, 'couid': th.purchase.Cou_ID, 'score': highest }
                        $api.post('TestPaper/ResultLogRecord', form).then(function (req) {
                            if (req.data.success) {
                                th.purchase.Stc_ExamScore = highest;
                                th.$notify({ type: 'success', message: '更新结课考试成绩' });
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            //alert(err);
                            Vue.prototype.$alert(err);
                            console.error(err);
                        });
                    }
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }

            }).catch(function (err) {
                th.loading = false;
                th.results = [];
                th.highest = -1;
                Vue.prototype.$alert(err);
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
        },
        //获取成绩
        getresults: function (index) {
            if (index != null) this.result_query.index = index;
            var th = this;
            if (th.result_query.tpid <= 0) {
                th.results = [];
                return;
            }
            th.loading_result = true;
            $api.get('TestPaper/ResultsPager', th.result_query).then(function (req) {
                th.loading_result = false;
                if (req.data.success) {
                    th.total = req.data.total;
                    th.results = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }

            }).catch(function (err) {
                th.loading_result = false;
                th.results = [];
                console.error(err);
            });
        },
        //得分样式
        scoreStyle: function (score) {
            //总分和及格分
            var total = this.finaltest.Tp_Total;
            var passscore = this.finaltest.Tp_PassScore;
            if (score == total) return "praise";
            if (score < passscore) return "nopass";
            if (score < total * 0.8) return "general";
            if (score >= total * 0.8) return "fine";
            return "";
        },
        //删除成绩
        btnDelete: function (item) {
            var th = this;
            th.loading_id = item.Tr_ID;
            $api.delete('TestPaper/ResultDelete', { 'trid': item.Tr_ID }).then(function (req) {
                th.loading_id = -1;
                if (req.data.success) {
                    var result = req.data.result;
                    th.getresults(1);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.loading_id = -1;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
         //成绩回顾
         viewresult: function (data) {
            var url = '/student/test/Review';
            url = $api.url.set(url, {
                'tr': data.Tr_ID,
                'tp': data.Tp_Id,
                'couid': data.Cou_ID,
                'stid': data.Ac_ID
            });
            return url;
        }
    },
    template: `<div class="exam_test">
        <div v-if="loading"><loading></loading></div>
        <div><span><icon>&#xe810</icon>测试/结课考试</span></div>
        <div class="exam_btns">
            <a :href="gourl(datas.length>0)" :target="datas.length>0 ? '_blank' : ''" :disabled="datas.length<1" 
                :title="'有'+datas.length+'个模拟测试'"
                :style="{'color':datas.length>0 ? '#67C23A' : ''}">
                <icon>&#xe72f</icon>
                <span v-if="datas.length>0">{{datas.length}}场测试</span>
                <span v-else>没有测试</span>
            </a>
            <a href="#" 
            :style="{'color':final ? '#409EFF' : ''}" @click="showFinaldata=true">
               <icon>&#xe816</icon>
               <span v-if="highest>=0">结课 ({{highest}}分)</span>
               <span v-else>结课考试</span>
            </a>
          
        </div>
        <el-dialog title="提示" :visible.sync="showFinaldata" width="70%" height="70%">
            <span slot="title"><icon>&#xe816</icon>结课成绩 - 《{{course.Cou_Name}}》</span>
            <template v-if="results.length>0">
                <div v-for="(item,i) in results" class="result">
                    <template v-if="loading_id==item.Tr_ID">
                        <loading></loading>
                    </template>
                    <template v-else>
                        <span class="index">{{i+1}}.</span>
                        <span class="score">
                            <a :href="viewresult(item)" target="_blank" class="viewresult"> {{item.Tr_Score}}分</a>                       
                            <score :class="scoreStyle(item.Tr_Score)"></score>
                        </span>
                        <span class="time">{{item.Tr_CrtTime|date("yyyy-MM-dd HH:mm")}}</span>
                        <el-button-group class="btns">
                            <el-popconfirm confirm-button-text='是的' cancel-button-text='不用'
                                icon="el-icon-info" icon-color="red" title="确定删除吗？" @confirm="btnDelete(item)">
                                <el-link type="danger" plain icon="el-icon-delete" slot="reference">删除
                                </el-link>
                            </el-popconfirm>                         
                        </el-button-group>
                    </template>
                </div>
            </template>
            <div v-else>
                没有结课成绩
            </div>
        <span slot="footer" class="dialog-footer">
            <el-button @click="showFinaldata = false">取 消</el-button>
            <el-button type="primary" @click="showFinaldata = false">确 定</el-button>
        </span>
        </el-dialog>
    </div>`
});