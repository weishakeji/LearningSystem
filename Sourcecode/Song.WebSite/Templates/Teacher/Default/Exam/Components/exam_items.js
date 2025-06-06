// 考试的场次
Vue.component('exam_items', {
    props: ['theme', 'organ'],
    data: function () {
        return {
            subjects: [],    //专业
            exams: [{ Exam_ID: -1 }],          //考试场次

            loading: false
        }
    },
    watch: {
        'organ': {
            handler: function (nv, ov) {
                this.getSubjects(nv);
            }, immediate: true
        },
        'theme': {
            handler: function (nv, ov) {
                this.getExamItems(nv);
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        var css = $dom.pagepath() + 'Components/Styles/exam_items.css';
        $dom.load.css([css]);
        //this.exams.push(this.createExam());
    },
    methods: {
        //获取考试场次
        getExamItems: function (val) {
            if (val == null || this.theme.Exam_ID <= 0)
                return;
            var th = this;
            $api.get('Exam/exams', { 'uid': th.theme.Exam_UID }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    for (var i = 0; i < result.length; i++) {
                        th.exams.push(result[i]);
                    }
                    console.log(th.exams);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        //获取专业
        getSubjects: function (org) {
            if (org == null || !org.Org_ID || org.Org_ID <= 0) return;
            var th = this;
            var form = { orgid: org.Org_ID, search: '', isuse: null };
            $api.get('Subject/list', form).then(function (req) {
                if (req.data.success) {
                    th.subjects = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
        },
        //新增或编辑的事件
        btnModfiy: function (exam, index) {
            //编辑
            if (exam != null) {
                var modify = this.$refs['modify'][index];
                modify.init(exam, true, 'modify', index);
            } else {  //新增
                exam = this.createExam();
                var add = this.$refs['add'];
                add.init(exam, true, 'add', this.exams.length);
            }
        },
        //创建场次
        createExam: function () {
            var th = this;
            return {
                Exam_ID: 0,
                Exam_Name: '',
                Tp_Id: '',
                Cou_ID: '',      //临时字段，数据实体中并不存在
                Exam_DateType: th.theme.Exam_DateType,
                Exam_Date: th.theme.Exam_Date,
                Exam_DateOver: th.theme.Exam_DateOver,
                Exam_GroupType: this.theme.Exam_GroupType,
                Exam_UID: this.theme.Exam_UID,
                Exam_Span: 0
            };
        },
        //增加场次
        addExam: function (exam, index) {
            this.$set(this.exams, index, exam);
            //this.exams.splice(index, 0, exam);
            //this.exams.push(exam);
            this.$forceUpdate();
            this.$emit('addexam', exam, this.exams);
        },
        //修改场次
        modifyExam: function (exam, index) {
            this.$set(this.exams, index, exam);
        },
        //获取生成的考试场次
        getexams: function () {
            var arr = [];
            for (var i = 0; i < this.exams.length; i++) {
                if (this.exams[i].Exam_ID >= 0) arr.push(this.exams[i]);
            }
            return arr;
        }
    },
    //
    template: `<dl class="exam_items">
      <dt>
            <el-link type="primary" class="el-icon-plus" @click="btnModfiy()">添加场次</el-link>
            <div>
                <el-tag type="info"><icon>&#xe81a</icon>
                    <span v-if="theme.Exam_DateType==1">定时开始</span>
                    <span v-if="theme.Exam_DateType==2">在指定的时间区间内考试</span>
                </el-tag>
                <el-tag type="info"><icon>&#xe84d</icon>共 {{exams.length-1}} 场考试</el-tag>              
            </div>
      </dt>
      <dd v-for="(item,i) in exams" v-if="item.Exam_ID>=0">
        <div class="exam_name">
            <index>{{i}}.</index>
            <name v-if="item.Exam_Name!=''">{{item.Exam_Name}}</name>
            <name v-else class="null">(没有设置名称)</name>
            <div class="btns">
                <el-link type="primary" plain icon="el-icon-edit" @click="btnModfiy(item,i-1)">
                    修改
                </el-link>
                <el-popconfirm confirm-button-text='是的' cancel-button-text='不用' icon="el-icon-info" icon-color="red"
                    title="确定删除吗？" @confirm="exams.splice(i, 1)">
                    <el-link type="danger" plain icon="el-icon-delete" slot="reference" @click.native.stop>删除
                    </el-link>
                </el-popconfirm>
            </div>            
        </div>
        <div>
            <el-tag>总分：{{item.Exam_Total}} 分</el-tag>
            <el-tag type="success">及格分：{{item.Exam_PassScore}} 分</el-tag>  
            <el-tag type="warning">限时：{{item.Exam_Span}}分钟</el-tag>                 
        </div>
        <div class="item">时间：
            <span v-if="theme.Exam_DateType==1">
                <!--准时开始-->
                {{item.Exam_Date|date("yyyy-M-dd HH:mm")}} （准时开始）
                </span>
            <span v-else>
                <!--区间时间-->
                {{theme.Exam_Date|date("yyyy-M-dd HH:mm")}} 至
                {{theme.Exam_DateOver|date("yyyy-M-dd HH:mm")}} 之间
            </span>
        </div>
        <exam_item_data :subject="subjects.find(e=>{return e.Sbj_ID == item.Sbj_ID})" :exam="item" :paperid='item.Tp_Id'></exam_item_data>
        <exam_item_modify ref="modify" :theme="theme" :subjects="subjects" :organ="organ" @modify="modifyExam"></exam_item_modify>
      </dd>     
      <exam_item_modify ref="add" :theme="theme" :subjects="subjects" :organ="organ" @add="addExam"></exam_item_modify>
    </dl>`
});


// 考试场次的相关信息，包括专业、课程、试卷
Vue.component('exam_item_data', {
    //exam:场次
    //subject:专业
    props: ['exam', 'subject', 'paperid'],
    data: function () {
        return {
            course: null,
            testpaper: null,
            loading: true
        }
    },
    watch: {
        'subject': {
            handler: function (nv, ov) {
                //console.log(nv);
            }, immediate: true
        },
        'paperid': {
            handler: function (nv, ov) {
                this.gettestpaper();
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () { },
    methods: {
        //获取试卷
        gettestpaper: function () {
            var th = this;
            $api.cache('TestPaper/ForID', { 'id': th.exam.Tp_Id }).then(function (req) {
                if (req.data.success) {
                    th.testpaper = req.data.result;
                    th.getcourse();
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                console.error(err);
            });
        },
        //获取课程
        getcourse: function () {
            var th = this;
            $api.get('Course/ForID', { 'id': th.testpaper.Cou_ID }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.course = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                console.error(err);
            });
        }
    },
    //
    template: `<div>
        <loading v-if="loading"></loading>
        <template v-else>
            <div v-if="subject">专业：{{subject.Sbj_Name}} </div>
            <div v-if="course">课程：{{course.Cou_Name}} </div>
            <div v-if="testpaper">试卷：{{testpaper.Tp_Name}} <el-tag type="info">（题量：{{testpaper.Tp_Count}}道）</el-tag> </div>
        </template>
    </div>`
});

//场次编辑
Vue.component('exam_item_modify', {
    //theme:考试主题
    //state:是新增add，还是modify修改
    props: ['theme', 'state', 'subjects', 'organ'],
    data: function () {
        return {
            index: -1,
            show: false,     //显示编辑面板
            exam: {},            //当前场次
            rules: {
                Exam_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    { validator: validate.name.proh, trigger: 'change' },   //禁止使用特殊字符
                    { validator: validate.name.danger, trigger: 'change' },
                    { min: 3, max: 255, message: '长度在 3 到 255 个字符', trigger: 'blur' },
                    {
                        validator: function (rule, value, callback) {
                            let v = $api.trim(value);
                            if (v == '' || v.length < 1) return callback(new Error('不能全部是空格'));
                            return callback();
                        }, trigger: 'blur'
                    }
                ],
                Tp_Id: [
                    { required: true, message: '不得为空', trigger: 'change' }
                ],
                Exam_Date: [
                    { required: true, message: '不得为空', trigger: 'change' }
                ]
            },
            //专业树形下拉选择器的配置项
            defaultSubjectProps: {
                children: 'children',
                label: 'Sbj_Name',
                value: 'Sbj_ID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            sbjTree: [],        //专业树
            sbjids: [],      //选择中的专业

            courses: [],
            course: {},
            couid: '',

            paper: {},
            papers: [],
            paperid: '',

            loading: true
        }
    },
    watch: {
        'theme': {
            handler: function (nv, ov) {
                if (nv != null && nv.Exam_ID > 0) {
                    if (nv.Exam_DateType == 2) {
                        this.exam.Exam_Date = nv.Exam_Date;
                        this.exam.Exam_DateOver = nv.Exam_DateOver;
                    }
                }

            }, immediate: true
        },
        'exam': {
            handler: function (nv, ov) {
                if (nv != null && nv.Exam_ID > 0) {
                    this.paperid = nv.Tp_Id;
                    this.gettestpaper(nv.Tp_Id);
                }

            }, immediate: true
        },
        'exam.Exam_Date': function (nv, ov) {
            //alert(nv);
        },
        'subjects': {
            handler: function (nv, ov) {
                if (nv) {
                    this.sbjTree = this.buildSbjtree(nv, 0);
                }
                //this.gettestpaper();
            }, immediate: true
        },
        'show': function (nv, ov) {
            var form = this.$refs['exam_form'];
            console.log(form);
        }
    },
    computed: {},
    mounted: function () { },
    methods: {
        //初始化
        init: function (exam, show, state, index) {
            this.exam = exam;
            this.show = show;
            this.state = state;
            this.index = index;

            //当前专业
            this.sbjids = [];
            this.clac_sbjids(this.subjects, this.exam.Sbj_ID);

            //当前课程
            this.sbjChange(this.sbjids);
        },
        //生成专业的树形
        buildSbjtree: function (data, pid) {
            var list = new Array();
            for (var i = 0; i < data.length; i++) {
                if (data[i].Sbj_PID == pid)
                    list.push(data[i]);
            }
            for (var i = 0; i < list.length; i++) {
                list[i].children = this.buildSbjtree(data, list[i].Sbj_ID);
            }
            return list.length > 0 ? list : null;
        },
        //计算当前选中项
        clac_sbjids: function (list, sbjid) {
            var subject = list.find(function (item) {
                return item.Sbj_ID == sbjid;
            });
            if (subject != null) {
                this.sbjids.splice(0, 0, sbjid);
                this.clac_sbjids(list, subject.Sbj_PID);
            }
        },

        //获取试卷
        gettestpaper: function (id) {
            if (id == null) return;
            if (JSON.stringify(this.paper) != '{}') return;
            var th = this;
            $api.get('TestPaper/ForID', { 'id': id }).then(function (req) {
                if (req.data.success) {
                    th.paper = req.data.result;
                    if (th.papers.length <= 0)
                        th.papers.push(th.paper);
                    th.paperid = th.paper.Tp_Id;

                    th.getcourse(th.paper.Cou_ID);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.paperid = '';
                console.error(err);
            });
        },
        //获取当前课程
        getcourse: function (id) {
            var th = this;
            $api.get('Course/ForID', { 'id': id }).then(function (req) {
                if (req.data.success) {
                    th.course = req.data.result;
                    if (th.courses.length <= 0)
                        th.courses.push(th.course);
                    th.couid = th.course.Cou_ID;
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
        //专业选择变更时
        sbjChange: function (val) {
            //关闭级联菜单的浮动层
            if (this.$refs["subjects"])
                this.$refs["subjects"].dropDownVisible = false;

            var currid = -1;
            if (val.length > 0) currid = val[val.length - 1];
            var th = this;
            var orgid = th.organ.Org_ID;
            $api.cache('Course/Pager', { 'orgid': orgid, 'sbjids': currid, 'thid': '', 'use': '', 'live': '','free':'','search': '', 'order': '', 'size': -1, 'index': 1 }).then(function (req) {
                if (req.data.success) {
                    th.courses = req.data.result;
                    if (th.courses.length > 0) {
                        th.couid = th.courses[0].Cou_ID;
                        th.courChange(th.couid);
                    }
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
        //课程选择变更时
        courChange: function (val) {
            var th = this;
            $api.get('TestPaper/ShowPager', { 'couid': val, 'search': '', 'diff': '', 'size': 99999, 'index': 1 }).then(function (req) {
                if (req.data.success) {
                    th.papers = req.data.result;
                    if (th.paperid <= 0 || th.paperid == '') {
                        if (th.papers.length > 0) {
                            th.paperid = th.papers[0].Tp_Id;
                            th.paperChange(th.paperid);
                        }
                    }
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
        //试卷选择变更时
        paperChange: function (val) {
            this.exam.Tp_Id = val;
            var paper = this.papers.find(function (item) {
                return item.Tp_Id == val;
            })
            if (paper != null) {
                this.exam.Sbj_ID = paper.Sbj_ID;
                this.exam.Sbj_Name = paper.Sbj_Name;
                this.exam.Exam_Total = paper.Tp_Total;
                this.exam.Exam_PassScore = paper.Tp_PassScore;
                this.exam.Exam_Span = paper.Tp_Span;

            }
            console.log(val);
        },
        //考试时间变更时
        examdate_change: function (val) {
            if (val < new Date())
                alert('考试开始时间小于当前时间');
        },
        //保存
        btnSave: function (formName) {
            var th = this;
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    //var parent = th.$parent;
                    if (th.exam.Exam_ID <= 0) {
                        this.$emit('add', th.exam, th.index);
                    } else {
                        this.$emit('modify', th.exam, th.index + 1);
                    }
                    th.show = false;
                } else {
                    console.error('场次编辑有误，请仔细检查');
                    return false;
                }
            });

        }
    },
    //
    template: `<el-dialog :visible.sync="show" :close-on-click-modal="false" append-to-body class="exam_item_modify">
       <span slot="title">
            编辑考试场次
       </span>
       <el-form ref="state" :model="exam" label-width="80px"  :rules="rules">
            <el-form-item label="场次名称" prop="Exam_Name">
                <el-input v-model="exam.Exam_Name"></el-input>
            </el-form-item>
            <el-form-item label="专业">
                <el-cascader ref="subjects" style="width: 100%;" clearable v-model="sbjids" placeholder="请选择课程专业"
                :options="sbjTree" separator="／" :props="defaultSubjectProps" filterable @change="sbjChange">
                    <template slot-scope="{ node, data }">
                        <span>{{ data.Sbj_Name }}</span>
                        <span class="sbj_course" v-if="data.Sbj_CourseCount>0">
                            <icon>&#xe813</icon>{{ data.Sbj_CourseCount }}
                        </span>
                    </template>
                </el-cascader>     
            </el-form-item>
            <el-form-item label="课程">
                <el-select v-model="couid" style="width: 100%;" filterable clearable @change="courChange" placeholder="-- 课程 --">
                    <el-option v-for="(item,i) in courses" :key="item.Cou_ID" :label="item.Cou_Name"
                        :value="item.Cou_ID">
                        <span>{{i+1}} . {{item.Cou_Name}}</span>
                        <icon test title='试卷数' style="float:right" v-if="item.Cou_TestCount>0">{{item.Cou_TestCount}}</icon>
                </el-select>
            </el-form-item>
            <el-form-item label="试卷" prop="Tp_Id">
                <el-select v-model="paperid" style="width: 100%;" filterable clearable  @change="paperChange" placeholder="-- 试卷 --">
                    <el-option v-for="(item,i) in papers" :key="item.Tp_Id" :label="item.Tp_Name"
                        :value="item.Tp_Id">
                        <span>{{i+1}} . </span>
                        <span>{{item.Tp_Name}}</span>
                    </el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="分数">
                <el-tag>总分：{{exam.Exam_Total}}</el-tag>
                <el-tag type="success">及格分：{{exam.Exam_PassScore}}</el-tag>
            </el-form-item>
            <el-form-item label="开始时间"  prop="Exam_Date">
                <span v-if="theme.Exam_DateType==1">
                    <!--准时开始-->
                    <el-date-picker v-model="exam.Exam_Date" @change="examdate_change" type="datetime" placeholder="选择日期时间">
                    </el-date-picker>                  
                </span>
                <span v-else>
                    <!--区间时间-->
                    {{theme.Exam_Date|date("yyyy-M-dd HH:mm")}} 至
                    {{theme.Exam_DateOver|date("yyyy-M-dd HH:mm")}} 之间 
                </span>
            </el-form-item>
            <el-form-item label="限时">
                <el-input v-model="exam.Exam_Span" type="number">
                    <template slot="append">分钟</template>
                </el-input>
            </el-form-item>
       </el-form>
       <template slot="footer">
            <el-button @click="show = false"><icon>&#xe748</icon>取 消</el-button>
            <el-button type="primary" @click="btnSave('state')"><icon>&#xe634</icon>确  定</el-button>
        </template>
    </el-dialog>`
});