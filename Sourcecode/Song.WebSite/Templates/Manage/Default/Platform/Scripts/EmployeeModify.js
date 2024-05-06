
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            //当前登录账号对象
            account: {
                Acc_IsUse: true,
                Acc_Photo: ''
            },
            position: [],   //岗位
            titles: [],      //职务或头衔
            accPingyin: [],  //账号名称的拼音
            organ: {},       //当前登录账号所在的机构
            rules: {
                Acc_Name: [
                    { required: true, message: '姓名不得为空', trigger: 'blur' }
                ],
                Acc_AccName: [
                    { required: true, message: '账号不得为空', trigger: 'blur' },
                    { min: 4, max: 50, message: '长度在 4 到 50 个字符', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (vapp.id != '') return callback();
                            var pat = /^[a-zA-Z0-9_-]{1,50}$/;
                            if (!pat.test(value))
                                callback(new Error('仅限字母与数字'));
                            else callback();
                        }, trigger: 'blur'
                    },
                    {
                        validator: async function (rule, value, callback) {
                            await vapp.isExist(value).then(res => {
                                if (res) callback(new Error('已经存在!'));
                            });
                        }, trigger: 'blur'
                    }
                ]
            },
            activeName: 'general',   //选项卡
            //图片文件
            upfile: null, //本地上传文件的对象   

            loading: false
        },
        watch: {
            'upfile': {
                handler(n, o) {
                    if (n == null) this.account.Acc_Photo = '';
                }, deep: true, immediate: true
            }
        },
        computed: {
            //是否新增账号
            isadd: t => t.id == null || t.id == '' || this.id == 0,
        },
        created: function () {
            var th = this;
            $api.get('Admin/Organ').then(function (req) {
                if (req.data.success) {
                    th.organ = req.data.result;
                    $api.bat(
                        $api.post('Position/EnableAll'),
                        $api.post("Admin/TitleEnabledList", { 'orgid': th.organ.Org_ID })
                    ).then(([posi, title]) => {
                        //获取结果
                        th.position = posi.data.result;
                        th.titles = title.data.result;
                    }).catch(err => console.error(err));
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });

            //如果是新增界面
            if (th.id == '') return;
            $api.post('Admin/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.account = result;
                    if (th.account.Posi_Id <= 0) th.account.Posi_Id = '';
                    if (th.account.Title_Id <= 0) th.account.Title_Id = '';
                    if (th.account.Acc_Photo != '')
                        th.upfile = {};
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
            //判断是否已经存在
            isExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('admin/IsExistAcc', { 'acc': val, 'id': th.id }).then(function (req) {
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
            btnEnter: function (formName, isclose) {
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        th.loading = true;
                        let apipath = th.id == '' ? api = 'Admin/add' : 'Admin/Modify';
                        if (th.id == '') th.account.Org_ID = th.organ.Org_ID;
                        //接口参数，如果有上传文件，则增加file
                        let para = {};
                        if (th.upfile == null || JSON.stringify(th.upfile) == '{}') para = { 'acc': th.account };
                        else
                            para = { 'file': th.upfile, 'acc': th.account };
                        $api.post(apipath, para).then(function (req) {
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
                        }).catch(function (err) {
                            th.$alert(err, '错误');
                        }).finally(() => th.loading = false);
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
            //名称转拼音
            pingyin: function () {
                this.accPingyin = makePy(this.account.Acc_Name);
                if (this.accPingyin.length > 0)
                    this.account.Acc_NamePinyin = this.accPingyin[0];
            },
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', isclose);
            }
        },
    });

}, ["../Scripts/hanzi2pinyin.js"]);
