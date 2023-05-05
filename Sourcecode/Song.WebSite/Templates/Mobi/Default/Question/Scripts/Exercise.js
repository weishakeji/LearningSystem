$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("couid", 0),
            olid: $api.querystring("olid", 0),

            account: {},     //当前登录账号        
            types: [],          //试题类型
            course: {},         //当前课程
            outline: {},        //当前章节   
            error: '',           //错误信息       


        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.batch(
                $api.get('Account/Currentd'),
                $api.cache('Question/Types:9999'),
                $api.cache('Course/ForID', { 'id': th.couid }),
                $api.cache('Outline/ForID', { 'id': th.olid })
            ).then(axios.spread(function (acc, type, cou, outline) {
                console.error(type);
            })).catch(err => alert(err))
                .finally(() => {
                    console.log('finally');
                });

        },
        created: function () {
            //if (window.ques) window.ques.get_cache_data();
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
        },
        watch: {

        },
        methods: {

        }
    });
}, []);
