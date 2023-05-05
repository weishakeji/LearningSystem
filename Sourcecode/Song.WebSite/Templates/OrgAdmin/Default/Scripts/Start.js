$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {      
            platinfo:{},     
            organ: {},
            config: {},      //当前机构配置项        
            datas: {},
            loading_init: true
        },
        mounted: function () {
            $api.bat(            
               // $api.cache('Platform/PlatInfo:60'),  
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                vapp.loading_init = false;
                //获取结果           
                //vapp.platinfo = platinfo.data.result;  
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
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
        }
    });

});
