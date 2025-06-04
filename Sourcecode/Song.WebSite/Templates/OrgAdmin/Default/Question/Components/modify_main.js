//试题编辑中的主要组件
$dom.load.css([$dom.path() + 'Question/Components/Styles/modify_main.css']);
//事件:
//load,
Vue.component('modify_main', {
    props: [],
    data: function () {
        return {
            id: $api.dot(),     //试题id
            typename: $api.querystring('typename'),
            course: {},          //当前试题的课程
            //当前试题
            question: {
                Qus_ID: 0,
                Qus_Title: '',
                Cou_ID: 0, Sbj_ID: 0, Ol_ID: 0, Ol_Name: '',
                Qus_Diff: 3,
                Qus_IsCorrect: false,
                Qus_Items: []
            },
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
            loading_init: true
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
        coursenull: t => $api.isnull(t.course),
        //试题是否为空
        'quesnull': function () {
            return $api.isnull(this.question) || this.question.Qus_ID == 0;
        },
        //是否是新增试题
        'isadd': t => t.id == '' || t.id == '0' || t.id == 0,
        //试题类型
        'quesType': function () {
            if (!$api.isnull(this.question) && this.question.Qus_Type > 0) return this.question.Qus_Type;
            //如果试题不存在，则取文件名
            let name = window.location.pathname;
            if (name.indexOf('.') > -1) name = name.substring(0, name.indexOf('.'));
            return name.substring(name.length - 1);
        }
    },
    mounted: function () {
        var th = this;
        $api.bat(
            $api.get('Organization/Current'),
            $api.cache('Question/Types:99999')
        ).then(([organ, types]) => {
            //获取结果
            th.organ = organ.data.result;
            th.config = $api.organ(th.organ).config;
            th.types = types.data.result;
            th.$emit('init', th.organ, th.config, th.types);
        }).catch((err) => console.error(err))
            .finally(() => th.loading_init = false);
        th.getEntity();
    },
    methods: {
        //获取试题信息
        getEntity: function () {
            var th = this;
            th.loading = true;
            var promise = new Promise((resolve, reject) => {
                if (th.isadd) {
                    $api.get('Snowflake/Generate').then(function (req) {
                        if (req.data.success) {
                            th.question.Qus_ID = req.data.result;
                            resolve(th.question);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch((err) => reject(err));
                } else {
                    $api.put('Question/ForID', { 'id': th.id }).then(function (req) {
                        if (req.data.success) {
                            let result = req.data.result;
                            th.question = window.ques.parseAnswer(result);
                            resolve(th.question);
                        } else {
                            throw '未查询到数据';
                        }
                    }).catch((err) => reject(err));
                }
            });
            promise.then(function (req) {
                th.getCourse();
            }).catch((err) => alert(err, '错误'))
                .finally(() => th.loading = false);
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
                    //设置试题的课程名称，这个在试题实体上并不存在，只是在前端临时使用
                    th.question['Cou_Name'] = th.course.Cou_Name;
                    th.$emit('load', th.question, th.course);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.$emit('load', th.question, th.course);
            });
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
        },
        //AI生成事件
        aievent: function () {
            //console.error(this.question);
            var th = this;
            $api.post('Question/AIGenerate', { 'type': th.question.Qus_Type, 'sbj': th.question.Sbj_Name, 'cou': th.question.Cou_Name }).then(req => {
                if (req.data.success) {
                    var result = req.data.result;
                    result = window.ques.parseAnswer(result);
                    th.$set(th.question, 'Qus_Title', result.Qus_Title);
                    th.$set(th.question, 'Qus_Items', result.Qus_Items);
                    th.$set(th.question, 'Qus_Diff', result.Qus_Diff);
                    th.$set(th.question, 'Qus_Explain', result.Qus_Explain);
                    if (th.question.Qus_Type == 3) th.$set(th.question, 'Qus_IsCorrect', result.Qus_IsCorrect);
                    if (th.question.Qus_Type == 4) th.$set(th.question, 'Qus_Answer', result.Qus_Answer);
                    //th.question.Qus_Title = result.Qus_Title;
                    th.$emit('load', th.question, th.course);
                    //console.error(result);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => { });
        }
    },
    template: `<div class="panel" v-show="!loading">
        <div class="ai_btn" @click="aievent">AI生成</div>
        <el-tabs type="border-card" v-model="activeName">
            <el-tab-pane name="question" v-if="question && types">
                <template slot="label">
                    <span v-if="loading_init && typename">...</span>
                    <ques_type v-else :type="quesType" :types="types" :showname="true"></ques_type>
                </template>
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
