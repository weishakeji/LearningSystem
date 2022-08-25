$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            stid: $api.dot(),        //学员id
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        
            datas: [],

            //表单数据对象
            formdata: {
                'limittime': '',
                'courses': []
            },
            rules: {
                'limittime': [{
                    validator: async function (rule, value, callback) {
                        if (value == '') {
                            callback(new Error('请选择时间区!'));
                        } else {
                            callback();
                        }
                    }, trigger: 'blur'
                }],
                'courses': [{
                    validator: async function (rule, value, callback) {
                        if (value.length <= 0) {
                            callback(new Error('请选择课程!'));
                        } else {
                            callback();
                        }
                    }, trigger: 'blur'
                }]
            },
            limittime: '',       //时间区单
            //专业树形下拉选择器的配置项
            defaultSubjectProps: {
                children: 'children',
                label: 'Sbj_Name',
                value: 'Sbj_ID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            subjects: [],       //专业
            sbjids: [],
            courses: [],        //供选择的课程       

            pickerOptions: {
                shortcuts: [{
                    text: '一周',
                    onClick(picker) {
                        const end = new Date();
                        end.setTime(end.getTime() + 3600 * 1000 * 24 * 7);
                        picker.$emit('pick', [new Date(), end]);
                    }
                }, {
                    text: '一个月',
                    onClick(picker) {
                        const end = new Date();
                        end.setTime(end.getTime() + 3600 * 1000 * 24 * 30);
                        picker.$emit('pick', [new Date(), end]);
                    }
                }, {
                    text: '两个月',
                    onClick(picker) {
                        const end = new Date();
                        end.setTime(end.getTime() + 3600 * 1000 * 24 * 60);
                        picker.$emit('pick', [new Date(), end]);
                    }
                }, {
                    text: '三个月',
                    onClick(picker) {
                        const end = new Date();
                        end.setTime(end.getTime() + 3600 * 1000 * 24 * 90);
                        picker.$emit('pick', [new Date(), end]);
                    }
                }, {
                    text: '六个月',
                    onClick(picker) {
                        const end = new Date();
                        end.setTime(end.getTime() + 3600 * 1000 * 24 * 180);
                        picker.$emit('pick', [new Date(), end]);
                    }
                }, {
                    text: '九个月',
                    onClick(picker) {
                        const end = new Date();
                        end.setTime(end.getTime() + 3600 * 1000 * 24 * 270);
                        picker.$emit('pick', [new Date(), end]);
                    }
                }, {
                    text: '一年',
                    onClick(picker) {
                        const end = new Date();
                        end.setTime(end.getTime() + 3600 * 1000 * 24 * 365);
                        picker.$emit('pick', [new Date(), end]);
                    }
                }, {
                    text: '两年',
                    onClick(picker) {
                        const end = new Date();
                        end.setTime(end.getTime() + 3600 * 1000 * 24 * 365 * 2);
                        picker.$emit('pick', [new Date(), end]);
                    }
                }]
            },

            loading: false,
            loading_init: true,
            loading_up: false        //提交数据的预载
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Account/ForID', { 'id': th.stid }),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.getSubjects(th.organ);
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否存在学员
            isexist: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
        },
        methods: {
            //获取课程专业的数据
            getSubjects: function (organ) {
                if (organ == null || !organ || !organ.Org_ID) return;
                var th = this;
                var form = { orgid: organ.Org_ID, search: '', isuse: null };
                $api.get('Subject/Tree', form).then(function (req) {
                    if (req.data.success) {
                        th.subjects = req.data.result;
                        th.getCourses();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            //专业更改时
            changeSbj: function (val) {
                //this.question['Sbj_ID'] = val.length > 0 ? val[val.length - 1] : 0;
                this.getCourses();
                //关闭级联菜单的浮动层
                this.$refs["subjects"].dropDownVisible = false;
            },
            //获取课程
            getCourses: function () {
                var th = this;
                var orgid = th.organ.Org_ID;
                var sbjid = 0;
                if (th.sbjids.length > 0) sbjid = th.sbjids[th.sbjids.length - 1];
                th.courses = [];
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
            //添加选中的课程
            changeCourse: function (val) {
                var obj = this.formdata.courses.find(o => o.Cou_ID === val.Cou_ID);
                if (obj == null)
                    this.$set(this.formdata.courses, this.formdata.courses.length, val);
                this.$refs['formdata'].clearValidate();
            },
            //移除选中的课程
            removeCourse: function (index) {
                this.$delete(this.formdata.courses, index);
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var params = { 'stid': th.stid, 'start': th.formdata.limittime[0], 'end': th.formdata.limittime[1], 'couid': [] };
                        for (let i = 0; i < th.formdata.courses.length; i++) {
                            params.couid.push(th.formdata.courses[i].Cou_ID);
                        }
                        th.uploading(true);
                        $api.get('Account/BeginCourse', params).then(function (req) {
                            th.uploading(false);
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
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            th.uploading(false);
                            console.error(err);
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //提交数据的预载
            uploading: function (t) {
                this.loading_up = t;
                if (t == true) {
                    if (window.up_loading_full == null)
                        window.up_loading_full = this.$loading({
                            lock: true,
                            text: 'Loading',
                            spinner: 'el-icon-loading',
                            background: 'rgba(255, 255, 255, 0.6)'
                        });
                } else {
                    if (window.up_loading_full != null)
                        window.up_loading_full.close();
                }
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.box(window.name, 'vapp.handleCurrentChange', true);
            }

        }
    });

});
