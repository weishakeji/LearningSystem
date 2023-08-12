//试题类型
$dom.load.css([$dom.path() + 'Question/Components/Styles/ques_type.css']);
Vue.component('ques_type', {
    //showname：是否显示题型名称
    props: ["type", "types", "showname"],
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
        seticon: function (index) {
            try {
                return '&#x' + this.icons[index - 1];
            } catch {
                return '&#xa01f'
            }
        },
        settitle: function (index) {
            try {
                return this.types[index - 1] + '题';
            } catch {
                return '...'
            }
        }
    },
    mounted: function () {

    },
    template: `<span>
        <loading star v-if="isnull || typeindex==0"></loading>
        <icon ques_type v-else v-html="seticon(typeindex)" :title="settitle(typeindex)" :showname="showname"></icon> 
    </span>`
});