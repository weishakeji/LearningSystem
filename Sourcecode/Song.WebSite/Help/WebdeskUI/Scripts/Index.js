$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            activeName: 'detail',
            //菜单项
            menus: [
                { name: '简述', tag: 'detail', icon: 'e834', size: 'large' },
                { name: '数据源', tag: 'datasource', icon: 'e85a', size: 'medium' },
                { name: 'pagebox.js', tag: 'pagebox', icon: 'a017', size: 'medium' },
                { name: 'treemenu.js', tag: 'treemenu', icon: 'a009', size: 'large' },
                { name: 'tabs.js', tag: 'tabs', icon: 'a018', size: 'medium' },             
                { name: 'dropmenu.js', tag: 'dropmenu', icon: 'a005', size: 'large' },
                { name: 'verticalbar.js', tag: 'verticalbar', icon: 'e667', size: 'medium' },
                { name: 'login.js', tag: 'login', icon: 'e808', size: 'medium' },               
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