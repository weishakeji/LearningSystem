$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            activeName: 'explain',
            //菜单项
            menus: [
                { name: '说明', tag: 'explain', icon: 'a026', size: 'medium'  },
                { name: 'API 接口查询', tag: 'APIList', icon: 'a01c', size: 'medium'  },              
                { name: '传输加密', tag: 'encrypt', icon: 'e613', size: 'large'  },             
                { name: '缓存策略', tag: 'cache', icon: 'e81a', size: 'medium' },
                { name: '导出', tag: 'export', icon: 'e73e', size: 'medium'  },
                { name: '$API.js', tag: 'Apijsdoc', icon: 'e820', size: 'large'  },
               
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