$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //机构id
            id: $api.querystring('id'),
            org: {},         //当前机构
            positions: [],       //岗位
            form: {
                orgid: 0, posi: 0, name: '',
                size: 20, index: 1
            },
            accounts: [], //账号列表
            total: 1, //总记录数
            totalpages: 1, //总页数

            dialogVisible: false,        //新增或编辑的面板
            current: {},                //要编辑的对象
            rules: {
                Acc_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Acc_AccName: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isExist(value).then(res => {
                                if (res) callback(new Error('当前账号已经存在!'));

                            });
                        }, trigger: 'blur'
                    }
                ],
                Acc_MobileTel: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            },
            //重置密码的面板
            pwVisible: false,
            pwEntify: {},            //要重置密码的对象
            pwrules: {
                Acc_Pw: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                Acc_Pw2: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (value !== vapp.pwEntify.Acc_Pw) {
                                callback(new Error('两次输入密码不一致!'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ],
            },

            loadingid: 0,        //当前操作中的对象id
            loading: false
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Organization/ForID', { 'id': th.id }),
                $api.get('Position/All4Organ', { 'orgid': th.id })
            ).then(axios.spread(function (org, posi) {
                //获取结果
                th.org = org.data.result;
                th.positions = posi.data.result;
                if (th.adminPosi.length < 1) {
                    th.creatPosi();
                }
                th.handleCurrentChange(1);
            })).catch(err => console.error(err))
                .finally(() => th.loading = false);

        },
        computed: {
            //当前机构的管理岗位
            adminPosi: function () {
                var arr = [];
                for (let i = 0; i < this.positions.length; i++) {
                    const element = this.positions[i];
                    if (element.Posi_IsAdmin) arr.push(element);
                }
                return arr;
            }
        },
        methods: {
            //创建岗位
            creatPosi: function () {
                var th = this;
                this.$confirm('当前机构没有管理岗位，是否立即创建管理岗?<br/>注：没有管理岗位，无法创建管理员', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    dangerouslyUseHTMLString: true,
                    type: 'warning'
                }).then(() => {
                    $api.post('Position/AddAdminPosi', { 'orgid': th.id, 'name': '机构管理' }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.$message({
                                type: 'success',
                                message: '操作成功!'
                            });
                            window.setTimeout(function () {
                                window.location.reload(true);
                            }, 500);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                }).catch(() => { });
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                $api.delete('Admin/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                th.form.orgid = this.id;
                if (th.adminPosi.length > 0) {
                    th.form.posi = th.adminPosi[0].Posi_Id;
                } else {
                    return;
                }
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 41);
                $api.get("Admin/pager", th.form).then(function (d) {
                    if (d.data.success) {
                        for (var i = 0; i < d.data.result.length; i++) {
                            //d.data.result[i].isAdminPosi = false;
                        }
                        th.accounts = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                this.dialogVisible = true;
                this.current = row;
            },
            //更改使用状态
            changeUse: function (row) {
                var th = this;
                this.loadingid = row.Acc_Id;
                $api.post('Admin/Modify', { 'acc': row }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    th.$alert(err, '错误');
                });
            },
            //重置密码的弹窗
            EmployeePwreset: function (row) {
                var pbox = top.$pagebox.create({
                    width: this.width ? this.width : 400,
                    height: this.height ? this.height : 300,
                    url: this.getfullpath(id),
                    pid: window.name,
                    id: window.name + '_' + id + '[modify]',
                    title: $dom('title').text() + ' - 修改'
                });
                pbox.open();
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = !!th.current.Acc_Id && Number(th.current.Acc_Id) > 0 ? api = 'Modify' : 'add';
                        th.current.Org_ID = th.org.Org_ID;
                        if (th.adminPosi.length > 0) {
                            th.current.Posi_Id = th.adminPosi[0].Posi_Id;
                        } else {
                            th.$alert('没有设置管理岗位', '错误');
                            th.loading = false;
                            return;
                        }
                        $api.post('admin/' + apipath, { 'acc': th.current }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.dialogVisible = false;
                                th.handleCurrentChange();
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            th.loading = false;
                            th.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //判断账号是否存在
            isExist: function (val) {
                return new Promise(function (resolve, reject) {
                    $api.get('Admin/IsExistAcc', { 'acc': val, 'id': vapp.current.Acc_Id }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            console.log(result);
                            return resolve(result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        return reject(err);
                    });
                });
            },
            //修改密码
            btnChangePw: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var form = { 'accid': th.pwEntify.Acc_Id, 'pw': th.pwEntify.Acc_Pw };
                        $api.post('Admin/ResetPw', form).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '修改密码成功!',
                                    center: true
                                });
                                th.pwVisible = false;
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        }).finally(() => th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
        }
    });

});