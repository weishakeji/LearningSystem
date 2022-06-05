
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
        watch: {
            'entity': {
                handler: function (nv, ov) {
                   
                }, immediate: false, deep: true
            }
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
}, ["/Utilities/editor/vue-html5-editor.js",
    'Components/ques_type.js',
    'Components/modify_main.js',
    'Components/knowledge.js',
    'Components/general.js',
    'Components/ques_error.js',
    'Components/ques_wrong.js',
    'Components/enter_button.js']);
