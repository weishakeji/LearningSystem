$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            activeName: 'detail',
            //菜单项
            menus: [
                { name: '简述', tag: 'detail', icon: 'e6b0' },
                { name: '表结构', tag: 'table', icon: 'e6a4' },
                { name: '索引', tag: 'dbindex', icon: 'e744' },
                { name: '查询', tag: 'query', icon: 'a00b' },
                { name: '实体生成', tag: 'entity', icon: 'e667' },
                { name: '导出', tag: 'export', icon: 'e73e' },
                { name: '其它', tag: 'other', icon: 'e67e' },
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