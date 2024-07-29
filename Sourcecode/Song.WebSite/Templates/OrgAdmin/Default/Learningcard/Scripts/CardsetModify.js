
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data() {
            let checkNum = (rule, value, callback) => {
                if (!value && value != 0) {
                    callback(new Error('必填信息'));
                } else {
                    if (!Number.isInteger(value)) {
                        callback(new Error('请输入整数'));
                    } else {
                        return callback();
                    }
                }
            };
            let checkDate = (rule, value, callback) => {
                if (this.entity == null || this.entity.Lcs_LimitStart == '')
                    return callback(new Error('请选择时间'));
                return callback();
            };
            return {
                id: $api.querystring('id'),
                minLength: 0,        //学习卡的卡号最小长度
                //当前对象    
                entity: {
                    Lcs_Count: 100,
                    Lcs_IsEnable: true,
                    Lcs_Span: 1,
                    Lcs_Unit: '月',
                    Lcs_CodeLength: 16,
                    Lcs_PwLength: 3,
                    Lcs_RelatedCourses: '',
                    Lcs_LimitStart: '',
                    Lcs_LimitEnd: ''
                },
                courses: [],     //关联的课程
                units: ['日', '周', '月', '年'],
                introEdit: false,        //简介是否处于编辑状态

                //学习时长记录,用于判断学习时长否被更改过
                studyspan: {
                    Lcs_Span: 0,
                    Lcs_Unit: ''
                },
                studyspan_visible: false,        // 学习卡时长变更的提示面板
                rules: {
                    Lcs_Theme: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] },
                        { min: 1, max: 500, message: '长度在 1 到 500 个字符', trigger: 'blur' },
                        {
                            validator: async function (rule, value, callback) {
                                await vapp.isExist(value).then(res => {
                                    if (res) callback(new Error('已经存在!'));
                                });
                            }, trigger: 'blur'
                        }
                    ],
                    Lcs_Price: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] },
                        { type: 'number', min: 1, message: '不能小于等于0', trigger: ["blur", "change"] }
                    ],
                    Lcs_Coupon: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] },
                        { validator: checkNum, trigger: ["blur", "change"] },
                        { type: 'number', min: 0, message: '不能小于0', trigger: ["blur", "change"] }
                    ],
                    Lcs_Count: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] },
                        { validator: checkNum, trigger: ["blur", "change"] },
                        { type: 'number', min: 1, message: '不能小于等于0', trigger: ["blur", "change"] }
                    ],
                    //学习时长
                    Lcs_Span: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] },
                        { validator: checkNum, trigger: ["blur", "change"] },
                        { type: 'number', min: 1, message: '不能小于等于0', trigger: ["blur", "change"] }
                    ],
                    //学习卡卡号的长度
                    Lcs_CodeLength: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] },
                        { validator: checkNum, trigger: ["blur", "change"] },
                        {
                            validator: function (rule, value, callback) {
                                var count = window.vapp.entity.Lcs_Count;
                                var min_len = String(count).length + 1;
                                min_len += window.vapp.minLength;
                                if (value < min_len) {
                                    callback(new Error('学习码长度不得小于' + min_len));
                                } else {
                                    return callback();
                                }
                            }, trigger: ["blur", "change"]
                        },
                        { type: 'number', min: 1, message: '不能小于等于0', trigger: ["blur", "change"] },
                        { type: 'number', max: 20, message: '不能大于20', trigger: ["blur", "change"] }
                    ],
                    //学习卡密码的长度
                    Lcs_PwLength: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] },
                        { validator: checkNum, trigger: ["blur", "change"] },
                        { type: 'number', min: 1, message: '不能小于等于0', trigger: ["blur", "change"] },
                        { type: 'number', max: 8, message: '不能大于8', trigger: ["blur", "change"] }
                    ],
                    Lcs_Limit: [
                        { validator: checkDate, type: 'date', trigger: ["blur", "change"] }
                    ]
                },
                loading: false,
                loading_init: true
            };
        },
        computed: {
            //是否新增对象
            isadd: t => { return t.id == null || t.id == ''; },
        },
        watch: {

        },
        created: function () {
            var th = this;
            th.loading = true;
            //学习卡的最小长度,取机构id最大数、学习设置项id最大值
            $api.get('Learningcard/MinLength').then(function (req) {
                if (req.data.success) {
                    th.minLength = req.data.result;
                    if (th.id != '') {
                        $api.get('Learningcard/SetForID', { 'id': th.id }).then(function (req) {
                            if (req.data.success) {
                                th.entity = req.data.result;
                                //记录学习时长,用于判断学习时长否被更改过
                                th.studyspan.Lcs_Span = th.entity.Lcs_Span;
                                th.studyspan.Lcs_Unit = th.entity.Lcs_Unit;
                                //获取学习卡关联的课程
                                $api.get('Learningcard/SetCourses', { 'id': th.id }).then(function (req) {
                                    if (req.data.success) {
                                        th.courses = req.data.result;
                                    }
                                }).catch(function (err) {
                                    alert(err);
                                    console.error(err);
                                }).finally(() => th.loading_init = false);
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(err => console.error(err))
                            .finally(() => th.loading = false);
                    } else {
                        th.loading = false;
                        th.loading_init = false;
                        th.entity.Lcs_IsEnable = true;
                        th.entity.Lcs_Span = 1;
                        th.entity.Lcs_Unit = '月';
                    }
                } else {
                    throw req.data;
                }
            }).catch(function (err) {
                alert(err.message, '错误');
                console.error(err.exception);
            }).finally(() => th.loading = false);

        },
        methods: {
            //选择时间区间
            selectDate: function (start, end) {
                this.entity.Lcs_LimitStart = start;
                this.entity.Lcs_LimitEnd = end;
            },
            //判断是否已经存在
            isExist: function (val) {
                var th = this;
                return new Promise(function (resolve, reject) {
                    $api.get('Learningcard/SetExist', { 'name': val, 'id': th.id }).then(function (req) {
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
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        //如果是修改
                        if (th.id != '') {
                            var studyspanIsChange = th.studyspan.Lcs_Span != th.entity.Lcs_Span
                                || th.studyspan.Lcs_Unit != th.entity.Lcs_Unit;
                            //如果学习时限有变化
                            if (studyspanIsChange && th.entity.Lsc_UsedCount > 0) {
                                th.studyspan_visible = true;
                            } else {
                                th.sumbitEnter('Learningcard/SetModify', { 'entity': th.entity, 'scope': 1 }, isclose);
                            }
                        } else {
                            //新增
                            th.sumbitEnter('Learningcard/Setadd', { 'entity': th.entity }, true);
                        }
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //学习时限的记录是否被更改
            studyspanChangeCalc: function () {
                var prev = calcday(this.studyspan.Lcs_Span, this.studyspan.Lcs_Unit);
                var current = calcday(this.entity.Lcs_Span, this.entity.Lcs_Unit);
                if (prev - current > 0) {
                    return '减少了 ' + Math.abs(prev - current);
                } else {
                    return '增加了 ' + Math.abs(current - prev);
                }
                function calcday(span, unit) {
                    if (unit == '年') return span * 365;
                    if (unit == '月') return span * 30;
                    if (unit == '周') return span * 7;
                    return span;
                }
            },
            //确认学习时限变更的选择,scope:1为更改使用的，已经使用的不改；2为更改全部
            studyspanEnter: function (scope) {
                this.$confirm('您选择了第 ' + scope + ' 项, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    var th = this;
                    th.studyspan_visible = false;
                    th.sumbitEnter('Learningcard/SetModify', { 'entity': th.entity, 'scope': scope });
                }).catch(() => { });
            },
            //提交更改
            sumbitEnter: function (apiurl, param, isclose) {
                if (!isclose && this.isadd) return;
                var th = this;
                th.loading = true;
                $api.post(apiurl, param).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: isclose ? '保存成功，并关闭！' : '保存当前编辑成功！',
                            center: true
                        });
                        th.operateSuccess(isclose);
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                }).finally(() => th.loading = false);
            },
            //打开课程选择的窗体
            courseSelect: function () {
                let file = 'courseSelect';
                //文件路径
                let url = window.location.href;
                url = url.substring(0, url.lastIndexOf("/") + 1) + file;
                //父窗体,通过父窗体生成当前窗体id
                let parent = window.top.$ctrls.get(window.name).obj;
                let boxid = parent.id + "_" + file;
                //创建新窗体
                var box = window.top.$pagebox.create({
                    width: '48%', height: '90%', left: '1%', resize: true, max: false,
                    min: false, id: boxid, showmask: true, ico: 'e813',
                    pid: window.name, url: url
                });
                parent.full = true;     //父窗体全屏显示
                if (this.entity.Lcs_Theme) {
                    box.title = '学习卡“' + this.entity.Lcs_Theme + '”与课程的关联';
                } else {
                    box.title = '新建的学习卡与课程的关联';
                }
                //当窗体关闭时，父窗体还原
                box.onshut(function (sender, event) {
                    sender.parent.full = false;
                });

                box.open();

            },
            //当更改学习关联的课程时
            coursesChange: function (arr) {
                this.courses = [];
                this.courses = arr;
                var xml = "<Items>";
                for (let index = 0; index < arr.length; index++) {
                    const element = arr[index];
                    xml += "<item Cou_ID=\"" + element.Cou_ID + "\" />";
                }
                xml += "</Items>";
                this.entity.Lcs_RelatedCourses = xml;
                console.log(xml);
            },
            //操作成功
            operateSuccess: function (isclose) {
                if (window.top.$pagebox)
                    window.top.$pagebox.source.tab(window.name, 'vapp.freshrow("' + this.id + '")', isclose);
            }
        },
    });

});
