
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.dot(),
            organ: {},           //当前机构
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项

            course: {},          //当前试题的课程
            //当前试题
            entity: {
                Qus_IsCorrect: false
            },

            loading: false
        },
        watch: {
            'entity': {
                handler: function (nv, ov) {

                }, immediate: false, deep: true
            }
        },
        updated: function () {
            this.$mathjax();
        },
        created: function () { },
        mounted: function () { },
        methods: {
            //验证方法
            verify: function (ques, alert) {
                return true;
            }
        },
    });
}, ['/Utilities/Components/question/function.js',
    '/Utilities/Scripts/marked.min.js', //markdown的处理，用于AI解析生成文件的处理
    'Components/ques_type.js',
    'Components/modify_main.js',
    'Components/knowledge.js',
    'Components/general.js',
    'Components/ques_error.js',
    'Components/ques_wrong.js',
    'Components/ques_ansitem.js',
    'Components/ques_ansedit.js',
    'Components/enter_button.js']);
