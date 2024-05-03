
$ready(function () {

    window.vue = new Vue({
        el: '#app',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            data: [], //待选择的项
            value: []   //当前选中的项
        },
        created: function () {
            var th = this;
            //供选择的菜单项
            $api.get('ManageMenu/EnableRoot').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    for (var i = 0; i < result.length; i++) {
                        th.data.push({
                            key: result[i].MM_Id,
                            label: result[i].MM_Name
                        });
                    }
                    //...
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
            });
            //已经选中的菜单项
            $api.get('Purview/Own', { 'id': this.id, 'type': 'posi' }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    for (var i = 0; i < result.length; i++) {
                        th.value.push(result[i].MM_Id);
                    }
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
            });
        },
        methods: {
            btnEnter: function () {
                var th = this;
                if (this.loading) return;
                var mm=this.value.toString();
                $api.post('Purview/Save', { 'id': this.id, 'type': 'posi', 'mm': mm }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '操作成功',
                            center: true
                        });
                        th.operateSuccess();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //操作成功
            operateSuccess: function () {
                var name = $dom.trim(window.name);
                if (window.top.$pagebox) {
                    //当前pagebox窗体对象
                    var pagebox = window.top.$pagebox.get(name);
                    //tabs.js标签页的页面区域
                    var iframe = window.top.$dom('iframe[name=' + pagebox.pid + ']');
                    if (iframe.length > 0) {
                        var win = iframe[0].contentWindow;
                        //刷新父页面数据
                        //if (win) win.vue.loadDatas();
                    }
                    window.setTimeout(function () {
                        var name = $dom.trim(window.name);
                        window.top.$pagebox.shut(name);
                    }, 1000);
                }

            }
        },
    });

});
