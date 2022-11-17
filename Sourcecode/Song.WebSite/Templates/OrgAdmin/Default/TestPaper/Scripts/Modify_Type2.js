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
                { name: '注意事项', tab: 'remind', icon: 'e845' },
                { name: '其它', tab: 'other', icon: 'e85a' }
            ],
            activeName: 'general',

            entity: {
                Tp_Id: 0,        //主键
                Tp_Type: 2,
                Tp_Total: 100,
                Tp_Diff: 2,
                Tp_Diff2: 4,
                Tp_FromType: 0,
                Sbj_ID: 0,
                Cou_ID: 0           //所属课程的id
            },
            Tp_Diff: [2, 4],     //难度范围
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

            loading: false
        },
        mounted: function () {
            var th = this;
            //当前试卷
            $api.get('TestPaper/ForID', { 'id': this.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.entity = result;
                    th.Tp_Diff = [];
                    Vue.set(th.Tp_Diff, 0, th.entity.Tp_Diff);
                    Vue.set(th.Tp_Diff, 1, th.entity.Tp_Diff2);
                } else {
                    //th.entity = {};     
                    console.log(333);
                    th.getCourse();
                }
            }).catch(function (err) {
                th.loading = false;
                console.error(err);
            });

            //获取当前机构
            $api.bat(
                $api.get('Organization/Current'),
                $api.cache('Question/Types:99999')
            ).then(axios.spread(function (organ, types) {
                th.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw arguments[i].config.way + ' ' + data.message;
                    }
                }
                //获取结果
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;
                th.types = types.data.result;

            })).catch(function (err) {
                th.loading_init = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否存在试卷对象
            exist: function () {
                return this.entity != null && this.entity.Tp_Id > 0;
            },
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
                    //alert(err);
                    Vue.prototype.$alert(err);
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
                    //alert(err);
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //获取课程
            getCourses: function (val) {
                var th = this;
                var orgid = th.organ.Org_ID;
                var sbjid = 0;
                if (th.sbjids.length > 0) sbjid = th.sbjids[th.sbjids.length - 1];
                $api.cache('Course/Pager', { 'orgid': orgid, 'sbjids': sbjid, 'thid': '', 'search': '', 'order': '', 'size': -1, 'index': 1 }).then(function (req) {
                    if (req.data.success) {
                        th.courses = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
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
                                    th.operateSuccess();
                                }, 300);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //window.top.ELEMENT.MessageBox(err, '错误');
                            th.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        th.$message.error('存在错误，请检查填写项');
                        return false;
                    }
                });
            },
            //生成配置项
            buildFromConfig: function () {
                //var type0 = this.$refs['fromtype0'];
                var xml = '<Config>';
                xml += this.$refs['fromtype0'].buildXml();
                xml += this.$refs['fromtype1'].buildXml();
                xml += '</Config>';
                console.log(xml);
                return xml;
            },
            //图片文件上传
            filechange: function (file) {
                var th = this;
                th.upfile = file;
            },
            //清除图片
            fileremove: function () {
                this.upfile = null;
                this.entity.Tp_Logo = '';
            },
            //操作成功
            operateSuccess: function () {
                //如果处于课程编辑页，则刷新
                var pagebox = window.top.$pagebox;
                if (pagebox && pagebox.source.box)
                    pagebox.source.box(window.name, 'vapp.fresh_frame', true);
            }
        }
    });

}, ['/Utilities/Components/sbj_cascader.js',
    'Components/ques_count.js',
    'Components/fromtype0.js',
    'Components/fromtype1.js',
    '../Question/Components/ques_type.js']);
