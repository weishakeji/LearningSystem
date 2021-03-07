$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            datas: [],
            loading: false,
            loadingid: false,
            selects: [], //数据表中选中的行   
            tmCount: 0,      //数据集的个数，用于获取机构数的返回结果是否完成
            tmProfit: 0      //用于获取分润方案的返回结果是否完成
        },
        created: function () {
            this.handleCurrentChange();
        },
        methods: {
            //删除
            deleteData: function (datas) {
                $api.delete('Organization/LevelDelete', {
                    'id': datas
                }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        window.vue.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //行的反选
            toggleSelection: function (rows) {
                if (rows) {
                    rows.forEach(row => {
                        this.$refs.multipleTable.toggleRowSelection(row);
                    });
                } else {
                    this.$refs.multipleTable.clearSelection();
                }
            },
            //选中行
            handleSelectionChange: function (val) {
                this.selects = val;
            },
            //加载数据页
            handleCurrentChange: function () {
                var th = this;
                $api.get('Organization/LevelAll').then(function (req) {
                    if (req.data.success) {
                        window.vue.datas = req.data.result;
                        vue.tmCount = vue.tmProfit = req.data.result.length;
                        for (var i = 0; i < window.vue.datas.length; i++) {
                            var item = window.vue.datas[i];
                            $api.get('Organization/LevelOrgcount', { 'id': item.Olv_ID }).then(function (req) {
                                if (req.data.success) {
                                    var id = req.config.parameters['id'];
                                    for (var j = 0; j < window.vue.datas.length; j++) {
                                        if (window.vue.datas[j].Olv_ID == id) {
                                            window.vue.datas[j]['count'] = req.data.result;
                                        }
                                    }
                                }
                                if (--vue.tmCount <= 0)
                                    window.vue.datas = $api.clone(window.vue.datas);
                            }).catch(function (err) {
                                console.error(err);
                            });
                            $api.get('ProfitSharing/ThemeForID', { 'id': item.Ps_ID }).then(function (req) {
                                if (req.data.success) {
                                    var id = req.config.parameters['id'];
                                    for (var j = 0; j < window.vue.datas.length; j++) {
                                        if (window.vue.datas[j].Ps_ID == id) {
                                            window.vue.datas[j]['psname'] = req.data.result.Ps_Name;
                                        }
                                    }

                                }
                                if (--vue.tmProfit <= 0)
                                    window.vue.datas = $api.clone(window.vue.datas);
                            }).catch(function (err) {
                                console.error(err);
                            });
                        }
                        th.rowdrop();
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
                var rowkey = this.$refs.datatable.rowKey;
                this.$refs.btngroup.modify(row[rowkey]);
            },
            //更改使用状态
            changeUse: function (row) {
                var th = this;
                this.loadingid = row.Olv_ID;
                $api.post('Organization/LevelModify', { 'entity': row }).then(function (req) {
                    if (req.data.success) {
                        vue.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    vue.$alert(err, '错误');
                });
            },
            //更改默认等级
            changeDefault: function (row) {
                var th = this;
                this.loadingid = row.Olv_ID;
                $api.post('Organization/LevelSetDefault', { 'id': row.Olv_ID }).then(function (req) {
                    if (req.data.success) {
                        vue.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    vue.$alert(err, '错误');
                });
            },
            //设置等级权限
            setPurview: function (row) {
                var th = this;
                var url = '/manage/organs/LevelPurview.html?id=' + row.Olv_ID;
                var title = "“" + row.Olv_Name + "”的权限设置";
                var boxid = window.name + '_' + row.Olv_ID + '[purview';
                this.$refs.btngroup.pagebox(url, title, boxid, 800, 600, { full: true });
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
                        // Example: http://jsbin.com/tuyafe/1/edit?js,output
                        evt.dragged; // dragged HTMLElement
                        evt.draggedRect; // TextRectangle {left, top, right и bottom}
                        evt.related; // HTMLElement on which have guided
                        evt.relatedRect; // TextRectangle
                        originalEvent.clientY; // mouse position
                        // return false; — for cancel
                    },
                    onEnd: (e) => {
                        var table = this.$refs.datatable;
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
                $api.post('Organization/LevelModifyTaxis', { 'items': arr }).then(function (req) {
                    if (req.data.success) {
                        vue.$notify({
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