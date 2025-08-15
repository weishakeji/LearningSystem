$ready([
    'Components/entities.js'
], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            entity: {},      //当前实体
            indexs: [],     //索引列表

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
            //设置当前表结构的实体
            setentity: function (ent, entities) {
                this.entity = ent;
                this.datas = entities;
                this.getIndexs();
            },
            //获取索引
            getIndexs: function () {
                var th = this;
                th.loading.get = true;
                $api.get("Helper/EntityIndexs", { "tablename": th.entity.name })
                    .then(req => {
                        if (req.data.success) {
                            th.indexs = req.data.result;                           
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                    .finally(() => th.loading.get = false);
            },
        },
        filters: {

        },
        components: {

        }
    });
});