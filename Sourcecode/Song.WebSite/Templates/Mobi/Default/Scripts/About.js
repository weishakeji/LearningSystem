$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项   
            account: {},

            editon: {},     //商业版权信息
            version: {},      //版本信息
            copyright_items: [],       //版权信息，来自copyright.xml

            browser: {},         //浏览器信息

            loading_init: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Platform/Version'),
                $api.post('Platform/Edition'), 
                $api.cache('Copyright/Datas')
            ).then(axios.spread(function (ver, editon, copyright) {
                th.loading_init = false;
                //获取结果     
                th.version = ver.data.result;
                th.editon = editon.data.result;            
                th.copyright_items = copyright.data.result;
                th.builbrowser();
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {           
            //当前年份
            year: function () {
                var date = new Date();
                return date.format('yyyy');
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
            builbrowser: function () {               
                var md = new MobileDetect(window.navigator.userAgent);
                var browser = {
                    'OS': md.os(),
                    'browser': md.userAgent()
                }
                if (md.mobile() != 'UnknownPhone')
                    browser['mobile'] = md.mobile();
                for (var s in window.navigator) {
                    let type = $api.getType(window.navigator[s]);
                    if (type != 'String') continue;
                    if (window.navigator[s] == '') continue;
                    browser[s] = window.navigator[s];
                }
                //console.log(browser);
                this.browser = browser;
            }
        }
    });

}, ['Components/page_header.js',
    '/Utilities/Scripts/mobile-detect.min.js']);
