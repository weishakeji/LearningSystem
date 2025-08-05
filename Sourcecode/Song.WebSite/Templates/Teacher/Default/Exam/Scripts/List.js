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
          
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(([organ]) => {
                //获取结果             
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {
            this.handleCurrentChange();
        },
        computed: {

        },
        watch: {
        },
        methods: {
             //选择时间区间
             selectDate: function (start, end) {
                this.form.start = start;
                this.form.end = end;
                this.handleCurrentChange(1);
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                var loading = this.$fulloading();
                th.loading = true;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 49);
                $api.post('Exam/ThemeAdminPager', this.form).then(function (req) {
                    if (req.data.success) {
                        th.datas = req.data.result;
                        th.totalpages = Number(req.data.totalpages);
                        th.total = req.data.total;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                    th.$nextTick(function () {
                        loading.close();
                    });
                }).catch(function (err) {
                    loading.close();
                    console.error(err);
                }).finally(() => th.loading = false);
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

                    if (req.data.success) {
                        th.$message({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err, '错误');
                    console.error(err);
                }).finally(() => th.loadingid = -1);
            },
            //删除考试主题
            deleteExam: function (row) {
                var th = this;
                th.loading = true;
                $api.delete('Exam/ThemeDelete', { 'id': row.Exam_ID }).then(function (req) {
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
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            }
        },
        components: {
            //考试主题下应该考加的人员数
            'studenttotal': {
                props: ['exam'],
                data: function () {
                    return {
                        num: -1
                    }
                },
                created: function () {
                    var th = this;
                    $api.cache('Exam/StudentTotalTheme', { 'examid': this.exam.Exam_ID }).then(function (req) {
                        if (req.data.success) {
                            th.num = req.data.result.number;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => { });
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
                        }).catch(err => console.error(err));
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
                    //获取考试场次的信息
                    getexams: function () {
                        var th = this;
                        $api.get('Exam/Exams', { 'uid': this.uid }).then(function (req) {
                            if (req.data.success) {
                                let result = req.data.result;
                                for (let j = 0; j < result.length; j++) {
                                    result[j].avg = -1;
                                    result[j].number = -1;
                                    result[j].manual = false;   //是否需要人工批阅
                                }
                                th.examlist = result;
                                for (let i = 0; i < th.examlist.length; i++) {
                                    th.getexaminfo(th.examlist[i], i);
                                }
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(err => console.error(err));
                    },
                    //获取考试场景的数据，例如平均数、参考人数
                    getexaminfo: function (exam, index) {
                        var th = this;
                        var examid = exam.Exam_ID;    //考试场次id
                        $api.bat(
                            $api.cache('Exam/Average4Exam', { 'examid': examid }),
                            $api.get("Exam/AttendCount", { 'examid': examid }),
                            $api.cache("Exam/Manual4Exam", { 'examid': examid })
                        ).then(([avg, num, manual]) => {
                            exam.avg = avg.data.result.average;
                            exam.number = num.data.result.number;
                            exam.manual = manual.data.result.number;
                            th.$set(th.examlist, index, exam);
                        }).catch(err => console.error(err));
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
