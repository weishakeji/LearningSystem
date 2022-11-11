//试题项的详细编辑
Vue.component('ques_ansedit', {
    //ans:试题选项
    props: ["ans"],
    data: function () {
        return {
            show: false,     //是否显示     
            index: -1,            //当前选项的索引号    
            text: ''         //文本内容
        }
    },
    watch: {
        'show': function (nv, ov) {
            if (!nv) {
                //关闭事件
                this.$emit('close', this.ans);
                return;
            }
        },
        text: function (nv, ov) {
            this.ans.Ans_Context = nv;
        }
    },
    computed: {
        //是否内容为空
        'isnull': function () {
            return this.ans == null || JSON.stringify(this.ans) == '{}';
        },
        'context': function () {
            if (this.isnull) return '';
            return this.ans.Ans_Context;
        }
    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'Question/Components/Styles/ques_ansedit.css']);
    },
    methods: {
        //设置试题选项
        set: function (ans, index) {
            this.ans = $api.clone(ans);
            this.index = index;
            this.$refs['editor_ques_ansedit'].setContent(ans.Ans_Context);
            if (!this.isnull) this.show = true;
        },
        //确定编辑
        editenter: function () {
            this.$emit('enter', this.ans);
            this.show = false;
        }
    },
    template: `<div v-show="show" class="showitem">
        <div slot="title">
            <icon>&#xe60d</icon>编辑“选项 {{index+1}}”
            <icon close @click="show = false"></icon>
        </div>
        <div slot="body">
            <editor :content="context" id="editor_ques_ansedit" ref="editor_ques_ansedit" :menubar="true"
                model="general" @change="t=>text=t"></editor>
        </div>
        <div slot="footer">
            <el-button type="cancel" @click="show = false">返 回</el-button>
            <el-button type="primary" define="enter" @click="editenter()">确认编辑</el-button>
        </div>
    </div>`
});
