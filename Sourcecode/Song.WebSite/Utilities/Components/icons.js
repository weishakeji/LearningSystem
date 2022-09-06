// 图标选择

Vue.component('icons', {
    props: ['selected', 'show'],
    data: function () {
        return {
            data: [],
            loading: false
        }
    },
    watch: {
        'selected': function (val, old) {
            this.$emit('change', val);
        }
    },
    computed: {

    },
    created: function () {
        var th = this;
        th.loading = true;
        //加载图标
        $api.cache('Platform/IconJson').then(function (req) {
            th.loading = false;
            if (req.data.success) {
                th.data = req.data.result;
            } else {
                throw req.data.message;
            }
        }).catch(function (err) {
            console.error(err);
        });
    },
    methods: {

    },
    template: `<div class="icons_select_panel">
    <div v-if="loading">loading...</div>
        <template v-for="ico in data">
            <span v-for="(value, key)  in ico" v-html="'&#x'+key+';'" @click="selected=key;show=false;"
            :title="value"  :selected="key==selected"></span>
        </template>   
    </div>`
});
