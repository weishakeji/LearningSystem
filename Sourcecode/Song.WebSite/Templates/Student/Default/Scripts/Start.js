$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        
            datas: {},
            loading_init: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.get('Organization/Current')
            ).then(([organ]) => {
                //获取结果           
                //th.platinfo = platinfo.data.result;  
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
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
