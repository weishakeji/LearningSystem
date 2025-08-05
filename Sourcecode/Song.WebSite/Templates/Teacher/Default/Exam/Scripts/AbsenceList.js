$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
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
                for (let t in this.loadstate) {
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