$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            organ: {},
            config: {},      //当前机构配置项      
            form: {
                orgid: -1,
                start: '',
                end: '',
                use:null,
                search: '',
                size: 8,
                index: 1
            },
            total: 1, //总记录数
            totalpages: 1, //总页数
            datas: [],
            loading_init:false,
            loading: false,
            loadingid: false,
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
        computed: {

        },
        watch: {
            //选择时间区间
            selectDate: {
                handler(nv, ov) {
                    if (!nv) {
                        this.form.start = '';
                        this.form.end = '';
                        return;
                    }
                    //设置日间区间，从开始时间的零时，到结束时间的最后一秒
                    //例如：2021-3-1至2021-3-2，实际是2021-3-1 0:0:0至2021-3-2 23:59:59
                    this.form.start = Date.parse(nv[0].format('yyyy/MM/dd 0:0:0'));
                    let end = Date.parse(nv[1].format('yyyy/MM/dd 0:0:0'));
                    end.setDate(end.getDate() + 1);
                    this.form.end = new Date(end.getTime() - 1000);;
                },
                immediate: true,  //刷新加载 立马触发一次handler
                deep: true
            }
        },
        created: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                th.loading_init = false;
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
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.form.orgid = th.organ.Org_ID;
                th.handleCurrentChange();
            })).catch(function (err) {
                console.error(err);
            });
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
                $api.get('Exam/ThemeAdminPager', this.form).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        window.vue.datas = req.data.result;
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
            //查看成绩详情的按钮
            btnResultView: function (row) {
                var file = 'ResultsDetails';
                var boxid = "ResultsDetails_" + row.Exam_ID + "_" + file;
                var title = '  “' + row.Exam_Title + "”";
                this.$refs.btngroup.pagebox(file + '?id=' + row.Exam_ID, title, boxid, 600, 400,
                    { pid: window.name, resize: true });
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
                        msg: 0
                    }
                },
                created: function () {
                    var th = this;
                    $api.cache('Exam/GroupType', { 'type': this.exam.Exam_GroupType, 'uid': this.exam.Exam_UID }).then(function (req) {
                        if (req.data.success) {
                            th.msg = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        //alert(err);
                        console.error(err);
                    });
                },
                template: '<div class="groups" :title="msg">{{msg}}</div>'
            },
            //考试主题的场次
            'exams': {
                props: ['uid'],
                data: function () {
                    return {
                        examlist: []
                    }
                },
                created: function () {
                    var th = this;
                    $api.cache('Exam/Exams', { 'uid': this.uid }).then(function (req) {
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
                                            th.examlist[n].manual = manual.data.result.manual;
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
                },
                methods: {
                    btnResultView: function (row) {
                        var file = 'ResultsDetail';
                        var boxid = "ResultsDetail_" + row.Exam_ID + "_" + file;
                        var title = '  “' + row.Exam_Title + "”";
                        window.vue.$refs.btngroup.pagebox(file + '?id=' + row.Exam_ID, title, boxid, 900, '80%',
                            { pid: window.name, resize: true, 'showmask': true, 'min': false, 'ico': 'e696' });
                    }
                },
                template: `<div><el-row :gutter="20" class="row_title">
                <el-col :span="8">考试场次</el-col>
                <el-col :span="6">专业</el-col>
                <el-col :span="4">及格/满分</el-col>              
                <el-col :span="2">平均分</el-col>
                <el-col :span="2">参考人数</el-col>
                <el-col :span="2"></el-col>
              </el-row>
              <el-row :gutter="20" v-for="(item,index) in examlist"  :key="index">
                <el-col :span="8">
                    <el-tooltip content="点击查看成绩" placement="bottom" effect="light">
                        <el-link type="primary" @click="btnResultView(item)">
                            <icon>&#xe696</icon>{{item.Exam_Title}}
                        </el-link>      
                    </el-tooltip>              
                </el-col>
                <el-col :span="6">{{item.Sbj_Name}}</el-col>
                <el-col :span="4">{{item.Exam_PassScore}}/{{item.Exam_Total}}</el-col>             
                <el-col :span="2"><span class="el-icon-loading" v-if="item.avg==-1"></span><span v-else>{{item.avg}}</span></el-col>
                <el-col :span="2"><span class="el-icon-loading" v-if="item.number==-1"></span><span v-else>{{item.number}}</span></el-col>
                <el-col :span="2" v-if="item.manual">
                    <el-tooltip content="考试存在主观题，需要人工判卷" placement="bottom" effect="light">
                        <el-link type="primary"><icon>&#xa02e</icon>批阅</el-link> 
                    </el-tooltip> 
                </el-col>
              </el-row>
              </div>`
            }
        }
    });

});