
//编辑器
Vue.component('editor', {
    props: ['content', "model", "toolbar", 'mincount', 'order'],
    data: function () {
        return {
            path: $dom.path(),   //模板路径
            show: false,         //是否显示，  
            datas: []
        }
    },
    watch: {

    },
    computed: {
    },
    mounted: function () {

    },
    methods: {

    },

    template: ``
});
