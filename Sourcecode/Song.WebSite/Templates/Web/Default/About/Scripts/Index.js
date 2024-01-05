$ready(
    ['../Components/courses.js',
        '../Components/course.js',
        '/Utilities/baiduMap/map_show.js'],
    function () {

        window.vapp = new Vue({
            el: '#vapp',
            data: {
                account: {},     //当前登录账号
                platinfo: {},
                org: {},
                config: {},      //当前机构配置项        


            },
            mounted: function () {
            },
            created: function () {
            },
            computed: {},
            watch: {
            },
            methods: {
            }
        });

    });
