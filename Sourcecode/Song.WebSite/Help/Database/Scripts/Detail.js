$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            dbType: '',   //数据库类型
            dbName: '',     //数据库名称
            verison: '',    //数据库版本

            tables: [],      //数据库表
            tablecount: 0,    //表数量
            total: 0,        //总记录数
            size: 0,         //数据库大小，单位mb
            indexsize: 0,    //索引占用的空间，单位kb
            indexsizeunit: '',    //索引占用的空间单位



            fieldcount: 0,    //字段数量
            indexcount: 0,    //索引数量


            loadstate: {
                def: false,        //初始化
                get: false,         //默认
                dbname: false,         //获取数据库名称
                table: false,      //获取表数据
                info: false          //获取详情
            }
        },
        mounted: function () {
            this.getdbtype();
            this.getdbversion();
            this.gettables();
            this.getdbinfo();

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
                $api.get('DataBase/DBMS').then(req => {
                    if (req.data.success) {
                        th.dbType = req.data.result;
                        th.getDbname();
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
            //获取数据库名称
            getDbname: function () {
                var th = this;
                th.loadstate.dbname = true;
                $api.post('DataBase/DbName').then(function (req) {
                    if (req.data.success) {
                        th.dbName = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => th.dbName = '')
                    .finally(() => th.loadstate.dbname = false);
            },
            //获取表数据
            gettables: function () {
                var th = this;
                th.loading.table = true;
                $api.get("DataBase/TableCount").then(req => {
                    if (req.data.success) {
                        th.tables = req.data.result;
                        let total = 0;
                        for (let key in th.tables) {
                            total += th.tables[key];
                            th.tablecount++;
                        }
                        th.total = total;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading.table = false);
            },
            //获取数据库信息
            getdbinfo: function () {
                var th = this;
                th.loading.info = true;
                $api.bat(
                    $api.get('DataBase/IndexTotal'),
                    $api.cache('DataBase/FieldTotal'),
                    $api.post('DataBase/DbSize'),
                    $api.get("DataBase/IndexSize")  //索引占用的空间，单位kb
                ).then(([idx, field, dbsize, idxsize]) => {
                    th.indexcount = idx.data.result;
                    th.fieldcount = field.data.result;
                    let size = dbsize.data.result;
                    if (size > 1000) size = Math.floor(size);
                    th.size = size;
                    //索引占用的空间，单位kb
                    let indexsize = Number(idxsize.data.result);
                    let indexvalue = th.showIndexSize(indexsize);
                    th.indexsize = indexvalue.size;
                    th.indexsizeunit = indexvalue.unit;
                }).catch(err => console.error(err))
                    .finally(() => th.loading.info = false);
            },
            //计算显示索引空间的大小
            showIndexSize: function (size) {
                if (size < 1024) return { size: size, unit: 'Kb' };
                size = Math.floor(size / 1024 * 100) / 100;
                if (size < 1024) return { size: size, unit: 'Mb' };
                size = Math.floor(size / 1024 * 100) / 100;
                return { size: size, unit: 'Gb' };
            }
        },
        filters: {

        },
        components: {

        }
    });
});