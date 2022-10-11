$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),     //课程Id
            uid: $api.querystring('uid'),       //课程的uid
            prices: [],      //价格列表
            course: {},     //当前课程
            form: {

            },
            //新增价格的对象
            addobj: {
                CP_ID: 0,
                CP_Span: '',
                CP_Unit: '月',
                CP_Price: '',
                CP_Coupon: ''
            },
            //限时免费的起此时间
            limitTime: [new Date(), new Date(new Date().setDate(new Date().getDate() + 7))],
            rules: {
                CP_Span: [{ required: true, message: '不得为空', trigger: 'blur' },
                {
                    validator: function (rule, value, callback) {
                        var num = parseInt(value);
                        if (isNaN(num) || num <= 0) {
                            callback(new Error('请输入大于零的正整数'));
                        } else {
                            return callback();
                        }
                    }, trigger: ["change"]
                }
                ],
                CP_Price: [{ required: true, message: '不得为空', trigger: 'blur' },
                {
                    validator: function (rule, value, callback) {
                        var num = parseInt(value);
                        if (isNaN(num) || num <= 0) {
                            callback(new Error('请输入大于零的正整数'));
                        } else {
                            return callback();
                        }
                    }, trigger: ["change"]
                }]
            },
            editobj: {},        //当前要编辑的对象

            loadingid: 0,        //当前操作中的id
            loading: false,
            loading_course: false,       //课程信息加载的状态
            loading_init: true
        },
        mounted: function () {
            var th = this;
            if (th.id == '' || th.id == null) return;
            $api.get('Course/ForID', { 'id': th.id }).then(function (req) {
                th.loading_init = false;
                if (req.data.success) {
                    th.course = req.data.result;
                    document.title = '课程价格：' + th.course.Cou_Name;
                    //设置限时免费的时间                   
                    if (th.course.Cou_FreeStart)
                        th.$set(th.limitTime, 0, th.course.Cou_FreeStart);
                    if (th.course.Cou_FreeEnd)
                        th.$set(th.limitTime, 1, th.course.Cou_FreeEnd);
                    th.getprices();
                } else {
                    throw '未查询到数据';
                }
            }).catch(function (err) {
                th.$alert(err, '错误');
                console.error(err);
            });

        },
        created: function () {

        },
        computed: {

        },
        watch: {
            limitTime: {
                handler: function (nv, ov) {
                    this.course.Cou_FreeStart = nv[0];
                    this.course.Cou_FreeEnd = nv[1];
                    //console.log('Cou_FreeStart:', this.course.Cou_FreeStart.format('yyyy-M-d HH:mm:ss'));
                    //console.log('Cou_FreeEnd:', this.course.Cou_FreeEnd.format('yyyy-M-d HH:mm:ss'));
                },
                immediate: true,
                deep: true,
            }
        },
        methods: {
            //获取价格列表
            getprices: function () {
                var th = this;
                $api.get('Course/PriceItems', { 'uid': th.course.Cou_UID }).then(function (req) {
                    if (req.data.success) {
                        th.prices = req.data.result;
                        th.$nextTick(function () {
                            th.rowdrop();
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //表格跨列的处理
            arraySpanMethod: function ({ row, column, rowIndex, columnIndex }) {
                //console.log(row);
                if (row.CP_ID === this.editobj.CP_ID) {
                    if (columnIndex === 1) {
                        return [1, 2];
                    } else if (columnIndex === 2) {
                        return [0, 0];
                    }
                }
            },
            //设定行的样式
            tableRowClassName: function ({ row, rowIndex }) {
                if (row.CP_ID === this.editobj.CP_ID) {
                    return 'edit_row'
                }
            },
            //添加价格
            btnadd: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var obj = $api.clone(this.addobj);
                        if (obj.CP_Coupon > obj.CP_Price) {
                            th.$alert('抵用券不得大于价格“' + obj.CP_Price + '”元。', '提示', {
                                confirmButtonText: '确定',
                                callback: () => { }
                            });
                            return;
                        }
                        //判断是否重复
                        var exist = th.checkExist(obj);
                        if (exist) {
                            th.$alert('时长为“' + obj.CP_Span + obj.CP_Unit + '”的价格已经添加过了，请勿重复添加。',
                                '提示', {
                                confirmButtonText: '确定',
                                callback: () => { }
                            });
                            return;
                        }
                        //添加记录到服务端
                        obj.Cou_UID = th.uid;
                        obj.CP_IsUse = true;
                        th.loading = true;
                        $api.post('Course/PriceAdd', { 'entity': obj }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    message: '添加价格项成功',
                                    type: 'success'
                                });
                                th.getprices();
                                th.fresh_course();
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        });
                    }
                });
            },
            //保存价格项
            updateItem: function (formName, row) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var obj = $api.clone(row);
                        if (obj.CP_Coupon > obj.CP_Price) {
                            th.$alert('抵用券不得大于价格“' + obj.CP_Price + '”元。', '提示', {
                                confirmButtonText: '确定',
                                callback: () => { }
                            });
                            return;
                        }
                        //判断是否重复
                        var exist = th.checkExist(obj);
                        if (exist) {
                            th.$alert('时长为“' + obj.CP_Span + obj.CP_Unit + '”的价格已经添加过了，请勿重复设置。',
                                '提示', {
                                confirmButtonText: '确定',
                                callback: () => { }
                            });
                            return;
                        }
                        //如果并没有更改，则退出编辑行
                        if (JSON.stringify(obj) === JSON.stringify(th.editobj)) {
                            th.editobj = {};
                            return;
                        }
                        //添加记录到服务端
                        th.loading = true;
                        $api.post('Course/PriceModify', { 'entity': obj }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    message: '编辑价格项成功',
                                    type: 'success'
                                });
                                th.editobj = {};
                                th.getprices();
                                th.fresh_course();
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        });
                    }

                });
            },
            //校验是否存在重复设置项
            checkExist: function (obj) {
                if (this.prices.length < 1) return false;
                var exist = false;
                for (let i = 0; i < this.prices.length; i++) {
                    const el = this.prices[i];
                    if (el.CP_ID == obj.CP_ID) continue;
                    if (el.CP_Unit == obj.CP_Unit && el.CP_Span == obj.CP_Span) {
                        exist = true;
                        break;
                    }
                }
                return exist;
            },
            //更改价格选项
            changeUse: function (item) {
                var th = this;
                th.loadingid = item.CP_ID;
                $api.post('Course/PriceModify', { 'entity': item }).then(function (req) {
                    th.loadingid = 0;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '修改价格信息成功!',
                            center: true
                        });
                        th.editobj = {};
                        th.fresh_course();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //行的拖动
            rowdrop: function () {
                // 首先获取需要拖拽的dom节点            
                const el1 = document.querySelectorAll('table>tbody')[0];
                if (el1 == null) return;
                Sortable.create(el1, {
                    disabled: false, // 是否开启拖拽
                    ghostClass: 'sortable-ghost', //拖拽样式
                    handle: '.draghandle',     //拖拽的操作元素
                    animation: 150, // 拖拽延时，效果更好看
                    group: { // 是否开启跨表拖拽
                        pull: false,
                        put: false
                    },
                    onStart: function (evt) { },
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
                        var table = this.$refs.prices;
                        let indexkey = table.$attrs['index-key'];
                        let arr = $api.clone(this.prices); // 记录旧数据
                        this.prices.splice(e.newIndex, 0, this.prices.splice(e.oldIndex, 1)[0]); // 数据处理
                        this.$nextTick(function () {
                            for (var i = 0; i < this.prices.length; i++) {
                                this.prices[i][indexkey] = i + 1;
                            }
                            if ($api.toJson(this.prices) != $api.toJson(arr)) {
                                this.changeTax();
                            }
                        });
                    }
                });
            },
            //更新排序
            changeTax: function () {
                var th = this;
                th.loading = true;
                $api.post('Course/PriceUpdateTaxis', { 'items': this.prices }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改顺序成功!',
                            center: true
                        });
                        th.fresh_course();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('Course/PriceDelete', { 'id': datas }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.getprices();
                        th.fresh_course();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    console.error(err);
                });
            },
            //进入编辑状态
            goedit: function (row) {
                if (JSON.stringify(this.editobj) != '{}') {
                    var index = -1;
                    for (let i = 0; i < this.prices.length; i++) {
                        if (this.prices[i].CP_ID == this.editobj.CP_ID) {
                            index = i;
                            break;
                        }
                    }
                    this.backedit(index);
                }
                this.editobj = $api.clone(row);
            },
            //退出编辑状态
            backedit: function (index) {
                if (JSON.stringify(this.editobj) == '{}') return;
                this.$set(this.prices, index, this.editobj);
                this.editobj = {};
            },
            //更改课程的设置项
            updateCourse: function () {
                if (JSON.stringify(this.course) == '{}') return;
                var th = this;
                th.loading_course = true;

                this.course.Cou_FreeStart = this.limitTime[0];
                this.course.Cou_FreeEnd = this.limitTime[1];
                //去除不相关属性
                var obj = th.remove_redundance(th.course);
                $api.post('Course/ModifyJson', { 'course': obj }).then(function (req) {
                    th.loading_course = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            position: 'bottom',
                            message: '修改课程信息成功',
                            center: true
                        });
                        th.fresh_course();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //清理冗余的属性，仅保持当前form表单的属性，未在表单中的不提交到服务器
            remove_redundance: function (obj) {
                //表单中的字段
                var props = ['Cou_ID', 'Cou_IsLimitFree', 'Cou_FreeStart', 'Cou_FreeEnd'];
                var fields = this.$refs['course'].fields;
                for (var i = 0; i < fields.length; i++)
                    props.push(fields[i].prop);
                //obj的属性字段,如果表单上没有，则删除               
                for (let att in obj) {
                    var exist = false;
                    for (var i = 0; i < props.length; i++) {
                        if (att == props[i]) {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) delete obj[att];
                }
                console.log(obj);
                return obj;
            },
            //刷新课程列表中的状态
            fresh_course: function () {
                var win = window.parent;
                if (win && win.vapp) {
                    win.vapp.close_fresh('vapp.handleCurrentChange()');
                }
            }
        }
    });

});
