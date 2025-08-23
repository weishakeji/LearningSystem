$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            activeName: 'detail',
            //菜单项
            menus: [
                { name: '简述', tag: 'detail', icon: 'e667', size: 'medium'  },
                { name: '表结构', tag: 'tables', icon: 'a02c', size: 'large'  },
                { name: '索引', tag: 'dbindex', icon: 'b006', size: 'huge'  },
                { name: '字段', tag: 'field', icon: 'e6cb', size: 'large'  },
                { name: '校验', tag: 'check', icon: 'e634', size: 'medium'  },
                { name: '导出', tag: 'export', icon: 'e73e', size: 'medium'  },                        
                { name: '数据迁移', tag: 'migration', icon: 'e79e', size: 'large' },
                { name: '其它', tag: 'other', icon: 'e67e', size: 'medium'  },
                { name: '升级脚本', tag: 'upgradesql', icon: 'a058', size: 'huge' },
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