//学历的组件

Vue.component('education', {
    //value:当前值
    //ctr:控件，默认是select，可以设置为span,只显示结果不进行选择
    props: ['value', 'ctr', 'width'],
    data: function () {
        return {
            datas: [
                { name: '小学', value: '81' },
                { name: '初中', value: '71' },
                { name: '高中', value: '61' },
                { name: '中等职业教育', value: '41' },
                { name: '大学（专科）', value: '31' },
                { name: '大学（本科）', value: '21' },
                { name: '硕士', value: '14' },
                { name: '博士', value: '11' },
                { name: '其它', value: '90' }]
        }
    },
    watch: {
        'value': function (val, old) {
            this.$emit('change', val);
        }
    },
    computed: {
        //控件类型
        'ctr_type': function () {
            if (this.ctr == '' || this.ctr == null) return 'select';
            return this.ctr;
        },
        //宽度
        'widthval': function () {
            if (!this.width) return '100%';
            if (this.width.indexOf('%') > -1) return this.width;
            if (this.width.indexOf('px') > -1) return this.width;
            return this.width + 'px';
        }
    },
    created: function () {
    },
    methods: {
        //根据value值，获取当前学历名称
        getname: function (val) {
            var item = this.datas.find(function (v) {
                return v.value == val;
            });
            if (item != null) return item.name;
            return '(未填写)';
        },
        //是否存在
        isexist: function (val) {
            var item = this.datas.find(function (v) {
                return v.value == val;
            });
            if (item != null) return true;
            return false;
        }
    },
    template: `<span class="weisha_education">
        <template v-if="ctr_type=='select'">
            <el-select v-model="value" placeholder="-- 学历 --" :style="'width:'+widthval">
            <icon slot="prefix">&#xe6112</icon>
            <el-option v-for="(item,i) in datas" :key="item.value" :label="item.name"
                :value="item.value">
                    <span style="float: left;margin-right: 10px;">{{ i+1 }}.&nbsp;</span>
                    <span style="float: left">{{ item.name }}</span>
                </el-option>
            </el-select>
        </template>
        <template v-if="ctr_type=='span'">
            <template v-if="isexist(value)">
                {{getname(value)}}
            </template>
            <span v-else style="color:#aaa;">(未填写)</span>
        </template>
    </span>`
});
