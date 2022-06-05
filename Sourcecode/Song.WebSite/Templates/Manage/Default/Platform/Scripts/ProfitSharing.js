$ready(function () {
    window.vapp = new Vue({
        el: '#app',
        data: {
            loading: false,
            loadingid: 0,        //当前操作中的对象id
            datas: [], //数据源           
            selects: [] //数据表中选中的行
        },
        watch: {
            'loading': function (val, old) {
                //console.log(val);
            }
        },
        created: function () {
            this.loadDatas();
        },
        methods: {
            //删除
            deleteData: function (datas) {
                $api.delete('ProfitSharing/ThemeDelete', {
                    'id': datas
                }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        window.vapp.loadDatas();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //加载数据页
            loadDatas: function () {
                $api.get('ProfitSharing/ThemeList').then(function (req) {
                    if (req.data.success) {
                        vapp.datas = req.data.result;
                        vapp.rowdrop();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                this.$refs.btngroup.modify(row[this.$refs.btngroup.idkey]);
            },
            //更改使用状态
            changeUse: function (row) {
                var th = this;
                this.loadingid = row.Ps_ID;
                $api.post('ProfitSharing/ThemeModify', { 'entity': row }).then(function (req) {
                    if (req.data.success) {
                        vapp.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    vapp.$alert(err, '错误');
                    th.loadingid = 0;
                });
            },
            //行的拖动
            rowdrop: function () {
                // 首先获取需要拖拽的dom节点            
                const el1 = document.querySelectorAll('table > tbody')[0];
                Sortable.create(el1, {
                    disabled: false, // 是否开启拖拽
                    ghostClass: 'sortable-ghost', //拖拽样式
                    handle: '.draghandle',     //拖拽的操作元素
                    animation: 150, // 拖拽延时，效果更好看
                    group: { // 是否开启跨表拖拽
                        pull: false,
                        put: false
                    },
                    onStart: function (evt) {
                    },
                    onMove: function (evt, originalEvent) {
                        if ($dom('table tr.expanded').length > 0) {
                            return false;
                        };
                        // Example: http://jsbin.com/tuyafe/1/edit?js,output
                        evt.dragged; // dragged HTMLElement
                        evt.draggedRect; // TextRectangle {left, top, right и bottom}
                        evt.related; // HTMLElement on which have guided
                        evt.relatedRect; // TextRectangle
                        originalEvent.clientY; // mouse position
                        // return false; — for cancel
                    },
                    onEnd: (e) => {
                        var table = this.$refs.datatables;
                        let indexkey = table.$attrs['index-key'];
                        let arr = this.datas; // 获取表数据
                        arr.splice(e.newIndex, 0, arr.splice(e.oldIndex, 1)[0]); // 数据处理
                        this.$nextTick(function () {
                            this.datas = arr;
                            for (var i = 0; i < this.datas.length; i++) {
                                this.datas[i][indexkey] = i * 1;
                            }
                            this.changeTax();
                        });
                    }
                });
            },
            //更新排序
            changeTax: function () {
                var arr = $api.clone(this.datas);
                $api.post('ProfitSharing/ModifyTaxis', { 'items': arr }).then(function (req) {
                    if (req.data.success) {
                        vapp.$notify({
                            type: 'success',
                            message: '修改顺序成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }
        }
    });

});