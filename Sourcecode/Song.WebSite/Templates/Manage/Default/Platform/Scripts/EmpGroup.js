
$ready(function () {
    window.vapp = new Vue({
        el: '#app',
        data: {
            form: {
                orgid: 0,
                name: '',
                size: 20,
                index: 1
            },
            organ: {},           //当前登录账号所在的机构
            loading: false,
            loadingid: 0,        //当前操作中的对象id           
            datas: [],     //数据源           
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [] //数据表中选中的行
        },
        created: function () {
            $api.get('Admin/Organ').then(function (req) {
                if (req.data.success) {
                    vapp.organ = req.data.result;
                    vapp.form.orgid = vapp.organ.Org_ID;
                    vapp.loadDatas();
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        methods: {
            //删除
            deleteData: function (datas) {
                if (datas == '') return;
                $api.delete('Admin/GroupDelete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vapp.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        window.vapp.loadDatas();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //加载数据页
            loadDatas: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 41);
                $api.post("Admin/GroupPager",this.form).then(function (d) {
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
                this.loadingid = row.Title_Id;
                $api.post('Admin/GroupModify', { 'entity': row }).then(function (req) {
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
                });
            },
            //在列中显示信息，包含检索
            showInfo: function (txt) {
                if (txt != '' && this.form.name != '') {
                    var regExp = new RegExp(this.form.name, 'g');
                    txt = txt.replace(regExp, `<red>${this.form.name}</red>`);
                }
                return txt;
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                this.$refs.btngroup.modifyrow(row);                
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
                                this.datas[i][indexkey] = i + 1;
                            }
                            this.changeTax();
                        });
                    }
                });
            },
            //更新排序
            changeTax: function () {
                var arr = $api.clone(this.datas);
                $api.post('Admin/GroupTaxis', { 'items': arr }).then(function (req) {
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


