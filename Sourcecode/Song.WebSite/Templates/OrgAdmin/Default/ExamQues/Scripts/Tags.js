$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //关键字的查询条件
            form: { "orgid": "", "couid": "", "isdeleted": false, "name": "", "size": 8, "index": 1 },
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数           

            draweradd: false,   //添加的面板
            formadd: { "Org_ID": "", "Qtag_Name": "", "Qtag_Weight": 6, "Qtag_IsDeleted": false },
            formaddrules: {
                "Qtag_Name": [{ required: true, message: '不得为空', trigger: 'blur' },
                { min: 1, max: 500, message: '长度在 1 到 500 个字符', trigger: 'blur' },
                {
                    validator: function (rule, value, callback) {
                        var pat = /^[a-zA-Z0-9\u4e00-\u9fa5\s,，]*$/;
                        if (!pat.test(value))
                            callback(new Error('只允许字母、数字、汉字，和逗号'));
                        else callback();
                    }, trigger: 'blur'
                },
                { validator: validate.name.proh, trigger: 'change' },   //禁止使用特殊字符
                { validator: validate.name.danger, trigger: 'change' },
                ]
            },
            //编辑表单的验证
            editrules: {
                "Qtag_Name": [{ required: true, message: '不得为空', trigger: 'blur' },
                { min: 1, max: 500, message: '长度在 1 到 500 个字符', trigger: 'blur' },
                {
                    validator: function (rule, value, callback) {
                        var pat = /^[\u4e00-\u9fa5a-zA-Z0-9\s]*$/;
                        if (!pat.test(value))
                            callback(new Error('只允许字母、数字、汉字!'));
                        else callback();
                    }, trigger: 'blur'
                },
                { validator: validate.name.proh, trigger: 'change' },   //禁止使用特殊字符
                { validator: validate.name.danger, trigger: 'change' }
                ]
            },
            loadstate: {
                init: false,        //初始化
                add: false,         //默认              
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {
            this.handleCurrentChange();

            this.$refs.btngroup.addbtn({
                text: '全选/取消', tips: '选择标签',
                id: 'select', type: 'primary',
                icon: 'a057'
            });
        },
        created: function () {
            var th = this;
            th.form.orgid = window.org.Org_ID;
            th.formadd.Org_ID = window.org.Org_ID;


        },
        computed: {
            loading: function () {
                if (!this.loadstate) return false;
                for (let key in this.loadstate) {
                    if (this.loadstate.hasOwnProperty(key)
                        && this.loadstate[key])
                        return true;
                }
                return false;
            }
        },
        watch: {

        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                var th = this;
                if (index != null) this.form.index = index;
                //每页多少条，通过界面高度自动计算
                let area = $dom.height() - 100;
                th.form.size = Math.floor(area / 135) * 5;
                var loading = this.$fulloading();
                $api.get("ExamQues/TagPager", th.form).then(function (d) {
                    if (d.data.success) {
                        var result = d.data.result;
                        //console.error(result);
                        th.datas = result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => {
                    th.$nextTick(function () {
                        loading.close();
                    });
                });
            },
            //添加新标签事件
            btnadd: function () {
                var th = this;
                this.$refs["formadd"].validate((valid, fields) => {
                    if (valid) {
                        if (this.loadstate.add) return;
                        th.loadstate.add = true;
                        $api.post("ExamQues/TagAdd", { "entity": th.formadd })
                            .then(req => {
                                if (req.data.success) {
                                    let result = req.data.result;
                                    th.$message({
                                        message: '成功添加 ' + result + ' 个标签',
                                        type: 'success'
                                    });
                                    th.formadd.Qtag_Name = "";
                                    th.formadd.Qtag_Weight = 6;
                                    th.handleCurrentChange();
                                } else {
                                    console.error(req.data.exception);
                                    throw req.config.way + ' ' + req.data.message;
                                }
                            }).catch(err => console.error(err))
                            .finally(() => th.loadstate.add = false);
                    }
                });

            },
            //批量删除
            btnbatdel: function () {
                var arr = [];
                for (let i = 0; i < this.datas.length; i++)
                    if (this.datas[i].checked)
                        arr.push(this.datas[i].Qtag_ID);
                if (arr.length < 1) {
                    this.$message({
                        message: '请选中要操作的数据项',
                        type: 'error'
                    });
                    return false;
                }
                var th = this;
                this.$confirm('是否确认删除这 ' + arr.length + ' 项数据? ', '谨慎操作', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(function () {
                    th.btndel(arr.join(","));
                }).catch(function () { });
            },
            //删除
            btndel: function (id) {
                var th = this;
                th.loadstate.del = true;
                $api.delete('ExamQues/TagDelete', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loadstate.del = false);
            },
            //批量选择
            selectall: function () {
                let isselected = true;   //是否全选
                for (let i = 0; i < this.datas.length; i++) {
                    if (this.datas[i].checked == null || !this.datas[i].checked) {
                        isselected = false;
                        break;
                    }
                }
                for (let i = 0; i < this.datas.length; i++) {
                    this.$set(this.datas[i], 'checked', !isselected);
                }
            },
            //进入编辑状态
            editstate: function (item, index) {
                this.$set(item, 'editing', true);
                this.$set(this.datas, index, item);
                var entity = $api.clone(item);
                this.$set(item, 'clone', entity);
            },
            //保存编辑
            editsave: function (formname, item) {
                var th = this;
                var form = th.$refs[formname][0];
                form.validate((valid, fields) => {
                    if (valid) {
                        if (item.loading) return;
                        th.$set(item, 'loading', true);
                        $api.post("ExamQues/TagModify", { "entity": item.clone })
                            .then(req => {
                                if (req.data.success) {
                                    let result = req.data.result;
                                    let index = th.datas.findIndex(n => n.Qtag_ID == item.Qtag_ID);
                                    th.$set(th.datas, index, result);
                                    th.$message({
                                        message: '修改成功',
                                        type: 'success'
                                    });
                                } else {
                                    console.error(req.data.exception);
                                    throw req.config.way + ' ' + req.data.message;
                                }
                            }).catch(err => console.error(err))
                            .finally(() => th.$set(item, 'loading', false));
                    }
                });
            },
            //标签的颜色
            tagcolor: function (item) {
                let colors = ["info", "success", "warning", "primary", "danger"];
                return colors[Math.floor(item.Qtag_Weight / 2)];
            },
        },
        filters: {

        },
        components: {
            
        }
    });
});