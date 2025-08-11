//批量创建考试成绩
$dom.load.css([$dom.pagepath() + 'Components/Styles/createbatchscore.css']);
Vue.component('createbatchscore', {
    //当前考试
    props: ['exam'],
    data: function () {
        //验证考试时间的方法
        var validateDate = (rule, value, callback) => {
            if (value != null) {
                if (!this.exam) return;
                if (this.exam.Exam_DateType == 2) {
                    if (value < this.exam.Exam_Date || value > this.exam.Exam_DateOver) {
                        return callback(new Error('考试时间超出限制'));
                    }
                }
                callback();
            }
        };
        return {
            //生成成绩的面板
            showpanel: false,
            scorerange: [0, 100],      //成绩范围
            durarange: [1, 60],        //考试用时范围
            timerange: [0, 0],        //考试时间范围

            form: { "examid": "", "minScore": "", "maxScore": "", "minTime": "", "maxTime": "", "minSpan": "", "maxSpan": "" },
            rules: {
                maxTime: [
                    { required: true, message: '考试时间不得为空', trigger: 'blur' },
                    { validator: validateDate, trigger: 'blur' }
                ],
            },
            //滑块的刻度
            scoremarks: { 0: '0 分' },
            durmarks: { 1: '1分钟' },    //用时的刻度

            progressvalue: {},       //执行的进度
            loading: false
        }
    },
    watch: {
        'exam': {
            handler: function (nv, ov) {
                if (nv == null) return;
                let exam = nv;
                this.form.examid = exam.Exam_ID;
                //设置表单的值               
                this.scoremarks[exam.Exam_Total] = '满分';
                this.scoremarks[exam.Exam_PassScore] = exam.Exam_PassScore + ' 分及格'
                //考试用时的刻度
                this.durmarks[exam.Exam_Span] = exam.Exam_Span + '分钟';
                this.scorerange = [exam.Exam_PassScore, exam.Exam_Total];
                //设置考试时间               
                this.timerange = [exam.Exam_Date, exam.Exam_DateOver];
                //持续时间
                this.durarange = [Math.floor(exam.Exam_Span / 3), exam.Exam_Span];

                this.getprogress();

            },
            immediate: false, deep: true
        },
        //得分区间
        'scorerange': function (nv, ov) {
            this.form.minScore = nv[0];
            this.form.maxScore = nv[1];
        },
        //持续时间
        'durarange': function (nv, ov) {
            this.form.minSpan = nv[0];
            this.form.maxSpan = nv[1];
        },
        //考试时间
        'timerange': function (nv, ov) {
            this.form.minTime = nv ? nv[0] : '';
            this.form.maxTime = nv ? nv[1] : '';
        },
    },
    computed: {
        //执行中
        processing: function () {
            return this.progressvalue.count > 0 || this.progressvalue.total > 0;
        },
        //处理完成
        completed: function () {
            return this.progressvalue.count <= 0 && this.progressvalue.total <= 0;
        },
        percentage: function () {
            if (this.progressvalue.total <= 0) return 0;
            let total = this.progressvalue.total;
            let count = this.progressvalue.count;
            return Math.floor((total - count) / total * 10000) / 100;

        }
    },
    mounted: function () {

    },
    methods: {
        //显示面板
        show: function () {
            this.showpanel = true;
        },
        //设置考试成绩
        setResultScore: function () {
            var th = this;
            this.$refs['updateform'].validate((valid) => {
                if (valid) {
                    //当前行正在处理中
                    th.loading = true;
                    $api.get('Exam/ResultAbsenceBatchScore', th.form).then(function (req) {
                        if (req.data.success) {
                            let result = req.data.result;
                            th.progressvalue = result;
                            //
                            let total = result.total;
                            let count = result.count;
                            //alert('共' + total + '人，成功创建' + (total - count) + '人');
                            th.getprogress();
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => th.loading = false);
                } else {
                    console.error('error submit!!');
                    return false;
                }
            });
        },
        //进度
        getprogress: function () {
            var th = this;
            $api.get("Exam/ResultAbsenceBatchScoreLoading", { "examid": th.form.examid })
                .then(req => {
                    if (req.data.success) {
                        th.progressvalue = req.data.result;
                        //未完成
                        if (th.processing) {
                            window.setTimeout(function () {
                                th.getprogress();
                            }, 500);
                        } else if (th.showpanel) {
                            th.$message({
                                message: '恭喜你，这是一条成功消息',
                                type: 'success'
                            });
                        }

                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => { });
        }
    },
    template: `<div v-if="exam && showpanel">
    <el-dialog :visible.sync="showpanel" width="80%">
        <div slot="title">
            <icon large>&#xe6b0</icon>批量创建缺考学员的考试成绩
        </div>
        <template v-if="completed">
            <el-form ref="updateform" :model="form" label-width="100px" :rules="rules" >
                <el-form-item label="成绩得分" class="range-item">
                    <el-slider v-model="scorerange" range :min="0" :max="exam.Exam_Total" :marks="scoremarks">
                    </el-slider>
                    <div>最低 {{scorerange[0]}} 分，最高 {{scorerange[1]}} 分</div>

                </el-form-item>
                <el-form-item label="">
                    <help multi style="margin-top:15px;">设置批量成绩的得分区间。
                    <br/>成绩随机生成，学员年龄越大、学历越低，得分越低，但不会低于上述设置的最低分。</help>
                </el-form-item>
                <el-form-item label="考试时间" prop="maxTime">
                <el-date-picker v-model="timerange" type="datetimerange" range-separator="至" start-placeholder="开始日期" end-placeholder="结束日期"></el-date-picker>                   
                </el-form-item>
                <el-form-item label="">
                    <warning multi  v-if="exam.Exam_DateType==1">当前考试为“固定时间”，不允许更改考试时间。</warning>
                    <warning multi  v-else>时间区间限定在<br/> {{exam.Exam_Date|date('yyyy-MM-dd HH:mm')}} 到 {{exam.Exam_DateOver|date('yyyy-MM-dd HH:mm')}} 之间。</warning>
                </el-form-item>
                <el-form-item label="考试用时" class="range-item">
                    <el-slider v-model="durarange" range :min="1" :max="exam.Exam_Span" :marks="durmarks">
                    </el-slider>
                    <div>用时 {{durarange[0]}} 分钟到 {{durarange[1]}} 分钟</div>

                </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button @click="showpanel = false" :disabled="loading">取 消</el-button>
                <el-button type="primary" @click="setResultScore()" :loading="loading">确 定</el-button>
            </span>
        </template>
        <template v-else>
            <div class="progress-info">
                <el-statistic group-separator="," :value="progressvalue.total" title="总数"></el-statistic>
                <el-statistic group-separator="," :value="progressvalue.total - progressvalue.count" title="完成"></el-statistic>
                <el-statistic group-separator="," :value="progressvalue.count" title="剩余"></el-statistic>            
            </div>
            <el-progress :text-inside="true" :stroke-width="26" :percentage="percentage"></el-progress>
            <div class="loading"><loading star>正在处理中...</loading></div>
        </template>

    </el-dialog>
    </div> `
});