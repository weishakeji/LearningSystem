$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            organ: {},
            types: {},          //题型
            entity: {}, //当前考试对象
            form: {
                examid: $api.querystring('id'),
                name: '', idcard: '',
                min: -1, max: -1,
                size: 20, index: 1
            },
            results: [],     //当前考试的所有成绩信息
            total: 1, //总记录数
            totalpages: 1, //总页数

            accountVisible: false,   //是否显示当前学员
            account: {},      //当前学员信息
            current: {},     //当前行对象

            quesAnswers: [],     //答题信息

            loading: false,
            loading_up: false,       //提交信息时预载
            loadingid: 0,
        },
        computed: {

        },
        watch: {

        },
        created: function () {
            //获取考试信息
            var th = this;
            th.loading = true;
            $api.bat(
                $api.cache('Question/Types:9999'),
                $api.get('Organization/Current'),
                $api.cache('Exam/ForID', { 'id': th.form.examid })
            ).then(axios.spread(function (types, org, exam) {
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
                th.organ = org.data.result;
                th.entity = exam.data.result;

            })).catch(function (err) {
                th.loading = false;
                alert(err);
                console.error(err);
            });
            this.handleCurrentChange(1, 0);
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index, move) {
                if (move == null) move = 0;
                if (index != null) this.form.index = index;
                var th = this;
                this.loading = true;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 105;
                th.form.size = Math.floor(area / 40);
                $api.get("Exam/Result4Exam", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.results = d.data.result;
                        if (move == 0 && th.results.length > 0)
                            th.setCurrent(th.results[0]);
                        if (move < 0 && th.results.length > 0)
                            th.setCurrent(th.results[th.results.length - 1]);
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //计算考试用时
            calcSpan: function (d1, d2) {
                if (d1 == null || d2 == null) return '';
                var total = (d2.getTime() - d1.getTime()) / 1000;
                var span = Math.floor(total / 60);
                return span <= 0 ? "<1" : span;
            },
            //设置为当前学员
            setCurrent: function (row, column, event) {
                row.index = getArrayIndex(this.results, row);
                this.current = row;
                this.$refs['datatables'].setCurrentRow(row);
                this.getaccount(this.current);
                this.quesAnswers = [];
                this.$nextTick(function () {
                    this.quesAnswers = this.analysisQuesAnswer(this.current);
                });

                function getArrayIndex(arr, obj) {
                    var i = arr.length;
                    while (i--) {
                        if (arr[i].Exr_ID === obj.Exr_ID) {
                            return i;
                        }
                    }
                    return -1;
                }
            },
            //取上一位和下一位，move为移动值，1或-1
            moveCurrent: function (move) {
                var index = this.current.index + move;
                if (index < 0) {
                    if (this.form.index > 1)
                        this.handleCurrentChange(--this.form.index, move);
                    return;
                }
                if (index >= this.results.length) {
                    if (this.form.index < this.totalpages)
                        this.handleCurrentChange(++this.form.index, 0);
                    return;
                }
                this.setCurrent(this.results[index]);
            },
            //当查看学员信息时，获取当前学员信息
            getaccount: function (row) {
                var th = this;
                $api.get('Account/ForID', { 'id': row.Ac_ID }).then(function (req) {
                    if (req.data.success) {
                        th.account = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.account = {};
                    console.error(err);
                });
            },
            //试卷中的答题信息
            //返回结构：先按试题分类，分类下是答题信息
            analysisQuesAnswer: function (exr) {
                if (!(exr && exr.Exr_Results)) return [];
                var exrxml = $api.loadxml(exr.Exr_Results);
                var arr = [];
                if (exrxml == null || JSON.stringify(exrxml) === '{}') return arr;
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
                        var num = Number(q.attr('num'));
                        if (type == 4 || type == 5) ans = q.text();
                        var sucess = q.attr('sucess') == 'true';
                        var score = Number(q.attr('score'));
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
            //保存手工批阅的信息
            savescore: function () {
                //计算总分
                var total = 0;
                for (let i = 0; i < this.quesAnswers.length; i++) {
                    const ques = this.quesAnswers[i].ques;
                    for (let j = 0; j < ques.length; j++) {
                        if (ques[j].num == ques[j].score)
                            ques[j].success = true;
                        total += ques[j].score;
                    }
                }
                this.current.Exr_ScoreFinal = total;
                //同步到成绩详细记录中（xml)
                var exrxml = $api.loadxml(this.current.Exr_Results);
                var elements = exrxml.getElementsByTagName("q");
                for (var i = 0; i < elements.length; i++) {
                    var q = $dom(elements[i]);
                    q.attr('score', getscore(q.attr('id'), this.quesAnswers));
                }
                //获取试题得分
                function getscore(qid, arr) {
                    for (let i = 0; i < arr.length; i++) {
                        for (let j = 0; j < arr[i].ques.length; j++) {
                            if (arr[i].ques[j].id == qid) return arr[i].ques[j].score;
                        }
                    }
                }
                this.current.Exr_Results = this.outputxml(exrxml);

                //保存到服务器端
                var th = this;
                th.loading_up = true;
                $api.post('Exam/ResultModify', { 'result': th.current }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$set(th.results, th.current.index, th.current);
                        th.moveCurrent(1);
                        th.$notify({
                            title: '成功',
                            message: '即将转到下一位学员',
                            type: 'success', duration: 2000,
                            position: 'bottom-right'
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(function () {
                    th.loading_up = false;
                });
            },
            outputxml: function (obj) {
                var type = $api.getType(obj);
                if (type != 'XMLDocument') return '';
                var dom = $dom(obj);
                var xml = '<?xml version="1.0" encoding="utf-8"?>';
                xml += dom.childs().outHtml();
                return xml;
            }
        },
        components: {
            //得分的输出，为了小数点对齐
            'score': {
                props: ['number'],
                data: function () {
                    return {
                        prev: '',
                        dot: '.',
                        after: ''
                    }
                },
                watch: {
                    'number': {
                        handler(nv, ov) {
                            this.init();
                        }, immediate: true
                    },
                },
                methods: {
                    init: function () {
                        var num = String(Math.round(this.number * 100) / 100);
                        if (num.indexOf('.') > -1) {
                            this.prev = num.substring(0, num.indexOf('.'));
                            this.after = num.substring(num.indexOf('.') + 1);
                        } else {
                            this.prev = num;
                            this.dot = '&nbsp;';
                        }
                    }
                },
                template: `<div class="score">
                        <span class="prev">{{prev}}</span>
                        <span class="dot" v-html="dot"></span>
                        <span class="after">{{after}}</span>
                    </div>`
            }
        }
    });

}, ['/Utilities/Components/question/review.js',
    '/Utilities/Components/question/function.js']);