
$ready(function () {

    window.vue = new Vue({
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
                    { min: 4, max: 20, message: '长度在 4 到 20 个字符', trigger: 'blur' }
                ]
            },

            //图片文件
            upfile: null, //本地上传文件的对象   

            loading: false
        },
        watch: {
            'upfile': {
                handler(n, o) {
                    if (n == null) this.account.Acc_Photo = '';
                },
                deep: true,
                immediate: true
            }
        },
        computed: {
            //是否新增对象
            isadd: t => { return t.id == null || t.id == ''; },
        },
        created: function () {
            var th = this;
            $api.get('Admin/Organ').then(function (req) {
                if (req.data.success) {
                    th.organ = req.data.result;
                    $api.bat(
                        $api.post('Position/EnableAll'),
                        $api.post("Admin/TitleEnabledList", { 'orgid': vue.organ.Org_ID })
                    ).then(axios.spread(function (posi, title) {
                        //获取结果
                        th.position = posi.data.result;
                        th.titles = title.data.result;
                    })).catch(function (err) {
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

            //如果是新增界面
            if (th.id == '') return;
            $api.get('Admin/ForID', { 'id': th.id }).then(function (req) {
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
            btnEnter: function (formName, isclose) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = th.id == '' ? api = 'Admin/add' : 'Admin/Modify';
                        if (th.id == '') vue.account.Org_ID = vue.organ.Org_ID;
                        //接口参数，如果有上传文件，则增加file
                        var para = {};
                        if (th.upfile == null || JSON.stringify(th.upfile) == '{}') para = { 'acc': th.account };
                        else
                            para = { 'file': th.upfile, 'acc': th.account };
                        $api.post(apipath, para).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$notify({
                                    type: 'success', position: 'bottom-left',
                                    message: '操作成功!'
                                });
                                th.operateSuccess(isclose);
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
                this.accPingyin = makePy(this.account.Acc_Name);
                if (this.accPingyin.length > 0)
                    this.account.Acc_NamePinyin = this.accPingyin[0];
            },
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.freshrow("' + this.id + '")', isclose);
            }
        },
    });

}, ["../Scripts/hanzi2pinyin.js"]);
