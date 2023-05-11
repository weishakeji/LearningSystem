$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("couid", 0),
            olid: $api.querystring("olid", 0),

            learnmode: 0,            //练习模式，0为练习模式，1为背题模式        
            showCourse: false,           //显示课程  

            account: {},     //当前登录账号        
            types: [],          //试题类型
            course: {},         //当前课程
            outline: {},        //当前章节   
            error: '',           //错误信息       

            questions: {},
            swipeIndex: 0,           //试题滑动时的索引，用于记录当前显示的试题索引号
            //答题的状态
            state: {},

            //一些数值         
            data: {
                total: 0,        //总数
                answer: 0,      //答题数量
                correct: 0,     //正确数
                wrong: 0,           //错误数
                rate: 0         //正确率
            },


        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Question/Types:9999'),
                $api.cache('Course/ForID', { 'id': th.couid }),
                $api.cache('Outline/ForID', { 'id': th.olid })
            ).then(axios.spread(function (acc, type, cou, outline) {
                th.account = acc.data.result;
                th.types = type.data.result;
                th.course = cou.data.result;
                th.outline = outline.data.result;
            })).catch(err => alert(err))
                .finally(() => {
                    th.loading_init = false;
                    th.getQuesSimplify(false);
                });

        },
        created: function () {
            //if (window.ques) window.ques.get_cache_data();
        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); },
            //课程是否加载正确
            iscourse: (t) => { return !$api.isnull(t.course); },
            //章节是否加载正确
            isoutline: (t) => { return !$api.isnull(t.outline); },
        },
        watch: {

        },
        methods: {
            //获取试题简要信息，只有试题类型与id
            getQuesSimplify: function (update) {
                var th = this;
                let form = { 'couid': th.couid, 'olid': th.olid, 'type': -1, 'count': 0 };
                let apiurl = 'Question/Simplify:' + (query = update === true ? (60 * 24 * 30) : 'update');
                $api.cache(apiurl, form).then(function (req) {
                    if (req.data.success) {
                        th.questions = req.data.result;
                        //计算总题数
                        for (let ty in th.questions)
                            th.data.total += th.questions[ty].length;
                        th.$nextTick(function () {
                            th.$refs['quesarea'].setindex(null, 1);
                        });

                        //console.log(th.questions);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }
        }
    });
}, ['/Utilities/Components/question/function.js',
    '/Utilities/Components/question/learnmode.js', //练习模式，答题或背题
    'Components/SetupMenu.js',          //右上角的设置项菜单 
    'Components/AnswerSheet.js',        //答题卡
    'Components/QuesArea.js',           //试题区域
    'Components/Question.js',           //单个试题的展示
]);
