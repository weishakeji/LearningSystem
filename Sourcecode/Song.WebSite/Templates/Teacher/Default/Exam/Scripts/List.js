$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项 

            form: {
                orgid: -1,
                start: '',
                end: '',
                use: null,
                search: '',
                size: 8,
                index: 1
            },
            total: 1, //总记录数
            totalpages: 1, //总页数
            datas: [],

            loading: false,
            loadingid: false,
            loading_init: true,
            pickerOptions: {
                shortcuts: [{
                    text: '最近一周', onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近一个月', onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近三个月', onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近六个月', onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 180);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '本周', onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setDate(start.getDate() - start.getDay() + 1);
                        end.setDate(start.getDate() + 6);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '本月', onClick(picker) {
                        const start = new Date();
                        start.setDate(1);
                        var yy = start.getFullYear();
                        var mm = start.getMonth() + 1;
                        if (mm > 12) {
                            mm = 1;
                            yy = yy + 1;
                        }
                        var end = new Date(yy, mm, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '本季度', onClick(picker) {
                        const start = new Date();
                        var yy = start.getFullYear();
                        var mm = start.getMonth();
                        if (mm >= 1 && mm <= 3) mm = 0;
                        if (mm >= 4 && mm <= 6) mm = 3;
                        if (mm >= 7 && mm <= 9) mm = 6;
                        if (mm >= 10 && mm <= 12) mm = 9;
                        start.setDate(1);
                        start.setMonth(mm);
                        const end = new Date(yy, mm + 3, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '本年', onClick(picker) {
                        const start = new Date();
                        start.setDate(1);
                        start.setMonth(0);
                        const end = new Date(start.getFullYear(), 12, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }]
            },
            selectDate: '',
        },
        mounted: function () {
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
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
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {
            this.handleCurrentChange();
        },
        computed: {

        },
        watch: {
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                th.loading = true;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 49);
                $api.post('Exam/ThemeAdminPager', this.form).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.datas = req.data.result;
                        th.totalpages = Number(req.data.totalpages);
                        th.total = req.data.total;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //表格行点击事件
            clickTable: function (row, index, e) {
                //调用,table的方法,展开/折叠 行
                this.$refs.datatable.toggleRowExpansion(row)
            },
            //更改状态
            changeState: function (row) {
                var th = this;
                this.loadingid = row.Exam_ID;
                $api.post('Exam/ModifyState', { 'id': row.Exam_ID, 'use': row.Exam_IsUse }).then(function (req) {
                    th.loadingid = -1;
                    if (req.data.success) {
                        th.$message({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = -1;
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    th.loadingid = -1;
                });
            },
            //删除考试主题
            deleteExam: function (row) {
                var th = this;
                th.loading = true;
                $api.delete('Exam/ThemeDelete', { 'id': row.Exam_ID }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '删除成功!',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            }
        },
        components: {
            //考试主题的参考人员
            'attend': {
                props: ['exam'],
                data: function () {
                    return {
                        num: -1
                    }
                },
                created: function () {
                    var th = this;
                    $api.cache('Exam/AttendTheme', { 'examid': this.exam.Exam_ID }).then(function (req) {
                        if (req.data.success) {
                            th.num = req.data.result.number;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        //alert(err);
                        console.error(err);
                    });
                },
                template: '<span><span class="el-icon-loading" v-if="num==-1"></span><span v-else>{{num}}</span></span>'
            },
            //考试主题的参考人员
            'group': {
                props: ['exam'],
                data: function () {
                    return {
                        groups: [],      //关联的学员组
                        loading: false
                    }
                },
                watch: {
                    'exam': {
                        handler: function (nv, ov) {
                            this.getgroups();
                        }, immediate: true, deep: true
                    }
                },
                methods: {
                    getgroups: function () {
                        var th = this;
                        if (th.exam.Exam_GroupType == 1) return;
                        $api.get('Exam/Groups', { 'uid': this.exam.Exam_UID }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.groups = result;
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //alert(err);
                            console.error(err);
                        });
                    }
                },
                created: function () {
                },
                template: `<div class="groups">
                        <span v-if="exam.Exam_GroupType == 1">全体学员</span>
                        <template v-else>
                            <span v-if="groups.length==0">(没有学员组)</span>
                            <template v-else>
                                <span v-for="(item, i) in groups">
                                    {{item.Sts_Name}},
                                </span>
                            </template>
                        </template>
                    </div>`
            },
            //考试主题的场次
            'exams': {
                props: ['uid', 'theme'],
                data: function () {
                    return {
                        examlist: []
                    }
                },
                watch: {
                    'theme': {
                        handler: function (nv, ov) {
                            this.getexams();
                        }, immediate: true, deep: true
                    }
                },
                methods: {
                    getexams: function () {
                        var th = this;
                        $api.get('Exam/Exams', { 'uid': this.uid }).then(function (req) {
                            if (req.data.success) {
                                for (var j = 0; j < req.data.result.length; j++) {
                                    req.data.result[j].avg = -1;
                                    req.data.result[j].number = -1;
                                    //是否需要人工批阅
                                    req.data.result[j].manual = false;
                                }
                                th.examlist = req.data.result;
                                for (var i = 0; i < th.examlist.length; i++) {
                                    $api.bat(
                                        $api.cache('Exam/Average4Exam', { 'examid': th.examlist[i].Exam_ID }),
                                        $api.get("Exam/AttendNumber", { 'examid': th.examlist[i].Exam_ID }),
                                        $api.cache("Exam/Manual4Exam", { 'examid': th.examlist[i].Exam_ID })
                                    ).then(axios.spread(function (avg, num, manual) {
                                        //判断结果是否正常
                                        for (var z = 0; z < arguments.length; z++) {
                                            if (arguments[z].status != 200)
                                                console.error(arguments[z]);
                                            var data = arguments[z].data;
                                            if (!data.success && data.exception != null) {
                                                console.error(data.exception);
                                                throw data.message;
                                            }
                                        }
                                        for (var n = 0; n < th.examlist.length; n++) {
                                            if (th.examlist[n].Exam_ID == avg.data.result.id) {
                                                th.examlist[n].avg = avg.data.result.average;
                                                th.examlist[n].number = num.data.result.number;
                                                th.examlist[n].manual = manual.data.result.number;
                                            }
                                        }
                                    })).catch(function (err) {
                                        console.error(err);
                                    });
                                }
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //alert(err);
                            console.error(err);
                        });
                    }
                },
                created: function () {

                },
                template: `<div>
                <el-row :gutter="20" class="row_title">
                    <el-col :span="10">考试场次</el-col>
                    <el-col :span="4">专业</el-col>
                    <el-col :span="4">及格/满分</el-col>    
                    <el-col :span="4">考试时间</el-col>          
                    <el-col :span="2">限时</el-col>                            
                </el-row>
              <el-row :gutter="20" v-for="(item,index) in examlist"  :key="index">
                <el-col :span="10" remark="场次" class='exam_name'>（{{index+1}}）{{item.Exam_Name}}</el-col>
                <el-col :span="4" remark="专业">{{item.Sbj_Name}}</el-col>
                <el-col :span="4" remark="分数">{{item.Exam_PassScore}}/{{item.Exam_Total}}</el-col>             
                <el-col :span="4" remark="时间">
                    <span v-if="theme.Exam_DateType==1"> {{item.Exam_Date|date('yyyy-MM-dd HH:mm')}}</span>
                    <span v-if="theme.Exam_DateType==2">
                        {{theme.Exam_Date|date('yyyy-MM-dd HH:mm')}} 至
                        {{theme.Exam_DateOver|date('yyyy-MM-dd HH:mm')}}
                    </span>
                </el-col>
                <el-col :span="2" remark="限时">{{item.Exam_Span}} 分钟</el-col>
              </el-row>
              </div>`
            }
        }
    });

});
