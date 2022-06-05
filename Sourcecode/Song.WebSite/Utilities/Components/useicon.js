// 是否启用的图标

Vue.component('useicon', {
    props: ['state'],
    data: function () {
        return {
            data: [],
            loading: false
        }
    },
    watch: {
        
    },
    computed: {

    },
    created: function () {
        $dom.load.css(['/Utilities/Components/Styles/useicon.css']);
    },
    methods: {

    },
    template: `<use_icon :class="{'el-icon-open':state,'el-icon-turn-off':!state}"
    :title="state ? '启用' : '禁用'">
    </use_icon>`
});
