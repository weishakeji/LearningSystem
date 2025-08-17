$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            activeName: 'detail',
            //菜单项
            menus: [
                { name: '简述', tag: 'detail', icon: 'e6b0', size: 'large'  },
                { name: '表结构', tag: 'tables', icon: 'e6a4', size: 'small'  },
                { name: '索引', tag: 'dbindex', icon: 'e744', size: 'large'  },
                { name: '查询', tag: 'query', icon: 'a00b', size: 'large'  },
                { name: '校验', tag: 'check', icon: 'e634', size: 'medium'  },
                { name: '实体生成', tag: 'entity', icon: 'e667', size: 'medium'  },
                { name: '导出', tag: 'export', icon: 'e73e', size: 'medium'  },
                { name: '其它', tag: 'other', icon: 'e67e', size: 'small'  },
                { name: '升级脚本', tag: 'upsql', icon: 'a058', size: 'huge' },
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