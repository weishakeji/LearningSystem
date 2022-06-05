
$ready(function () {
    Vue.use(VueHtml5Editor, {
        showModuleName: true,
        image: {
            sizeLimit: 512 * 1024,
            compress: true,
            quality: 80
        }
    });
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.dot(),
            organ: {},           //当前机构
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项

            entity: {},      //当前试题            

            loading: false
        },
        watch: {},
        created: function () { },
        mounted: function () { },
        methods: {
            //验证方法
            verify: function (ques, alert) {
                var th = this;
                //答案不得为空
                if (!ques.Qus_Answer || ques.Qus_Answer == '') {
                    return alert('简答题的答案不得为空！', 0);
                }
                return true;
            }
        },
    });
}, ["/Utilities/editor/vue-html5-editor.js",
    'Components/ques_type.js',
    'Components/modify_main.js',
    'Components/knowledge.js',
    'Components/general.js',
    'Components/ques_error.js',
    'Components/ques_wrong.js',
    'Components/enter_button.js']);
