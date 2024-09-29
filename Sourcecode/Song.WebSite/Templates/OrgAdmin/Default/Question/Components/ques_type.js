//试题类型
$dom.load.css([$dom.path() + 'Question/Components/Styles/ques_type.css']);
Vue.component('ques_type', {
    //showname：是否显示题型名称
    //showicon：是否显示图标
    props: ["type", "types", "showname","showicon"],
    data: function () {
        return {
            //单选，多选，判断，简答，填空
            icons: ['e85b', 'a057', 'e74c', 'a055', 'e823']
        }
    },
    computed: {
        //是否为空
        'isnull': t => $api.isnull(t.type) || $api.isnull(t.types),
        typeindex: t => t.type == null ? 0 : Number(t.type)

    },
    methods: {
        //设置图标
        seticon: function (index) {
            if (index == undefined || index == null || index < 0 || (
                this.types && index > this.types.length
            )) return '&#xa01f';
            else return '&#x' + this.icons[index - 1];

        },
        //标题试题类型的名称
        settitle: function (index) {
            if (!this.types || index == undefined || index == null || index < 0 || (
                this.types && index > this.types.length
            )) return '...';
            else return this.types[index - 1] + '题';
        }
    },
    mounted: function () {

    },
    template: `<span>
        <loading star v-if="isnull || typeindex==0"></loading>
        <template v-else>
            <icon v-if="showicon || !showname" ques_type v-html="seticon(typeindex)" :title="settitle(typeindex)" :showname="showname"></icon> 
            <span v-else :title="settitle(typeindex)" :showname="showname">{{settitle(typeindex)}}</span>
        </template>
    </span>`
});