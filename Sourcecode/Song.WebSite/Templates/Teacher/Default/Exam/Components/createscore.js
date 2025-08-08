//创建考试成绩
Vue.component('createscore', {
    //当前考试，指定的学员
    props: ['exam', 'account'],
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
            form: { "examid": "", "acid": "", "score": "", "time": "", "dura": "" },
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
                this.form.examid = exam.Exam_ID;
                //设置表单的值               
                this.scoremarks[exam.Exam_Total] = '满分';
                this.scoremarks[exam.Exam_PassScore] = exam.Exam_PassScore + ' 分及格'
                //考试用时的刻度
                this.durmarks[exam.Exam_Span] = exam.Exam_Span + '分钟';
                this.form.dura = Math.floor(Math.random() * exam.Exam_Span * 0.2 + exam.Exam_Span * 0.8);
            },
            immediate: false, deep: true
        },
        'account': function (nv, ov) {
            if ($api.isnull(this.exam) || $api.isnull(nv)) return;
            let exam = this.exam;
            //设置考试得分，不低于原始分和及格分
            let lowest = exam.Exam_PassScore;
            this.form.score = Math.round(Math.random() * (exam.Exam_Total - lowest) + lowest);
            //设置考试时间
            if (exam.Exam_DateType == 1) this.form.time = exam.Exam_Date;
            if (exam.Exam_DateType == 2) {
                //生成随机开始时间
                let maxspan = (exam.Exam_DateOver.getTime() - exam.Exam_Date.getTime()) / 1000;
                if (maxspan > 31536000) maxspan = 31536000;   //最长一年
                else maxspan = maxspan / 3;
                this.form.time = new Date(exam.Exam_Date.getTime() + Math.round(Math.random() * (maxspan)) * 1000);
            }
            //设置考试用时
            this.form.dura = Math.floor((Math.random() + 1) * exam.Exam_Span / 2);
        }
    },
    computed: {},
    mounted: function () {

    },
    methods: {
        //显示面板
        show: function (acc) {
            this.showpanel = true;
            this.account = acc;
            this.form.acid = acc.Ac_ID;
        },
        //设置考试成绩
        setResultScore: function () {
            var th = this;
            this.$refs['updateform'].validate((valid) => {
                if (valid) {
                    //当前行正在处理中
                    th.loading = true;
                    /*
                    window.setTimeout(function () {
                        th.loading = false
                    }, 6000);
                    return;*/
                    $api.get('Exam/ResultCreateScore', th.form).then(function (req) {
                        if (req.data.success) {
                            let result = req.data.result;
                            th.$emit('update', result);
                            th.showpanel = false;
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
    },
    template: `<div v-if="exam && account && showpanel">
    <el-dialog :visible.sync="showpanel" width="80%">
        <div slot="title">
            <icon large>&#xe6b0</icon>创建考试成绩 - <b title="学员名称">{{account.Ac_Name}}</b> 
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
            <el-button @click="showpanel = false" :disabled="loading">取 消</el-button>
            <el-button type="primary" @click="setResultScore()" :loading="loading">确 定</el-button>
        </span>
    </el-dialog>
    </div> `
});