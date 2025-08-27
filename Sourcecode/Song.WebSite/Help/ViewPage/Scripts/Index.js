$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            activeName: 'detail',
            //菜单项
            menus: [
                { name: '简述', tag: 'detail', icon: 'a026', size: 'medium', completed: true },
                { name: '模板库', tag: 'Templates', icon: 'a018', size: 'medium', completed: true },
                { name: '创建页面', tag: 'CreatePage', icon: 'a022', size: 'medium', completed: false },
                { name: '资源引用', tag: 'Resource', icon: 'a029', size: 'medium', completed: false },              
            ]
        },
        watch: {

        },
        computed: {

        },
        mounted: function () {

        },
        methods: {
            //获取选项卡的url
            geturl: function (menu) {
                if (menu.completed)
                    return menu.tag + '.htm';
                else return this.menus[1].tag + '.htm';
            }
        },
    });
});