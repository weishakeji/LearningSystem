$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            dbType: '',   //数据库类型
            dbName: '',     //数据库名称
            verison: '',    //数据库版本

            tables: [],      //数据库表
            tablecount:0,    //表数量
            total:0,        //总记录数

            fieldcount:0,    //字段数量
            indexcount:0,    //索引数量


            loadstate: {
                init: false,        //初始化
                get: false,         //默认
                index: false,         //获取索引
                table: false,      //获取表数据
                field: false          //获取字段数
            }
        },
        mounted: function () {
            this.getdbtype();
            this.getdbversion();
            this.gettables();
            this.getfield();
            this.getindex();

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
            //检测数据库名称
            getdbtype: function () {
                var th = this;
                th.loadstate.get = true;
                $api.get('DataBase/DbType').then(req => {
                    if (req.data.success) {
                        th.dbType = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.get = false);
            },
            //获取版本信息
            getdbversion: function () {
                var th = this;
                th.loadstate.def = true;
                $api.post('DataBase/DbVersion').then(function (req) {
                    if (req.data.success) {
                        th.verison = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.def = false);
            },
            //获取表数据
            gettables: function () {
                var th=this;
                th.loading.table=true;
                $api.get("DataBase/TableCount").then(req => {
                    if (req.data.success) {
                        th.tables = req.data.result;
                        let total = 0;
                        for (let key in th.tables) {
                            total += th.tables[key];
                            th.tablecount++;

                        }
                        th.total=total;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() =>th.loading.table=false);
            },
            //获取字段
            getfield:function(){
                var th=this;
                th.loading.field=true;
                $api.get("DataBase/FieldTotal").then(req => {
                    if (req.data.success) {
                        th.fieldcount = req.data.result;                        
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() =>th.loading.field=false);
            },
             //获取索引
             getindex:function(){
                var th=this;
                th.loading.index=true;
                $api.get("DataBase/IndexTotal").then(req => {
                    if (req.data.success) {
                        th.indexcount = req.data.result;                                          
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() =>th.loading.index=false);
            }
        },
        filters: {

        },
        components: {

        }
    });
});