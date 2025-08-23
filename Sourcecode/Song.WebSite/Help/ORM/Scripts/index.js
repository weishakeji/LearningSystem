$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            activeName: 'detail',
            //菜单项
            menus: [
                { name: '简述', tag: 'detail', icon: 'e667', size: 'medium'  },
                { name: '实体生成', tag: 'entity', icon: 'e85a', size: 'medium'  },   
             
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