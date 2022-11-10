//试题项的简单编辑
Vue.component('ques_ansitem', {
    //ans:试题选项
    props: ["ans"],
    data: function () {
        return {
            show: false,     //是否显示              
            text: ''         //文本内容
        }
    },
    watch: {
        'show': function (nv, ov) {
            //编辑框下面的按钮组是否禁用
            var btnCrt = this.$parent.$parent.$refs['enter_button'];
            if (btnCrt) btnCrt.disabled = nv;
            //关闭的效果
            if (!nv) {
                //其它答题项的高度还原
                var rows = $dom('#ansitems .text');
                rows.height('auto');
                //关闭事件
                this.$emit('close', this.ans);
                return;
            }
            //设置当前选项的高度
            var minheight = 200;
            var row = $dom('#ans_' + this.ans.Ans_ID);
            if (row.height() < minheight) row.height(minheight);
            //选项文本所在位置
            var panel = $dom('.panel>div[remark="试题"]');
            var area = $dom('.ques_ansitem .editor_box');
            var offset = panel.offset();
            area.top(row.top() - offset.top + panel[0].scrollTop);
            area.height(row.height() < minheight ? minheight : row.height());
            area.css('padding-left', (row.left() - offset.left) + 'px');
            $dom('.ques_ansitem').height(panel[0].scrollHeight);
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
        $dom.load.css([$dom.path() + 'Question/Components/Styles/ques_ansitem.css']);
    },
    methods: {
        //设置试题选项
        set: function (ans) {
            this.ans = $api.clone(ans);     
            this.$refs['editor_ques_ansitem'].setContent(ans.Ans_Context);
            if (!this.isnull) this.show = true;
        }
    },
    template: `<div class="ques_ansitem" v-show="show" @click="show=false">
        <div class="editor_box">
            <editor :content="context" ref="editor_ques_ansitem" :menubar="false" model="mini"
                   @change='t=>text=t' ></editor>
        </div>
    </div>`
});
