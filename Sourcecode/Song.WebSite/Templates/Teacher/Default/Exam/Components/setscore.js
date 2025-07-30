//设置考试成绩
Vue.component('setscore', {
    //当前考试，当前成绩
    props: ['exam', 'exresult'],
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
            form: {
                score: 0,
                dura: 0,     //考试用时
                time: '',        //考试时间
                exrid: 0
            },
            rules: {
                time: [
                    { required: true, message: '考试时间不得为空', trigger: 'blur' },
                    { validator: validateDate, trigger: 'blur' }
                ],
            },
            //滑块的刻度
            scoremarks: { 0: '0 分' },
            durmarks: { 1: '1分钟' },    //用时的刻度

            loading: false
        }
    },
    watch: {
        'exam': {
            handler: function (nv, ov) {
                let exam = nv;
                //设置表单的值               
                this.scoremarks[exam.Exam_Total] = '满分';
                this.scoremarks[exam.Exam_PassScore] = exam.Exam_PassScore + ' 分及格'
                //考试用时的刻度
                this.durmarks[exam.Exam_Span] = exam.Exam_Span + '分钟';
                this.form.dura = Math.floor(Math.random() * exam.Exam_Span * 0.2 + exam.Exam_Span * 0.8);
            },
            immediate: false, deep: true
        },
        'exresult': function (nv, ov) {
            if ($api.isnull(this.exam) || $api.isnull(nv)) return;
            let exam = this.exam, exr = nv;
            //设置考试得分，不低于原始分和及格分
            let lowest = exr.Exr_ScoreFinal <= exam.Exam_PassScore ? exam.Exam_PassScore : exr.Exr_ScoreFinal;
            this.form.score = Math.round(Math.random() * (exam.Exam_Total - lowest) + lowest);
            //设置考试用时
            this.form.dura = Math.floor((Math.random() + 1) * exam.Exam_Span / 2);
            //设置考试时间
            this.form.time = exr.Exr_CrtTime;
        }
    },
    computed: {},
    mounted: function () {

    },
    methods: {
        //显示面板
        show: function (exr) {
            this.showpanel = true;
            this.exresult = exr;
            this.form.exrid = exr.Exr_ID;
        },
        //设置考试成绩
        setResultScore: function () {
            var th = this;
            this.$refs['updateform'].validate((valid) => {
                if (valid) {
                    var item = th.exresult;
                    //当前行正在处理中
                    th.$set(item, 'inprocess', true);
                    $api.get('Exam/ResultSetScore', th.form).then(function (req) {
                        if (req.data.success) {
                            let result = req.data.result;
                            th.$emit('update', result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => th.$set(item, 'inprocess', false));
                } else {
                    console.error('error submit!!');
                    return false;
                }
            });
        },
    },
    template: `<div v-if="exam && exresult && showpanel">
    <el-dialog :visible.sync="showpanel">
        <div slot="title">
            <icon large>&#xe6b0</icon>设置考试成绩 - <b title="学员名称">{{exresult.Ac_Name}}</b> （ 原分数 {{exresult.Exr_ScoreFinal}}  ）
        </div>
        <el-form ref="updateform" :model="form" label-width="100px" :rules="rules" >
            <el-form-item label="成绩得分">
                <el-slider v-model="form.score" :min="0" show-input :max="exam.Exam_Total" :marks="scoremarks">
                </el-slider>
            </el-form-item>
            <el-form-item label="">
                <help multi style="margin-top:15px;">实际成绩会略大于这个分数。
                <br/>例如：每道题2分，如果设置得分75，最终成绩会是76分。</help>
            </el-form-item>
            <el-form-item label="考试时间" prop="time">
                <el-date-picker v-model="form.time" type="datetime" placeholder="选择日期时间" :disabled="exam.Exam_DateType==1"></el-date-picker>                        
            </el-form-item>
            <el-form-item label="">
                <warning multi  v-if="exam.Exam_DateType==1">当前考试为“固定时间”，不允许更改考试时间。</warning>
                <warning multi  v-else>时间区间限定在<br/> {{exam.Exam_Date|date('yyyy-MM-dd HH:mm')}} 到 {{exam.Exam_DateOver|date('yyyy-MM-dd HH:mm')}} 之间。</warning>
            </el-form-item>
            <el-form-item label="用时(分钟）">
                <el-slider v-model="form.dura" :min="1" show-input :max="exam.Exam_Span" :marks="durmarks">
                </el-slider>
            </el-form-item>
        </el-form>
        <span slot="footer" class="dialog-footer">
            <el-button @click="showpanel = false">取 消</el-button>
            <el-button type="primary" @click="setResultScore()">确 定</el-button>
        </span>
    </el-dialog>
    </div> `
});