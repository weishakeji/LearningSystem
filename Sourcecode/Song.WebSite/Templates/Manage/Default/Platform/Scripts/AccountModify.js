
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            account: {},    //当前账号对象
            position: [],   //岗位
            titles: [],      //职务或头衔
            accPingyin: [],  //账号名称的拼音
            organ: {},       //当前登录账号所在的机构
            rules: {
                Ac_Name: [
                     //{ required: true, message: '姓名不得为空', trigger: 'blur' },
                     { validator: validate.name.proh, trigger: 'change' },   //禁止使用特殊字符
                     { validator: validate.name.danger, trigger: 'change' },
                ],
                Ac_AccName: [
                    { required: true, message: '账号不得为空', trigger: 'blur' },
                    { min: 5, max: 50, message: '长度在 5 到 50 个字符', trigger: 'blur' },
                    { pattern: /^[a-zA-Z]|[a-zA-Z0-9_]{5,50}$/, message: '账号仅限字母、数字、下划线', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.vefify_accname_exist(value).then(res => {
                                if (res === true) {
                                    callback(new Error('账号已经存在'));
                                }
                            }).catch(err => callback(new Error(err)));
                        }, trigger: 'blur'
                    }
                ]
            },
            activeName: 'general',      //选项卡
            loading: false
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.get('Account/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                    th.pingyin();
                    $api.get('Organization/ForID', { 'id': th.account.Org_ID }).then(function (req) {
                        if (req.data.success) {
                            th.organ = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() => th.loading = false);
        },
        computed: {
            //是否存在账号
            isexist: t => !$api.isnull(t.account) && t.id != 0,
            //是否新增账号
            isadd: t => t.id == null || t.id == '' || this.id == 0,
            //学员的组是否存在
            sortexist: function () {
                return !$api.isnull(this.account) && this.account.Sts_ID != '' && !$api.isnull(this.accsort);
            },
            //是否已经绑定手机号
            isbindmobi: function () {
                if (!this.isexist) return false;
                let tel1 = this.account.Ac_MobiTel1;
                let tel2 = this.account.Ac_MobiTel2;
                if (tel2 == null || tel2 == '') return false;
                console.log(tel1 == tel2)
                return tel1 == tel2;
            }
        },
        methods: {
            //验证账号是否已经存在
            vefify_accname_exist: function (val) {
                var id = this.account.Ac_ID ? this.account.Ac_ID : 0;
                return new Promise(function (resolve, reject) {
                    $api.get('Account/IsExistAcc', { 'acc': val, 'id': id }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
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
             //绑定手机号
             bindmobi: function (val) {
                if (val && (this.account.Ac_MobiTel1 != null && this.account.Ac_MobiTel1 != ''))
                    this.account.Ac_MobiTel2 = this.account.Ac_MobiTel1;
                if (!val) this.account.Ac_MobiTel2 = '';
                console.log(val);
                console.log(this.isbindmobi);
            },
            btnEnter: function (formName, isclose) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = 'Account/Modify';
                        $api.post(apipath, { 'acc': th.account }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success', center: true,
                                    message: '操作成功!'
                                });
                                window.setTimeout(function () {
                                    th.operateSuccess(isclose);
                                }, 600);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err, '错误');
                        }).finally(() => th.loading = false);
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
            },
            handleAvatarSuccess: function (res, file) {
                if (file.status == "success") {
                    this.account.Ac_Photo = res.url;
                    this.btnEnter('account');
                }
            },
            beforeAvatarUpload: function (file) {
                const isJPG = file.type === 'image/jpeg' || file.type === 'image/png';
                const isLt2M = file.size / 1024 / 1024 < 2;

                if (!isJPG) {
                    this.$message.error('上传头像图片只能是 JPG 格式!');
                }
                if (!isLt2M) {
                    this.$message.error('上传头像图片大小不能超过 2MB!');
                }
                return isJPG && isLt2M;
            },
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', isclose);
            }
        },
    });

}, ["../Scripts/hanzi2pinyin.js",
"/Utilities/Components/education.js"]);
