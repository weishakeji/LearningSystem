$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //考试id和成绩id
            examid: $api.querystring('examid', 0),
            exrid: $api.querystring('exrid', 0),
            student: {},     //当前参考的学员，有可能不是当前学员
            exam: {},
            result: {},         //考试成绩的籹据实体对象
            exrxml: {},          //答题信息，xml
            paper: {},           //试卷信息
            config: {},      //当前机构配置项        
            types: {},          //题型

            scoreFinal: 0,       //考试得分

            tabactive: '',      //答题选项卡的状态，即：所有、正确、错误等
            error: '',           //错误提示信息，例如不能查看考虑成绩时
            loading: false
        },
        mounted: function () {
            window.addEventListener('scroll', this.handleScroll, true);
            var th = this;
            th.loading = true;
            $api.bat(
                $api.cache('Question/Types:9999'),
                $api.cache('Exam/ForID', { 'id': this.examid })
            ).then(([types, exam]) => {
                //获取结果       
                th.types = types.data.result;
                th.exam = exam.data.result;
            }).catch(function (err) {
                console.error(err);
                th.error = err;
            });

            $api.get('Exam/ResultReview', { 'id': th.exrid }).then(function (req) {
                if (req.data.success) {
                    th.result = req.data.result;
                    th.exrxml = $api.loadxml(th.result.Exr_Results);
                    th.scoreFinal = th.result.Exr_ScoreFinal;
                    //获取学员与试卷
                    $api.bat(
                        $api.cache('Account/ForID', { 'id': th.result.Ac_ID }),
                        $api.cache('TestPaper/ForID', { 'id': th.result.Tp_Id })
                    ).then(([student, paper]) => {
                        //获取结果
                        th.student = student.data.result;
                        th.paper = paper.data.result;
                    }).catch(err => console.error(err))
                        .finally(() => th.loading = false);
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
                th.error = err;
            }).finally(() => th.loading = false);
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: t => !$api.isnull(t.student),
            //试卷中的答题信息
            //返回结构：先按试题分类，分类下是答题信息
            questions: function () {
                var exrxml = this.exrxml;
                var arr = [];
                if (JSON.stringify(exrxml) === '{}') return arr;
                var elements = exrxml.getElementsByTagName("ques");
                for (let i = 0; i < elements.length; i++) {
                    let gruop = $dom(elements[i]);
                    //题型,题量，总分
                    let type = Number(gruop.attr('type'));
                    let count = Number(gruop.attr('count'));
                    let number = Number(gruop.attr('number'));
                    //试题
                    let qarr = [];
                    let list = gruop.find('q');
                    for (let j = 0; j < list.length; j++) {
                        let q = $dom(list[j]);
                        let qid = q.attr('id');
                        let ans = q.attr('ans');
                        //如果是简答题，答题内容与节点文本
                        if (type == 4 || type == 5) ans = q.text();
                        let num = Number(q.attr('num'));
                        let sucess = q.attr('sucess') == 'true';
                        let score = Number(q.attr('score'));
                        qarr.push({
                            'id': qid, 'type': type, 'num': num,
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
                let count = 0;
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
            },
            //返回按钮事件
            goback: function () {
                let url = '/mobi/exam';
                window.navigateTo($api.url.set(url, { 'tab': 'score_exam' }));
            }
        }
    });

}, ['/Utilities/Components/question/review.js',
    '/Utilities/Components/question/function.js',
    '/Utilities/panel/scripts/ctrls.js',
    '/Utilities/panel/scripts/pagebox.js',
    'Components/group.js']);
$dom.load.css(['/Utilities/panel/Skins/Education/pagebox.css']);
