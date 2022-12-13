$ready(function () {
    //禁用鼠标右键
    document.addEventListener('contextmenu', function (e) {
        e.preventDefault();
    });
    //禁止选择文本
    document.addEventListener('selectstart', function (e) {
        e.preventDefault();
    });
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //考试id和成绩id
            examid: $api.querystring('examid', 0),
            exrid: $api.querystring('exrid', 0),

            student: {},     //当前参考的学员，有可能不是当前学员
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项    

            exam: {},
            result: {},         //考试成绩的籹据实体对象
            exrxml: {},          //答题信息，xml
            paper: {},           //试卷信息              
            types: {},          //题型

            scoreFinal: 0,       //考试得分

            tabactive: '',      //选项卡的状态
            error: '',           //错误提示信息，例如不能查看考虑成绩时
            loading: false
        },     
        mounted: function () {
            window.addEventListener('scroll', this.handleScroll, true);
            var th = this;
            th.loading = true;
            $api.bat(
                $api.cache('Question/Types:9999'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current'),
                $api.cache('Exam/ForID', { 'id': th.examid }),
                $api.get('Exam/ResultReview', { 'id': th.exrid })
            ).then(axios.spread(function (types, plat, org, exam, result) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                        throw arguments[i].config.way + ' ' + data.message;
                    }
                }
                //获取结果           
                th.types = types.data.result;
                th.platinfo = plat.data.result;
                th.organ = org.data.result;
                th.config = $api.organ(th.organ).config;
                th.exam = exam.data.result;
                th.result = result.data.result;
                th.scoreFinal = th.result.Exr_ScoreFinal;
                //解析答题信息
                th.exrxml = $api.loadxml(th.result.Exr_Results);
                //console.log('答题信息：');
                //console.log(th.exrxml);
                $api.bat(
                    $api.cache('Account/ForID', { 'id': th.result.Ac_ID }),
                    $api.cache('TestPaper/ForID', { 'id': th.result.Tp_Id })
                ).then(axios.spread(function (student, paper) {
                    th.loading = false;
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
                    vapp.student = student.data.result;
                    vapp.paper = paper.data.result;
                })).catch(function (err) {
                    th.loading = false;
                    alert(err);
                    console.error(err);
                });

            })).catch(function (err) {
                th.loading = false;
                th.error=err;
                alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //试卷中的答题信息
            //返回结构：先按试题分类，分类下是答题信息
            questions: function () {
                var exrxml = this.exrxml;
                var arr = [];
                if (JSON.stringify(exrxml) === '{}') return arr;
                var elements = exrxml.getElementsByTagName("ques");
                for (var i = 0; i < elements.length; i++) {
                    var gruop = $dom(elements[i]);
                    //题型,题量，总分
                    var type = Number(gruop.attr('type'));
                    var count = Number(gruop.attr('count'));
                    var number = Number(gruop.attr('number'));
                    //试题
                    var qarr = [];
                    var list = gruop.find('q');
                    for (var j = 0; j < list.length; j++) {
                        var q = $dom(list[j]);
                        var qid = q.attr('id');
                        var ans = q.attr('ans');
                        var sucess = q.attr('sucess') == 'true';
                        var score = Number(q.attr('score'));
                        qarr.push({
                            'id': qid, 'type': type,
                            'ans': ans, 'success': sucess, 'score': score
                        });
                    }
                    arr.push({
                        'type': type, 'count': count, 'number': number, 'ques': qarr
                    });
                }
                return arr;
            },
            //总题数
            ques_all_count: function () {
                var count = 0;
                for (var i = 0; i < this.questions.length; i++)
                    count += this.questions[i].ques.length;
                return count;
            },
            //答对的题数
            ques_success_count: function () {
                var count = 0;
                for (var i = 0; i < this.questions.length; i++) {
                    var ques = this.questions[i].ques;
                    for (var j = 0; j < ques.length; j++) {
                        if (ques[j].success) count++;
                    }
                }
                return count;
            },
            //答错的题数
            ques_error_count: function () {
                var count = 0;
                for (var i = 0; i < this.questions.length; i++) {
                    var ques = this.questions[i].ques;
                    for (var j = 0; j < ques.length; j++) {
                        if (!ques[j].success) count++;
                    }
                }
                return count;
            },
            //未做的题数
            ques_unanswerd_count: function () {
                var count = 0;
                for (var i = 0; i < this.questions.length; i++) {
                    var ques = this.questions[i].ques;
                    for (var j = 0; j < ques.length; j++) {
                        if (ques[j].ans == '') count++;
                    }
                }
                return count;
            },
        },
        watch: {
            'tabactive': function (nv, ov) {
                //console.log(nv);
            }
        },
        methods: {
            //得分样式
            scoreStyle: function (score) {
                //总分和及格分
                var total = this.exam ? this.exam.Exam_Total : -1;
                var passscore = this.paper ? this.paper.Tp_PassScore : -1;
                if (score == total) return "praise";
                if (score < passscore) return "nopass";
                if (score < total * 0.8) return "general";
                if (score >= total * 0.8) return "fine";
                return "";
            }
        }
    });
}, ['/Utilities/Components/question/review.js',
    '/Utilities/Components/question/function.js',
    'Components/group.js']);
