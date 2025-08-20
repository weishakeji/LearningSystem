$ready([
    'Components/entities.js'
], function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            dbType: '',     //数据库类型

            entity: null,      //当前实体
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
            this.getDbtype();
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
            //获取数据库类型
            getDbtype: function () {
                var th = this;
                th.loading.init = true;
                $api.get('DataBase/DBMS').then(req => {
                    if (req.data.success) {
                        th.dbType = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading.init = false);
            },
            //获取索引
            getIndexs: function () {
                var th = this;
                th.loading.get = true;
                $api.get("DataBase/Indexs", { "tablename": th.entity.name })
                    .then(req => {
                        if (req.data.success) {
                            let result = req.data.result;
                            if (th.dbType == 'PostgreSQL') th.indexs = th.showcolumn(result);
                            else th.indexs = result;
                            console.error(result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                    .finally(() => th.loading.get = false);
            },
            //将索引关联的列，转为json
            showcolumn: function (result) {
                for (let i = 0; i < result.length; i++) {
                    let items = [];   //列
                    let columns = result[i].columnname.split(",");
                    for (let j = 0; j < columns.length; j++) {
                        if (columns[j].replace(/\s+/g, '') == '') continue;
                        let str = columns[j].replace(/^\s+|\s+$/g, "");
                        const col = str.replace(/\s+/g, ' ').split(" ");
                        if (col.length < 2)
                            items.push({ 'column': col[0], 'sort': 'ASC' });
                        else items.push({ 'column': col[0], 'sort': col[1] });
                    }
                    result[i].columnname = items;
                }
                return result;
            },
        },
        filters: {

        },
        components: {

        }
    });
});