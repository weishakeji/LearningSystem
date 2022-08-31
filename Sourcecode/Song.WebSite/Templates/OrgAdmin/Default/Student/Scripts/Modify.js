
$ready(function () {
    Vue.use(VueHtml5Editor, {
        showModuleName: true,
        image: {
            sizeLimit: 512 * 1024,
            compress: true,
            width: 500,
            height: 350,
            quality: 80
        }
    });
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id', 0),
            activeName: 'general',      //选项卡
            //当前账号
            account: {
                Ac_IsUse: true,
                Ac_IsPass: true,
                Ac_Photo: '',
                Ac_Name: '',
                Ac_AccName: ''
            },
            position: [],   //岗位
            titles: [],      //职务或头衔
            accPingyin: [],  //账号名称的拼音
            organ: {},       //当前登录账号所在的机构
            rules: {
                Ac_Name: [
                    { required: true, message: '姓名不得为空', trigger: 'blur' }
                ],
                Ac_AccName: [
                    { required: true, message: '账号不得为空', trigger: 'blur' },
                    { min: 4, max: 20, message: '长度在 6 到 50 个字符', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            await window.vapp.duplicate_check(value).then(res => {
                                if (res) callback(new Error('当前账号已经存在!'));

                            });
                        }, trigger: 'blur'
                    }
                ]
            },
            defaultpw: '',      //默认密码

            //图片文件
            upfile: null, //本地上传文件的对象   

            loading: false
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.get('Account/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            }).finally(function () {
                th.loading = false;
                th.getorgan();
                if (!th.isexist) {
                    $api.get('Account/DefaultPw').then(function (req) {
                        if (req.data.success) {
                            th.defaultpw = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        //alert(err);
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
                }
            });
        },
        computed: {
            //是否存在账号
            isexist: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null && !!this.account.Ac_ID;
            }
        },
        methods: {
            //获取机构
            getorgan: function (orgid) {
                var th = this;
                (th.isexist ?
                    $api.get('Organization/ForID', { 'id': th.account.Org_ID }) :
                    $api.get('Organization/Current'))
                    .then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            th.organ = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = th.id == '' ? api = 'Account/add' : 'Account/Modify';
                        if (th.id == '') th.account.Org_ID = th.organ.Org_ID;
                        //接口参数，如果有上传文件，则增加file
                        var para = {};
                        if (th.upfile == null || JSON.stringify(th.upfile) == '{}') para = { 'acc': th.account };
                        else
                            para = { 'file': th.upfile, 'acc': th.account };
                        $api.post(apipath, para).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                window.setTimeout(function () {
                                    th.operateSuccess();
                                }, 600);
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
            //名称转拼音
            pingyin: function () {
                this.accPingyin = makePy(this.account.Ac_Name);
                if (this.accPingyin.length > 0)
                    this.account.Ac_Pinyin = this.accPingyin[0];
                //console.log(this.accPingyin);
            },
            //图片文件上传
            filechange: function (file) {
                if (!this.isexist) return;
                var th = this;
                th.loading = true;
                $api.post('Account/ModifyPhoto', { 'file': file, 'account': th.account }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.account.Ac_Photo = result.Ac_Photo;
                        th.$message({
                            type: 'success',
                            message: '上传头像成功!',
                            center: true
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
            //判断账号是否存在
            duplicate_check: function (val) {
                return new Promise(function (resolve, reject) {
                    $api.get('Account/IsExistAcc', { 'acc': val, 'id': vapp.account.Ac_ID }).then(function (req) {
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
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vue.handleCurrentChange', true);
            }
        },
    });

}, ["../Scripts/hanzi2pinyin.js",
    "/Utilities/editor/vue-html5-editor.js",
    "/Utilities/Components/education.js"]);
