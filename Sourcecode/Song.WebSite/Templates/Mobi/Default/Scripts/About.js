$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项     
            editon: {},     //商业版权信息
            version: {},      //版本信息
            copyright_items: [],       //版权信息，来自copyright.xml
            loading_init: true
        },
        mounted: function () {
            $api.bat(
                $api.get('Platform/Version'),
                $api.post('Platform/Edition'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current'),
                $api.cache('Copyright/Datas')
            ).then(axios.spread(function (ver, editon, platinfo, organ, copyright) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果     
                vapp.version = ver.data.result;
                vapp.editon = editon.data.result;
                vapp.platinfo = platinfo.data.result;
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                vapp.copyright_items = copyright.data.result;
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
        },
        methods: {
            //获取版权信息
            copyright: function (name) {
                var text = '';
                for (var i = 0; i < this.copyright_items.length; i++) {
                    var item = this.copyright_items[i];
                    if (item.name == name) {
                        text = item.text;
                        break;
                    }
                }
                return text;               
            },
            //年份
            year: function () {
                var date = new Date();
                return date.format('yyyy');
            }
        }
    });

}, ['Components/page_header.js']);
