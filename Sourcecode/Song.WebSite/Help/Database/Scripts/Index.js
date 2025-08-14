$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            activeName: 'detail',
            //菜单项
            menus: [
                { name: '简述', tag: 'detail', url: 'detail.html', icon: 'e6b0'}, 
                { name: '表结构', tag: 'structure', url: 'structure.html', icon: 'e6a4'}, 
                { name: '索引', tag: 'index', url: 'index.html', icon: 'e744'}, 
                { name: '查询', tag: 'query', url: 'query.html', icon: 'a00b'}, 
                { name: '实体生成', tag: 'entity', url: 'entity.html', icon: 'e667'}, 
                { name: '导出', tag: 'export', url: 'export.html', icon: 'e73e'}, 
                { name: '其它', tag: 'other', url: 'other.html', icon: 'e67e'}, 
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