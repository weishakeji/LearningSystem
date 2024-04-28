
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
        data: {
            site: $api.dot('web').toLowerCase(),
            datas: [],    //数据源
            selected_template: '',     //已经选中的模板
            detail: {},       //当前需要显示的模板
            showDetail: false,       //显示详情
            loading: false
        },
        created: function () {
            var apimethod = 'Template/WebTemplates';
            if (this.site == 'mobi') apimethod = 'Template/MobiTemplates';
            var th = this;
            th.loading = true;
            $api.cache(apimethod).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    //增加多条数据，用于显示是调试布局样式，没有实际业务意义
                    var j = 0;
                    do {
                        for (var i = 0; i < result.length; i++)
                            th.datas.push(result[i]);
                        j--;
                    }
                    while (j > 0);
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() => th.loading = false);
            //当前模版
            $api.get('Template/CurrentTemplate', { 'device': this.site }).then(function (req) {
                if (req.data.success) {
                    th.selected_template = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => { });
        },
        watch: {
            'detail': function (nv, ov) {
                console.log(nv);
            }
        },
        methods: {
            //选择当前模板的按钮事件
            btnSelectClick: function (item) {
                if (item.Tag == this.currTemplate) return;
                var th = this;
                this.$confirm('是否切换模版风格, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    $api.post('Template/SetCurrTemplate', { 'device': this.site, 'tag': item.Tag }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.currTemplate = result.Tag;
                            $api.put('Template/CurrentTemplate', { 'device': th.site }).then(function (rq) {
                                var result = rq.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!'
                                });
                            });
                        } else {
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    }).finally(() => { });
                }).catch(() => { });
            }
        }
    });

});
