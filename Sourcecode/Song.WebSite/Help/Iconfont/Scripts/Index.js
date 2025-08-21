$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            activeName: 'icon',
            //菜单项
            menus: [
                { name: '图标库', tag: 'icon', icon: 'a007', size: 'huge',src:'/Utilities/Fonts/index.html'  },
                { name: '引用说明', tag: 'help', icon: 'a026', size: 'large',src:'/Utilities/Fonts/help.html'   },
                { name: '预载效果', tag: 'loading', icon: 'e620', size: 'huge',src:'/Utilities/Fonts/Loading.html'   },
                { name: '新增图标', tag: 'addsvg', icon: 'e759', size: 'large' ,src:'/Utilities/Fonts/svg.html'  },              
            ]
        },
        watch: {

        },
        computed: {

        },
        mounted: function () {

        },
        methods: {

        },
    });
});