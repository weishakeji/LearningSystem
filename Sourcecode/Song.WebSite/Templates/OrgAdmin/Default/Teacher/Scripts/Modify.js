$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id', 0),
            organ: {},
            config: {},      //当前机构配置项      
            titles: [],          //教师职称  
            //当前对象的实体
            entity: {
                Th_IsUse: true,
                Th_Photo: ''
            },
            activeName: 'general',      //选项卡
            accPingyin: [],  //账号名称的拼音
            rules: {
                Th_Name: [
                    { required: true, message: '姓名不得为空', trigger: 'blur' }
                ],
                Th_AccName: [
                    { required: true, message: '账号不得为空', trigger: 'blur' },
                    { min: 6, max: 20, message: '长度在 6 到 20 个字符', trigger: 'blur' }
                ]
            },
            //图片文件
            upfile: null, //本地上传文件的对象   

            loading: false,
            loading_init: true
        },
        mounted: function () {
            this.getEntity();
        },
        created: function () {

        },
        computed: {
            //是否存在账号
            isexist: function () {
                return JSON.stringify(this.entity) != '{}' && this.entity != null && this.id != 0;
            },
            //是否新增账号
            isadd: t => t.id == null || t.id == '' || this.id == 0,
        },
        watch: {
            'organ': function (n, o) {
                var th = this;
                th.loading = true;
                $api.get('Teacher/Titles', { 'orgid': th.organ.Org_ID, 'name': '', 'use': true }).then(function (req) {
                    if (req.data.success) {
                        th.titles = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            }
        },
        methods: {
            //获取当前对象
            getEntity: function () {
                var th = this;
                th.loading = true;
                if (th.id != "") {
                    $api.get('Teacher/ForID', { 'id': th.id }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.entity = result;
                            if (th.entity.Ths_ID <= 0) th.entity.Ths_ID = '';
                            if ($api.isnull(th.entity.Th_Birthday) || $api.getType(th.entity.Th_Birthday) != 'Date')
                                th.entity.Th_Birthday = '';
                            $api.get('Organization/ForID', { 'id': th.entity.Org_ID }).then(function (req) {
                                if (req.data.success) {
                                    th.organ = req.data.result;
                                    th.config = $api.organ(th.organ).config;
                                } else {
                                    console.error(req.data.exception);
                                    throw req.data.message;
                                }
                            }).catch(function (err) {
                                alert(err);
                                console.error(err);
                            }).finally(() => th.loading = false);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                } else {
                    $api.bat(
                        $api.put('Snowflake/Generate'),
                        $api.get('Organization/Current')
                    ).then(([snowid, org]) => {
                        //获取结果
                        //th.entity.Th_ID = snowid.data.result;
                        th.organ = org.data.result;
                        th.config = $api.organ(th.organ).config;
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                }
            },
            btnEnter: function (formName, isclose) {
                if (!isclose && this.isadd) return;
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = th.id == '' ? api = 'Teacher/add' : 'Teacher/Modify';
                        if (th.id == '') th.entity.Org_ID = th.organ.Org_ID;
                        //接口参数，如果有上传文件，则增加file
                        var para = {};
                        if (th.upfile == null || JSON.stringify(th.upfile) == '{}') para = { 'entity': th.entity };
                        else
                            para = { 'file': th.upfile, 'entity': th.entity };
                        $api.post(apipath, para).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: isclose ? '保存成功，并关闭！' : '保存当前编辑成功！',
                                    center: true
                                });
                                window.setTimeout(function () {
                                    th.operateSuccess(isclose);
                                }, 300);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err, '错误');
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
                this.accPingyin = makePy(this.entity.Th_Name);
                if (this.accPingyin.length > 0)
                    this.entity.Th_Pinyin = this.accPingyin[0];
            },
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.freshrow("' + this.id + '")', isclose);
            }
        }
    });

}, ["../Scripts/hanzi2pinyin.js",
    "/Utilities/Components/education.js"]);
