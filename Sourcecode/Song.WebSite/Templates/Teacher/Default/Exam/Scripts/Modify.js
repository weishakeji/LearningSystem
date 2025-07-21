
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id', 0),
            organ: {},
            tabs: [
                { title: '基本信息', name: 'general', icon: 'e6b0' },
                { title: '考试场次', name: 'exams', icon: 'e834' },
                { title: '参考人员', name: 'range', icon: 'e67d' }],
            activeName: 'general',     //选项卡

            //当前数据实体
            entity: {
                Exam_ID: 0,
                Exam_IsTheme: true,
                Exam_DateType: 1,
                Exam_IsRightClick: true,
                Exam_IsUse: true,
                Exam_GroupType: 1,
                Exam_UID: new Date().getTime()
            },
            rules: {
                Exam_Title: [
                    { required: true, message: '标题不得为空', trigger: 'blur' },
                    { min: 1, max: 255, message: '最长输入255个字符', trigger: 'change' },
                    { validator: validate.name.proh, trigger: 'change' },   //禁止使用特殊字符
                    { validator: validate.name.danger, trigger: 'change' },
                    {
                        validator: function (rule, value, callback) {
                            let v = $api.trim(value);
                            if (v == '' || v.length < 1) return callback(new Error('不能全部是空格'));
                            return callback();
                        }, trigger: 'blur'
                    }
                ]
            },
            //考试时间区间（当设置为“设定时间区间”时)
            dateRange: [],

            loadstate: {
                init: false,        //初始化            
                get: false,         //加载数据
                update: false,      //更新数据              
            },
        },
        watch: {
            //当考试时间方式更改时
            'entity.Exam_DateType': function (nv, ov) {
                if (nv == 2) {
                    var date = this.entity.Exam_Date;
                    var over = this.entity.Exam_DateOver;
                    if (date.getFullYear() - 100 > new Date().getFullYear() || date.getFullYear() + 100 < new Date().getFullYear())
                        date = new Date();
                    if (over.getFullYear() - 100 > new Date().getFullYear() || over.getFullYear() + 100 < new Date().getFullYear())
                        over = date.setMonth(date.getMonth() + 1);
                    this.dateRange[0] = this.entity.Exam_Date = date;
                    this.dateRange[1] = this.entity.Exam_DateOver = over;
                }
                console.log(nv);
            }
        },
        created: function () {
            var th = this;
            th.loadstate.init = true;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    th.organ = req.data.result;
                    th.getTheme();
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }

            }).catch(err => console.error(err))
                .finally(() => th.loadstate.init = false);
        },
        mounted: function () {

        },
        computed: {
            //是否新增账号
            isadd: t => t.id == null || t.id == '' || this.id == 0,
            //是否加载中
            loading: function () {
                if (!this.loadstate) return false;
                for (let key in this.loadstate) {
                    if (this.loadstate.hasOwnProperty(key)
                        && this.loadstate[key])
                        return true;
                }
                return false;
            }
        },
        methods: {
            //获取考试主题
            getTheme: function () {
                var th = this;
                if (th.id == 0) return;
                th.loadstate.get = true;
                $api.get('Exam/ForID', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.entity = result;
                        //时间区间
                        th.dateRange[0] = th.entity.Exam_Date;
                        th.dateRange[1] = th.entity.Exam_DateOver;
                    } else {
                        throw '未查询到数据';
                    }
                }).catch(err => alert(err)).finally(() => th.loadstate.get = false);
            },
            //当前增加场次时
            addexam: function (exam, exams) {
                this.$forceUpdate();
            },
            //保存
            btnEnter: function (formName, isclose) {
                var th = this;
                if (th.loadstate.update) return;
                th.loadstate.update = true;
                //考试场次
                var exams = th.$refs['exam_items'].getexams();
                //关联的学员组
                var groups = th.$refs['group_select'].examGroup;

                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        console.log(th.entity);
                        var apipath = th.id == 0 ? 'Exam/add' : 'Exam/Modify';
                        $api.post(apipath, { 'theme': th.entity, 'items': exams, 'groups': groups }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$notify({
                                    message: '考试管理操作成功！',
                                    type: 'success',
                                    position: 'bottom-left'
                                });
                                th.operateSuccess(isclose);
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        }).finally(() => th.loadstate.update = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function (isclose) {
                //当处理教师管理状态时
                var pagebox = window.top.$pagebox;
                var box = pagebox.get(window.name);
                var pid = box.pid;
                if (window.top.vapp && window.top.vapp.fresh) {
                    window.top.vapp.fresh(pid, 'vapp.handleCurrentChange');
                    window.setTimeout(function () {
                        pagebox.shut(window.name);
                    }, 1000);
                }
                else {
                    //当处于机构管理界面时
                    window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', isclose);
                }
            }
        },
    });

}, ["Components/group_select.js",
    "Components/exam_items.js"]);
