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
        clickEvent: function () {
            var state = this.state == null ? false : this.state;
            console.log(state);
            this.$emit('change', !state);
        }
    },
    template: `<use_icon :class="{'el-icon-open':state,'el-icon-turn-off':!state}" @click.stop="clickEvent"
    :title="state ? '启用' : '禁用'">
    </use_icon>`
});
