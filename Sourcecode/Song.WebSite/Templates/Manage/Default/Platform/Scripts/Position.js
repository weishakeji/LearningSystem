
$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            form: {
                name: '',
                size: 20,
                index: 1
            },
            loading: false,
            loadingid: 0,        //当前操作中的对象id         
            datas: [],     //数据源           
            selects: []      //数据表中选中的行
        },
        created: function () {
            this.loadDatas();
        },
        methods: {
            //删除
            deleteData: function (datas) {
                if (datas == '') return;
                $api.delete('Position/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        window.vue.loadDatas();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //加载数据页
            loadDatas: function () {
                var th = this;
                var area = document.documentElement.clientHeight - 100;
                $api.post("Position/all").then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.rowdrop();
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //更改使用状态
            changeUse: function (row) {
                var th = this;
                this.loadingid = row.Posi_Id;
                $api.post('Position/Modify', { 'posi': row }).then(function (req) {
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
            //双击事件
            rowdblclick: function (row, column, event) {
                this.$refs.btngroup.modifyrow(row)
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
                $api.post('Position/UpdateTaxis', { 'items': arr }).then(function (req) {
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


