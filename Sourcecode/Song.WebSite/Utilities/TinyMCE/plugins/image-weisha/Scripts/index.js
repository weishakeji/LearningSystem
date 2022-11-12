
window.vapp = new Vue({
    el: '#vapp',
    data: {
        uploadkey: $api.querystring('uploadkey'),   //上传资源的路径配置项，来自web.config中的upload节点
        dataid: $api.querystring('dataid'),         //数据项的id，例如试题id,新闻id
        editorid: $api.querystring('editorid'),     //编辑器的id
        param: $api.querystring('param'),           //来自编辑器的参数

        //organ: {},
        loading: false
    },
    mounted: function () {
        var th = this;
        th.loading = true;

    },
    updated: function () {

    },
    created: function () {

    },
    computed: {
    },
    watch: {
    },
    methods: {

    }
});
