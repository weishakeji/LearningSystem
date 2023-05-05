$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('couid', 0),        //课程id
            organ: {},
            config: {},      //当前机构配置项     
            types: [],        //试题类型，来自web.config中配置项

            courses: [],     //课程列表
            course: {},
            form: { 'Cou_ID': '' },       //查询课程的条件
            rules: {
                Cou_ID: [
                    { required: true, message: '标题不得为空', trigger: 'change' }
                ]
            },

            step: 0,         //步数
            qtype: 0,       //当前题型       

            loading_init: true,
            loading: true
        },
        mounted: function () {
            var th = this;
            console.log(th.couid);
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current'),
                $api.cache('Question/Types:99999')
            ).then(axios.spread(function (organ, types) {
                th.loading_init = false;
                //获取结果
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;
                th.sbjChange();
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
            //试题类型的名称
            'tname': function () {
                if (this.qtype <= 0 || this.qtype > this.types.length) return '';
                return this.types[this.qtype - 1];
            },
            //是否处于课程内部，当处于课程内部时，专业、课程等信息无须再选择
            'within': function () {
                return this.couid != 0 && this.couid != '';
            },
            //是否选中的课程
            'selectedCourse': function () {
                return JSON.stringify(this.course) != '{}' && this.course != null;
            }
        },
        watch: {
        },
        methods: {
            //获取当前课程
            getCourse: function () {
                var couid = this.couid;
                if (couid == '' || couid == 0) return;
                var th = this;
                th.loading = true;
                $api.get('Course/ForID', { 'id': couid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.course = req.data.result;
                        th.courseChange(th.course.Cou_ID);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
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
                        if (th.within) th.getCourse();
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
            //课程选择变更
            courseChange: function (couid) {
                this.form.Cou_ID = couid;
                var cou = this.courses.find(x => x.Cou_ID == couid);
                if (cou != null) {
                    this.$refs['subject'].setsbj(cou.Sbj_ID);
                    this.course = cou;
                }
                console.log(cou);
            },
            //选择试题类型
            selectType: function (type) {
                //没有选中课程
                if (!this.selectedCourse) {
                    this.$refs['form'].validate(function (valid) {
                        if (valid) {
                            console.log(3);
                        } else {
                            console.log('error submit!!');
                            return false;
                        }
                    });
                    return;
                }
                this.qtype = type;
            },
            //完成导入的事件
            finish: function (count) {
                console.log(count);
            },

        }
    });

}, ["/Utilities/Components/upload-excel.js",
    '../Components/sbj_cascader.js',
    'Components/ques_type.js']);
