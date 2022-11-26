//试题编辑中的主要组件
//事件:
//load,
Vue.component('modify_main', {
    props: [],
    data: function () {
        return {
            id: $api.dot(),     //试题id

            course: {},          //当前试题的课程
            question: {
                Qus_ID: 0,
                Cou_ID: 0,
                Sbj_ID: 0,
                Ol_ID: 0,
                Ol_Name: '',
                Qus_IsCorrect: false,
                Qus_Items: []
            },      //当前试题
            organ: {},           //当前机构
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项


            //选项卡
            tabs: [
                //{ 'label': '试题', 'name': 'question', icon: 'e76d', size: 18 },
                { 'label': '基本信息', 'name': 'base', 'show': true, 'icon': 'e6cb', 'size': 18 },
                { 'label': '解析', 'name': 'explan', 'show': true, 'icon': 'e6f1', 'size': 17 },
                { 'label': '知识点', 'name': 'knowledge', 'show': true, 'icon': 'e84d', 'size': 16 },
                { 'label': '错误', 'name': 'error', 'show': true, 'icon': 'e70e', 'size': 16, 'color': '#F56C6C' },
                { 'label': '反馈', 'name': 'wrong', 'show': true, 'icon': 'e61f', 'size': 18, 'color': '#E6A23C' },
            ],
            //当前选项卡
            activeName: 'question',

            loading: false,
            loading_init: false
        }
    },
    watch: {
        //试题类型
        'types': {
            handler: function (nv, ov) {
                //console.log(nv);
            }, immediate: true
        },
        'question': {
            handler: function (nv, ov) {
                //如果试题类型不明确（例如新增试题），类型从路径中取
                if (!nv.Qus_Type) {
                    let name = window.location.pathname;
                    if (name.indexOf('.') > -1) name = name.substring(0, name.indexOf('.'));
                    let type = name.substring(name.length - 1);
                    nv['Qus_Type'] = Number(type);
                }
            }, immediate: true, deep: true
        }
    },
    computed: {
        //课程是否为空
        coursenull: function () {
            return JSON.stringify(this.course) != '{}' && this.course != null;
        },
        //试题是否为空
        'quesnull': function () {
            var ques = JSON.stringify(this.question) != '{}' && this.question != null;
            return !ques || this.question.Qus_ID == 0;
        },
        //试题类型
        'quesType': function () {
            var ques = JSON.stringify(this.question) != '{}' && this.question != null;
            if (ques && this.question.Qus_Type > 0) return this.question.Qus_Type;
            //如果试题不存在，则取文件名
            let name = window.location.pathname;
            if (name.indexOf('.') > -1) name = name.substring(0, name.indexOf('.'));
            return name.substring(name.length - 1);
        }
    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'Question/Components/Styles/modify_main.css']);
        //
        var th = this;
        th.getEntity();
        th.loading_init = true;
        $api.bat(
            $api.get('Organization/Current'),
            $api.cache('Question/Types:99999')
        ).then(axios.spread(function (organ, types) {
            th.loading_init = false;
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
            th.organ = organ.data.result;
            th.config = $api.organ(th.organ).config;
            th.types = types.data.result;
            th.$emit('init', th.organ, th.config, th.types);
        })).catch(function (err) {
            th.loading_init = false;
            Vue.prototype.$alert(err);
            console.error(err);
        });
    },
    methods: {
        //获取试题信息
        getEntity: function () {
            var th = this;
            if (th.id == '') {
                //th.$emit('load', th.question);
                $api.get('Snowflake/Generate').then(function (req) {
                    if (req.data.success) {
                        th.question.Qus_ID = req.data.result;
                        th.getCourse();
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
                return;
            }
            th.loading = true;
            $api.put('Question/ForID', { 'id': th.id }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    var result = req.data.result;
                    th.question = th.parseAnswer(result);
                    th.getCourse();
                    //th.$emit('load', th.question);
                } else {
                    //th.$emit('load', th.question);
                    throw '未查询到数据';
                }
            }).catch(function (err) {
                th.$alert(err, '错误');
            });
        },
        //获取课程
        getCourse: function () {
            //课程uid
            var uid = $api.querystring("uid");

            var th = this;
            var apipath = uid != '' ? 'Course/ForUID' : 'Course/ForID';
            var apipara = uid != '' ? { 'uid': uid } : { 'id': th.question.Cou_ID };
            $api.get(apipath, apipara).then(function (req) {
                if (req.data.success) {
                    th.course = req.data.result;
                    th.$emit('load', th.question, th.course);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.$emit('load', th.question, th.course);
                //Vue.prototype.$alert(err);
                //console.error(err);
            });
        },
        //将试题对象中的Qus_Items，解析为json
        parseAnswer: function (ques) {
            if (ques.Qus_Type == 1 || ques.Qus_Type == 2 || ques.Qus_Type == 5) {
                if ($api.getType(ques.Qus_Items) != 'String') return ques;
                var xml = $api.loadxml(ques.Qus_Items);
                var arr = [];
                var items = xml.getElementsByTagName("item");
                for (var i = 0; i < items.length; i++) {
                    var item = $dom(items[i]);
                    var ansid = Number(item.find("Ans_ID").html());
                    var uid = item.find("Qus_UID").text();
                    var context = item.find("Ans_Context").text();
                    var isCorrect = item.find("Ans_IsCorrect").text() == "True";
                    arr.push({
                        "Ans_ID": ansid,
                        "Qus_ID": ques.Qus_ID,
                        "Qus_UID": uid,
                        "Ans_Context": context,
                        "Ans_IsCorrect": isCorrect,
                        "selected": false,
                        "answer": ''        //答题内容，用于填空题
                    });
                }
                ques.Qus_Items = arr;
            }
            return ques;
        },
        //选项卡是否显示
        tabshow: function (item) {
            if (item.name == 'error') return this.question.Qus_IsError;
            if (item.name == 'wrong') return this.question.Qus_IsWrong;
            return true;
        },
        //设置选项卡的索引,即第几个选项卡打开
        setindex: function (index) {
            if (index == null) index = 0;
            if (index < 0 || index > this.tabs.length) return;
            this.activeName = index == 0 ? 'question' : this.tabs[index - 1].name;
        }
    },
    template: `<div class="panel" v-show="!loading">
        <el-tabs type="border-card" v-model="activeName">
            <el-tab-pane name="question" v-if="question && types">
                <span slot="label">
                    <ques_type :type="quesType" :types="types" :showname="true"></ques_type>
                </span>
            </el-tab-pane>   
            <el-tab-pane v-for="(item,index) in tabs" :name="item.name" v-if="tabshow(item)">
                <span slot="label" :style="'color:'+item.color">
                    <icon :style="'font-size: '+item.size+'px;'" v-html="'&#x'+item.icon"></icon> {{item.label}}
                </span>
            </el-tab-pane>           
        </el-tabs>
        <div v-show="activeName=='question'" remark="试题"><slot  v-if="!quesnull"></slot></div>
        <div v-show="activeName=='base'" class="base" remark="基本信息">
            <general :question="question" :organ="organ" :course="course"></general>
        </div>
        <div v-show="activeName=='explan'" remark="解析">
            <template v-if="quesnull"></template>
            <editor v-else ref="editor" :content="question.Qus_Explain" id="explain" upload="ques" :dataid="question.Qus_ID" 
            :menubar="false" model="question" @change="text=>question.Qus_Explain=text"></editor>          
        </div>
        <div v-show="activeName=='knowledge'" remark="知识点">
            <knowledge :question="question"></knowledge>
        </div>
        <div v-show="activeName=='error'" remark="存在编辑错误">
            <ques_error :question="question"></ques_error>
        </div>
        <div v-show="activeName=='wrong'" remark="存在反馈错误">
            <ques_wrong :question="question"></ques_wrong>
        </div>
    </div>
`
});
