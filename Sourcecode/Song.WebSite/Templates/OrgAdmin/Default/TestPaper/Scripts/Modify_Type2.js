$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),     //试卷id
            couid: $api.querystring('couid', 0),

            organ: {},
            config: {},      //当前机构配置项     
            types: [],        //试题类型，来自web.config中配置项
            course: {},              //当前课程

            courses: [],         //课程列表
            //图片文件
            upfile: null, //本地上传文件的对象          

            tabs: [
                { name: '基本信息', tab: 'general', icon: 'e72f' },
                { name: '出题范围', tab: 'range', icon: 'e731' },
                { name: '简介', tab: 'intro', icon: 'e6fd' },
                { name: '注意事项', tab: 'remind', icon: 'e697' },
                { name: '其它', tab: 'other', icon: 'e67e' }
            ],
            activeName: 'general',
            //实体
            entity: {
                Tp_Id: 0,        //主键
                Tp_IsUse: true,
                Tp_Type: 2,
                Tp_Total: 100,
                Tp_Diff: 1,
                Tp_Diff2: 5,
                Tp_FromType: 0,
                Sbj_ID: 0,
                Cou_ID: 0           //所属课程的id
            },
            Tp_Diff: [1, 5],     //难度范围
            //录入校验的规划
            rules: {
                Tp_Name: [
                    { required: true, message: '标题不得为空', trigger: 'blur' }
                ],
                Tp_Span: [
                    { required: true, message: '限时不得为空', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (Number(value) <= 0) {
                                callback(new Error('请输入大于零的整数'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ],
                Tp_Total: [
                    { required: true, message: '分数不得为空', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (Number(value) <= 0) {
                                callback(new Error('请输入大于零的整数'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ],
                Tp_PassScore: [
                    { required: true, message: '分数不得为空', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            if (Number(value) <= 0) {
                                callback(new Error('请输入大于零的整数'));
                            } else if (Number(value) > vapp.entity.Tp_Total) {
                                callback(new Error('及格分不得大于满分'));
                            } else {
                                callback();
                            }
                        }, trigger: 'blur'
                    }
                ]
            },
            loading_init: false,
            loading: false
        },
        mounted: function () {
            var th = this;
            //获取当前机构
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current'),
                $api.cache('Question/Types:99999')
            ).then(axios.spread(function (organ, types) {
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;
                th.types = types.data.result;
                //当前试卷
                th.loading = true;
                $api.put('TestPaper/ForID', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        th.entity = req.data.result;
                        th.Tp_Diff = [];
                        Vue.set(th.Tp_Diff, 0, th.entity.Tp_Diff);
                        Vue.set(th.Tp_Diff, 1, th.entity.Tp_Diff2);
                    } else {
                        //th.entity = {};
                        th.getCourse();
                    }
                }).catch(err => console.error(err)).finally(() => th.loading = false);
            })).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
            //是否存在试卷对象
            exist: t => !$api.isnull(t.entity) && t.entity.Tp_Id > 0,
            //是否是编辑状态
            edit: function () {
                if (!this.exist) return false;
                if (this.id != '') return true;
                return false;
            },
            orgid: function () {
                if (this.exist) return this.entity.Org_ID;
                return this.organ.Org_ID;
            }
        },
        watch: {
            'Tp_Diff': function (nv, ov) {
                //console.log(nv);
            }
        },
        methods: {
            //难度范围变更时
            diffChange: function (val) {
                this.entity['Tp_Diff'] = val[0];
                this.entity['Tp_Diff2'] = val[1];
            },
            //获取当前课程
            getCourse: function () {
                var couid = $api.querystring('couid');
                if (this.couid == '') return;
                var th = this;
                $api.get('Course/ForID', { 'id': couid }).then(function (req) {
                    if (req.data.success) {
                        th.course = req.data.result;
                        th.entity.Sbj_ID = th.course.Sbj_ID;
                        th.entity.Cou_ID = th.course.Cou_ID;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //专业选择变更时，返回两个参数，一个是当前专业id，一个是当前专业的id路径（数组）
            sbjChange: function (sbjid, sbjs) {
                var th = this;
                var orgid = th.organ.Org_ID;
                $api.cache('Course/Pager', { 'orgid': orgid, 'sbjids': sbjid, 'thid': '', 'search': '', 'order': '', 'size': -1, 'index': 1 }).then(function (req) {
                    if (req.data.success) {
                        th.courses = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //获取课程
            getCourses: function (val) {
                var th = this;
                var orgid = th.organ.Org_ID;
                var sbjid = 0;
                if (th.sbjids.length > 0) sbjid = th.sbjids[th.sbjids.length - 1];
                $api.cache('Course/Pager', { 'orgid': orgid, 'sbjids': sbjid, 'thid': '', 'search': '', 'order': '', 'size': -1, 'index': 1 })
                    .then(function (req) {
                        if (req.data.success) {
                            th.courses = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err));
            },
            //确定操作
            btnEnter: function (formName, isclose) {
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        var errmsg = '存在错误，请检查填写项';
                        let fromtype = th.$refs['fromtype' + th.entity.Tp_FromType];
                        //var checked = this.$refs['fromtype0'].check();
                        if (fromtype != null && !fromtype.check()) {
                            th.$message.error(errmsg);
                            th.activeName = 'range';
                            return;
                        }
                        th.update_save(isclose);
                    } else {
                        let field = Object.keys(fields)[0];
                        th.setErrorlabel(field);
                    }
                });
            },
            //当有错误时，显示对应的选项卡
            setErrorlabel: function (field) {
                //未通过验证的字段                
                let label = $dom('label[for="' + field + '"]');
                if (label.length < 1) return false;
                while (label.attr('tab') == null)
                    label = label.parent();
                this.activeName = label.attr('tab');
                return false;
            },
            //提交保存操作
            update_save: function (isclose) {
                var th = this;
                th.entity.Tp_FromConfig = th.buildFromConfig();
                th.entity.Tp_Type = 2;    //试卷类型为随时抽题
                th.loading = true;
                var apipath = 'TestPaper/' + (th.edit ? 'Modify' : 'add');
                //接口参数，如果有上传文件，则增加file
                var para = { 'entity': th.entity };
                if (th.upfile != null) para['file'] = th.upfile;
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
                            th.operateSuccess(isclose);
                        }, 300);
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                });
            },
            //生成配置项
            buildFromConfig: function () {
                //var type0 = this.$refs['fromtype0'];
                var xml = '<Config>';
                xml += this.$refs['fromtype0'].buildXml();
                xml += this.$refs['fromtype1'].buildXml();
                xml += '</Config>';
                //console.log(xml);
                return xml;
            },
            //操作成功
            operateSuccess: function (isclose) {
                //如果处于课程编辑页，则刷新
                var pagebox = window.top.$pagebox;
                if (pagebox && pagebox.source.box)
                    pagebox.source.box(window.name, 'vapp.fresh_frame', isclose);
            }
        }
    });

}, ['/Utilities/Components/sbj_cascader.js',
    'Components/ques_count.js',
    'Components/fromtype0.js',
    'Components/fromtype1.js',
    '../Question/Components/ques_type.js']);
