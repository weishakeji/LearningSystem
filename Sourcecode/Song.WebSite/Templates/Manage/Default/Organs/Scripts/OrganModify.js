
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            //当前对象    
            entity: {
                Org_IsUse: true, Org_IsShow: true, Org_IsPass: true
            },
            levels: [], //机构等列表
            domain: '',  //主域
            lv_id: '',   //当前机构等级
            rules: {
                Org_PlatformName: [
                    { required: true, message: '平台名称不得为空', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isPlatefromExist(value).then(res => {
                                if (res) callback(new Error('平台名称已经存在!'));
                            });
                        }, trigger: 'blur'
                    }
                ],
                Org_Name: [
                    { required: true, message: '机构名称不得为空', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isNameExist(value).then(res => {
                                if (res) callback(new Error('机构名称已经存在!'));
                            });
                        }, trigger: 'blur'
                    }
                ],
                Org_AbbrName: [
                    { required: true, message: '机构简称不得为空', trigger: 'blur' }
                ],
                Org_TwoDomain: [
                    { required: true, message: '平台域名不得为空', trigger: 'blur' },
                    { min: 1, max: 20, message: '长度在 1 到 20 个字符', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            var pat = /^[a-zA-Z0-9_-]{1,20}$/;
                            if (!pat.test(value))
                                callback(new Error('域名仅限字母与数字!'));
                            else callback();
                        }, trigger: 'blur'
                    },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isDomainExist(value).then(res => {
                                if (res) callback(new Error('域名已经存在!'));
                            });
                        }, trigger: 'blur'
                    },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isDomainAllow(value).then(res => {
                                if (!res) callback(new Error('该域名被保留使用'));
                            });
                        }, trigger: 'blur'
                    }
                ]
            },
            activeName: 'general',   //选项卡
            mapshow: false,      //是否显示地图信息
            loading: false
        },
        watch: {
            'lv_id': function (nl, ol) {
                this.entity.Olv_ID = nl;
            }
        },
        computed: {
            //是否新增账号
            isadd: t => t.id == null || t.id == '' || this.id == 0
        },
        created: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/LevelAll', { 'search': '', 'use': '' }),
                $api.get('Platform/Domain')
            ).then(([level, domain]) => {
                //获取结果
                th.levels = level.data.result;
                th.domain = domain.data.result;
                //默认机构
                var deflevel = th.levels.filter(item => item.Olv_IsDefault);
                if (deflevel.length > 0) th.lv_id = deflevel[0].Olv_ID;
                if (th.id != '') {
                    $api.get('Organization/ForID', { 'id': th.id }).then(function (req) {
                        if (req.data.success) {
                            th.entity = req.data.result;
                            th.lv_id = th.entity.Olv_ID;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                }
            }).catch(function (err) {
                console.error(err);
            });
        },
        methods: {
            //获取实体信息
            getentity: function () {
                var th = this;
                return new Promise(function (resolve, reject) {
                    if (th.id == '' || th.id == 0) {
                        resolve(th.entity);
                    } else {
                        $api.get('Organization/ForID', { 'id': th.id }).then(function (req) {
                            if (req.data.success) {
                                resolve(req.data.result);
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => reject(err));
                    }
                });
            },
            //判断平台名称是否存在
            isPlatefromExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('Organization/ExistPlatform', { 'name': val, 'id': th.id }).then(function (req) {
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
            //判断机构名称是否存在
            isNameExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('Organization/ExistName', { 'name': val, 'id': th.id }).then(function (req) {
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
            //判断域名是否已经存在
            isDomainExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('Organization/ExistDomain', { 'name': val, 'id': th.id }).then(function (req) {
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
            //判断域名是否允许，如果属于限定的域名，则不允许使用
            isDomainAllow: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('Organization/DomainAllow', { 'name': val }).then(function (req) {
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
            //保存信息
            btnEnter: function (formName, isclose) {
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        th.loading = true;
                        var query = null;
                        if (this.id == '') {
                            query = $api.post('Organization/add', { 'entity': th.entity });
                        } else {
                            query = $api.post('Organization/Modify', { 'entity': th.entity, 'exclude': '' });
                        }
                        query.then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.operateSuccess(isclose);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(err => alert(err)).finally(() => th.loading = false);
                    } else {
                        //未通过验证的字段
                        let field = Object.keys(fields)[0];
                        let label = $dom('label[for="' + field + '"]');
                        while (label.attr('tab') == null)
                            label = label.parent();
                        th.activeName = label.attr('tab');
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', isclose);
            }
        },
    });

}, ['/Utilities/baiduMap/convertor.js',
    '/Utilities/baiduMap/map_setup.js']);
