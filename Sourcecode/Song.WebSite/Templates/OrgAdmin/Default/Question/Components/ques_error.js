//试题错误
Vue.component('ques_error', {
    props: ["question"],
    data: function () {
        return {

        }
    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'Question/Components/Styles/ques_error.css']);
    },
    watch: {
        'question': {
            handler: function (nv, ov) {
                //nv.Qus_IsError = false;
            }, immediate: false
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
                this.question.Qus_IsError = false;
                var p = this.$parent;
                if (p) p.setindex();
            }).catch(() => {
            });
        }
    },
    template: `<div class="ques_error">
        <el-form ref="question" :model="question" @submit.native.prevent label-width="80px">            
            <el-form-item label="错误信息" prop="Qus_ErrorInfo">            
               <alert v-html="question.Qus_ErrorInfo" v-if="question.Qus_ErrorInfo!=''"></alert>
               <alert v-else>当前试题存在错误，但没有明确的错误提示，请仔细核对</alert>
            </el-form-item>
            <el-form-item label="" prop="Qus_IsError">
                <el-button type="primary" plain @click="changeError">确定没有错误，取消错误状态</el-button>
            </el-form-item>
        </el-form>
        </div>`
});