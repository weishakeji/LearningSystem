
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
    window.vue = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            activeName: 'general',      //选项卡
            
            account: {}, //当前登录账号对象
            position: [],   //岗位
            titles: [],      //职务或头衔
            accPingyin: [],  //账号名称的拼音
            organ: {},       //当前登录账号所在的机构
            rules: {
                Ac_Name: [
                    { required: true, message: '姓名不得为空', trigger: 'blur' }
                ],
                Acc_AcName: [
                    { required: true, message: '账号不得为空', trigger: 'blur' },
                    { min: 4, max: 20, message: '长度在 4 到 20 个字符', trigger: 'blur' }
                ]
            },
            loading: false
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.get('Account/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                    $api.get('Organization/ForID', { 'id': th.account.Org_ID }).then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            vue.organ = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
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
            });

        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = 'Account/Modify';
                        $api.post(apipath, { 'acc': vue.account }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                vue.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                window.setTimeout(function () {
                                    vue.operateSuccess();
                                }, 600);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //window.top.ELEMENT.MessageBox(err, '错误');
                            vue.$alert(err, '错误');
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
                var th = this;
                th.loading = true;
                $api.post('Account/ModifyPhoto', { 'file': file, 'account': th.account }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.account.Ac_Photo = result.Ac_Photo;
                        vue.$message({
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
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vue.handleCurrentChange', true);
            }
        },
    });

}, ["../Scripts/hanzi2pinyin.js",
"/Utilities/editor/vue-html5-editor.js",
"/Utilities/Components/education.js"]);
