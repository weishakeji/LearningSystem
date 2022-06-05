
$ready(function () {

    window.vapp = new Vue({
        el: '#app',
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
            }
            let checkDate = (rule, value, callback) => {
                if (this.datespan) {
                    if (this.datespan.length == 0) {
                        return callback(new Error('请选择时间'));
                    }
                    return callback();
                } else {
                    return callback();
                }
            }
            return {
                id: $api.querystring('id'),
                minLength: 0,        //学习卡的卡号最小长度
                entity: {
                    Lcs_Count: 100,
                    Lcs_IsEnable: true,
                    Lcs_Span: 1,
                    Lcs_Unit: '月',
                    Lcs_CodeLength: 16,
                    Lcs_PwLength: 3,
                    Lcs_RelatedCourses: ''
                }, //当前对象    
                courses: [],     //关联的课程
                units: ['日', '周', '月', '年'],
                introEdit: false,        //简介是否处于编辑状态
                datespan: [],            //时间区间,内有两个值，一个开始，一个结束
                rules: {
                    Lcs_Theme: [
                        { required: true, message: '不得为空', trigger: ["blur", "change"] }
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
                loading: false
            }
        },
        watch: {
            //有效期
            'datespan': { //监听的对象
                deep: true, //深度监听设置为 true
                handler: function (newV, oldV) {
                    if (newV != null) {
                        this.entity.Lcs_LimitStart = newV[0];
                        this.entity.Lcs_LimitEnd = newV[1];
                    }
                }
            },
            'courses': function (nl, ol) {
                //console.log(nl);
            }
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.get('Learningcard/MinLength').then(function (req) {
                if (req.data.success) {
                    th.minLength = req.data.result;
                    if (th.id != '') {
                        $api.get('Learningcard/SetForID', { 'id': th.id }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                th.entity = req.data.result;
                                th.datespan.push(th.entity.Lcs_LimitStart);
                                th.datespan.push(th.entity.Lcs_LimitEnd);
                                $api.get('Learningcard/SetCourses', { 'id': th.id }).then(function (req) {
                                    if (req.data.success) {
                                        vapp.courses = req.data.result;
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
                            th.loading = false;
                            console.error(err);
                        });
                    } else {
                        th.loading = false;
                        th.entity.Lcs_IsEnable = true;
                        th.entity.Lcs_Span = 1;
                        th.entity.Lcs_Unit = '月';
                    }
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });

        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        console.log("验证通过！！");
                        th.loading = true;
                        var apipath = 'Learningcard/Set' + (th.id == '' ? api = 'add' : 'Modify');
                        console.log(vapp.entity);
                        $api.post(apipath, { 'entity': vapp.entity }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                vapp.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                vapp.operateSuccess();
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            th.loading = false;
                            vapp.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //打开课程选择的窗体
            courseSelect: function () {
                var file = 'courseSelect';
                //文件路径
                var url = window.location.href;
                url = url.substring(0, url.lastIndexOf("/") + 1) + file;
                //父窗体,通过父窗体生成当前窗体id
                var parent = window.top.$ctrls.get(window.name).obj;
                var boxid = parent.id + "_" + file;
                //创建
                var box = window.top.$pagebox.create({
                    width: 800,
                    height: 400,
                    resize: true,
                    max: false,
                    min: false,
                    id: boxid,
                    pid: window.name,
                    url: url
                });
                parent.full = true;
                box.title = '学习卡“' + this.entity.Lcs_Theme + "”与课程的关联";
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
            operateSuccess: function () {
                if (window.top.$pagebox)
                    window.top.$pagebox.source.tab(window.name, 'vue.handleCurrentChange', true);
            }
        },
    });

});
