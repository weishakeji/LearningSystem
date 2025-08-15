$ready([
    'Components/entities.js'
], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            entity: {},      //当前实体
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
            //alert(3)
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
            'entity': {
                handler: function (nv, ov) {
                    console.error(nv);
                }, deep: true
            }
        },
        methods: {

        },
        filters: {

        },
        components: {

        }
    });
});