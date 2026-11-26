
$ready(["/Utilities/Components/upload-excel.js"], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            org: {},
            config: {},      //当前机构配置项        
            datas: {},
        },
        mounted: function () {
            var th = this;
            th.org = window.org;
            th.config = window.config;
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {

            //完成导入的事件
            finish: function (count) {
                var pagebox = window.top.$pagebox;
                if (pagebox && pagebox.source.top)
                    pagebox.source.tab(window.name, 'vapp.handleCurrentChange', false);
            },
        }
    });
});
