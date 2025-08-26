$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            activeName: 'detail',
            //菜单项
            menus: [
                { name: '简述', tag: 'detail', icon: 'e667', size: 'medium', completed: true },
                { name: '实体生成', tag: 'entity', icon: 'e85a', size: 'medium', completed: true },
                { name: '查询', tag: 'query', icon: 'a00b', size: 'medium' },
                { name: '删改', tag: 'delete', icon: 'e800', size: 'medium' },
                { name: '统计', tag: 'statistic', icon: 'e83c', size: 'medium' },
                { name: '事务', tag: 'trans', icon: 'e634', size: 'medium' },
                { name: '复杂查询', tag: 'complexquery', icon: 'a04d', size: 'medium' },
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
                else return this.menus[0].tag + '.htm';
            }
        },
    });
});