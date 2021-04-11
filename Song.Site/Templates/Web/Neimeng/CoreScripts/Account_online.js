
window.vapp_account_online = new Vue({
    el: '#accoutbox',
    data: {
        account: {},        //当前登录对象    
        teacher: {},         //当前登录的教师 
        config: {},           //当前机构的配置项
        islogin: false,  //是否登录
        loading: false
    },
    computed: {
    },
    watch: {
    },
    created: function () {
        var th = this;
        th.loading = true;
        $api.bat(
            $api.get('Account/Current'),
            $api.get("Teacher/Current"),
            $api.get('Organ/Config', { 'orgid': '0' })
        ).then(axios.spread(function (acc, teacher, config) {
            th.loading = false;
            //判断结果是否正常
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i].status != 200)
                    console.error(arguments[i]);
                var data = arguments[i].data;
                if (!data.success && data.exception != '') {
                    console.error(data.exception);                   
                }
            }
            //获取结果
            th.account = acc.data.result;
            th.teacher = teacher.data.result;
            th.config = config.data.result;
            if (th.account != null) th.islogin = true;
            th.$nextTick(function(){
                console.log(th.$el.textContent);
            });
        })).catch(function (err) {
            console.error(err);
        });
    },
    methods: {

    },
    filters: {
        money: function (value) {
            var num = Number(value);
            if (isNaN(num)) return 0;
            return Math.round(num * 100) / 100;
        }
    }
})