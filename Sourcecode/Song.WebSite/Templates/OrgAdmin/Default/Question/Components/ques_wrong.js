//试题错误
Vue.component('ques_wrong', {
    props: ["question"],
    data: function () {
        return {
            list: [],       //反馈的信息列表
            other: ''       //反馈的其它信息
        }
    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'Question/Components/Styles/ques_wrong.css']);
    },
    watch: {
        'question': {
            handler: function (nv, ov) {
                if (!nv || !nv.Qus_WrongInfo) return;
                //反馈的信息
                var info = nv.Qus_WrongInfo;
                var wrong = '';
                if (info.indexOf("；") > -1) {
                    this.other = info.substring(info.indexOf("；") + 1);
                    wrong = info.substring(0, info.indexOf("；"));
                } else {
                    wrong = info;
                }
                this.list = wrong.split(',');
                console.log(this.list);
            }, immediate: true
        },
    },
    methods: {
        //更改状态状态
        changeError: function () {
            this.$confirm('是否确定当前试题没有错误?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                this.question.Qus_IsWrong = false;
                var p = this.$parent;
                if (p) p.setindex();
            }).catch(() => {
            });
        }
    },
    template: `<div class="ques_wrong">
        <el-form ref="question" :model="question" @submit.native.prevent label-width="80px">            
            <el-form-item label="反馈信息" prop="Qus_ErrorInfo">            
               <alert v-if="question.Qus_WrongInfo==''">学员反馈试题有问题，但没有明确问题内容，请认真核查</alert>
               <template v-else>
                    <div v-for="(item,i) in list">
                        {{i+1}}. {{item}}
                    </div>                                   
               </template>
            </el-form-item>
            <el-form-item label="其它" prop="Qus_IsWrong" v-if="other!=''">
                {{other}}
            </el-form-item>
            <el-form-item label="" prop="Qus_IsWrong">
                <el-button type="primary" plain @click="changeError">反馈的问题不存在，或反馈问题已经修正，确定修改状态为正常</el-button>
            </el-form-item>
        </el-form>
        </div>`
});