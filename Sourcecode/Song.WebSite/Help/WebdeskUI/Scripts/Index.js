$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            activeName: 'detail',
            //菜单项
            menus: [
                { name: '简述', tag: 'detail', icon: 'e667', size: 'medium' },
                { name: '数据源', tag: 'entity', icon: 'e85a', size: 'medium' },
                { name: 'pagebox.js', tag: 'query', icon: 'a00b', size: 'medium' },
                { name: 'tabs.js', tag: 'delete', icon: 'e800', size: 'medium' },
                { name: 'treemenu.js', tag: 'statistic', icon: 'e83c', size: 'medium' },
                { name: 'dropmenu.js', tag: 'trans', icon: 'e634', size: 'medium' },
                { name: 'verticalbar.js', tag: 'batch', icon: 'a04d', size: 'medium' },
                { name: 'login.js', tag: 'batch', icon: 'a04d', size: 'medium' },               
            ],
            loadstate: {
                init: false,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {

        },
        created: function () {

        },
        computed: {
            loading: function () {
                if (!this.loadstate) return false;
                for (let key in this.loadstate) {
                    if (this.loadstate.hasOwnProperty(key)
                        && this.loadstate[key])
                        return true;
                }
                return false;
            }
        },
        watch: {

        },
        methods: {

        },
        filters: {

        },
        components: {

        }
    });
});