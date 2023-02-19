
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            entity: {}, //当前数据对象
            childs: [],  //下级子项目
            rules: {
                Ps_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { min: 1, max: 50, message: '长度在 1 到 50 个字符', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isExist(value).then(res => {
                                if (res) callback(new Error('已经存在!'));
                            });
                        }, trigger: 'blur'
                    }
                ]
            },
            editindex: -1,       //编辑行的id
            newitem: {}  //用于记录新增分润项的对象
        },
        created: function () {
            this.newitem = this.createitem();
            //如果是新增界面
            if (this.id == '') {
                this.entity.Ps_IsUse = true;
            } else {
                //如果是修改界面
                var th = this;
                $api.get('ProfitSharing/ThemeForID', { 'id': this.id }).then(function (req) {
                    if (req.data.success) {
                        th.entity = req.data.result;
                        $api.get('ProfitSharing/ProfitList', { 'tid': th.entity.Ps_ID }).then(function (req) {
                            if (req.data.success) {
                                vapp.childs = req.data.result;
                                vapp.rowdrop();
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            }
        },
        methods: {
            //判断是否已经存在
            isExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('ProfitSharing/ThemeExist', { 'name': val, 'id': th.id }).then(function (req) {
                        if (req.data.success) {
                            return resolve(req.data.result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        return reject(err);
                    });
                });
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.modify();
                    }
                });
            },
            modify: function () {
                if (this.loading) return;
                this.loading = true;
                var th = this;
                var childs = $api.clone(this.childs);
                childs.forEach(c => c.Ps_ID = 0);
                $api.post('ProfitSharing/ProfitSave', { 'theme': th.entity, 'items': childs }).then(function (req) {
                    if (req.data.success) {
                        th.$message({
                            type: 'success',
                            message: '修改成功!',
                            center: true
                        });
                        th.operateSuccess();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }                   
                }).catch(function (err) {
                    th.loading = false;
                    alert(err, '错误');
                });
            },
            //增加分润项
            additem: function () {
                var obj = this.newitem;
                if (!this.verifyitem(obj, 'Ps_Moneyratio', '资金比例')) return;
                if (!this.verifyitem(obj, 'Ps_Couponratio', '卡券比例')) return;
                //
                obj.Ps_ID = new Date().getTime();
                obj.Ps_Level = this.childs.length;
                this.childs.push($api.clone(obj));
                this.newitem = this.createitem();
                function getint(str) {
                    var num = parseInt(str);
                    if (isNaN(num)) num = 0;
                    num = num < 0 ? 0 : num;
                    num = num > 100 ? 100 : num;
                    return num;
                }
            },
            //修改分润项
            modifyitem: function (obj) {
                if (!this.verifyitem(obj, 'Ps_Moneyratio', '资金比例')) return;
                if (!this.verifyitem(obj, 'Ps_Couponratio', '卡券比例')) return;
                this.editindex = -1;
            },
            //验证输入
            verifyitem: function (obj, key, msg) {
                obj[key] = getint(obj[key]);
                var max = this.getmax(key, obj);
                if (obj[key] > max || obj[key] < 0) {
                    this.$alert('用于“' + msg + "”的可分配空间：" + max + " %", '填写错误', {
                        confirmButtonText: '确定',
                        callback: action => {
                            obj[key] = max;
                        }
                    });
                    return false;
                }
                return true;
                function getint(str) {
                    var num = parseInt(str);
                    if (isNaN(num)) num = 0;
                    num = num < 0 ? 0 : num;
                    num = num > 100 ? 100 : num;
                    return num;
                }
            },
            //生成新分润对象
            createitem: function () {
                var obj = {
                    "Ps_ID": 0, "Ps_IsTheme": false, "Ps_IsUse": true,
                    "Ps_Level": 0, "Ps_Intro": "", "Ps_Moneyratio": 10,
                    "Ps_Couponratio": 0, "Ps_MoneyValue": 0, "Ps_CouponValue": 0,
                    "Ps_PID": 0, "Ps_Name": ""
                }
                return obj;
            },
            //获取最大可填写的值
            //key:属性名称
            //curr:当前项
            getmax: function (key, curr) {
                var currid = curr ? curr.Ps_ID : 0;
                var n = 0;
                this.childs.forEach(element => {
                    if (element.Ps_ID != currid)
                        n += element[key];
                });
                return 100 - n;
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.loadDatas', true);
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
                        let arr = this.childs; // 获取表数据
                        arr.splice(e.newIndex, 0, arr.splice(e.oldIndex, 1)[0]); // 数据处理
                        this.$nextTick(function () {
                            this.datas = arr;
                            for (var i = 0; i < this.datas.length; i++) {
                                this.datas[i][indexkey] = i * 1;
                            }                           
                        });
                    }
                });
            }
        },
    });

});
